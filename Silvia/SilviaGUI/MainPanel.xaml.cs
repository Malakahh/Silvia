using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using SilviaCore;
using System.Windows.Forms.Integration;

namespace SilviaGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainPanel : Window
    {
        public MainPanel()
        {
            InitializeComponent();

            ElementHost.EnableModelessKeyboardInterop(this);

            SilviaCore.SilviaApp.OnApplicationInit += SilviaApp_OnApplicationInit;

            Storyboard s = (Storyboard)TryFindResource("sb");
            s.Begin();

            header.PreviewMouseDown += Header_PreviewMouseDown;
            headerOpen.MouseEnter += HeaderOpen_MouseEnter;
            headerOpen.MouseLeave += HeaderOpen_MouseLeave;
            headerOpen.PreviewMouseDown += HeaderOpen_PreviewMouseDown;
            BtnHide.Click += BtnHide_Click;
            BtnRefresh.Click += BtnRefresh_Click;
            InputCmd.KeyDown += InputCmd_KeyDown;
        }

        private void InputCmd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                InputCmd.Text = "";
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnHide_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void HeaderOpen_MouseLeave(object sender, MouseEventArgs e)
        {
            headerOpen.Source = SilviaCore.Images.HeaderIcons.OpenNormal.ToWPFImageSource();
        }

        private void HeaderOpen_MouseEnter(object sender, MouseEventArgs e)
        {
            headerOpen.Source = SilviaCore.Images.HeaderIcons.OpenHighlight.ToWPFImageSource();
        }

        private void HeaderOpen_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SilviaApp_OnApplicationInit()
        {
            headerOpen.Source = SilviaCore.Images.HeaderIcons.OpenNormal.ToWPFImageSource();
        }

        private void Header_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
