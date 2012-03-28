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

        public Interface(ulong id, TcpClient userClient)
        {
            InitializeComponent();

            client = userClient;
            player = new Player(id, client);

            host = Properties.Settings.Default.Host;
            port = Properties.Settings.Default.Port;

            idText.Text = player.Id.ToString();
            loginText.Text = player.Login;
            emailText.Text = player.Email;
        }

        //zamiana stringa cmd na akcję i ciąg argumentów
        private string[] cmdToArgs(string command)
        {
            string[] args = command.Split(';');
            return args;
        }

    }
}
