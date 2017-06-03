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
        private StickyWindow stickyParent;
        private List<StickyWindow> stickyChildren = new List<StickyWindow>();
        private Point dragAnchorPoint;

        public StickyWindow()
        {
            this.ResizeMode = ResizeMode.NoResize;
            allStickyWindows.Add(this);
        }

        ~StickyWindow()
        {
            allStickyWindows.Remove(this);
        }

        private bool StickTo(StickyWindow sw)
        {
            bool withinVerticalBounds = Top + Height > sw.Top && Top < sw.Top + sw.Height;
            bool withinHorizontalBounds = Left + Width > sw.Left && Left < sw.Left + sw.Width;

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
                return false;
            }

            return true;
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

            if (!IsMasterWindow && stickyParent != null)
            {
                this.stickyParent?.stickyChildren.Remove(this);
                this.stickyParent = null;
            }
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
                else
                {
                    Left += diff.X;
                    Top += diff.Y;

                    foreach (StickyWindow sw in allStickyWindows)
                    {
                        if (sw != this)
                        {
                            if (StickTo(sw))
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void DragStop()
        {
            isDragging = false;
            this.PreviewMouseMove -= StickyWindow_PreviewMouseMove;
            ReleaseMouseCapture();

            //TODO: All of this logic should be handled better. Maybe a hashtable where the master is the key?
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
                        StickyWindow connectedTo = sw;

                        while (connectedTo.stickyParent != null)
                        {
                            connectedTo = connectedTo.stickyParent;
                        }

                        if (connectedTo.IsMasterWindow)
                        {
                            connectedTo.stickyChildren.Add(this);
                        }

                        stickyParent = connectedTo;
                        break;
                    }
                }
            }
        }
    }
}
