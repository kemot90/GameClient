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
                incDex.IsEnabled = incLuck.IsEnabled = incStam.IsEnabled = incStr.IsEnabled = false;
            }
            else
            {
                incDex.IsEnabled = incLuck.IsEnabled = incStam.IsEnabled = incStr.IsEnabled = true;
            }
        }

    }
}
