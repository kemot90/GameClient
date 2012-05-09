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
    public class City
    {
        private uint id;
        private string name;
        private uint accessLevel;
        private uint leftCoordinate;
        private uint topCoordinate;
        private string icon;

        public City(uint _id, string _name, uint _accessLevel, uint _leftCoordinate, uint _topCoordinate, string _icon)
        {
            id = _id;
            name = _name;
            accessLevel = _accessLevel;
            leftCoordinate = _leftCoordinate;
            topCoordinate = _topCoordinate;
            icon = _icon;
        }
        public uint Id
        {
            get
            {
                return id;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public uint AccessLevel
        {
            get
            {
                return accessLevel;
            }
        }
        public uint LeftCoordinate
        {
            get
            {
                return leftCoordinate;
            }
        }
        public uint TopCoordinate
        {
            get
            {
                return topCoordinate;
            }
        }
        public string Icon
        {
            get
            {
                return icon;
            }
        }
    }

    public class Map
    {
        private List<Button> cityButtons = new List<Button>();
        private List<City> cityData = new List<City>();
        private Socket socket;

        public Map(TcpClient clientSocket)
        {
            socket = clientSocket.Client;

            uint citiesNumber = 0;
            string[] citiesData;
            Command request = new Command();
            request.Request(ClientCmd.GET_CITIES);
            citiesData = request.Apply(socket, true);

            //obliczenie liczby miast
            citiesNumber = uint.Parse(citiesData[1]);
            for (int i = 2; i < citiesData.Length; i += 6)
            {
                City city = new City(
                    uint.Parse(citiesData[i]),      //id
                    citiesData[i + 1],              //name
                    uint.Parse(citiesData[i + 2]),  //accessLevel
                    uint.Parse(citiesData[i + 3]),  //leftCoordinate
                    uint.Parse(citiesData[i + 4]),  //topCoordinate
                    citiesData[i + 5]               //icon
                    );
                cityData.Add(city);
            }
            //tworzenie buttonów na podstawie danych z bazki
            foreach (City city in cityData)
            {
                //najpierw sam button
                Button btn = new Button();
                //potem stackpanel przechowujący jego zawartość
                StackPanel btnContent = new StackPanel();
                //obrazek, który będzie jego zawartością
                Image btnImage = new Image();
                //i jego źródło
                btnImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + city.Icon + ".png"));
                btnImage.Width = 44;
                btnImage.Height = 60;
                //oraz textblock, który będzie wyświetlał nazwę miasta
                TextBlock btnText = new TextBlock();
                //przypisanie do texblocka nazwy miasta
                btnText.Text = city.Name;
                //szerokość stała na 50 px
                btnText.Width = 50;
                //nazwa kontrolki aby można było dynamicznie zmieniać kolor tekstu w buttonie
                btnText.Name = "city" + city.Id + "Name";
                //wyśrodkowanie tekstu
                btnText.TextAlignment = TextAlignment.Center;
                //ustawienie stackpanelu na 40 szerokości i 75 wysokości (obrazek wtedy sam się zeskaluje)
                btnContent.Width = 50;
                btnContent.Height = 75;

                btnContent.Margin = new Thickness(3, 3, 3, 3);
                //wyśrodkowanie zawartości w pionie do środka
                btnContent.VerticalAlignment = VerticalAlignment.Center;
                //dodanie obrazka i texblocka z nazwą miasta do stackpanelu
                btnContent.Children.Add(btnImage);
                btnContent.Children.Add(btnText);
                //nazwanie buttona miasta
                btn.Name = "city" + city.Id;
                //jego szerokość na 55 px
                btn.Width = 55;
                //cały content wyśrodkowany
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                //przypisanie stackpanelu jako zawartości buttona
                btn.Content = btnContent;
                //dodanie tagu z identyfikatorem miasta
                btn.Tag = city.Id;
                //określenie położenia na mapie
                Canvas.SetLeft(btn, city.LeftCoordinate);
                Canvas.SetTop(btn, city.TopCoordinate);
                //dodanie do listy buttonów mapy
                cityButtons.Add(btn);
            }
        }

        public List<Button> CityButtons
        {
            get
            {
                return cityButtons;
            }
        }

        public List<City> CityData
        {
            get
            {
                return cityData;
            }
        }
    }
}
