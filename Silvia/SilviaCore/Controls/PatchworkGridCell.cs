using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SilviaCore.Controls
{
    public class PatchworkGridCell : UserControl
    {
        public event Action<PatchworkGridCell> Click;

        public Vector Coord { get; set; }
        public Style DefaultStyle { get; set; }
        public bool IsContentDefault { get { return Content == btn; } }

        internal Button btn;

        public PatchworkGridCell()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                btn = new Button();
                btn.Click += (sender, args) => Click?.Invoke(this);

                Image img = new Image();
                img.Source = Images.Icons.PlusNormal.ToWPFImageSource();
                btn.Content = img;

                Content = btn;
            }

            this.MouseEnter += PatchworkGridCell_MouseEnter;
            this.MouseLeave += PatchworkGridCell_MouseLeave;
        }

        public PatchworkGridCell(Style defaultStyle) : this()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DefaultStyle = defaultStyle;
                btn.Style = DefaultStyle;
            }
        }

        private void PatchworkGridCell_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsContentDefault)
            {
                ((Image)btn.Content).Source = Images.Icons.PlusNormal.ToWPFImageSource();
            }
        }

        private void PatchworkGridCell_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsContentDefault)
            {
                ((Image)btn.Content).Source = Images.Icons.PlusHighlight.ToWPFImageSource();
            }
        }
    }
}
