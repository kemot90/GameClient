﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Commands
{
    public class ClientCmd
    {
        public const string LOGIN = "LOGIN";
        public const string GET_PLAYER_DATA = "GET_PLAYER_DATA";
        public const string GET_CHARACTER_DATA = "GET_CHARACTER_DATA";
        public const string GET_CHARACTER_EQUIPMENT = "GET_CHARACTER_EQUIPMENT";
        public const string GET_SKILLS = "GET_SKILLS";
        public const string GET_CITIES = "GET_CITIES";
        public const string UPDATE_DATA_BASE = "UPDATE_DATA_BASE";
        public const string GET_SHORTEST_PATH = "GET_SHORTEST_PATH";
        public const string GET_ENEMIES = "GET_ENEMIES";
        public const string GET_ITEMS_ARMORS = "GET_ITEMS_ARMORS";
        public const string GET_ITEMS_WEAPONS = "GET_ITEMS_WEAPONS";
    }
    public class ServerCmd
    {
        public const string PLAYER_DATA = "PLAYER_DATA";
        public const string CHARACTER_DATA = "CHARACTER_DATA";
        public const string CHARACTER_EQUIPMENT = "CHARACTER_EQUIPMENT";
        public const string CITIES = "CITIES";
        public const string SKILLS = "SKILLS";
        public const string DATA_BASE_UPDATED = "DATA_BASE_UPDATED";
        public const string SHORTEST_PATH = "SHORTEST_PATH";
        public const string ENEMIES = "ENEMIES";
        public const string ARMORS = "ARMORS";
        public const string WEAPONS = "WEAPONS";
    }
    public class Command
    {
        private byte[] cmd;
        private UTF8Encoding code;
        private List<string> args = new List<string>();
        private string cmdString = "";
        private bool isRequest;

        public Command()
        {
            cmdString = "";
            code = new UTF8Encoding();
            isRequest = false;
            args.Clear();
        }
        //dodanie żądania/odpowiedzi w konstruktorze
        public Command(string request)
        {
            cmdString = "";
            code = new UTF8Encoding();
            isRequest = false;
            args.Clear();
            this.Request(request);
        }

        //pobieranie żądania/odpowiedzi
        //jako stringa
        public string String
        {
            get
            {
                cmdString = "";
                foreach (string arg in args)
                {
                    cmdString += ";" + arg;
                }
                cmdString = cmdString.Remove(0, 1);

                return cmdString;
            }
        }

        //jako tablicę bajtów
        public byte[] Byte
        {
            get
            {
                cmdString = "";
                foreach (string arg in args)
                {
                    cmdString += ";" + arg;
                }
                cmdString = cmdString.Remove(0, 1);

                cmd = code.GetBytes(cmdString);

                return cmd;
            }
        }

        //funkcje dodające argumenty do listy argumentów
        //dodanie jednego argumentu
        public void Add(string argument)
        {
            args.Add(argument);
        }

        //dodanie tablicy argumentów
        public void Add(string[] arguments)
        {
            foreach (string arg in arguments)
            {
                args.Add(arg);
            }
        }

        //funkcje dodające argumenty do listy argumentów w konkretne miejsce
        //dodanie jednego argumentu
        public void Insert(int index, string argument)
        {
            args.Insert(index, argument);
        }

        //dodanie tablicy argumentów
        public void Insert(int index, string[] arguments)
        {
            foreach (string arg in arguments)
            {
                args.Insert(index, arg);
                index++;
            }
        }

        //metoda ustawiająca jakiego typu akcja ma zostać wykonana
        public void Request(string req)
        {
            if (!isRequest)
            {
                args.Insert(0, req);
                isRequest = true;
            }
            else
            {
                args.RemoveAt(0);
                args.Insert(0, req);
            }
        }

        //zatwierdzanie argumentów i wysłanie przez gniazdo podane jako arguement
        public string[] Send(Socket client, bool ExpectedResponse = false)
        {
            //zmienne lokalne

            //rozmiar paczki danych
            int packageSize = 0;

            //bufor do wczytywania danych wysłanych przez serwer
            byte[] buf;


            if (isRequest)
            {
                cmdString = "";
                foreach (string arg in args)
                {
                    cmdString += ";" + arg;
                }
                cmdString = cmdString.Remove(0, 1);

                cmd = code.GetBytes(cmdString);

                try
                {
                    client.Send(BitConverter.GetBytes(cmd.Length));
                    client.Send(cmd);
                    Clear();
                    while (ExpectedResponse)
                    {
                        if (client.Available > 0)
                        {
                            if (packageSize == 0)
                            {
                                buf = new byte[4];
                                client.Receive(buf);
                                packageSize = BitConverter.ToInt32(buf, 0);
                            }
                            else
                            {
                                buf = new byte[packageSize];
                                return CommandToArguments(code.GetString(buf, 0, client.Receive(buf)));
                            }
                        }
                        Thread.Sleep(1);
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //czyszczenie komendy
        public void Clear()
        {
            cmd = null;
            cmdString = null;
            args.Clear();
            isRequest = false;
        }

        //zamiana stringa cmd na żądanie i ciąg argumentów
        private string[] CommandToArguments(string command)
        {
            string[] args = command.Split(';');
            return args;
        }
    }
}
