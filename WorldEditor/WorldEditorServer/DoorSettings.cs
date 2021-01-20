using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fougerite;
using Newtonsoft.Json;
using UnityEngine;

namespace WorldEditorServer
{
    public class DoorSettings
    {
        public static DoorJson Doors;
        private static string _jsonPath = Util.GetRootFolder() + "\\Save\\WorldEditorServer\\Doors.json";

        public static void Load()
        {
            if (!File.Exists(_jsonPath))
            {
                File.Create(_jsonPath).Dispose();

                Doors = new DoorJson();
                Doors.Doors = new List<Door>();
                Save();
            }

            Doors = JsonConvert.DeserializeObject<DoorJson>(File.ReadAllText(_jsonPath));
        }

        public static void Save()
        {
            File.WriteAllText(_jsonPath, JsonConvert.SerializeObject(Doors, Formatting.Indented));
        }
        
        public static Door GetDoorFromID(uint id)
        {
            Load();
            
            var doors = Doors.Doors.Where(t => t.ID == id).ToArray();

            Door door;
            if (doors.Length == 0)
            {
                door = new Door(id, "asdf");
                
                Doors.Doors.Add(door);
                DoorSettings.Save();
            }
            else
            {
                door = doors[0];
            }
            
            return door;
        }
    }
    
    public class DoorJson
    {
        public List<Door> Doors;
    }

    public class Door
    {
        public uint ID;
        public string Item;

        [JsonConstructor]
        public Door(uint id, string item)
        {
            ID = id;
            Item = item;
        }
    }
}