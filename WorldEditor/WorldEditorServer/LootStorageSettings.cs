using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fougerite;
using Newtonsoft.Json;

namespace WorldEditorServer
{
    public class LootStorageSettings
    {
        public static LootStorageJson LootStorages;
        private static string _jsonPath = Util.GetRootFolder() + "\\Save\\WorldEditorServer\\LootStorage.json";

        public static void Load()
        {
            if (!File.Exists(_jsonPath))
            {
                File.Create(_jsonPath).Dispose();

                LootStorages = new LootStorageJson();
                LootStorages.LootStorages = new List<LootStorage>();
                Save();
            }

            LootStorages = JsonConvert.DeserializeObject<LootStorageJson>(File.ReadAllText(_jsonPath));
        }

        public static void Save()
        {
            File.WriteAllText(_jsonPath, JsonConvert.SerializeObject(LootStorages, Formatting.Indented));
        }
        
        public static LootStorage GetLootStorageFromID(uint id)
        {
            Load();
            
            var lootStorages = LootStorages.LootStorages.Where(t => t.ID == id).ToArray();

            LootStorage lootStorage;
            if (lootStorages.Length == 0)
            {
                var exampleItems = new List<Item>();
                exampleItems.Add(new Item("Paper", 20));
                exampleItems.Add(new Item("Gunpowder", 10));
                
                lootStorage = new LootStorage(id, 10, Item.ItemsToString(exampleItems));
                
                LootStorages.LootStorages.Add(lootStorage);
                Save();
            }
            else
            {
                lootStorage = lootStorages[0];
            }
            
            return lootStorage;
        }
    }
    
    public class LootStorageJson
    {
        public List<LootStorage> LootStorages;
    }

    public class LootStorage
    {
        public uint ID;
        public int RefillSeconds;
        public string s_Items;
        [JsonIgnore] private List<Item> Items;

        [JsonConstructor]
        public LootStorage(uint id, int refillSeconds, string items)
        {
            ID = id;
            RefillSeconds = refillSeconds;
            s_Items = items;
        }

        public LootStorage(uint id, int refillSeconds, List<Item> items)
        {
            ID = id;
            RefillSeconds = refillSeconds;
            s_Items = Item.ItemsToString(items);
        }

        public List<Item> GetItems()
        {
            if (Items != null)
            {
                return Items;
            }

            Items = Item.StringToItems(s_Items);
            return Items;
        }
    }

    public class Item
    {
        public string ItemName;
        public int Amount;
        
        public Item(string itemName, int amount)
        {
            ItemName = itemName;
            Amount = amount;
        }

        public static List<Item> StringToItems(string items)
        {
            var itemList = new List<Item>();
            var itemEntries = items.Split('/');

            foreach (var itemEntry in itemEntries)
            {
                if (string.IsNullOrEmpty(itemEntry))
                    continue;
                
                var splittedEntry = itemEntry.Split(':');
                itemList.Add(new Item(splittedEntry[0], int.Parse(splittedEntry[1])));
            }
            
            return itemList;
        }

        public static string ItemsToString(List<Item> items)
        {
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.Append(item.ItemName + ":" + item.Amount);
                sb.Append("/");
            }

            var itemsToString = sb.ToString();
            return itemsToString.Substring(0, itemsToString.Length - 1);
        }
    }
}