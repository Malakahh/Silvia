using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using SilviaCore;

namespace SilviaGUI
{
    class NotificationIcon
    {
        const string iconName = "Silvia.ico";
        NotifyIcon nIcon;

        public NotificationIcon()
        {
            string iconPath = Directory.GetCurrentDirectory() + "\\" + iconName;

            nIcon = new NotifyIcon()
            {
                Icon = new Icon(iconPath),
                Text = SilviaApp.AppNameFull,
                Visible = true
            };
            nIcon.MouseUp += NIcon_MouseUp;

            SilviaApp.OnApplicationClosing += SilviaApp_OnApplicationClosing;
            SilviaApp.OnApplicationInit += SilviaApp_OnApplicationInit;
        }

        private void SilviaApp_OnApplicationInit()
        {
            nIcon.ContextMenu = new ContextMenu();
            nIcon.ContextMenu.MenuItems.Add("Hide", (s,e) => SilviaGUI.mainPanel.Hide());
            nIcon.ContextMenu.MenuItems.Add("Show", (s,e) => SilviaGUI.mainPanel.Show());
            nIcon.ContextMenu.MenuItems.Add("Options", (s,e) => SilviaGUI.options.Show());
            nIcon.ContextMenu.MenuItems.Add("-");
            nIcon.ContextMenu.MenuItems.Add("Close Application", (s, e) => SilviaApp.Close());

            nIcon.ContextMenu.MenuItems[0].Visible = SilviaGUI.mainPanel.IsVisible;
            nIcon.ContextMenu.MenuItems[1].Visible = !SilviaGUI.mainPanel.IsVisible;

            SilviaGUI.mainPanel.IsVisibleChanged += MainPanel_IsVisibleChanged;
        }

        private void MainPanel_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            nIcon.ContextMenu.MenuItems[0].Visible = SilviaGUI.mainPanel.IsVisible;
            nIcon.ContextMenu.MenuItems[1].Visible = !SilviaGUI.mainPanel.IsVisible;
        }

        private void SilviaApp_OnApplicationClosing()
        {
            nIcon.Visible = false;
            nIcon.Dispose();
        }

        private void NIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SilviaGUI.mainPanel.IsVisible)
                    SilviaGUI.mainPanel.Hide();
                else
                    SilviaGUI.mainPanel.Show();

            }
        }
    }
}
