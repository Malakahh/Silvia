using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using SilviaCore.Properties;
using Newtonsoft;

namespace SilviaCore.Controls
{
    public class StickyWindow : Window
    {
        public static int StickyRange { get; } = 10;

        private static List<StickyWindow> allStickyWindows = new List<StickyWindow>();
        private static Dictionary<StickyWindow, HashSet<StickyWindow>> masterSlaveTrees = new Dictionary<StickyWindow, HashSet<StickyWindow>>();
        private static Vector xAxis = new Vector(1, 0);
        private static Vector mouseNullPos = new Vector(-1, -1);
        private static uint idGlobalCnt { get; set; } = 0;
        private static WindowPositions settings;

        public uint Id { get; private set; }
        private bool _isMasterWindow = false;
        public bool IsMasterWindow
        {
            get
            {
                return _isMasterWindow;
            }

            set
            {
                if (value)
                    masterSlaveTrees.Add(this, new HashSet<StickyWindow>());
                else
                    masterSlaveTrees.Remove(this);

                _isMasterWindow = value;
            }
        }

        private bool isDragging = false;
        private Point dragAnchorPoint;

        public StickyWindow()
        {
            this.Id = idGlobalCnt++;
            this.ResizeMode = ResizeMode.NoResize;
            allStickyWindows.Add(this);
            SilviaApp.OnApplicationInit += SilviaApp_OnApplicationInit;
            SilviaApp.OnApplicationClosing += SilviaApp_OnApplicationClosing;
        }

        private void SilviaApp_OnApplicationClosing()
        {
            allStickyWindows.Remove(this);
        }

        private void SilviaApp_OnApplicationInit()
        {
            if (settings == null)
            {
                settings = WindowPositions.Create();
            }

            settings.Windows.Add(new PosSetting()
            {
                Id = this.Id,
                Left = this.Left,
                Top = this.Top
            });
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

            if (!IsMasterWindow)
            {
                foreach (var kv in masterSlaveTrees)
                {
                    if (kv.Value.Contains(this))
                    {
                        kv.Value.Remove(this);
                        break;
                    }
                }
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

                    foreach (StickyWindow sw in masterSlaveTrees[this])
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
                        if (sw.IsMasterWindow)
                        {
                            masterSlaveTrees[sw].Add(this);
                        }
                        else
                        {
                            foreach (var kv in masterSlaveTrees)
                            {
                                if (kv.Value.Contains(sw))
                                {
                                    masterSlaveTrees[kv.Key].Add(this);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class StickyWindowSettings : Settings
    {
        public uint Id;
    }

    class PosSetting : StickyWindowSettings
    {
        public double Left;
        public double Top;
    }

    class WindowPositions : Settings
    {
        public List<PosSetting> Windows = new List<PosSetting>();

        public WindowPositions()
        {
            SilviaApp.OnApplicationClosing += SilviaApp_OnApplicationClosing;
        }

        public static WindowPositions Create()
        {
            var loaded = Load<WindowPositions>();

            return loaded;
        }

        private void SilviaApp_OnApplicationClosing()
        {
            Save(this);
        }
    }
}
