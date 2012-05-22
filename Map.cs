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

    public class Spot
    {
        private uint id_loc;
        private uint id_city;
        private char type;
        private string name;
        private uint leftCoordinate;
        private uint topCoordinate;

        public Spot(uint _id_loc, uint _id_city, char _type, string _name, uint _leftCoordinate, uint _topCoordinate)
        {
            id_loc = _id_loc;
            id_city = _id_city;
            type = _type;
            name = _name;
            leftCoordinate = _leftCoordinate;
            topCoordinate = _topCoordinate;
        }
        public uint IdLoc
        {
            get
            {
                return id_loc;
            }
        }
        public uint IdCity
        {
            get
            {
                return id_city;
            }
        }
        public char Type
        {
            get
            {
                return type;
            }
        }
        public string Name
        {
            get
            {
                return name;
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
    }

    public class Map
    {
        private List<Button> cityButtons = new List<Button>();
        private List<Button> spotButtons = new List<Button>();
        private List<City> cityData = new List<City>();
        private List<Spot> spotData = new List<Spot>();
        private Socket socket;

        public Map(TcpClient clientSocket)
        {
            socket = clientSocket.Client;

            uint citiesNumber = 0;
            uint spotsNumber = 0;
            string[] citiesData, spotsData;
            Command request = new Command();
            request.Request(ClientCmd.GET_CITIES);
            citiesData = request.Send(socket, true);
            request.Request(ClientCmd.GET_SPOTS);
            spotsData = request.Send(socket, true);

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

            //obliczenie liczby spotów
            spotsNumber = uint.Parse(spotsData[1]);
            for (int i = 2; i < spotsData.Length; i += 6)
            {
                Spot spot = new Spot(
                    uint.Parse(spotsData[i]),       //id lokacji
                    uint.Parse(spotsData[i + 1]),   //id miasta do którego przynależy
                    char.Parse(spotsData[i + 2]),   //typ lookacji
                    spotsData[i + 3],               //name
                    uint.Parse(spotsData[i + 4]),   //leftCoordinate
                    uint.Parse(spotsData[i + 5])    //topCoordinate
                    );
                spotData.Add(spot);
            }

            //tworzenie buttonów miast na podstawie danych z bazki
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

            //tworzenie buttonów spotów na podstawie danych z bazki
            foreach (Spot spot in spotData)
            {
                //najpierw sam button
                Button btn = new Button();
                //obrazek, który będzie jego zawartością
                Image btnImage = new Image();
                //i jego źródło
                btnImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/spot_" + spot.Type + ".png"));
                btnImage.Width = 30;
                btnImage.Height = 30;
                //dodanie obrazka do contentu
                btn.Content = btnImage;
                //nazwanie buttona spotu
                btn.Name = "city" + spot.IdCity + "spot" + spot.IdLoc;
                //jego szerokość na 30 px
                btn.Width = 30;
                //cały content wyśrodkowany
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                //dodanie tagu ze spotem
                btn.Tag = spot;
                //ukrycie buttona
                btn.Visibility = Visibility.Hidden;
                //określenie położenia na mapie
                Canvas.SetLeft(btn, spot.LeftCoordinate);
                Canvas.SetTop(btn, spot.TopCoordinate);
                //dodanie do listy buttonów mapy
                spotButtons.Add(btn);
            }
        }

        //włączanie/wyłączanie buttonów miast/okolic dla currentLocation
        public void SetMapButtons(bool value, uint currentLocation)
        {
            foreach (Button btn in CityButtons)
            {
                btn.IsEnabled = value;
            }
            foreach (Button btn in SpotButtons)
            {
                Spot spot = btn.Tag as Spot;
                if (value)
                {
                    if (spot.IdCity == currentLocation)
                    {
                        btn.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    btn.Visibility = Visibility.Hidden;
                }
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

        public List<Button> SpotButtons
        {
            get
            {
                return spotButtons;
            }
        }

        public List<Spot> SpotData
        {
            get
            {
                return spotData;
            }
        }
    }
}
