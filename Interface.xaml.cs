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
        private TcpClient client;
        private TcpClient test; //do sprawdzenia czy da się połączyć z serwerem
        private Player player;
        private Character character;
        private Map map;
        private Skills skills;
        private ItemsArmor armors;
        private ItemsWeapon weapons;
        private long timeDifference;
        private DispatcherTimer barsUpdater;
        private DispatcherTimer statusUpdater;
        private DispatcherTimer expLevelUpdater;
        private DispatcherTimer checkServerAvail;

        public Interface(ulong id, TcpClient userClient, long diff)
        {
            InitializeComponent();

            #region Inicjalizacja obiektów i pól klasy

            timeDifference = diff;
            client = userClient;

            player = new Player(id, client);
            character = new Character(id, client);
            map = new Map(client);
            skills = new Skills(client);
            armors = new ItemsArmor(client);
            weapons = new ItemsWeapon(client);

            #endregion

            #region Wczytanie mapy
            // WCZYTANIE MAPY
            foreach (Button btn in map.CityButtons)
            {
                btn.Click += new RoutedEventHandler(showCityForm);
                mapContainer.Children.Add(btn);
            }

            // okolice
            foreach (Button btn in map.SpotButtons)
            {
                btn.Click += new RoutedEventHandler(showSpotForm);
                mapContainer.Children.Add(btn);
            }

            // ustawienie buttonów lokacji
            map.SetMapButtons(true, character.Location);

            mapContainer.ApplyTemplate();

            TextBlock text = CurrentLocationButtonText(character.Location);
            //ustawienie jego czcionki na pomarańczową
            text.Foreground = Brushes.DarkOrange;
            text.FontWeight = FontWeights.Bold;

            #endregion

            #region Wczytanie informacji o graczu, postaci i paskach stanu

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

            //zainicjalizowanie pasków stanu HP, Kondycja
            HPStatus.Text = "HP: " + character.GetHP(CurrentTime()) + "/" + character.GetMaxHP();
            HPBar.Width = Convert.ToInt32(Math.Round(200 * (double)character.GetHP(CurrentTime()) / (double)character.GetMaxHP(), 0));

            ConditionStatus.Text = "Kon: " + character.GetStamina(CurrentTime()) + "/" + character.GetMaxStamina();
            ConBar.Width = Convert.ToInt32(Math.Round(200 * (double)character.GetStamina(CurrentTime()) / (double)character.GetMaxStamina(), 0));

            //czas serwera do celów testowych
            serverTimeStamp.Text = CurrentTime().ToString();

            //ustawienie przycisków zwiększania cech na odpowiedni stan
            switchIncreaseButtons();

            //pozostałe punkty do rozdania
            remainingPoints.Text = "Pozostałe ("+character.RemainingPoints()+"): ";

            #endregion

            #region Wczytanie ekwipunku i ustawienie interfceu 

            RefreshItems();

            #endregion

            #region Inicjalizacja i ustawienie DispatcherTimerów

            barsUpdater = new DispatcherTimer();
            barsUpdater.Tick += new EventHandler(UpdateBars);
            barsUpdater.Interval = TimeSpan.FromSeconds(1);
            barsUpdater.Start();

            statusUpdater = new DispatcherTimer();
            statusUpdater.Tick += new EventHandler(UpdateStatus);
            statusUpdater.Interval = TimeSpan.FromSeconds(1);
            statusUpdater.Start();

            expLevelUpdater = new DispatcherTimer();
            expLevelUpdater.Tick += new EventHandler(UpdateExpLevel);
            expLevelUpdater.Interval = TimeSpan.FromMilliseconds(30);
            expLevelUpdater.Start();

            checkServerAvail = new DispatcherTimer();
            checkServerAvail.Tick += new EventHandler(checkServerAvailability);
            checkServerAvail.Interval = TimeSpan.FromMilliseconds(100);
            checkServerAvail.Start();

            #endregion
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

        #region Metody wykonywane przez DispatcherTimery

        //funkcja timera, uaktuaniająca kontrolki co sekundę
        private void UpdateBars(object sender, EventArgs e)
        {
            //zainicjalizowanie pasków stanu HP, Kondycja
            HPStatus.Text = "HP: " + character.GetHP(CurrentTime()) + "/" + character.GetMaxHP();
            HPBar.Width = Convert.ToInt32(Math.Floor(200 * (double)character.GetHP(CurrentTime()) / (double)character.GetMaxHP()));

            ConditionStatus.Text = "Kon: " + character.GetStamina(CurrentTime()) + "/" + character.GetMaxStamina();
            ConBar.Width = Convert.ToInt32(Math.Round(200 * (double)character.GetStamina(CurrentTime()) / (double)character.GetMaxStamina(), 0));

            serverTimeStamp.Text = CurrentTime().ToString();
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


        private void checkServerAvailability(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                test = new TcpClient(endPoint.Address.ToString(), endPoint.Port);
            }
            catch
            {
                statusUpdater.IsEnabled = false;
                barsUpdater.IsEnabled = false;
                expLevelUpdater.IsEnabled = false;
                checkServerAvail.IsEnabled = false;
                client.Client.Close();
                new MainWindow().Show();
                this.Close();
            }
        }
        #endregion

        #region Metody i funkcje obsługi mapy
        //zwrócenie TextBlocku z Contentu Buttona lokacji o zadanym Id
        private TextBlock CurrentLocationButtonText(uint locationId)
        {
            try
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
            catch
            {
                return new TextBlock();
            }
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
                Location cityForm = new Location(city, character.Location, client, character, timeDifference, map);
                cityForm.ShowDialog();
            }
        }

        //okno spotu - wyświetlnie okienka dialogowego z detalami i opcjami spotu
        private void showSpotForm(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Spot spot = btn.Tag as Spot;
            Enemies en = new Enemies(spot.IdLoc, client);
            Place spotForm = new Place(spot, en);
            Point mousePoint = Mouse.GetPosition(this);
            spotForm.Left = mousePoint.X;
            spotForm.Top = mousePoint.Y - spotForm.Height;
            spotForm.ShowDialog();
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

        //pobieranie spotu o zadanym identyfikatorze
        private Spot getSpotById(uint id)
        {
            IEnumerable<Spot> spots =
                from spotFromList in map.SpotData
                where spotFromList.IdLoc == id
                select spotFromList;
            foreach (Spot spot in spots)
            {
                return spot;
            }
            return null;
        }
        #endregion

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
                        text.FontWeight = FontWeights.Normal;
                        text = CurrentLocationButtonText(character.TravelDestination);
                        text.Foreground = Brushes.DarkOrange;
                        text.FontWeight = FontWeights.Bold;

                        character.Status = CharacterStatus.IN_STANDBY;
                        character.TravelEndTime = 0;
                        character.Location = character.TravelDestination;
                        character.TravelDestination = 0;

                        map.SetMapButtons(true, character.Location);

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
            statusUpdater.IsEnabled = false;
            barsUpdater.IsEnabled = false;
            expLevelUpdater.IsEnabled = false;
            checkServerAvail.IsEnabled = false;

            //Thread.Sleep(1000);

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

        #region Obsługa broni
        ///////////// METODY EKWIPUNKU /////////////////////////


        private void RefreshItems()
        {
            RefreshArmor();
            RefreshWeapon();
            CreateItemsList(character.Storage.StorageList);
        }

        // metoda odswieżenia broni
        private void RefreshWeapon()
        {
            if (character.Equipment.Weapon == 0)
            {
                weaponName.Text = "Brak broni";
                weaponAttack.Text = "";
                weaponDexterity.Text = "";
                weaponLuck.Text = "";
                weaponStamina.Text = "";
                weaponStrength.Text = "";
                weaponSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/weaponDisabled.png"));
                weaponTakeoff.Tag = 0;
            }
            else
            {
                Weapon weapon = weapons.GetWeaponById(character.Equipment.Weapon);

                weaponName.Text = weapon.Name.ToString();
                weaponAttack.Text = weapon.Min_attack.ToString() + "-" + weapon.Max_attack.ToString();
                weaponDexterity.Text = "+ " + weapon.Dexterity.ToString();
                weaponLuck.Text = "+ " + weapon.Luck.ToString();
                weaponStamina.Text = "+ " + weapon.Stamina.ToString();
                weaponStrength.Text = "+ " + weapon.Strength.ToString();

                weaponSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/weaponEnabled.png"));
                weaponTakeoff.Tag = weapon.Id;
            }
        }


        // metoda odswieżania zbroi
        private void RefreshArmor()
        {
            if (character.Equipment.Chest < 1)
            {
                armorName.Text = "Brak napierśnika";
                armorArmor.Text = "";
                armorDexterity.Text = "";
                armorLuck.Text = "";
                armorStamina.Text = "";
                armorStrength.Text = "";

                chestSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/chestDisabled.png"));
                chestTakeoff.Tag = 0;
            }
            else
            {
                Armor armor = armors.GetArmorById(character.Equipment.Chest);

                armorName.Text = armor.Name.ToString();
                armorArmor.Text = armor.ArmorPoints.ToString();
                armorDexterity.Text = "+ " + armor.Dexterity.ToString();
                armorLuck.Text = "+ " + armor.Luck.ToString();
                armorStamina.Text = "+ " + armor.Stamina.ToString();
                armorStrength.Text = "+ " + armor.Strength.ToString();

                chestSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/chestEnabled.png"));
                chestTakeoff.Tag = armor.Id;
            }
            if (character.Equipment.Head < 1)
            {
                headName.Text = "Brak hełmu";
                headArmor.Text = "";
                headDexterity.Text = "";
                headLuck.Text = "";
                headStamina.Text = "";
                headStrength.Text = "";

                helmetSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/helmetDisabled.png"));
                helmetTakeoff.Tag = 0;
            }
            else
            {
                Armor armor = armors.GetArmorById(character.Equipment.Head);

                headName.Text = armor.Name.ToString();
                headArmor.Text = armor.ArmorPoints.ToString();
                headDexterity.Text = "+ " + armor.Dexterity.ToString();
                headLuck.Text = "+ " + armor.Luck.ToString();
                headStamina.Text = "+ " + armor.Stamina.ToString();
                headStrength.Text = "+ " + armor.Strength.ToString();

                helmetSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/helmetEnabled.png"));
                helmetTakeoff.Tag = armor.Id;
            }

            if (character.Equipment.Legs < 1)
            {
                legsName.Text = "Brak nagolenników";
                legsArmor.Text = "";
                legsDexterity.Text = "";
                legsLuck.Text = "";
                legsStamina.Text = "";
                legsStrength.Text = "";

                bootsSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/bootsDisabled.png"));
                bootsTakeoff.Tag = 0;
            }
            else
            {
                Armor armor = armors.GetArmorById(character.Equipment.Legs);

                legsName.Text = armor.Name.ToString();
                legsArmor.Text = armor.ArmorPoints.ToString();
                legsDexterity.Text = "+ " + armor.Dexterity.ToString();
                legsLuck.Text = "+ " + armor.Luck.ToString();
                legsStamina.Text = "+ " + armor.Stamina.ToString();
                legsStrength.Text = "+ " + armor.Strength.ToString();

                bootsSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/bootsEnabled.png"));
                bootsTakeoff.Tag = armor.Id;
            }
            if (character.Equipment.Shield < 1)
            {
                shieldName.Text = "Brak tarczy";
                shieldArmor.Text = "";
                shieldDexterity.Text = "";
                shieldLuck.Text = "";
                shieldStamina.Text = "";
                shieldStrength.Text = "";

                shieldSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/shieldDisabled.png"));
                shieldTakeoff.Tag = 0;
            }
            else
            {
                Armor armor = armors.GetArmorById(character.Equipment.Shield);

                shieldName.Text = armor.Name.ToString();
                shieldArmor.Text = armor.ArmorPoints.ToString();
                shieldDexterity.Text = "+ " + armor.Dexterity.ToString();
                shieldLuck.Text = "+ " + armor.Luck.ToString();
                shieldStamina.Text = "+ " + armor.Stamina.ToString();
                shieldStrength.Text = "+ " + armor.Strength.ToString();

                shieldSpotImg.Source = new BitmapImage(new Uri("pack://application:,,,/Images/shieldEnabled.png"));
                shieldTakeoff.Tag = armor.Id;
            }
        }

        //rysowanie listy ekwipunku
        private void CreateItemsList(List<Storage> storageList)
        {
            weaponList.Children.Clear();
            armorList.Children.Clear();

            foreach (Storage position in storageList)
            {
                Item item = character.Storage.GetItemById(position.ItemId);

                Border border = new Border();
                border.CornerRadius = new CornerRadius(12);
                border.Margin = new Thickness(5);
                border.Padding = new Thickness(5);
                border.Background = new SolidColorBrush(Color.FromArgb(136, 0, 0, 0));
                border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                border.BorderThickness = new Thickness(2);

                Grid gridContainer = new Grid();

                ColumnDefinition col0 = new ColumnDefinition();
                col0.Width = new GridLength(50, GridUnitType.Pixel);

                ColumnDefinition col1 = new ColumnDefinition();
                col1.Width = new GridLength(155, GridUnitType.Pixel);

                ColumnDefinition col2 = new ColumnDefinition();
                col2.Width = new GridLength(150, GridUnitType.Pixel);

                ColumnDefinition col3 = new ColumnDefinition();
                col3.Width = new GridLength(50, GridUnitType.Pixel);

                gridContainer.ColumnDefinitions.Add(col0);
                gridContainer.ColumnDefinitions.Add(col1);
                gridContainer.ColumnDefinitions.Add(col2);
                gridContainer.ColumnDefinitions.Add(col3);

                Image img = new Image();
                img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Items/" + item.Icon));
                img.Width = 50;
                img.Height = 50;
                img.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                StackPanel infoPanel = new StackPanel();
                StackPanel bonusPanel = new StackPanel();

                infoPanel.Margin = new Thickness(3, 0, 0, 0);
                bonusPanel.Margin = new Thickness(3, 0, 0, 0);

                TextBlock nameAndAmount = new TextBlock();
                nameAndAmount.Foreground = Brushes.White;
                nameAndAmount.Text = item.Name;
                infoPanel.Children.Add(nameAndAmount);

                TextBlock amountInfo = new TextBlock();
                amountInfo.Foreground = Brushes.White;
                amountInfo.Text = "Ilość: " + position.Amount;
                infoPanel.Children.Add(amountInfo);

                TextBlock price = new TextBlock();
                price.Foreground = Brushes.White;
                price.Text = "Wartość: " + item.Price;
                infoPanel.Children.Add(price);

                TextBlock bonus = new TextBlock();
                bonus.Foreground = Brushes.White;
                bonus.Text = "Premie do cech:";

                if (item.Type == "weapon")
                {
                    Weapon weapon = weapons.GetWeaponById(item.Id);

                    WrapPanel dmgWrap = new WrapPanel();
                    dmgWrap.Width = 200;
                    dmgWrap.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    TextBlock damageLabel = new TextBlock();
                    damageLabel.Foreground = Brushes.White;
                    damageLabel.Text = "Obrażenia: ";

                    TextBlock damage = new TextBlock();
                    damage.Foreground = Brushes.Red;
                    damage.FontWeight = FontWeights.Bold;
                    damage.Text = weapon.Min_attack.ToString();
                    damage.Text += " - " + weapon.Max_attack;

                    dmgWrap.Children.Add(damageLabel);
                    dmgWrap.Children.Add(damage);

                    TextBlock bonuses = new TextBlock();
                    bonuses.Foreground = Brushes.Green;
                    bonuses.Text = "";
                    if (weapon.Strength > 0)
                    {
                        bonuses.Text += "+" + weapon.Strength + " Siła \n";
                    }
                    if (weapon.Stamina > 0)
                    {
                        bonuses.Text += "+" + weapon.Stamina + " Wytrzymałość \n";
                    }
                    if (weapon.Dexterity > 0)
                    {
                        bonuses.Text += "+" + weapon.Dexterity + " Zręczność \n";
                    }
                    if (weapon.Luck > 0)
                    {
                        bonuses.Text += "+" + weapon.Luck + " Szczęście \n";
                    }

                    bonuses.Text = bonuses.Text.Remove(bonuses.Text.Length - 1, 1);
                    
                    infoPanel.Children.Add(dmgWrap);
                    bonusPanel.Children.Add(bonus);
                    bonusPanel.Children.Add(bonuses);
                }
                if (item.Type == "armor")
                {
                    Armor armor = armors.GetArmorById(item.Id);

                    WrapPanel armWrap = new WrapPanel();
                    armWrap.Width = 200;
                    armWrap.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    TextBlock armLabel = new TextBlock();
                    armLabel.Foreground = Brushes.White;
                    armLabel.Text = "Punkty pancerza: ";

                    TextBlock armorPoints = new TextBlock();
                    armorPoints.Foreground = Brushes.LightBlue;
                    armorPoints.Text = armor.ArmorPoints.ToString();
                    armorPoints.FontWeight = FontWeights.Bold;

                    armWrap.Children.Add(armLabel);
                    armWrap.Children.Add(armorPoints);

                    TextBlock bonuses = new TextBlock();
                    bonuses.Foreground = Brushes.Green;
                    bonuses.Text = "";

                    if (armor.Strength > 0)
                    {
                        bonuses.Text += "+" + armor.Strength + " Siła \n";
                    }
                    if (armor.Stamina > 0)
                    {
                        bonuses.Text += "+" + armor.Stamina + " Wytrzymałość \n";
                    }
                    if (armor.Dexterity > 0)
                    {
                        bonuses.Text += "+" + armor.Dexterity + " Zręczność \n";
                    }
                    if (armor.Luck > 0)
                    {
                        bonuses.Text += "+" + armor.Luck + " Szczęście \n";
                    }

                    bonuses.Text = bonuses.Text.Remove(bonuses.Text.Length - 1, 1);

                    infoPanel.Children.Add(armWrap);
                    bonusPanel.Children.Add(bonus);
                    bonusPanel.Children.Add(bonuses);
                }

                Button wear = new Button();
                wear.Width = 50;
                wear.Height = 25;
                wear.Content = "Załóż";
                wear.Tag = position.ItemId;
                wear.Click += new RoutedEventHandler(WearItem);

                Grid.SetColumn(img, 0);
                gridContainer.Children.Add(img);
                Grid.SetColumn(infoPanel, 1);
                gridContainer.Children.Add(infoPanel);
                Grid.SetColumn(bonusPanel, 2);
                gridContainer.Children.Add(bonusPanel);
                Grid.SetColumn(wear, 3);
                gridContainer.Children.Add(wear);

                border.Child = gridContainer;

                if (item.Type == "weapon")
                {
                    weaponList.Children.Add(border);
                }
                if (item.Type == "armor")
                {
                    armorList.Children.Add(border);
                }

            }
        }

        private CharacterEquipment.Part GetPart(Item item)
        {
            if (item.Type == "weapon")
            {
                return CharacterEquipment.Part.Weapon;
            }
            else if (item.Type == "armor")
            {
                Armor armor = armors.GetArmorById(item.Id);

                switch (armor.Part)
                {
                    case "chest":
                        return CharacterEquipment.Part.Chest;
                    case "head":
                        return CharacterEquipment.Part.Head;
                    case "legs":
                        return CharacterEquipment.Part.Legs;
                    case "shield":
                        return CharacterEquipment.Part.Shield;
                    default:
                        return CharacterEquipment.Part.Other;
                }
            }
            else
            {
                return CharacterEquipment.Part.Other;
            }
        }

        private void WearItem(object sender, RoutedEventArgs e)
        {
            Button btn;
            Item item;
            uint itemId;
            uint previousItem;

            try
            {
                btn = sender as Button;
                itemId = Convert.ToUInt32(btn.Tag);
                item = character.Storage.GetItemById(itemId);
            }
            catch
            {
                return;
            }

            character.Storage.RemoveOneItem(itemId);
            
            previousItem = character.Equipment.WearItem(GetPart(item), itemId);

            if (previousItem > 0)
            {
                character.Storage.AddItem(previousItem);
            }

            RefreshItems();
        }

        private void takeoff(object sender, RoutedEventArgs e)
        {
            Button takeoffBtn = null;
            uint previousItem;

            try
            {
                takeoffBtn = sender as Button;
                previousItem = Convert.ToUInt32(takeoffBtn.Tag);
            }
            catch
            {
                MessageBox.Show("Brak itemu do ściągnięcia.");
                return;
            }

            if (previousItem > 0)
            {
                Item item = character.Storage.GetItemById(previousItem);

                switch (GetPart(item))
                {
                    case CharacterEquipment.Part.Chest:
                        character.Equipment.Chest = 0;
                        break;
                    case CharacterEquipment.Part.Head:
                        character.Equipment.Head = 0;
                        break;
                    case CharacterEquipment.Part.Legs:
                        character.Equipment.Legs = 0;
                        break;
                    case CharacterEquipment.Part.Shield:
                        character.Equipment.Shield = 0;
                        break;
                    case CharacterEquipment.Part.Weapon:
                        character.Equipment.Weapon = 0;
                        break;
                    case CharacterEquipment.Part.Other:
                        break;
                }
                character.Storage.AddItem(previousItem);
            }

            takeoffBtn.Tag = 0;

            RefreshItems();
        }

        private void BtnTakeOffWeapon_Click(object sender, RoutedEventArgs e)
        {
            character.Equipment.takeoff(CharacterEquipment.Part.Weapon);
            RefreshWeapon();
        }

        private void BtnTakeOffChest_Click(object sender, RoutedEventArgs e)
        {
            character.Equipment.takeoff(CharacterEquipment.Part.Chest);
            RefreshArmor();
        }

        private void BtnTakeOffShield_Click(object sender, RoutedEventArgs e)
        {
            character.Equipment.takeoff(CharacterEquipment.Part.Shield);
            RefreshArmor();
        }

        private void BtnTakeOffHead_Click(object sender, RoutedEventArgs e)
        {
            character.Equipment.takeoff(CharacterEquipment.Part.Head);
            RefreshArmor();
        }

        private void BtnTakeOffLegs_Click(object sender, RoutedEventArgs e)
        {
            character.Equipment.takeoff(CharacterEquipment.Part.Legs);
            RefreshArmor();
        }
        #endregion

    }
}
