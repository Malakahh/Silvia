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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SilviaGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : SilviaCore.Controls.StickyWindow
    {
        public Window1()
        {
            InitializeComponent();
            this.PreviewMouseDown += Window1_PreviewMouseDown;
            this.PreviewMouseUp += Window1_PreviewMouseUp;
        }

        private void Window1_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.DragStop();
        }

        private void Window1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragStart();
        }
    }
}
