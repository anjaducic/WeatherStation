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
    public partial class MinMaxTemperatureView : Page
    {
        public MinMaxTemperatureView()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchLocationTextBox.Text) ||
                SearchDatePicker.SelectedDate == null)
            {
                MessageBox.Show("All fields are required for search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string location = SearchLocationTextBox.Text;
            DateTime selectedDate = DateTime.Parse(SearchDatePicker.SelectedDate.Value.ToShortDateString());

            (double min, double max)? minMax = ServiceManager.GetMinMaxTemperature(location, selectedDate);

            if (minMax == null)
                return;

            string result = $"Min temperature for {location} on {selectedDate.ToShortDateString()} is {minMax.Value.min}°C.\n" +
                            $"Max temperature for {location} on {selectedDate.ToShortDateString()} is {minMax.Value.max}°C.";

            SearchResultTextBlock.Text = result;
        }


    }
}
