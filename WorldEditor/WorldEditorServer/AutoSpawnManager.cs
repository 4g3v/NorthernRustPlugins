//TODO: AutoSpawn
//using System.Collections.Generic;
//using UnityEngine;
//
//namespace WorldEditorServer
//{
//    public class AutoSpawnManager
//    {
//        private static uint _currentID = 0;
//        public static Dictionary<uint, SpawnedObject> SpawnedObjects = new Dictionary<uint, SpawnedObject>();
//
//        public static uint Add(SpawnedObject spawnedObject)
//        {
//            SpawnedObjects[_currentID] = spawnedObject;
//            return _currentID++;
//        }
//        
//        public static void Remove(uint id)
//        {
//            SpawnedObjects.Remove(id);
//        }
//        
//        public class SpawnedObject
//        {
//            public float Seconds;
//            public float DestroySeconds;
//            public string AssetName;
//            public string BundleName;
//            public Vector3 Position;
//            public Quaternion Rotation;
//            public Vector3 Size;
//            
//            public SpawnedObject(float seconds, float destroySeconds, string assetName, string bundleName, Vector3 position, Quaternion rotation,
//                Vector3 size)
//            {
//                Seconds = seconds;
//                DestroySeconds = destroySeconds;
//                AssetName = assetName;
//                BundleName = bundleName;
//                Position = position;
//                Rotation = rotation;
//                Size = size;
//            }
//        }
//    }
//}