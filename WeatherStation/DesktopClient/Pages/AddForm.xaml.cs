using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Media;
using SharedLibrary;
using System;
using System.Linq;

namespace DesktopClient.Pages
{
    public partial class AddForm : Page
    {
        public AddForm()
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

            string location = LocationTextBox.Text;
            double temperature = Double.Parse(TemperatureTextBox.Text);
            double pressure = Double.Parse(PressureTextBox.Text);
            double windSpeed = Double.Parse(WindSpeedTextBox.Text);
            string selectedDirection = ((ComboBoxItem)WindDirectionComboBox.SelectedItem).Content.ToString();
            Enum.TryParse(selectedDirection, out WindDirection windDirection);
            double precipitation = Double.Parse(PrecipitationTextBox.Text);
            double uvIndex = Double.Parse(UVIndexTextBox.Text);
            double humidity = Double.Parse (HumidityTextBox.Text);


            DateTime selectedDate = DatePicker.SelectedDate.Value;
            string time = TimeTextBox.Text;
            string timestampStr = $"{selectedDate.ToShortDateString()} {time}";
            DateTime timestamp = DateTime.Parse(timestampStr);


            CurrentWeatherData newWeatherData = new CurrentWeatherData(location, timestamp, temperature, pressure, windSpeed, windDirection, precipitation, uvIndex, humidity);
            if(ServiceManager.AddNewData(newWeatherData))
            {
                MessageBox.Show("Data saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DefaultPage defaultPage = new DefaultPage();
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = defaultPage;
            }
            else
                MessageBox.Show("Error while adding new data. Try again.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
