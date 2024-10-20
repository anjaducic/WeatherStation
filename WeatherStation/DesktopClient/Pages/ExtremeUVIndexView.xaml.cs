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
    public partial class ExtremeUVIndexView : Page
    {
        public ExtremeUVIndexView()
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

            List<int> hours = ServiceManager.GetExtremeUVIndexHours(location, selectedDate);
            if (hours == null)
                return;


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
