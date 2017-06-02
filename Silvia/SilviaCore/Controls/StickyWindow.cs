using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SilviaCore.Controls
{
    public class StickyWindow : Window
    {
        public static int Offset { get; } = 10;

        private static uint idCount = 0;
        private static List<StickyWindow> allStickyWindows = new List<StickyWindow>();

        public enum IntersectDetail { None, Left, Right, Top, Bottom, Encompassed }

        public uint Id { get; set; } = idCount++;

        private (double x1, double x2) HorizontalExtremes;
        private (double y1, double y2) VerticalExtremes;
        private Vector CenterPoint;
        private Vector prevCenterPoint = new Vector(-1, -1);

        private bool isDragging = false;
        private Point dragAnchorPoint = new Point(-1, -1);

        public StickyWindow()
        {
            this.ResizeMode = ResizeMode.NoResize;
            allStickyWindows.Add(this);

            this.LocationChanged += StickyWindow_LocationChanged;
            this.IsVisibleChanged += StickyWindow_IsVisibleChanged;
        }

        private void StickyWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CalculateWindowPoints();
        }

        ~StickyWindow()
        {
            allStickyWindows.Remove(this);
        }

        private void SnapTo(StickyWindow sw, IntersectDetail intersect)
        {
            switch (intersect)
            {
                case IntersectDetail.Left:
                    this.Left = sw.Left + sw.Width;
                    break;
                case IntersectDetail.Right:
                    this.Left = sw.Left - this.Width;
                    break;
                case IntersectDetail.Top:
                    this.Top = sw.Top + sw.Height;
                    break;
                case IntersectDetail.Bottom:
                    this.Top = sw.Top - this.Height;
                    break;
            }

            if (intersect != IntersectDetail.None && intersect != IntersectDetail.Encompassed)
            {
                SetMouseDragPoint();
            }
        }

        private void StickyWindow_LocationChanged(object sender, EventArgs e)
        {
            if (prevCenterPoint == new Vector(-1, -1))
            {
                prevCenterPoint = CenterPoint;
                return;
            }

            CalculateWindowPoints();

            foreach (StickyWindow sw in allStickyWindows)
            {
                if (sw == this)
                {
                    continue;
                }

                var p1 = (CenterPoint - sw.CenterPoint);
                var p2 = (prevCenterPoint - sw.CenterPoint);

                if (p1.Length <= p2.Length)
                {
                    IntersectDetail intersectPoint = this.Intersects(sw);
                    Console.WriteLine("Intersect: " + intersectPoint.ToString());
                    
                    SnapTo(sw, intersectPoint);
                    
                }
                else
                {
                    Console.WriteLine("Away: " + sw.Title);
                }
            }

            prevCenterPoint = CenterPoint;
        }

        private IntersectDetail Intersects(StickyWindow other)
        {
            if (other == null || other == this)
                return IntersectDetail.None;

            //Check if in bounds
            if (HorizontalExtremes.x1 <= other.HorizontalExtremes.x2 &&
                HorizontalExtremes.x2 >= other.HorizontalExtremes.x1 &&
                VerticalExtremes.y1 <= other.VerticalExtremes.y2 &&
                VerticalExtremes.y2 >= other.VerticalExtremes.y1)
            {
                //Check if encompassed
                if (HorizontalExtremes.x1 >= other.HorizontalExtremes.x1 &&
                    HorizontalExtremes.x2 <= other.HorizontalExtremes.x2 &&
                    VerticalExtremes.y1 >= other.VerticalExtremes.y1 &&
                    VerticalExtremes.y2 <= other.VerticalExtremes.y2)
                {
                    return IntersectDetail.Encompassed;
                }

                double deg = Vector.AngleBetween(other.CenterPoint - CenterPoint, new Vector(1,0));

                //Normalize deg
                deg %= 360;
                if (deg < 0)
                {
                    deg += 360;
                }

                if (deg <= 45 || deg >= 315)
                {
                    return IntersectDetail.Right;
                }
                else if (deg < 135 && deg > 45)
                {
                    return IntersectDetail.Top;
                }
                else if (deg <= 225 && deg >= 135)
                {
                    return IntersectDetail.Left;
                }
                else if (deg < 315 && deg > 225)
                {
                    return IntersectDetail.Bottom;
                }
            }

            return IntersectDetail.None;
        }

        private void CalculateWindowPoints()
        {
            HorizontalExtremes = (
                Left - Offset,
                Left + Width + Offset);

            VerticalExtremes = (
                Top - Offset,
                Top + Height + Offset);

            CenterPoint = new Vector(
                HorizontalExtremes.x1 + Width / 2 + Offset,
                VerticalExtremes.y1 + Height / 2 + Offset);
        }

        private void SetMouseDragPoint()
        {
            if (Control.MousePosition.X > Left && 
                Control.MousePosition.X < Left + Width &&
                Control.MousePosition.Y > Top &&
                Control.MousePosition.Y < Top + Height)
            {
                dragAnchorPoint.X = Control.MousePosition.X - Left;
                dragAnchorPoint.Y = Control.MousePosition.Y - Top;
            }
        }

        public void StartDrag()
        {
            isDragging = true;
            SetMouseDragPoint();
            CaptureMouse();
            this.PreviewMouseMove += StickyWindow_PreviewMouseMove;
        }

        private void StickyWindow_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDragging)
            {
                var currentPoint = PointToScreen(e.GetPosition(this));
                Left = currentPoint.X - dragAnchorPoint.X;
                Top = currentPoint.Y - dragAnchorPoint.Y;
            }
        }

        public void StopDrag()
        {
            isDragging = false;
            this.PreviewMouseMove -= StickyWindow_PreviewMouseMove;
            ReleaseMouseCapture();
            dragAnchorPoint = new Point(-1, -1);
            prevCenterPoint = new Vector(-1, -1);
        }
    }
}
