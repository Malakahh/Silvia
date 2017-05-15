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

            nIcon = new NotifyIcon();
            nIcon.Icon = new Icon(iconPath);
            nIcon.Text = SilviaApp.AppNameFull;
            nIcon.Visible = true;

            nIcon.MouseUp += NIcon_MouseUp;

            SilviaApp.OnApplicationClosing += SilviaApp_OnApplicationClosing;
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
            else if (e.Button == MouseButtons.Right)
            {
                SilviaApp.Close();
            }
        }
    }
}
