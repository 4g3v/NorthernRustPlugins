using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fougerite;
using Newtonsoft.Json;
using UnityEngine;

namespace WorldEditorServer
{
    public class TeleporterSettings
    {
        public static TeleporterJson Teleporters;
        private static string _jsonPath = Util.GetRootFolder() + "\\Save\\WorldEditorServer\\Teleporters.json";

        public static void Load()
        {
            if (!File.Exists(_jsonPath))
            {
                File.Create(_jsonPath).Dispose();

                Teleporters = new TeleporterJson();
                Teleporters.Teleporters = new List<Teleporter>();
                Save();
            }

            Teleporters = JsonConvert.DeserializeObject<TeleporterJson>(File.ReadAllText(_jsonPath));
        }

        public static void Save()
        {
            File.WriteAllText(_jsonPath, JsonConvert.SerializeObject(Teleporters, Formatting.Indented));
        }
        
        public static Teleporter GetTeleporterFromID(uint id)
        {
            Load();
            
            var teleporterFromId = Teleporters.Teleporters.Where(t => t.ID == id).ToArray();

            Teleporter teleporter;
            if (teleporterFromId.Length == 0)
            {
                teleporter = new Teleporter(id, Vector3.zero, 0.5f);
                
                Teleporters.Teleporters.Add(teleporter);
                Save();
            }
            else
            {
                teleporter = teleporterFromId[0];
            }
            
            return teleporter;
        }
    }

    public class TeleporterJson
    {
        public List<Teleporter> Teleporters;
    }

    public class Teleporter
    {
        public uint ID;
        public string s_Position;
        [JsonIgnore] private Vector3 _position = Vector3.zero;
        public float Radius;

        [JsonConstructor]
        public Teleporter(uint id, string position, float radius)
        {
            ID = id;
            s_Position = position;
            Radius = radius;
        }
        public Teleporter(uint id, Vector3 position, float radius)
        {
            ID = id;
            s_Position = Utils.Vector3ToString(position);
            Radius = radius;
        }
        
        public Vector3 GetPosition()
        {
            if (_position != Vector3.zero)
                return _position;
            _position = Utils.StringToVector3(s_Position);
            return _position;
        }
    }
}