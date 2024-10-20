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
using SharedLibrary;

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
            string timestampStr = $"{selectedDate.ToShortDateString()} {time}";
            DateTime timestamp = DateTime.Parse(timestampStr);


            CurrentWeatherData data = ServiceManager.GetOneData(location, timestamp);
            if(data == null)
            {
                return;
            }

            SearchResultTextBlock.Text = $"Weather for {location} on {selectedDate.ToShortDateString()} at {data.Timestamp.Hour}h";

            LocationTextBlock.Text = $"Location: {data.Location}";
            TemperatureTextBlock.Text = $"Temperature: {data.Temperature}°C";
            PressureTextBlock.Text = $"Pressure: {data.Pressure}hPa";
            WindSpeedTextBlock.Text = $"Wind Speed: {data.WindSpeed}km/h";
            WindDirectionTextBlock.Text = $"Wind Direction: {data.WindDirection}";
            PrecipitationTextBlock.Text = $"Precipitation: {data.Precipitation}mm";
            UVIndexTextBlock.Text = $"UV Index: {data.UVIndex}";
            HumidityTextBlock.Text = $"Humidity: {data.Humidity}%";
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

