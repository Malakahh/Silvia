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
        private List<StickyWindow> stickyChildren = new List<StickyWindow>();
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

        private void StickTo(StickyWindow sw)
        {
            bool withinVerticalBounds = Top + Height > sw.Top && Top < sw.Top + sw.Height;
            bool withinHorizontalBounds = Left + Width > sw.Left && Left < sw.Left + sw.Width;

            //Stick to sw.left
            if (withinVerticalBounds && 
                Left + Width + StickyRange > sw.Left - StickyRange &&
                Left + Width - StickyRange < sw.Left + StickyRange)
            {
                Left = sw.Left - Width;
                SetSticky();
            }

            //Stick to sw.left + sw.width
            else if (withinVerticalBounds && 
                Left - StickyRange < sw.Left + sw.Width + StickyRange &&
                Left + StickyRange > sw.Left + sw.Width - StickyRange)
            {
                Left = sw.Left + sw.Width;
                SetSticky();
            }

            //Stick to sw.top
            else if (withinHorizontalBounds &&
                Top + Height + StickyRange > sw.Top - StickyRange &&
                Top + Height - StickyRange < sw.Top + StickyRange)
            {
                Top = sw.Top - Height;
                SetSticky();
            }

            //Stick to sw.top + sw.height
            else if (withinHorizontalBounds &&
                Top - StickyRange < sw.Top + sw.Height + StickyRange &&
                Top + StickyRange > sw.Top + sw.Height - StickyRange)
            {
                Top = sw.Top + sw.Height;
                SetSticky();
            }

            void SetSticky()
            {
                isSticking = true;
                stickyParent = sw;
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

        public void DragStart()
        {
            isDragging = true;
            SetMouseDragPoint();
            CaptureMouse();
            this.PreviewMouseMove += StickyWindow_PreviewMouseMove;
            this.stickyParent?.stickyChildren.Remove(this);
        }

        private bool IsWithinBounds(Vector v, StickyWindow checkAgainst, double offset)
        {
            return v.X > checkAgainst.Left - offset &&
                v.X < checkAgainst.Left + checkAgainst.Width + offset &&
                v.Y > checkAgainst.Top - offset &&
                v.Y < checkAgainst.Top + checkAgainst.Height + offset;
        }

        private void StickyWindow_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDragging)
            {
                Vector currentPoint = PointToScreen(e.GetPosition(this)).ToVector();

                Vector diff = new Vector(
                    currentPoint.X - dragAnchorPoint.X - Left,
                    currentPoint.Y - dragAnchorPoint.Y - Top);

                if (IsMasterWindow)
                {
                    Left += diff.X;
                    Top += diff.Y;

                    foreach (StickyWindow sw in stickyChildren)
                    {
                        sw.Left += diff.X;
                        sw.Top += diff.Y;
                    }
                }
                else if (!isSticking)
                {
                    Left += diff.X;
                    Top += diff.Y;

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
                    //Predictions
                    Vector predTopLeft = new Vector(
                        Left + diff.X,
                        Top + diff.Y);

                    Vector predTopRight = new Vector(
                        predTopLeft.X + Width,
                        predTopLeft.Y);

                    Vector predBottomLeft = new Vector(
                        predTopLeft.X,
                        predTopLeft.Y + Height);

                    Vector predBottomRight = new Vector(
                        predTopLeft.X + Width,
                        predTopLeft.Y + Height);

                    if (stickyParent != null &&
                        !IsWithinBounds(predTopLeft, stickyParent, StickyRange) &&
                        !IsWithinBounds(predTopRight, stickyParent, StickyRange) &&
                        !IsWithinBounds(predBottomLeft, stickyParent, StickyRange) &&
                        !IsWithinBounds(predBottomRight, stickyParent, StickyRange))
                    {
                        isSticking = false;
                        stickyParent = null;
                    }
                }
            }
        }

        public void DragStop()
        {
            isDragging = false;
            this.PreviewMouseMove -= StickyWindow_PreviewMouseMove;
            ReleaseMouseCapture();
            dragAnchorPoint = new Point(-1, -1);

            if (!IsMasterWindow)
            {
                foreach (StickyWindow sw in allStickyWindows)
                {
                    if (sw != this && ((
                        IsWithinBounds(new Vector(Left, Top), sw, 1) ||
                        IsWithinBounds(new Vector(Left + Width, Top), sw, 1) ||
                        IsWithinBounds(new Vector(Left, Top + Height), sw, 1) ||
                        IsWithinBounds(new Vector(Left + Width, Top + Height), sw, 1) ||
                        //The below cases can occour when the moving window is larger than the target Master window
                        (IsWithinBounds(new Vector(sw.Left, sw.Top), this, 1) ||
                        IsWithinBounds(new Vector(sw.Left + sw.Width, sw.Top), this, 1) ||
                        IsWithinBounds(new Vector(sw.Left, sw.Top + sw.Height), this, 1) ||
                        IsWithinBounds(new Vector(sw.Left + sw.Width, sw.Top + sw.Height), this, 1)))))
                    {
                        isSticking = true;
                        stickyParent = sw;
                        stickyParent.stickyChildren.Add(this);
                    }
                }
            }
        }
    }
}
