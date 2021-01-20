using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fougerite;
using Newtonsoft.Json;
using UnityEngine;

namespace WorldEditorServer
{
    public class ImageZoneSettings
    {
        public static ImageZoneJson ImageZones;
        private static string _jsonPath = Util.GetRootFolder() + "\\Save\\WorldEditorServer\\ImageZones.json";

        public static void Load()
        {
            if (!File.Exists(_jsonPath))
            {
                File.Create(_jsonPath).Dispose();

                ImageZones = new ImageZoneJson();
                ImageZones.ImageZones = new List<ImageZone>();
                Save();
            }

            ImageZones = JsonConvert.DeserializeObject<ImageZoneJson>(File.ReadAllText(_jsonPath));
        }

        public static void Save()
        {
            File.WriteAllText(_jsonPath, JsonConvert.SerializeObject(ImageZones, Formatting.Indented));
        }

        public static ImageZone GetImageZoneFromID(uint id)
        {
            Load();

            var imageZones = ImageZones.ImageZones.Where(t => t.ID == id).ToArray();

            ImageZone imageZone;
            if (imageZones.Length == 0)
            {
                imageZone = new ImageZone(id, 0.5f, "Asdf.png");

                ImageZones.ImageZones.Add(imageZone);
                Save();
            }
            else
            {
                imageZone = imageZones[0];
            }

            return imageZone;
        }
    }

    public class ImageZoneJson
    {
        public List<ImageZone> ImageZones;
    }

    public class ImageZone
    {
        public uint ID;
        public float Radius;
        public string Image;
        
        [JsonConstructor]
        public ImageZone(uint id, float radius, string image)
        {
            ID = id;
            Radius = radius;
            Image = image;
        }
    }
}