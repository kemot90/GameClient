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
using System.Net.Sockets;
using Commands;

namespace RPGClient
{
    /// <summary>
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : Window
    {
        private City city;
        private uint currentLocation;
        private Socket socket;
        private Character character;
        private long timeDifference;
        private uint _travelTime;


        public Location(City _city, uint location, TcpClient client, Character _character, long _timeDifference)
        {
            InitializeComponent();

            city = _city;
            character = _character;

            timeDifference = _timeDifference;
            idTag.Text = city.Id.ToString();
            idCityName.Text = city.Name;
            currentLocation = location;
            socket = client.Client;
            _travelTime = GetShortestPath();
            travelTime.Text = _travelTime.ToString();
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

        private uint GetShortestPath()
        {
            string[] result;
            Command request = new Command();
            request.Request(ClientCmd.GET_SHORTEST_PATH);
            request.Add(currentLocation.ToString());
            request.Add(city.Id.ToString());
            result = request.Send(socket, true);
            return uint.Parse(result[1]);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnTravel_Click(object sender, RoutedEventArgs e)
        {
            //BtnTravel.IsEnabled = false;
            Travel();
            Close();
        }

        //obliczenie aktualnego czasu zsynchronizowanego z serwerem
        private long CurrentTime()
        {
            //Find unix timestamp (seconds since 01/01/1970)
            long ticks = DateTime.UtcNow.Ticks + timeDifference - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000; //Convert windows ticks to seconds
            return ticks;
        }

        private void Travel()
        {
            character.Status = "IS_TRAVELING";
            character.TravelEndTime = Convert.ToUInt32(CurrentTime()) + _travelTime;
            character.TravelDestination = city.Id;
        }
    }
}
