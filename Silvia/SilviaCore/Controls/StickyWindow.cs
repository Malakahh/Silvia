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
        public static int StickyRange { get; } = 10;

        private static List<StickyWindow> allStickyWindows = new List<StickyWindow>();
        private static Vector xAxis = new Vector(1, 0);
        private static Vector mouseNullPos = new Vector(-1, -1);

        public bool IsMasterWindow { get; set; } = false;

        private bool isDragging = false;
        private bool isSticking = false;
        private StickyWindow stickyParent;
        private Point dragAnchorPoint = new Point(-1, -1);

        public StickyWindow()
        {
            this.ResizeMode = ResizeMode.NoResize;
            allStickyWindows.Add(this);
        }

        ~StickyWindow()
        {
            allStickyWindows.Remove(this);
        }

        //private enum RelativePosition { Left, Right, Top, Bottom }
        //private RelativePosition GetVectorDirection(Vector v1, Vector v2)
        //{
        //    double deg = Vector.AngleBetween(v1 - v2, xAxis);

        //    //Normalise deg
        //    deg %= 360;
        //    if (deg < 0)
        //        deg += 360;
            
        //    if (deg <= 45 || deg >= 315)
        //    {
        //        return RelativePosition.Right;
        //    }
        //    else if (deg < 135 && deg > 45)
        //    {
        //        return RelativePosition.Top;
        //    }
        //    else if (deg <= 225 && deg >= 135)
        //    {
        //        return RelativePosition.Left;
        //    }
        //    else
        //    {
        //        return RelativePosition.Bottom;
        //    }
        //}

        private void StickTo(StickyWindow sw)
        {
            bool withinVerticalBounds = Top + Height > sw.Top && Top < sw.Top + sw.Height;
            bool withinHorizontalBounds = Left + Width > sw.Left && Left < sw.Left + sw.Width;

            isSticking = true;
            stickyParent = sw;

            //Stick to sw.left
            if (withinVerticalBounds && 
                Left + Width + StickyRange > sw.Left - StickyRange &&
                Left + Width - StickyRange < sw.Left + StickyRange)
            {
                Left = sw.Left - Width;
            }

            //Stick to sw.left + sw.width
            else if (withinVerticalBounds && 
                Left - StickyRange < sw.Left + sw.Width + StickyRange &&
                Left + StickyRange > sw.Left + sw.Width - StickyRange)
            {
                Left = sw.Left + sw.Width;
            }

            //Stick to sw.top
            else if (withinHorizontalBounds &&
                Top + Height + StickyRange > sw.Top - StickyRange &&
                Top + Height - StickyRange < sw.Top + StickyRange)
            {
                Top = sw.Top - Height;
            }

            //Stick to sw.top + sw.height
            else if (withinHorizontalBounds &&
                Top - StickyRange < sw.Top + sw.Height + StickyRange &&
                Top + StickyRange > sw.Top + sw.Height - StickyRange)
            {
                Top = sw.Top + sw.Height;
            }

            else
            {
                isSticking = false;
                stickyParent = null;
            }
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

        Vector prevMousePos = mouseNullPos;
        private void StickyWindow_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDragging)
            {
                Vector currentPoint = PointToScreen(e.GetPosition(this)).ToVector();

                if (!isSticking)
                {
                    Left = currentPoint.X - dragAnchorPoint.X;
                    Top = currentPoint.Y - dragAnchorPoint.Y;

                    foreach (StickyWindow sw in allStickyWindows)
                    {
                        if (sw != this)
                        {
                            StickTo(sw);
                            break;
                        }
                    }
                }
                else
                {
                    //Are we far enough away?
                    if (stickyParent != null && !(currentPoint.X > stickyParent.Left - StickyRange &&
                        currentPoint.X < stickyParent.Left + stickyParent.Width + StickyRange &&
                        currentPoint.Y > stickyParent.Top - StickyRange &&
                        currentPoint.Y < stickyParent.Top + stickyParent.Height + StickyRange))
                    {
                        isSticking = false;
                        stickyParent = null;
                    }
                }

                prevMousePos = Control.MousePosition.ToVector();
            }
        }

        public void StopDrag()
        {
            isDragging = false;
            this.PreviewMouseMove -= StickyWindow_PreviewMouseMove;
            ReleaseMouseCapture();
            dragAnchorPoint = new Point(-1, -1);
        }
    }
}
