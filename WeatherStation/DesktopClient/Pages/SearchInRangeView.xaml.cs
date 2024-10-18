using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SharedLibrary;

namespace DesktopClient.Pages
{
    public partial class SearchInRangeView : Page
    {
        public SearchInRangeView()
        {
            InitializeComponent();
        }

        private void StartTimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9:]");
            e.Handled = regex.IsMatch(e.Text);

            if (SearchStartTimeTextBox.Text.Length == 2 && SearchStartTimeTextBox.Text.IndexOf(":") == -1)
            {
                SearchStartTimeTextBox.Text += ":";
                SearchStartTimeTextBox.SelectionStart = SearchStartTimeTextBox.Text.Length;
            }
            else if (SearchStartTimeTextBox.Text.Length == 5 && SearchStartTimeTextBox.Text.IndexOf(":") == -1)
            {
                SearchStartTimeTextBox.Text += ":";
                SearchStartTimeTextBox.SelectionStart = SearchStartTimeTextBox.Text.Length;
            }
        }

        private void EndTimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9:]");
            e.Handled = regex.IsMatch(e.Text);

            if (SearchEndTimeTextBox.Text.Length == 2 && SearchEndTimeTextBox.Text.IndexOf(":") == -1)
            {
                SearchEndTimeTextBox.Text += ":";
                SearchEndTimeTextBox.SelectionStart = SearchEndTimeTextBox.Text.Length;
            }
            else if (SearchEndTimeTextBox.Text.Length == 5 && SearchEndTimeTextBox.Text.IndexOf(":") == -1)
            {
                SearchEndTimeTextBox.Text += ":";
                SearchEndTimeTextBox.SelectionStart = SearchEndTimeTextBox.Text.Length;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string location = SearchLocationTextBox.Text;
            DateTime startDate = SearchStartDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = SearchEndDatePicker.SelectedDate ?? DateTime.Now;
            string startTime = SearchStartTimeTextBox.Text;
            string endTime = SearchEndTimeTextBox.Text;

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                MessageBox.Show("All fields are required for search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            DateTime startDateTime = DateTime.Parse(startDate.ToShortDateString() + " " + startTime);
            DateTime endDateTime = DateTime.Parse(endDate.ToShortDateString() + " " + endTime);

            if(startDateTime > endDateTime)
            {
                MessageBox.Show("Invalid date time.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<CurrentWeatherData> weatherDataList = GetWeatherData(location, startDateTime, endDateTime);

            ResultsListView.ItemsSource = weatherDataList;
        }

        private List<CurrentWeatherData> GetWeatherData(string location, DateTime startDateTime, DateTime endDateTime)
        {
            List<CurrentWeatherData> dataList = new List<CurrentWeatherData>
            {
                new CurrentWeatherData("Ruma", DateTime.Now.Date.AddHours(8), 15.4, 1012, 12.5, WindDirection.East, 0.0, 5.2, 85),
                new CurrentWeatherData("Ruma", DateTime.Now.Date.AddHours(18), 17.8, 1010, 8.3, WindDirection.SouthEast, 2.3, 4.1, 78),

                new CurrentWeatherData("Novi Sad", DateTime.Now.Date.AddDays(1).AddHours(9), 14.2, 1013, 10.7, WindDirection.NorthEast, 0.0, 6.1, 88),
                new CurrentWeatherData("Novi Sad", DateTime.Now.Date.AddDays(1).AddHours(20), 16.9, 1010, 7.1, WindDirection.West, 1.5, 3.3, 76),

                new CurrentWeatherData("Sremski Karlovci", DateTime.Now.Date.AddDays(2).AddHours(6), 13.3, 1015, 5.0, WindDirection.SouthWest, 0.0, 3.2, 90),
                new CurrentWeatherData("Sremski Karlovci", DateTime.Now.Date.AddDays(2).AddHours(17), 19.1, 1011, 10.3, WindDirection.North, 0.0, 7.6, 79),

                new CurrentWeatherData("Petrovaradin", DateTime.Now.Date.AddDays(3).AddHours(7), 16.5, 1014, 11.4, WindDirection.East, 0.1, 4.9, 81),
                new CurrentWeatherData("Petrovaradin", DateTime.Now.Date.AddDays(3).AddHours(19), 18.2, 1010, 9.3, WindDirection.South, 0.3, 5.4, 77),

                new CurrentWeatherData("Backa Topola", DateTime.Now.Date.AddDays(4).AddHours(8), 12.7, 1012, 7.6, WindDirection.NorthWest, 0.0, 2.1, 84),
                new CurrentWeatherData("Backa Topola", DateTime.Now.Date.AddDays(4).AddHours(17), 20.4, 1009, 6.8, WindDirection.West, 0.0, 8.4, 70)
            };

            return dataList;
        }
    }

  
}
