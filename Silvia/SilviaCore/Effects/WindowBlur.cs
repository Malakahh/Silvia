using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace SilviaCore.Effects
{
    internal enum AccentState
    {
        ACCENT_DISABLED = 1,
        ACCENT_ENABLE_GRADIENT = 0,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }

    public class WindowBlur
    {
        static Dictionary<Window, bool> hasBlurred = new Dictionary<Window, bool>();
        static bool hasRegisteredThemeEvent = false;

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private static void Blur(Window w)
        {
            WindowInteropHelper windowHelper = new WindowInteropHelper(w);

            AccentPolicy accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            int accentStructSize = Marshal.SizeOf(accent);

            IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            WindowCompositionAttributeData data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);

            hasBlurred[w] = true;
        }

        public static void Apply(Window w)
        {
            if (!hasRegisteredThemeEvent)
            {
                hasRegisteredThemeEvent = true;
                Themes.ThemeSettings.Instance.PropertyChanged += Instance_PropertyChanged;
            }

            if (!hasBlurred.ContainsKey(w))
            {
                hasBlurred.Add(w, false);
                w.IsVisibleChanged += W_IsVisibleChanged;
            }
        }

        private static void W_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Window w = (Window)sender;
            w.IsVisibleChanged -= W_IsVisibleChanged;

            if (!hasBlurred.ContainsKey(w))
                return;

            if (hasBlurred[w])
                return;

            Blur(w);
        }

        private static void Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UseWindowBlur" && Themes.ThemeSettings.Instance.UseWindowBlur)
            {
                foreach (var kv in hasBlurred)
                {
                    if (!kv.Value)
                    {
                        Blur(kv.Key);
                    }
                }
            }
        }
    }
}
