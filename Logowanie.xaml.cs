using System;
using System.Text;
using System.Windows;
using System.Net.Sockets;
using Commands;
using System.Diagnostics;

namespace RPGClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ustawienie obiektu klienta TCP/IP
        private TcpClient user;
        
        //ustawienie kodera/dekodera
        private UTF8Encoding code;

        //ustawienie hosta serwera
        private string host;

        //ustawienie portu, na którym nasłuchuje serwer
        private int port;

        //delegat funkcji przyjmującej jako argument bool
        private delegate void SetBool(bool logic);

        public MainWindow()
        {
            InitializeComponent();

            host = Properties.Settings.Default.Host;
            port = Properties.Settings.Default.Port;
        }
        
        //funkcja konwerująca ciąg znaków na kod MD5
        public string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        //zamiana stringa cmd na akcję i ciąg argumentów
        private string[] cmdToArgs(string command)
        {
            string[] args = command.Split(';');
            return args;
        }
        
        //logowanie do gry
        private void singin_Click(object sender, RoutedEventArgs e)
        {
            //inicjalizacja gniazda
            try
            {
                user = new TcpClient(host, port);
                
                //ustawienie czasu oczekiwania na odpowiedź serwera
                user.ReceiveTimeout = 5;
            }
            catch
            {
                MessageBox.Show("Czas oczekiwania na połączenie minął. Nie można było połączyć się z serwerem. Sprawdź połączenia z Internetami.", "Błąd połączenia!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //inicjalizacja kodera/dekodera
            code = new UTF8Encoding();

            Command cmd = new Command();
            cmd.Request(ClientCmd.LOGIN);
            cmd.Add(login.Text);
            cmd.Add(GetMD5Hash(password.Password));

            Stopwatch watch = new Stopwatch();
            watch.Start();
            string[] args = cmd.Send(user.Client, true);
            
            if (Convert.ToUInt64(args[1]) == 0)
            {
                MessageBox.Show("[Klient] Nie udało się zalogować.");
                user.Close();
                singin.IsEnabled = true;
                return;
            }
            else
            {
                watch.Stop();
                new Interface(Convert.ToUInt64(args[1]), user, (long.Parse(args[2]) + watch.ElapsedTicks) - DateTime.UtcNow.Ticks).Show();
                this.Close();
            }
        }
    }
}
