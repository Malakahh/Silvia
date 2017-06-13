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
    public class PatchworkGridItem : UserControl
    {
        public event Action<PatchworkGridItem> Click;

        public Vector coord { get; set; }
        public Style DefaultStyle { get; set; }
        public bool IsContentDefault { get; private set; }

        Button btn;

        public PatchworkGridItem()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                btn = new Button();
                btn.Click += (sender, args) => Click?.Invoke(this);

                Image img = new Image();
                img.Source = Images.Icons.PlusNormal.ToWPFImageSource();
                btn.Content = img;

                ResetContent();
            }

            this.MouseEnter += PatchworkGridItem_MouseEnter;
            this.MouseLeave += PatchworkGridItem_MouseLeave;
        }

        public PatchworkGridItem(Style defaultStyle) : this()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DefaultStyle = defaultStyle;
                btn.Style = DefaultStyle;
            }
        }
        
        public void SetContent(object content)
        {
            if (content == null)
                return;

            Content = content;
            IsContentDefault = false;
        }

        public void ResetContent()
        {
            Content = btn;
            IsContentDefault = true;
        }

        private void PatchworkGridItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsContentDefault)
            {
                ((Image)btn.Content).Source = Images.Icons.PlusNormal.ToWPFImageSource();
            }
        }

        private void PatchworkGridItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsContentDefault)
            {
                ((Image)btn.Content).Source = Images.Icons.PlusHighlight.ToWPFImageSource();
            }
        }
    }
}
