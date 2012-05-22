using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Commands;

namespace RPGClient
{
    public class Enemies
    {
        //tutaj będą trzymane wszystkie potworki w danej okolicy
        private List<Mob> enemiesList = new List<Mob>();
        private uint mobsCount = 0;

        //do polaczenia
        private Socket client;

        //konstruktor
        public Enemies(uint location, TcpClient clientTcp)
        {
            client = clientTcp.Client;

            //odpyta bazke o moby dla danej lokaclizacji
            Command command = new Command();
            command.Request(ClientCmd.GET_ENEMIES);
            command.Add(location.ToString());
            string[] result = command.Send(client, true);

            if (result[0] == ServerCmd.ENEMIES)
            {
                mobsCount = uint.Parse(result[1]);

                for (int i = 2; i < result.Length; i += 11)
                {
                    Mob mb = new Mob(
                                ulong.Parse(result[i]),
                                (result[i + 1]),
                                uint.Parse(result[i + 2]),
                                ulong.Parse(result[i + 3]),
                                uint.Parse(result[i + 4]),
                                uint.Parse(result[i + 5]),
                                uint.Parse(result[i + 6]),
                                uint.Parse(result[i + 7]),
                                uint.Parse(result[i + 8]),
                                uint.Parse(result[i + 9]),
                                (result[i + 10])
                                );

                    enemiesList.Add(mb);
                }
            }
        }

        public List<Mob> EnemiesList
        {
            get
            {
                return enemiesList;
            }
        }

        public uint MobsCount
        {
            get
            {
                return mobsCount;
            }
        }
    }
}
