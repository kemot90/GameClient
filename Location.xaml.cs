using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RPGClient
{
    /// <summary>
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : Window
    {
        private City city;

        public Location(City _city)
        {
            InitializeComponent();

            city = _city;

            idTag.Text = city.Id.ToString();
            idCityName.Text = city.Name;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public City CityData
        {
            get
            {
                return city;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
