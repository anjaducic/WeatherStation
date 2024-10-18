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
    public partial class UVIndexView : Page
    {
        public UVIndexView()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchLocationTextBox.Text) ||
                SearchDatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(SearchTimeTextBox.Text))
            {
                MessageBox.Show("All fields are required for search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string location = SearchLocationTextBox.Text;
            DateTime selectedDate = SearchDatePicker.SelectedDate.Value;
            string time = SearchTimeTextBox.Text;

            string dateTimeString = $"{selectedDate.ToShortDateString()} {time}";
            string result = $"UV index for {location} on {dateTimeString} is 22.";

            SearchResultTextBlock.Text = result;
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

