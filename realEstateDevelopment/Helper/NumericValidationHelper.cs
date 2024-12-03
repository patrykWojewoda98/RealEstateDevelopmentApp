using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace realEstateDevelopment.Helper
{
    public static class NumericValidationHelper
    {
        public static readonly DependencyProperty NumericOnlyProperty =
            DependencyProperty.RegisterAttached(
                "NumericOnly",
                typeof(bool),
                typeof(NumericValidationHelper),
                new PropertyMetadata(false, OnNumericOnlyChanged));
        public static bool GetNumericOnly(TextBox textBox) => (bool)textBox.GetValue(NumericOnlyProperty);
        public static void SetNumericOnly(TextBox textBox, bool value) => textBox.SetValue(NumericOnlyProperty, value);
        private static void OnNumericOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                }
                else
                {
                    textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                }
            }
        }
        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }
        private static bool IsTextNumeric(string text)
        {
            Regex regex = new Regex("^[0-9]+$");
            return regex.IsMatch(text);
        }
    }
}