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
        private ulong userID;
        private TcpClient client;
        private UTF8Encoding code;
        public Interface(ulong id, TcpClient userClient)
        {
            InitializeComponent();
            userID = id;
            client = userClient;
            idText.Text = userID.ToString();
            code = new UTF8Encoding();
            client.Client.Send(code.GetBytes("GET_PLAYER_DATA;" + userID));
            while (true)
            {
                if (client.Available > 0)
                {
                    byte[] buf = new byte[4096];
                    string[] dane;
                    string cmd = code.GetString(buf, 0, client.Client.Receive(buf));
                    dane = cmdToArgs(cmd);

                    if (dane[0] == ServerCmd.PLAYER_DATA)
                    {
                        loginText.Text = dane[1];
                        emailText.Text = dane[4];
                    }
                    break;
                }
            }
        }

        //zamiana stringa cmd na akcję i ciąg argumentów
        private string[] cmdToArgs(string command)
        {
            string[] args = command.Split(';');
            return args;
        }
    }
}
