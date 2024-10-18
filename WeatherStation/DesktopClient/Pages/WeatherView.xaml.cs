using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopClient.Pages
{
    public partial class WeatherView : Page
    {
        public WeatherView()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string location = SearchLocationTextBox.Text;
            DateTime selectedDate = SearchDatePicker.SelectedDate ?? DateTime.Now;
            string time = SearchTimeTextBox.Text;

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(time))
            {
                MessageBox.Show("All fields are required for search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SearchResultTextBlock.Text = $"Weather for {location} on {selectedDate.ToShortDateString()} at {time}";

            LocationTextBlock.Text = $"Location: {location}";
            TimestampTextBlock.Text = $"Timestamp: {selectedDate.ToShortDateString()} {time}";
            TemperatureTextBlock.Text = $"Temperature: 25°C";
            PressureTextBlock.Text = $"Pressure: 1013 hPa";
            WindSpeedTextBlock.Text = $"Wind Speed: 15 km/h";
            WindDirectionTextBlock.Text = $"Wind Direction: North";
            PrecipitationTextBlock.Text = $"Precipitation: 0 mm";
            UVIndexTextBlock.Text = $"UV Index: 5";
            HumidityTextBlock.Text = $"Humidity: 60%";
        }

        private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9:]");
            e.Handled = regex.IsMatch(e.Text);

            if (SearchTimeTextBox.Text.Length == 2 && SearchTimeTextBox.Text.IndexOf(":") == -1)
            {
                SearchTimeTextBox.Text += ":";
                SearchTimeTextBox.SelectionStart = SearchTimeTextBox.Text.Length;
            }
            else if (SearchTimeTextBox.Text.Length == 5 && SearchTimeTextBox.Text.IndexOf(":") == -1)
            {
                SearchTimeTextBox.Text += ":";
                SearchTimeTextBox.SelectionStart = SearchTimeTextBox.Text.Length;
            }
        }
    }
}

