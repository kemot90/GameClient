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

        public Interface(ulong id, TcpClient userClient)
        {
            InitializeComponent();

            code = new UTF8Encoding();

            client = userClient;
            player = new Player(id, client);
            character = new Character(id, client);

            host = Properties.Settings.Default.Host;
            port = Properties.Settings.Default.Port;

            idText.Text = "Identyfikator: " + player.Id.ToString();
            loginText.Text = "Login: " + player.Login;
            emailText.Text = "E-mail: " + player.Email;

            characterName.Text = character.Name;
            characterLevel.Text = "(" + character.Level + " level)";
            characterStrength.Text = character.Strength.ToString();
            characterStamina.Text = character.Stamina.ToString();
            characterDexterity.Text = character.Dexterity.ToString();
            characterLuck.Text = character.Luck.ToString();

            characterStrengthBonus.Text = "0";
            characterStaminaBonus.Text = "0";
            characterDexterityBonus.Text = "0";
            characterLuckBonus.Text = "0";

            remainingPoints.Text = "Pozostałe ("+character.RemainingPoints()+"): ";

            headId.Text = character.Equipment.Head.ToString();
        }

        //zamiana stringa cmd na akcję i ciąg argumentów
        private string[] cmdToArgs(string command)
        {
            string[] args = command.Split(';');
            return args;
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            client.Client.Close();
            new MainWindow().Show();
            this.Close();
        }

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
