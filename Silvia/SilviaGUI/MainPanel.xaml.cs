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
using System.Windows.Forms.Integration;
using SilviaCore;
using SilviaCore.Commands;
using NLog;

namespace SilviaGUI
{
    class Hi : SilviaCore.Controls.StickyWindowSettings
    {
        public string TestString = "T";

        public Hi() : base()
        {
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainPanel : SilviaCore.Controls.StickyWindow
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private CmdTabCompletion tabCompletion = new CmdTabCompletion();

        public MainPanel() : base()
        {
            InitializeComponent();

            ElementHost.EnableModelessKeyboardInterop(this);

            this.IsMasterWindow = true;

            Storyboard s = (Storyboard)TryFindResource("sb");
            s.Begin();

            SilviaCore.SilviaApp.OnApplicationInit += SilviaApp_OnApplicationInit;
            header.PreviewMouseDown += Header_PreviewMouseDown;
            this.PreviewMouseUp += Header_PreviewMouseUp;
            headerOpen.MouseEnter += HeaderOpen_MouseEnter;
            headerOpen.MouseLeave += HeaderOpen_MouseLeave;
            headerOpen.PreviewMouseDown += HeaderOpen_PreviewMouseDown;
            BtnHide.Click += BtnHide_Click;
            BtnRefresh.Click += BtnRefresh_Click;
            InputCmd.PreviewKeyDown += InputCmd_PreviewKeyDown;
            InputCmd.TextChanged += InputCmd_TextChanged;
            InputCmd.GotFocus += InputCmd_GotFocus;
            InputCmd.LostFocus += InputCmd_LostFocus;
            InputCmd.PreviewMouseDown += InputCmd_PreviewMouseDown;
            this.LocationChanged += MainPanel_LocationChanged;
            this.IsVisibleChanged += MainPanel_IsVisibleChanged;

            SilviaCore.Commands.CmdHandler.AddCmd(new Command(
                "^hide$",
                (args) => {
                    this.Hide();
                    tabCompletion.Hide();
                }));

            var test = Settings.GetSettings<Hi>();

            if (test == null)
            {
                test = new Hi()
                {
                    TestString = "T is for Test",
                    Left = 2,
                    Top = 3
                };
            }
            else
            {
                test.TestString = "THIS HAS BEEN LOADED AND EDITED";
            }
        }

        private void PositionCmdTabCompletionWindow()
        {
            tabCompletion.Width = InputCmd.ActualWidth;
            tabCompletion.Left = this.Left + InputCmd.BorderThickness.Left + InputCmd.Margin.Left - InputCmd.BorderThickness.Left;

            if (this.Top + this.ActualHeight + tabCompletion.ActualHeight < SystemParameters.WorkArea.Bottom)
                tabCompletion.Top = this.Top + test.RowDefinitions[2].Offset + InputCmd.Margin.Top + InputCmd.ActualHeight;
            else
                tabCompletion.Top = this.Top + test.RowDefinitions[2].Offset + InputCmd.Margin.Top - tabCompletion.ActualHeight;
        }

        private void MainPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                tabCompletion.Hide();
            }
        }

        private void MainPanel_LocationChanged(object sender, EventArgs e)
        {
            PositionCmdTabCompletionWindow();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            foreach (Plugin p in SilviaCore.PluginLoader.Plugins.Values)
            {
                p.Refresh();
            }
        }

        private void BtnHide_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void SilviaApp_OnApplicationInit()
        {
            headerOpen.Source = SilviaCore.Images.HeaderIcons.OpenNormal.ToWPFImageSource();
        }

    #region InputCmdEvents

        private void InputCmd_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tabCompletion.Deselect();
        }

        private void InputCmd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isTabCompletionOngoing)
            {
                if (InputCmd.Text.Length >= tabCompletion.MinimumLength)
                {
                    tabCompletion.UpdateEntires(InputCmd.Text);

                    if (tabCompletion.Entries.Count > 0)
                    {
                        tabCompletion.Show();
                        PositionCmdTabCompletionWindow();
                        this.Focus();
                    }
                    else
                    {
                        tabCompletion.Hide();
                    }
                }
                else
                {
                    tabCompletion.Hide();
                }
            }
        }

        private void InputCmd_LostFocus(object sender, RoutedEventArgs e)
        {
            tabCompletion.Hide();
        }

        private void InputCmd_GotFocus(object sender, RoutedEventArgs e)
        {
            PositionCmdTabCompletionWindow();
            InputCmd_TextChanged(this, null);
        }

        bool isTabCompletionOngoing = false;
        private void InputCmd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //For tab completion
            if (e.Key == Key.Up)
            {
                isTabCompletionOngoing = true;

                Command cmd = tabCompletion.SelectPreviousItem();
                InputCmd.Text = cmd?.Prediction;
                InputCmd.CaretIndex = InputCmd.Text.Length;
            }
            else if (e.Key == Key.Tab && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                e.Handled = true;

                isTabCompletionOngoing = true;

                Command cmd = tabCompletion.SelectPreviousItem();
                InputCmd.Text = cmd?.Prediction;
                InputCmd.CaretIndex = InputCmd.Text.Length;
            }
            else if (e.Key == Key.Tab)
            {
                e.Handled = true;

                isTabCompletionOngoing = true;

                Command cmd = tabCompletion.SelectNextItem();
                InputCmd.Text = cmd?.Prediction;
                InputCmd.CaretIndex = InputCmd.Text.Length;
            }
            else if (e.Key == Key.Down)
            {
                isTabCompletionOngoing = true;

                Command cmd = tabCompletion.SelectNextItem();
                InputCmd.Text = cmd?.Prediction;
                InputCmd.CaretIndex = InputCmd.Text.Length;
            }
            else
            {
                //Disable tab completion
                isTabCompletionOngoing = false;
            }

            //Done writing
            if (e.Key == Key.Enter && InputCmd.Text != "")
            {
                logger.Trace("CmdInput: " + InputCmd.Text);

                SilviaCore.Commands.CmdHandler.Cmds.ForEach(x => x.InvokeWithStringParams(InputCmd.Text));
                InputCmd.Text = "";
            }
            if (e.Key == Key.Escape)
            {
                InputCmd.Text = "";
            }
        }

    #endregion
    #region HeaderEvents

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
            SilviaGUI.options.Show();
        }

        private void Header_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragStart();
            }
        }

        private void Header_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                this.DragStop();
            }
        }

    #endregion
    }
}
