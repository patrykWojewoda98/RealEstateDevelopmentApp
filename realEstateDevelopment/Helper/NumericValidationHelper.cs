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

        //TODO zamienić w AddViews funkcje z IsextNumeric na IsTextInteger oraz IsTextDouble
        private static bool IsTextNumeric(string text)
        {

            return int.TryParse(text, out var result);
            
            //return decimal.TryParse(text, out var result);
            //Metoda nie dokonca poprawna
            //Regex regex = new Regex("^[0-9]+$");
            //return regex.IsMatch(text);
        }


        private static bool IsTextInteger(string text)
        {

            return int.TryParse(text, out var result);
            //todo zrobić drugą metodę do sprawdzania liczb zmienno przecinkowych taką samą tylko z returnem poniżej ***vvv***
            //return decimal.TryParse(text, out var result);
            //Regex regex = new Regex("^[0-9]+$");
            //return regex.IsMatch(text);
        }

        private static bool IsTextDouble(string text)
        {
            return decimal.TryParse(text, out var result);
        }
    }
}