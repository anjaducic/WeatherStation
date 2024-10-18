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
    /// Interaction logic for ExtremeUVIndexView.xaml
    /// </summary>
    public partial class ExtremeUVIndexView : Page
    {
        public ExtremeUVIndexView()
        {
            InitializeComponent();
        }

        private List<int> GetHours(string location, DateTime date)
        {
            Random rand = new Random();
            List<int> hours = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                hours.Add(rand.Next(5, 31));  
            }

            return hours;
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

            List<int> hours = GetHours(location, selectedDate);

            StringBuilder result = new StringBuilder();
            result.AppendLine($"Extreme UV for {location} on {selectedDate.ToShortDateString()} at :");

            for (int i = 0; i < hours.Count; i++)
            {
                result.Append($"{hours[i]}h; ");
            }

            SearchResultTextBlock.Text = result.ToString();
        }


    }
}
