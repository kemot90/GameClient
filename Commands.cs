using System;
using System.Collections.Generic;
using System.Text;

namespace Commands
{
    public class ClientCmd
    {
        public const string LOGIN = "LOGIN";
        public const string GET_PLAYER_DATA = "GET_PLAYER_DATA";
        public const string UPDATE_FIELD = "UPDATE_FIELD";
    }
    public class ServerCmd
    {
        public const string PLAYER_DATA = "PLAYER_DATA";
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
                return cmdString;
            }
        }
        //jako tablicę bajtów
        public byte[] Byte
        {
            get
            {
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
        //zatwierdzanie argumentów
        public bool Apply()
        {
            if (isRequest)
            {
                foreach (string arg in args)
                {
                    cmdString += ";" + arg;
                }
                cmdString = cmdString.Remove(0, 1);

                cmd = code.GetBytes(cmdString);

                return true;
            }
            else
            {
                return false;
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
    }
}
