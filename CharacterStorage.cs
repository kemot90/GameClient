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
using System.Net.Sockets;
using Commands;

namespace RPGClient
{
    public class Item
    {
        private uint id;
        private string name;
        private string type;
        private uint price;
        private string icon;

        public Item(uint id, string name, string type, uint price, string icon)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.price = price;
            this.icon = icon;
        }

        #region Akcesory
        public uint Id
        {
            get
            {
                return id;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Type
        {
            get
            {
                return type;
            }
        }
        public uint Price
        {
            get
            {
                return price;
            }
        }
        public string Icon
        {
            get
            {
                return icon;
            }
        }
        #endregion
    }

    public class Storage
    {
        private ulong id; //id gracza
        private uint itemid;
        private uint amount;

        public Storage(ulong id, uint itemid, uint amount)
        {
            this.id = id;
            this.itemid = itemid;
            this.amount = amount;
        }

        public void SetAmount(uint amount, Socket socket)
        {
            this.amount = amount;
            Command command = new Command();
            //ustawienie żądania uaktualnienia bazy danych
            command.Request(ClientCmd.UPDATE_DATA_BASE);
            //w tabeli character
            command.Add("character_storage");
            //pola name
            command.Add("amount");
            //na wartość updateTest
            command.Add(amount.ToString());
            //gdzie wartość pola id
            command.Add("id");
            //jest równa identyfikatorowi gracza
            command.Add(this.CharacterId.ToString() + " AND `character_storage`.`id_item` = " + this.CharacterId);
            //uaktualnij i nie czekaj na odpowiedź
            command.Send(socket, false);
        }

        #region Akcesory
        public ulong CharacterId
        {
            get
            {
                return id;
            }
        }

        public uint ItemId
        {
            get
            {
                return itemid;
            }
        }

        public uint Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }
        #endregion
    }

    public class CharacterStorage
    {
        private Socket socket;

        private ulong id;
        private List<Item> items;
        private List<Storage> storage;

        public CharacterStorage(ulong id, TcpClient client)
        {
            socket = client.Client;

            this.id = id;

            string[] result;
            items = new List<Item>();
            storage = new List<Storage>();

            Command request = new Command();
            request.Request(ClientCmd.GET_CHARACTER_STORAGE);
            request.Add(id.ToString());
            result = request.Send(socket, true);

            for (int i = 1; i < result.Length; i += 3)
            {
                Storage position = new Storage(
                    ulong.Parse(result[i]),
                    uint.Parse(result[i + 1]),
                    uint.Parse(result[i + 2])
                    );
                storage.Add(position);
            }
        }

        public Item GetItemById(uint id)
        {
            string[] result;
            Item item;
            Command request = new Command();
            request.Request(ClientCmd.GET_ITEM_BY_ID);
            request.Add(id.ToString());

            result = request.Send(socket, true);

            try
            {
                item = new Item(
                    uint.Parse(result[1]),
                    result[2],
                    result[3],
                    uint.Parse(result[4]),
                    result[5]
                    );
            }
            catch
            {
                item = null;
            }

            return item;
        }

        public void RemoveOneItem(uint itemId)
        {
            Storage position = storage.Find(pos => pos.ItemId == itemId);
            uint am = position.Amount;

            if (position.Amount > 1)
            {
                position.Amount--;
            }
            else
            {
                storage.RemoveAll(pos => pos.ItemId == itemId);
            }

            Command request = new Command();
            request.Request(ClientCmd.REMOVE_ONE_ITEM);
            request.Add(itemId.ToString());
            request.Send(socket);
        }
        public void AddItem(uint itemId, uint amount = 1)
        {
            Storage position = storage.Find(pos => pos.ItemId == itemId);

            if (position == null)
            {
                storage.Add(new Storage(this.id, itemId, amount));
            }
            else
            {
                position.Amount += amount;
            }

            Command request = new Command();
            request.Request(ClientCmd.ADD_ITEM);
            request.Add(itemId.ToString());
            request.Add(amount.ToString());
            request.Send(socket);
        }

        public void RemoveByUserItemId(uint id)
        {
            items.RemoveAll(item => item.Id == id);
        }

        #region Akcesory

        public List<Item> ItemList
        {
            get
            {
                return items;
            }
        }

        public List<Storage> StorageList
        {
            get
            {
                return storage;
            }
        }

        #endregion
    }
}
