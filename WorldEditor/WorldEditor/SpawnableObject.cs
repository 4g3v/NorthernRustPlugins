using UnityEngine;

namespace WorldEditor
{
    public class SpawnableObject
    {
        public uint ID;
        public string SteamID;
        public string BundleName;
        public string Name;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Size;

        public SpawnableObject(uint id, string steamID, string bundleName, string name, Vector3 position, Quaternion rotation, Vector3 size)
        {
            ID = id;
            SteamID = steamID;
            BundleName = bundleName;
            Name = name;
            Position = position;
            Rotation = rotation;
            Size = size;
        }
    }

    public class SpawnableObjectBehaviour : MonoBehaviour
    {
        public SpawnableObject SpawnableObject;
        public GameObject LoaderObject;
    }
}