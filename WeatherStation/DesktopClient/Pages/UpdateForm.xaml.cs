using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace DesktopClient.Pages
{
    public partial class UpdateForm : Page
    {
        public UpdateForm()
        {
            InitializeComponent();
        }

        // Validation - only numbers
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }


        private bool IsTextNumeric(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c) && c != '.' && c != ',')
                {
                    return false;
                }
            }
            return true;
        }

        // Method to validate time input
        private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9:]");
            e.Handled = regex.IsMatch(e.Text);

            if (TimeTextBox.Text.Length == 2 && TimeTextBox.Text.IndexOf(":") == -1)
            {
                TimeTextBox.Text += ":";
                TimeTextBox.SelectionStart = TimeTextBox.Text.Length;
            }
            else if (TimeTextBox.Text.Length == 5 && TimeTextBox.Text.IndexOf(":", 3) == -1)
            {
                TimeTextBox.Text += ":";
                TimeTextBox.SelectionStart = TimeTextBox.Text.Length;
            }
        }

        // LostFocus event for field validation
        private void Field_LostFocus(object sender, RoutedEventArgs e)
        {
            var control = sender as UIElement;
            if (control != null)
            {
                if (control is TextBox textBox)
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.Background = Brushes.Red;
                    }
                    else
                    {
                        textBox.Background = (Brush)FindResource("LightGray");
                    }
                }
                else if (control is ComboBox comboBox)
                {
                    if (comboBox.SelectedIndex == -1)
                    {
                        comboBox.Background = Brushes.Red;
                    }
                    else
                    {
                        comboBox.Background = (Brush)FindResource("LightGray");
                    }
                }
                else if (control is DatePicker datePicker)
                {
                    if (datePicker.SelectedDate == null)
                    {
                        datePicker.Background = Brushes.Red;
                    }
                    else
                    {
                        datePicker.Background = (Brush)FindResource("LightGray");
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if all required fields are filled
            if (string.IsNullOrWhiteSpace(LocationTextBox.Text) ||
                string.IsNullOrWhiteSpace(TemperatureTextBox.Text) ||
                string.IsNullOrWhiteSpace(PressureTextBox.Text) ||
                string.IsNullOrWhiteSpace(WindSpeedTextBox.Text) ||
                WindDirectionComboBox.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(PrecipitationTextBox.Text) ||
                string.IsNullOrWhiteSpace(UVIndexTextBox.Text) ||
                string.IsNullOrWhiteSpace(HumidityTextBox.Text) ||
                DatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(TimeTextBox.Text))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            MessageBox.Show("Data saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
