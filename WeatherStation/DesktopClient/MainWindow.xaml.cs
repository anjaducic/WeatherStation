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
    using DesktopClient.Pages;

    namespace DesktopClient
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            AddForm addForm = new AddForm();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = addForm;
        }
        


        
        private void Weather_Click(object sender, RoutedEventArgs e)
        {
           WeatherView weatherView = new WeatherView();
           Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = weatherView;

        }

        private void Temperature_Click(object sender, RoutedEventArgs e)
        {
           TemperatureView temperatureView = new TemperatureView();
           Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = temperatureView;

        }

        private void Pressure_Click(object sender, RoutedEventArgs e)
        {
            PressureView pressureView = new PressureView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = pressureView;

        }

        private void WindSpeed_Click(object sender, RoutedEventArgs e)
        {
            WindSpeedView windSpeedView = new WindSpeedView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = windSpeedView;

        }

        private void WindDirection_Click(object sender, RoutedEventArgs e)
        {
            WindDirectionView windDirectionView = new WindDirectionView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = windDirectionView;

        }

        private void Precipitation_Click(object sender, RoutedEventArgs e)
        {
            PrecipitationView precipitationView = new PrecipitationView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = precipitationView;

        }

        private void Humidity_Click(object sender, RoutedEventArgs e)
        {
            HumidityView humidityView = new HumidityView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = humidityView;

        }

        private void UVIndex_Click(object sender, RoutedEventArgs e)
        {
            UVIndexView uVIndexView = new UVIndexView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = uVIndexView;

        }

        private void SearchInRange_Click(object sender, RoutedEventArgs e)
        {
            SearchInRangeView searchInRangeView = new SearchInRangeView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = searchInRangeView;

        }
        private void AverageTemperature_Click(object sender, RoutedEventArgs e)
        {
            AverageTemperatureView averageTemperatureView = new AverageTemperatureView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = averageTemperatureView;

        }

        private void MinMaxTemperature_Click(object sender, RoutedEventArgs e)
        {
            MinMaxTemperatureView minMaxTemperatureView = new MinMaxTemperatureView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = minMaxTemperatureView;

        }

        private void MinMaxPrecipitation_Click(object sender, RoutedEventArgs e)
        {
            MinMaxPrecipitationView minMaxPrecipitationView = new MinMaxPrecipitationView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = minMaxPrecipitationView;

        }

        private void AverageHumidity_Click(object sender, RoutedEventArgs e)
        {
            AverageHumidityView averageHumidityView = new AverageHumidityView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = averageHumidityView;

        }

        private void ExtremeUVIndex_Click(object sender, RoutedEventArgs e)
        {
            ExtremeUVIndexView extremeUVIndexView = new ExtremeUVIndexView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = extremeUVIndexView;

        }
        private void ClearDays_Click(object sender, RoutedEventArgs e)
        {
            ClearDaysView clearDaysView = new ClearDaysView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = clearDaysView;
        }

        private void RainyDays_Click(object sender, RoutedEventArgs e)
        {
            RainyDaysView rainyDaysView = new RainyDaysView();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = rainyDaysView;

        }
    }
}

