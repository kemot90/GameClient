using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using Commands;

namespace RPGClient
{
    class Player
    {
        //właściwości gracza
        private ulong id;
        private string login;
        private string password;
        private int access;
        private string email;

        //ustawienie połączenia z serwerem
        //obiekt gniazda do połączenia z serwerem
        private TcpClient client;

        //koder/dekoder danych z serwera
        private UTF8Encoding code;

        //właściwości gniazda
        private string host;
        private int port;

        //konstruktor gracza
        public Player(ulong _id, TcpClient userClient)
        {
            //ustawienie identyfikatora gracza
            id = _id;

            //wczytanie ustawień dla połączenia z serwerem
            host = Properties.Settings.Default.Host;
            port = Properties.Settings.Default.Port;

            //inicjalizacja gniazda połączenia z serwerem
            try
            {
                client = userClient;
                code = new UTF8Encoding();
                Command request = new Command(ClientCmd.GET_PLAYER_DATA);
                request.Add(id.ToString());
                
                string[] dane = cmdToArgs(request.Apply(client.Client, true));

                if (dane[0] == ServerCmd.PLAYER_DATA)
                {
                    login = dane[1];
                    password = dane[2];
                    access = int.Parse(dane[3]);
                    email = dane[4];
                }
            }
            catch
            {
                //obsługa wyjątku
            }
        }

        public ulong Id
        {
            get
            {
                return id;
            }
        }
        public string Login
        {
            get
            {
                return login;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
        }
        public int Access
        {
            get
            {
                return access;
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
