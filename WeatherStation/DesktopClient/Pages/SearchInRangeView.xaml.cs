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

            List<CurrentWeatherData> weatherDataList = ServiceManager.GetDataInRange(startDateTime, endDateTime, location);
            if (weatherDataList == null)
                return;


            ResultsListView.ItemsSource = weatherDataList;
        }

        
    }

  
}
