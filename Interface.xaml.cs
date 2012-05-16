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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using Commands;

namespace RPGClient
{
    /// <summary>
    /// Interaction logic for Interface.xaml
    /// </summary>
    public partial class Interface : Window
    {
        private string host;
        private int port;
        private TcpClient client;
        private UTF8Encoding code;
        private Player player;
        private Character character;
        private Map map;
        private Skills skills;
        private long timeDifference;

        public Interface(ulong id, TcpClient userClient, long diff)
        {
            InitializeComponent();

            code = new UTF8Encoding();

            //MessageBox.Show(CurrentTime().ToString());
            timeDifference = diff;

            client = userClient;
            player = new Player(id, client);
            character = new Character(id, client);
            map = new Map(client);
            skills = new Skills(client);

            // WCZYTANIE MAPY
            foreach (Button btn in map.CityButtons)
            {
                //if (uint.Parse(btn.Tag.ToString()) == character.Location)
                //{
                //    btn.SendTemplate();
                //    TextBlock text = (TextBlock)btn.Template.FindName("city" + character.Location + "Name", btn);
                //    text.Foreground = Brushes.DarkOrange;
                //}
                btn.Click += new RoutedEventHandler(showCityForm);
                mapContainer.Children.Add(btn);
                //Canvas.SetLeft(btn, 50);
                //Canvas.SetTop(btn, 50);
            }

            mapContainer.ApplyTemplate();

            TextBlock text = CurrentLocationButtonText(character.Location);
            //ustawienie jego czcionki na pomarańczową
            text.Foreground = Brushes.DarkOrange;

            host = Properties.Settings.Default.Host;
            port = Properties.Settings.Default.Port;

            idText.Text = player.Login + " (ID " + player.Id.ToString() + ")";

            //wypełnie informacji o postaci
            characterNameTop.Text = character.Name;
            characterLvlTop.Text = character.Level.ToString();
            goldTop.Text = character.Gold.ToString();
            characterStrength.Text = character.Strength.ToString();
            characterStamina.Text = character.Stamina.ToString();
            characterDexterity.Text = character.Dexterity.ToString();
            characterLuck.Text = character.Luck.ToString();

            characterStrengthBonus.Text = "0";
            characterStaminaBonus.Text = "0";
            characterDexterityBonus.Text = "0";
            characterLuckBonus.Text = "0";

            //zmiana właściwości buttonów mapy w zależności o lokalizacji postaci
            ChangeLocationButtonsState();

            //zainicjalizowanie pasków stanu HP, Kondycja
            HPStatus.Text = "HP: " + character.GetHP(CurrentTime()) + "/" + character.GetMaxHP();
            HPBar.Width = Convert.ToInt32(Math.Round(200 * (double)character.GetHP(CurrentTime()) / (double)character.GetMaxHP(), 0));

            ConditionStatus.Text = "Kon: " + character.GetStamina(CurrentTime()) + "/" + character.GetMaxStamina();
            ConBar.Width = Convert.ToInt32(Math.Round(200 * (double)character.GetStamina(CurrentTime()) / (double)character.GetMaxStamina(), 0));

            //czas serwera do celów testowych
            serverTimeStamp.Text = CurrentTime().ToString();

            switchIncreaseButtons();

            remainingPoints.Text = "Pozostałe ("+character.RemainingPoints()+"): ";

            headId.Text = character.Equipment.Head.ToString();

            DispatcherTimer barsUpdater = new DispatcherTimer();
            barsUpdater.Tick += new EventHandler(UpdateBars);
            barsUpdater.Interval = TimeSpan.FromSeconds(1);
            barsUpdater.Start();

            DispatcherTimer statusUpdater = new DispatcherTimer();
            statusUpdater.Tick += new EventHandler(UpdateStatus);
            statusUpdater.Interval = TimeSpan.FromSeconds(1);
            statusUpdater.Start();

            DispatcherTimer expLevelUpdater = new DispatcherTimer();
            expLevelUpdater.Tick += new EventHandler(UpdateExpLevel);
            expLevelUpdater.Interval = TimeSpan.FromMilliseconds(30);
            expLevelUpdater.Start();
        }

        public static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByName<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        //funkcja timera, uaktuaniająca kontrolki co sekundę
        private void UpdateBars(object sender, EventArgs e)
        {
            //zainicjalizowanie pasków stanu HP, Kondycja
            HPStatus.Text = "HP: " + character.GetHP(CurrentTime()) + "/" + character.GetMaxHP();
            HPBar.Width = Convert.ToInt32(Math.Floor(200 * (double)character.GetHP(CurrentTime()) / (double)character.GetMaxHP()));

            ConditionStatus.Text = "Kon: " + character.GetStamina(CurrentTime()) + "/" + character.GetMaxStamina();
            ConBar.Width = Convert.ToInt32(Math.Round(200 * (double)character.GetStamina(CurrentTime()) / (double)character.GetMaxStamina(), 0));

            serverTimeStamp.Text = CurrentTime().ToString();
            ChangeLocationButtonsState();
        }

        private void UpdateExpLevel(object sender, EventArgs e)
        {
            if (character.Experience >= character.ExpToNextLevel())
            {
                character.Experience -= character.ExpToNextLevel();
                character.Level++;

                characterLvlTop.Text = character.Level.ToString();

                switchIncreaseButtons();
                remainingPoints.Text = "Pozostałe (" + character.RemainingPoints() + "): ";
            }

            ExpStatus.Text = "Exp: " + character.Experience + "/" + character.ExpToNextLevel();
            ExpBar.Width = Convert.ToInt32(Math.Round(300 * (double)character.Experience / (double)character.ExpToNextLevel(), 0));
        }

        private void UpdateStatus(object sender, EventArgs e)
        {
            characterStatusInfo.Text = StatusUpdate();
        }

        private void ChangeLocationButtonsState()
        {
            //city1Name.Foreground = Brushes.White;
            //city1Name.Foreground = Brushes.White;

            //city1spot1.Visibility = city1spot2.Visibility = city1spot3.Visibility = Visibility.Hidden;

            //switch (character.Location)
            //{
            //    case 1:
            //        city1Name.Foreground = Brushes.DarkOrange;
            //        city1spot1.Visibility = city1spot2.Visibility = city1spot3.Visibility = Visibility.Visible;
            //        break;
            //    case 2:
            //        city2Name.Foreground = Brushes.DarkOrange;
            //        break;
            //    default:
            //        break;
            //}
        }

        private TextBlock CurrentLocationButtonText(uint locationId)
        {
            //wyciągnięcie buttona z mapContainer o nazwie cityIDENTYFIKATOR
            Button button = FindVisualChildByName<Button>(mapContainer, "city" + locationId);
            //pobranie stackpanela z jego contentu
            StackPanel panel = (StackPanel)button.Content;
            //wyszukanie w stackpanelu textblock z nazwą miasta
            TextBlock text = FindVisualChildByName<TextBlock>(panel, "city" + locationId + "Name");
            //ustawienie jego czcionki na pomarańczową
            return text;
        }

        //okno miasta - wyświetlnie okienka dialogowego z detalami i opcjami miasta
        private void showCityForm(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            //szuaknie w liście miasta o id wysłanym z sendera
            IEnumerable<City> cities = 
                from cityFromList in map.CityData
                where cityFromList.Id == uint.Parse(btn.Tag.ToString())
                select cityFromList;
            foreach (City city in cities)
            {
                Location cityForm = new Location(city, character.Location, client, character, timeDifference);
                cityForm.ShowDialog();
            }
        }

        //pobiernia miasta o zadanym identyfikatorze
        private City getCityById(uint id)
        {
            IEnumerable<City> cities =
                from cityFromList in map.CityData
                where cityFromList.Id == id
                select cityFromList;
            foreach (City city in cities)
            {
                return city;
            }
            return null;
        }

        //zamiana stałej statusu na informację o statusie
        private string StatusUpdate()
        {
            switch (character.Status)
            {
                case CharacterStatus.IN_STANDBY:
                    return "Twoja postać jest gotowa do działania.";
                case CharacterStatus.IS_TRAVELING:
                    if (character.TravelEndTime <= Convert.ToUInt64(CurrentTime()))
                    {
                        TextBlock text = CurrentLocationButtonText(character.Location);
                        //ustawienie jego czcionki na pomarańczową
                        text.Foreground = Brushes.White;
                        text = CurrentLocationButtonText(character.TravelDestination);
                        text.Foreground = Brushes.DarkOrange;

                        character.Status = CharacterStatus.IN_STANDBY;
                        character.TravelEndTime = 0;
                        character.Location = character.TravelDestination;
                        character.TravelDestination = 0;

                        return "Twoja postać jest gotowa do działania.";
                    }
                    return "Twoja postać podróżuje z " + getCityById(character.Location).Name + " do "+getCityById(character.TravelDestination).Name +"\nNa miejsce dotrze za s. " + (character.TravelEndTime - Convert.ToUInt64(CurrentTime()));
                default:
                    return null;
            }
        }

        //obliczenie aktualnego czasu zsynchronizowanego z serwerem
        private long CurrentTime()
        {
            //Find unix timestamp (seconds since 01/01/1970)
            long ticks = DateTime.UtcNow.Ticks + timeDifference - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000; //Convert windows ticks to seconds
            return ticks;
        }

        //wylogowanie
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            client.Client.Close();
            new MainWindow().Show();
            this.Close();
        }

        //zwiększenie umiejętności
        private void IncreaseFeature(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            string feature = clickedBtn.Tag.ToString();

            switch (feature)
            {
                case "strength":
                    character.Strength++;
                    characterStrength.Text = character.Strength.ToString();
                    break;
                case "stamina":
                    character.Stamina++;
                    characterStamina.Text = character.Stamina.ToString();
                    break;
                case "dexterity":
                    character.Dexterity++;
                    characterDexterity.Text = character.Dexterity.ToString();
                    break;
                case "luck":
                    character.Luck++;
                    characterLuck.Text = character.Luck.ToString();
                    break;
                default:
                    break;
            }
            remainingPoints.Text = "Pozostałe (" + character.RemainingPoints() + "): ";
            switchIncreaseButtons();
        }

        private void switchIncreaseButtons()
        {
            if (character.RemainingPoints() < 1)
            {
                incDex.Visibility = incLuck.Visibility = incStam.Visibility = incStr.Visibility = Visibility.Hidden;
            }
            else
            {
                incDex.Visibility = incLuck.Visibility = incStam.Visibility = incStr.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            character.Experience += 50;
        }

        private void city1spot2_Click(object sender, RoutedEventArgs e)
        {
            Enemies en = new Enemies(1, client);
            foreach (Mob mb in en.EnemiesList)
            {
                MessageBox.Show(mb.Id.ToString() + " " + mb.Name + " " + mb.Level.ToString() + " " + mb.BonusHP + " " + mb.Strength.ToString() + " " + mb.Luck.ToString() + " " + mb.Dexterity.ToString() + " " + mb.Stamina.ToString() + " " + mb.GoldDrop.ToString());
            }
        }

    }
}
