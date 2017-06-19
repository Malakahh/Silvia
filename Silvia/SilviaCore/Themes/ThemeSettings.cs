using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DColor = System.Drawing.Color;

namespace SilviaCore.Themes
{
    public class ThemeSettings : Settings, INotifyPropertyChanged
    {
        public static ThemeSettings Instance { get; private set; } //We're not using a static class to allow for WPF binding
        public event PropertyChangedEventHandler PropertyChanged;

        private SolidColorBrush _backgroundbrush = new SolidColorBrush().SetARGB(0x7C000000);
        public SolidColorBrush BackgroundBrush
        {
            get { return _backgroundbrush; }
            set
            {
                _backgroundbrush = value;
                FirePropertyChanged();
            }
        }

        private SolidColorBrush _specialBrush = new SolidColorBrush().SetARGB(0xFF7C7C7C);
        public SolidColorBrush SpecialBrush
        {
            get { return _specialBrush; }
            set
            {
                _specialBrush = value;
                FirePropertyChanged();
            }
        }

        #region Control
        private SolidColorBrush _controlBackgroundBrush = new SolidColorBrush().SetARGB(0x00000000);
        public SolidColorBrush ControlBackgroundBrush
        {
            get { return _controlBackgroundBrush; }
            set
            {
                _controlBackgroundBrush = value;
                FirePropertyChanged();
            }
        }

        private SolidColorBrush _controlHighlightBackgroundBrush = new SolidColorBrush().SetARGB(0x000000);
        public SolidColorBrush ControlHighlightBackgroundBrush
        {
            get { return _controlHighlightBackgroundBrush; }
            set
            {
                _controlHighlightBackgroundBrush = value;
                FirePropertyChanged();
            }
        }

        private SolidColorBrush _controlInteractBackgroundBrush = new SolidColorBrush().SetARGB(0x000000);
        public SolidColorBrush ControlInteractBackgroundBrush
        {
            get { return _controlInteractBackgroundBrush; }
            set
            {
                _controlInteractBackgroundBrush = value;
                FirePropertyChanged();
            }
        }

        private SolidColorBrush _controlNormalBrush = new SolidColorBrush().SetRGB(0xFFFFFF);
        public SolidColorBrush ControlNormalBrush
        {
            get { return _controlNormalBrush; }
            set
            {
                _controlNormalBrush = value;
                FirePropertyChanged();
            }
        }

        private SolidColorBrush _controlHighlightBrush = new SolidColorBrush().SetRGB(0xFFFFFF);
        public SolidColorBrush ControlHighlightBrush
        {
            get { return _controlHighlightBrush; }
            set
            {
                _controlHighlightBrush = value;
                FirePropertyChanged();
            }
        }

        private SolidColorBrush _controlInteractBrush = new SolidColorBrush().SetARGB(0x7CFFFFFF);
        public SolidColorBrush ControlInteractBrush
        {
            get { return _controlInteractBrush; }
            set
            {
                _controlInteractBrush = value;
                FirePropertyChanged();
            }
        }

        private uint _controlBorderThickness = 1;
        public uint ControlBorderThickness
        {
            get { return _controlBorderThickness; }
            set
            {
                _controlBorderThickness = value;
                FirePropertyChanged();
            }
        }

        private uint _controlHighlightBorderThickness = 2;
        public uint ControlHighlightBorderThickness
        {
            get { return _controlHighlightBorderThickness; }
            set
            {
                _controlHighlightBorderThickness = value;
                FirePropertyChanged();
            }
        }

        private uint _controlInteractBorderThickness = 1;
        public uint ControlInteractBorderThickness
        {
            get { return _controlInteractBorderThickness; }
            set
            {
                _controlInteractBorderThickness = value;
                FirePropertyChanged();
            }
        }
        #endregion

        #region ImageColors
        private DColor _headerIconNormal = DColor.FromArgb(0xFF, 0x00, 0x00, 0x00);
        public DColor HeaderIconNormal
        {
            get { return _headerIconNormal; }
            set
            {
                _headerIconNormal = value;
                FirePropertyChanged();
            }
        }

        private DColor _headerIconHighlight = DColor.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public DColor HeaderIconHighlight
        {
            get { return _headerIconHighlight; }
            set
            {
                _headerIconHighlight = value;
                FirePropertyChanged();
            }
        }

        private DColor _iconNormal = DColor.FromArgb(0x7C, 0xFF, 0xFF, 0xFF);
        public DColor IconNormal
        {
            get { return _iconNormal; }
            set
            {
                _iconNormal = value;
                FirePropertyChanged();
            }
        }

        private DColor _iconHighlight = DColor.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        public DColor IconHighlight
        {
            get { return _iconHighlight; }
            set
            {
                _iconHighlight = value;
                FirePropertyChanged();
            }
        }
        #endregion

        private bool _useWindowBlur = true;
        public bool UseWindowBlur
        {
            get { return _useWindowBlur; }
            set
            {
                _useWindowBlur = value;
                FirePropertyChanged();
            }
        }

        internal static void Init()
        {
            Instance = Settings.GetSettings<ThemeSettings>(0) ?? new ThemeSettings();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FirePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
