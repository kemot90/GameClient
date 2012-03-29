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

        public Interface(ulong id, TcpClient userClient)
        {
            InitializeComponent();

            code = new UTF8Encoding();

            client = userClient;
            player = new Player(id, client);

            host = Properties.Settings.Default.Host;
            port = Properties.Settings.Default.Port;

            idText.Text = player.Id.ToString();
            loginText.Text = player.Login;
            emailText.Text = player.Email;

            /*
             * Część przeznaczona do celów dydaktycznych xD
             */

            /*Command request = new Command(ClientCmd.GET_DUPA);
            request.Add(id.ToString());
            request.Apply(client.Client);

            request.Clear();

            request.Request(ClientCmd.UPDATE_DATA_BASE);
            request.Add(id.ToString());
            request.Add("dupy");
            request.Add("wartosc");
            request.Add("Dupa2");
            request.Apply(client.Client);

            if ((WaitForResponse(client)) == ServerCmd.DATA_BASE_UPDATED)
            {
                request.Clear();

                request.Request(ClientCmd.GET_DUPA);
                request.Add(id.ToString());
                request.Apply(client.Client);
            }*/

            
            /*request.Add(id.ToString());
            request.Add("player");
            request.Add("login;password;email");
            request.Add("dupa;haslo;dupa@dupa.pl");
            request.Apply(client.Client);*/

            /*
             * Koniec części dydaktycznej xD
             */
        }

        //zamiana stringa cmd na akcję i ciąg argumentów
        private string[] cmdToArgs(string command)
        {
            string[] args = command.Split(';');
            return args;
        }

        private string WaitForResponse(TcpClient client)
        {
            string[] args;
            while (true)
            {
                if (client.Available > 0)
                {
                    byte[] buf = new byte[4096];
                    string response = code.GetString(buf, 0, client.Client.Receive(buf));
                    args = cmdToArgs(response);
                    return args[0];
                }
                Thread.Sleep(1);
            }
        }

    }
}
