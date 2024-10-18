using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// Interaction logic for ClearDaysView.xaml
    /// </summary>
    public partial class ClearDaysView : Page
    {
        public ClearDaysView()
        {
            InitializeComponent();
        }

        private void YearTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        private bool IsTextNumeric(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private int GetMonthNumber(string monthName)
        {
            switch (monthName)
            {
                case "January": return 1;
                case "February": return 2;
                case "March": return 3;
                case "April": return 4;
                case "May": return 5;
                case "June": return 6;
                case "July": return 7;
                case "August": return 8;
                case "September": return 9;
                case "October": return 10;
                case "November": return 11;
                case "December": return 12;
                default: throw new ArgumentException("Invalid month name");
            }
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchLocationTextBox.Text) ||
                SearchMonthComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(SearchYearTextBox.Text))
            {
                MessageBox.Show("All fields are required for search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string location = SearchLocationTextBox.Text;
            string selectedMonth = ((ComboBoxItem)SearchMonthComboBox.SelectedItem).Content.ToString();
            int selectedYear;

            if (!int.TryParse(SearchYearTextBox.Text, out selectedYear))
            {
                MessageBox.Show("Please enter a valid year.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime selectedDate = new DateTime(selectedYear, GetMonthNumber(selectedMonth), 1);


            StringBuilder result = new StringBuilder();
            result.AppendLine($"Number of clear days for {location} in {selectedMonth} {selectedYear} is 5.");

           

            SearchResultTextBlock.Text = result.ToString();
        }


    }
}
