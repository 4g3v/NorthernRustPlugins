using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Fougerite;
using Newtonsoft.Json;
using UnityEngine;
using WorldEditorServer.ObjectBehaviours;
using Random = System.Random;

namespace WorldEditorServer
{
    public class SpawnManager
    {
        public static Dictionary<uint, SpawnableObjectBehaviour> SpawnedObjects =
            new Dictionary<uint, SpawnableObjectBehaviour>();

        public static Dictionary<string, IInventoryItem> InventoryItems = new Dictionary<string, IInventoryItem>();

        private static Random _random = new Random();

        private static SpawnableObjectsJson SavedObjects;
        private static string _jsonPath = Util.GetRootFolder() + "\\Save\\WorldEditorServer\\SpawnedObjects.json";

        public static int MaxLandmineCount = 10;
        public static int MaxTurretCount = 3;

//        private static GameObject _turretCheckObject;
//        private static Dictionary<uint, TurretBehaviour> _turrets = new Dictionary<uint, TurretBehaviour>();

        public static void LoadSpawnedObjects()
        {
            if (!File.Exists(_jsonPath))
            {
                File.Create(_jsonPath).Dispose();

                SavedObjects = new SpawnableObjectsJson();
                SavedObjects.SpawnableObjects = new List<SpawnableObject>();
                File.WriteAllText(_jsonPath, JsonConvert.SerializeObject(SavedObjects, Formatting.Indented));
            }

            SavedObjects = JsonConvert.DeserializeObject<SpawnableObjectsJson>(File.ReadAllText(_jsonPath));
            SavedObjects.SpawnableObjects.RemoveAll(so =>
                so.Name == "NUKE" || so.Name == "Meiler" || so.Name == "BrokenMeiler" || so.Name == "RadZone");
            File.WriteAllText(_jsonPath, JsonConvert.SerializeObject(SavedObjects, Formatting.Indented));

#if DEBUG
            Logger.Log("[SpawnableObjects] Loaded " + SavedObjects.SpawnableObjects.Count + " objects");
#endif

            foreach (var spawnableObject in SavedObjects.SpawnableObjects)
            {
#if DEBUG
                Logger.Log("[SpawnableObjects] Spawning object " + spawnableObject.BundleName + ":" +
                           spawnableObject.Name + " (" +
                           spawnableObject.ID + ") at " + spawnableObject.s_Position + " from " +
                           spawnableObject.SteamID);
#endif

                SpawnedObjects[spawnableObject.ID] = SpawnObject(spawnableObject);
            }

//            if (_turretCheckObject != null)
//            {
//                GameObject.Destroy(_turretCheckObject);
//            }
//            
//            _turretCheckObject = new GameObject("TurretCheck");
//            _turretCheckObject.AddComponent<TurretCheckBehaviour>();
//            
//            GameObject.DontDestroyOnLoad(_turretCheckObject);
        }

//        private class TurretCheckBehaviour : MonoBehaviour
//        {
//            private void Start()
//            {
//                StartCoroutine(CheckCoroutine());
//            }
//
//            private IEnumerator CheckCoroutine()
//            {
//                while (true)
//                {
//                    yield return new WaitForSeconds(5);
//                    
//                    var stopwatch = new Stopwatch();       
//                    stopwatch.Start();
//                    
////                    var turrets = new List<TurretBehaviour>();
////                    foreach (var spawnableObjectBehaviour in SpawnedObjects.Values)
////                    {
////                        var turretBehaviour = spawnableObjectBehaviour.GetComponent<TurretBehaviour>();
////                        if (turretBehaviour != null)
////                            turrets.Add(turretBehaviour);
////                    }
//                    
//                    foreach (var turretBehaviour in _turrets.Values)
//                    {
//                        var stillNearStructure = false;
//                        foreach (var collider in Facepunch.MeshBatch.MeshBatchPhysics.OverlapSphere(turretBehaviour.transform.position, 4))
//                        {
//                            var structureComponent = collider.GetComponent<StructureComponent>();
//                            if (structureComponent != null)
//                            {
//                                if (structureComponent.name.Contains("Foundation") || structureComponent.name.Contains("Ceiling"))
//                                {
//                                    stillNearStructure = true;
//                                    break;
//                                }
//                            }
//                        }
//
//                        if (!stillNearStructure)
//                        {
//                            var spawnableObject = turretBehaviour.GetComponent<SpawnableObjectBehaviour>().SpawnableObject;
//                            SpawnManager.RemoveObject(spawnableObject.ID, true);
//                            
//                            var player = Server.GetServer().FindPlayer(spawnableObject.SteamID);
//                            if (player != null && player.IsOnline)
//                            {
//                                player.Notice("Turret has been removed because there's no structure near it anymore!");
//                            }
//                        }
//                    }
//                    
//                    stopwatch.Stop();
//                    Logger.Log("CheckCoroutine() took " + stopwatch.ElapsedMilliseconds + "ms / " + stopwatch.ElapsedMilliseconds / 1000f + "s");
//                    stopwatch.Reset();
//                }
//            }
//        }

        private static SpawnableObjectBehaviour SpawnObject(SpawnableObject spawnableObject)
        {
            var TempGameObject = new GameObject();
            var SpawnedObject = TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();

            SpawnedObject.Create(spawnableObject.Name, spawnableObject.BundleName, spawnableObject.GetPosition(),
                spawnableObject.GetRotation(), spawnableObject.GetSize());

            var spawnableObjectBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<SpawnableObjectBehaviour>();
            spawnableObjectBehaviour.SpawnableObject = spawnableObject;
            spawnableObjectBehaviour.LoaderObject = TempGameObject;

            UnityEngine.Object.DontDestroyOnLoad(TempGameObject);
            UnityEngine.Object.DontDestroyOnLoad(SpawnedObject.ObjectInstantiate);

            switch (spawnableObject.Name)
            {
                case "HealingStation":
                {
                    SpawnedObject.ObjectInstantiate.AddComponent<HealingStationBehaviour>();
                    break;
                }

                case "Landmine":
                {
                    SpawnedObject.ObjectInstantiate.AddComponent<LandmineBehaviour>();

                    if (WorldEditorServer.LandmineCount.ContainsKey(spawnableObject.SteamID))
                    {
                        WorldEditorServer.LandmineCount[spawnableObject.SteamID]++;
                    }
                    else
                    {
                        WorldEditorServer.LandmineCount[spawnableObject.SteamID] = 1;
                    }

                    break;
                }

                case "RespawnFlag":
                {
                    WorldEditorServer.RespawnFlags[spawnableObject.SteamID] = spawnableObject.GetPosition();
                    break;
                }

                case "MetalBarricade":
                {
                    var destroyBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<DestroyBehaviour>();
                    destroyBehaviour.Seconds = 3 * 60;
                    destroyBehaviour.StartWaitAndDestroy();
                    break;
                }

                case "OneManBunker":
                {
                    var destroyBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<DestroyBehaviour>();
                    destroyBehaviour.Seconds = 5 * 60;
                    destroyBehaviour.StartWaitAndDestroy();
                    break;
                }

                case "WaterBridge":
                {
                    var destroyBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<DestroyBehaviour>();
                    destroyBehaviour.Seconds = 20 * 60;
                    destroyBehaviour.StartWaitAndDestroy();
                    break;
                }

//                case "SmallLadder":
//                case "BigLadder":
//                {
//                    var destroyBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<DestroyBehaviour>();
//                    destroyBehaviour.Seconds = 10 * 60;
//                    destroyBehaviour.StartWaitAndDestroy();
//                    break;
//                }
                case "LootStorage":
                {
                    var storageBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<LootStorageBehaviour>();
                    break;
                }

                case "Turret":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.Damage = 4f;
                    if (WorldEditorServer.TurretCount.ContainsKey(spawnableObject.SteamID))
                    {
                        WorldEditorServer.TurretCount[spawnableObject.SteamID]++;
                    }
                    else
                    {
                        WorldEditorServer.TurretCount[spawnableObject.SteamID] = 1;
                    }

//                    _turrets.Add(spawnableObject.ID, turretBehaviour);
                    break;
                }

                case "Turret2":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.Damage = 2.5f;
                    break;
                }

                case "Turret_Silencer":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.Damage = 2.5f;
                    if (WorldEditorServer.TurretCount.ContainsKey(spawnableObject.SteamID))
                    {
                        WorldEditorServer.TurretCount[spawnableObject.SteamID]++;
                    }
                    else
                    {
                        WorldEditorServer.TurretCount[spawnableObject.SteamID] = 1;
                    }

                    break;
                }

                case "Turret_Range":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.Damage = 4f;
                    if (WorldEditorServer.TurretCount.ContainsKey(spawnableObject.SteamID))
                    {
                        WorldEditorServer.TurretCount[spawnableObject.SteamID]++;
                    }
                    else
                    {
                        WorldEditorServer.TurretCount[spawnableObject.SteamID] = 1;
                    }

                    break;
                }

                case "Turret_Damage":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.Damage = 6f;
                    if (WorldEditorServer.TurretCount.ContainsKey(spawnableObject.SteamID))
                    {
                        WorldEditorServer.TurretCount[spawnableObject.SteamID]++;
                    }
                    else
                    {
                        WorldEditorServer.TurretCount[spawnableObject.SteamID] = 1;
                    }

                    break;
                }

                case "Explosion":
                {
                    var destroyBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<DestroyBehaviour>();
                    destroyBehaviour.Seconds = 6;
                    destroyBehaviour.StartWaitAndDestroy();
                    break;
                }

                case "RadZone":
                {
                    var radZoneBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<RadZoneBehaviour>();
                    break;
                }
            }

            return spawnableObjectBehaviour;
        }

        public static uint Spawn(SpawnableObject spawnableObject, bool save = true)
        {
            uint id = (uint) _random.Next(10000);
            while (SpawnedObjects.ContainsKey(id))
                id = (uint) _random.Next(10000);

            foreach (var rpcBehaviour in WorldEditorServer.RPCDictionary.Values)
            {
                rpcBehaviour.WorldEditorSpawn(id, spawnableObject.SteamID, spawnableObject.Name,
                    spawnableObject.BundleName,
                    spawnableObject.GetPosition(), spawnableObject.GetRotation(), spawnableObject.GetSize());
            }

            spawnableObject.ID = id;
            SpawnedObjects[id] = SpawnObject(spawnableObject);

            SavedObjects.SpawnableObjects.Add(spawnableObject);
            
            var savedWithoutNukeStuff = new SpawnableObjectsJson();
            savedWithoutNukeStuff.SpawnableObjects = new List<SpawnableObject>();
            foreach (var so in SavedObjects.SpawnableObjects)
            {
                if (so.Name == "Meiler")
                {
                    continue;
                }

                if (so.Name == "BrokenMeiler")
                {
                    continue;
                }

                if (so.Name == "NUKE")
                {
                    continue;
                }

                if (so.Name == "RadZone")
                {
                    continue;
                }

                savedWithoutNukeStuff.SpawnableObjects.Add(so);
            }
            
            File.WriteAllText(_jsonPath, JsonConvert.SerializeObject(savedWithoutNukeStuff, Formatting.Indented));

#if DEBUG
            Logger.Log("[SpawnableObjects] Spawned and saved object " + spawnableObject.BundleName + ":" +
                       spawnableObject.Name + " (" +
                       id + ") at " + spawnableObject.s_Position + " from " + spawnableObject.SteamID);
#endif

            return id;
        }

        public static void StartPlacement(Fougerite.Player player, SpawnableObject spawnableObject,
            IInventoryItem inventoryItem)
        {
            if (spawnableObject.Name == "RespawnFlag")
            {
                if (WorldEditorServer.RespawnFlags.ContainsKey(player.SteamID))
                {
                    player.Notice("You already have a Respawn Flag. Destroy it to place a new one!");
                    return;
                }
            }
            else if (spawnableObject.Name == "Landmine")
            {
                if (WorldEditorServer.LandmineCount.ContainsKey(spawnableObject.SteamID))
                {
                    if (WorldEditorServer.LandmineCount[spawnableObject.SteamID] >= MaxLandmineCount)
                    {
                        player.Notice("You can't place more than " + MaxLandmineCount + " landmines!");
                        return;
                    }
                }
            }
            else if (spawnableObject.Name.StartsWith("Turret"))
            {
                if (WorldEditorServer.TurretCount.ContainsKey(spawnableObject.SteamID))
                {
                    if (WorldEditorServer.TurretCount[spawnableObject.SteamID] >= MaxTurretCount)
                    {
                        player.Notice("You can't place more than " + MaxTurretCount + " turrets!");
                        return;
                    }
                }
            }

            player.Notice("Place using left click, right click to cancel. Rotate with the left / right arrow.");

            InventoryItems[player.SteamID] = inventoryItem;
            var rpcBehaviour = WorldEditorServer.RPCDictionary[player.SteamID];
            rpcBehaviour.WorldEditorStartPlacement(spawnableObject.Name, spawnableObject.BundleName,
                spawnableObject.GetRotation(), spawnableObject.GetSize());
        }

        public static void SpawnObjects(Fougerite.Player player)
        {
            var rpcBehaviour = WorldEditorServer.RPCDictionary[player.SteamID];
            foreach (var id in SpawnedObjects.Keys)
            {
                var spawnableObjectBehaviour = SpawnedObjects[id];

                rpcBehaviour.WorldEditorSpawn(id, spawnableObjectBehaviour.SpawnableObject.SteamID,
                    spawnableObjectBehaviour.SpawnableObject.Name,
                    spawnableObjectBehaviour.SpawnableObject.BundleName,
                    spawnableObjectBehaviour.SpawnableObject.GetPosition(),
                    spawnableObjectBehaviour.SpawnableObject.GetRotation(),
                    spawnableObjectBehaviour.SpawnableObject.GetSize());
            }
        }

        public static void RemoveObject(uint id, bool destroy, bool save = true)
        {
            var spawnableObjectBehaviour = SpawnedObjects[id];

            if (spawnableObjectBehaviour.SpawnableObject.Name.StartsWith("Turret"))
            {
                if (WorldEditorServer.TurretCount.TryGetValue(spawnableObjectBehaviour.SpawnableObject.SteamID,
                    out var count))
                {
                    count--;
                    WorldEditorServer.TurretCount[spawnableObjectBehaviour.SpawnableObject.SteamID] = count;
                }
            }

#if DEBUG
            Logger.Log("[SpawnableObjects] Removing object " + spawnableObjectBehaviour.SpawnableObject.BundleName +
                       ":" +
                       spawnableObjectBehaviour.SpawnableObject.Name + " (" + id + ") at " +
                       spawnableObjectBehaviour.SpawnableObject.s_Position + " from " +
                       spawnableObjectBehaviour.SpawnableObject.SteamID);
#endif

            if (spawnableObjectBehaviour.SpawnableObject.Name == "RespawnFlag")
                WorldEditorServer.RespawnFlags.Remove(spawnableObjectBehaviour.SpawnableObject.SteamID);

            if (destroy)
                foreach (var rpcBehaviour in WorldEditorServer.RPCDictionary.Values)
                    rpcBehaviour.WorldEditorDestroy(id);

            SpawnedObjects.Remove(id);

            UnityEngine.Object.Destroy(spawnableObjectBehaviour.LoaderObject);
            UnityEngine.Object.Destroy(spawnableObjectBehaviour.gameObject);

            var savedWithoutNukeStuff = new SpawnableObjectsJson();
            savedWithoutNukeStuff.SpawnableObjects = new List<SpawnableObject>();
            foreach (var spawnableObject in SavedObjects.SpawnableObjects)
            {
                if (spawnableObject.Name == "Meiler")
                {
                    continue;
                }

                if (spawnableObject.Name == "BrokenMeiler")
                {
                    continue;
                }

                if (spawnableObject.Name == "NUKE")
                {
                    continue;
                }

                if (spawnableObject.Name == "RadZone")
                {
                    continue;
                }

                savedWithoutNukeStuff.SpawnableObjects.Add(spawnableObject);
            }

            SavedObjects.SpawnableObjects.Remove(spawnableObjectBehaviour.SpawnableObject);
            File.WriteAllText(_jsonPath, JsonConvert.SerializeObject(savedWithoutNukeStuff, Formatting.Indented));
        }

        public class SpawnableObjectsJson
        {
            public List<SpawnableObject> SpawnableObjects;
        }

        public class SpawnableObject
        {
            public uint ID;
            public string SteamID;
            public string BundleName;
            public string Name;

            public string s_Position;
            [JsonIgnore] private Vector3 _position = Vector3.zero;

            public string s_Rotation;
            [JsonIgnore] private Quaternion _rotation = Quaternion.identity;

            public string s_Size;
            [JsonIgnore] private Vector3 _size = Vector3.zero;

            [JsonConstructor]
            public SpawnableObject(uint id, string steamID, string bundleName, string name, string position,
                string rotation, string size)
            {
                ID = id;
                SteamID = steamID;
                BundleName = bundleName;
                Name = name;

                s_Position = position;
                s_Rotation = rotation;
                s_Size = size;
            }

            public SpawnableObject(uint id, string steamID, string bundleName, string name, Vector3 position,
                Quaternion rotation, Vector3 size)
            {
                ID = id;
                SteamID = steamID;
                BundleName = bundleName;
                Name = name;

                s_Position = Utils.Vector3ToString(position);
                _position = position;
                s_Rotation = Utils.QuaternionToString(rotation);
                _rotation = rotation;
                s_Size = Utils.Vector3ToString(size);
                _size = size;
            }

            public Vector3 GetPosition()
            {
                if (_position != Vector3.zero)
                    return _position;
                _position = Utils.StringToVector3(s_Position);
                return _position;
            }

            public Quaternion GetRotation()
            {
                if (_rotation != Quaternion.identity)
                    return _rotation;
                _rotation = Utils.StringToQuaternion(s_Rotation);
                return _rotation;
            }

            public Vector3 GetSize()
            {
                if (_size != Vector3.zero)
                    return _size;
                _size = Utils.StringToVector3(s_Size);
                return _size;
            }
        }

        public class SpawnableObjectBehaviour : MonoBehaviour
        {
            public SpawnableObject SpawnableObject;
            public GameObject LoaderObject;
        }
    }
}