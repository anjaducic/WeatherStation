﻿using System;
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
    public partial class AverageHumidityView : Page
    {
        public AverageHumidityView()
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
            DateTime selectedDate = SearchDatePicker.SelectedDate.Value;

            string dateTimeString = $"{selectedDate.ToShortDateString()}";
            string result = $"Average humidity for {location} on {dateTimeString} is 12%.";

            SearchResultTextBlock.Text = result;
        }

        
    }
}
