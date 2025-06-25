using System.Windows;
using System.Windows.Controls;

namespace Lol_Overlay_MVVM.Utils
{
    public static class PasswordBoxUtils
    {
        public static string GetBoundPassword(DependencyObject dp)
            => (string)dp.GetValue(BoundPasswordProperty);

        public static void SetBoundPassword(DependencyObject dp, string value)
            => dp.SetValue(BoundPasswordProperty, value);

        public static bool GetBindPassword(DependencyObject dp)
            => (bool)dp.GetValue(BindPasswordProperty);

        public static void SetBindPassword(DependencyObject dp, bool value)
            => dp.SetValue(BindPasswordProperty, value);

        public static readonly DependencyProperty BindPasswordProperty =
        DependencyProperty.RegisterAttached(
            "BindPassword",
            typeof(bool),
            typeof(PasswordBoxUtils),
            new PropertyMetadata(false, OnBindPasswordChanged));

        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxUtils),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox pb)
            {
                if ((bool)e.NewValue)
                    pb.PasswordChanged += PasswordChanged;
                else
                    pb.PasswordChanged -= PasswordChanged;
            }
        }

        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox pb && e.NewValue is string s && pb.Password != s)
                pb.Password = s;
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = (PasswordBox)sender;
            SetBoundPassword(pb, pb.Password);
        }
    }
}