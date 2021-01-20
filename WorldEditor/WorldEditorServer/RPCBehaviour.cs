using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Fougerite;
using UnityEngine;
using WorldEditorServer.ObjectBehaviours;

namespace WorldEditorServer
{
    public class RPCBehaviour : MonoBehaviour
    {
        public Fougerite.Player FougeritePlayer;

//        TODO: AutoSpawn
//        public uint WorldEditorSpawn(AutoSpawnManager.SpawnedObject objectToSpawn)
//        {
//            uint id = AutoSpawnManager.Add(new AutoSpawnManager.SpawnedObject(objectToSpawn.Seconds,
//                objectToSpawn.DestroySeconds, objectToSpawn.AssetName, objectToSpawn.BundleName,
//                objectToSpawn.Position, objectToSpawn.Rotation, objectToSpawn.Size));

//            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
//                .RPC("WorldEditorSpawn", FougeritePlayer.NetworkPlayer, id, objectToSpawn.AssetName,
//                    objectToSpawn.BundleName, objectToSpawn.Position, objectToSpawn.Rotation,
//                    objectToSpawn.Size);

//            return id;
//        }

        public void WorldEditorSpawn(uint id, string steamID, string name, string bundleName, Vector3 position,
            Quaternion rotation,
            Vector3 size)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("WorldEditorSpawn", FougeritePlayer.NetworkPlayer, id, steamID, name, bundleName, position,
                    rotation, size);
        }

        public void WorldEditorDestroy(uint id)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("WorldEditorDestroy", FougeritePlayer.NetworkPlayer, id);

//        TODO: AutoSpawn
//            AutoSpawnManager.Remove(id);
        }

        public void WorldEditorStartPlacement(string assetName, string bundleName, Quaternion rotation,
            Vector3 size)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("WorldEditorStartPlacement", FougeritePlayer.NetworkPlayer, assetName, bundleName, rotation, size);
        }

        public void WorldEditorStartDestroy()
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("WorldEditorStartDestroy", FougeritePlayer.NetworkPlayer);
        }

        public void LandmineBehaviour_PlayParticleSystem(uint id)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("LandmineBehaviour_PlayParticleSystem", FougeritePlayer.NetworkPlayer, id);
        }

        [RPC]
        public void LandmineBehaviour_HandleCollision(uint id)
        {
            if (!SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour)) return;

            var landmineBehaviour = spawnableObjectBehaviour.GetComponent<LandmineBehaviour>();
            if (landmineBehaviour == null) return;

            if (Vector3.Distance(FougeritePlayer.Location,
                    spawnableObjectBehaviour.SpawnableObject.GetPosition()) >= 4)
                return;

            landmineBehaviour.HandleCollision(FougeritePlayer);
        }

        public void TeleporterBehaviour_SetRadius(uint id, float radius)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("TeleporterBehaviour_SetRadius", FougeritePlayer.NetworkPlayer, id, radius);
        }

        [RPC]
        public void TeleporterBehaviour_Teleport(uint id)
        {
            var teleporter = TeleporterSettings.GetTeleporterFromID(id);
            if (SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                if (Vector3.Distance(FougeritePlayer.Location, spawnableObjectBehaviour.transform.position) <=
                    teleporter.Radius)
                {
                    FougeritePlayer.TeleportTo(teleporter.GetPosition());
                }
            }
        }

        [RPC]
        public void TeleporterBehaviour_GetRadius(uint id)
        {
            var teleporter = TeleporterSettings.GetTeleporterFromID(id);
            TeleporterBehaviour_SetRadius(id, teleporter.Radius);
        }

        [RPC]
        public void ImageZoneBehaviour_GetInfo(uint id)
        {
            var imageZone = ImageZoneSettings.GetImageZoneFromID(id);
            ImageZoneBehaviour_SetInfo(id, imageZone.Radius, imageZone.Image);
        }

        public void ImageZoneBehaviour_SetInfo(uint id, float radius, string image)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("ImageZoneBehaviour_SetInfo", FougeritePlayer.NetworkPlayer, id, radius, image);
        }

        [RPC]
        public void RadZoneBehaviour_GetInfo(uint id)
        {
            if (SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                var radZoneBehaviour = spawnableObjectBehaviour.GetComponent<RadZoneBehaviour>();
                RadZoneBehaviour_SetInfo(id, radZoneBehaviour.Radius);
            }
        }

        [RPC]
        public void RadZoneBehaviour_AddRads(uint id)
        {
            if (SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                var radZoneBehaviour = spawnableObjectBehaviour.GetComponent<RadZoneBehaviour>();

                var rads = radZoneBehaviour.Rads;
                var amountToReduce = 0f;
                
                foreach (var armorItem in FougeritePlayer.Inventory.ArmorItems)
                {
                    if (armorItem.Name == "Rad Suit Vest")
                    {
                        amountToReduce += rads * 0.30f;
                    }
                    else if (armorItem.Name == "Rad Suit Pants")
                    {
                        amountToReduce += rads * 0.20f;
                    }
                    else if (armorItem.Name == "Rad Suit Helmet")
                    {
                        amountToReduce += rads * 0.15f;
                    }
                    else if (armorItem.Name == "Rad Suit Boots")
                    {
                        amountToReduce += rads * 0.1f;
                    }
                }
                FougeritePlayer.AddRads(radZoneBehaviour.Rads - amountToReduce);                
            }
        }

        public void RadZoneBehaviour_SetInfo(uint id, float radius)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("RadZoneBehaviour_SetInfo", FougeritePlayer.NetworkPlayer, id, radius);
        }

        public void DoorBehaviour_SetNearDoor(uint id, bool b)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("DoorBehaviour_SetNearDoor", FougeritePlayer.NetworkPlayer, id, b);
        }

        public void DoorBehaviour_SetOpen(uint id, bool b)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("DoorBehaviour_SetOpen", FougeritePlayer.NetworkPlayer, id, b);
        }

        private static Dictionary<uint, bool> _doorOpenDictionary = new Dictionary<uint, bool>();
        private bool _doorBehaviour_closing;

        [RPC]
        public void DoorBehaviour_GetOpen(uint id)
        {
            DoorBehaviour_SetOpen(id, _doorOpenDictionary.ContainsKey(id) && _doorOpenDictionary[id]);
        }

        [RPC]
        public void DoorBehaviour_Use(uint id)
        {
            if (SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                if (Vector3.Distance(FougeritePlayer.Location, spawnableObjectBehaviour.transform.position) <= 4)
                {
                    var door = DoorSettings.GetDoorFromID(id);
                    if (!FougeritePlayer.Inventory.HasItem(door.Item))
                    {
                        FougeritePlayer.Notice("You need to have " + door.Item + " in your inventory!");
                        return;
                    }

                    bool open;
                    if (_doorOpenDictionary.ContainsKey(id))
                    {
                        open = !_doorOpenDictionary[id];
                    }
                    else
                    {
                        open = true;
                    }

                    spawnableObjectBehaviour.transform.FindChild("Lock").gameObject.SetActive(!open);

                    _doorOpenDictionary[id] = open;

                    foreach (var rpcDictionaryValue in WorldEditorServer.RPCDictionary.Values)
                    {
                        rpcDictionaryValue.DoorBehaviour_SetOpen(id, open);
                    }

                    if (!_doorBehaviour_closing)
                        StartCoroutine(DoorBehaviour_AutoCloseCoroutine(id));

//                    Debug.Log("Set open to " + open);
                }
                else
                {
                    DoorBehaviour_SetNearDoor(id, false);

//                    Debug.Log("Set NearDoor to false.");
                }
            }
        }

        private static Dictionary<uint, DateTime> _towerUseDictionary = new Dictionary<uint, DateTime>();

        [RPC]
        public void AirdropTowerBehaviour_Use(uint id)
        {
            if (SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                if (Vector3.Distance(FougeritePlayer.Location, spawnableObjectBehaviour.transform.position) <= 4)
                {
                    if (_towerUseDictionary.TryGetValue(id, out var time))
                    {
                        Logger.Log("Dict has value");
                        var timeSpan = DateTime.Now.Subtract(time);
                        Logger.Log(timeSpan.ToString());
                        if (timeSpan.TotalHours < 1.5)
//                        if (timeSpan.TotalMinutes < 1)
                        {
                            var waitSpan = new TimeSpan(1, 30, 0).Subtract(timeSpan);
//                            var waitSpan = new TimeSpan(0, 1, 0).Subtract(timeSpan);
                            FougeritePlayer.MessageFrom("AirdropTower",
                                "You still have to wait " + waitSpan.Hours + " hour(s) and " + waitSpan.Minutes +
                                " minute(s)");
                            FougeritePlayer.MessageFrom("AirdropTower", "before you can use this AirdropTower!");
                            return;
                        }
                        
                        _towerUseDictionary[id] = DateTime.Now;

                        Logger.Log("Set dict value");
                    }
                    else
                    {
                        _towerUseDictionary[id] = DateTime.Now;
                        Logger.Log("Added dict value");
                    }
                    
                    Logger.Log("Doing shit");

                    World.GetWorld().AirdropAtOriginal(spawnableObjectBehaviour.SpawnableObject.GetPosition());

                    foreach (var rpcDictionaryValue in WorldEditorServer.RPCDictionary.Values)
                    {
                        rpcDictionaryValue.AirdropTowerBehaviour_PlaySound();
                    }
                }
                else
                {
                    AirdropTowerBehaviour_SetNearTower(id, false);
                }
            }
        }

        public void AirdropTowerBehaviour_SetNearTower(uint id, bool b)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("AirdropTowerBehaviour_SetNearTower", FougeritePlayer.NetworkPlayer, id, b);
        }

        public void AirdropTowerBehaviour_PlaySound()
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("AirdropTowerBehaviour_PlaySound", FougeritePlayer.NetworkPlayer);
        }

        private IEnumerator DoorBehaviour_AutoCloseCoroutine(uint id)
        {
            if (SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                _doorBehaviour_closing = true;
                yield return new WaitForSeconds(4f);

                spawnableObjectBehaviour.transform.FindChild("Lock").gameObject.SetActive(true);
                _doorOpenDictionary[id] = false;
                DoorBehaviour_SetOpen(id, false);

//                Debug.Log("Autoclosed");
                _doorBehaviour_closing = false;
            }
        }

        public static Dictionary<uint, string> _turretTargetDictionary = new Dictionary<uint, string>();
        private static Dictionary<uint, int> _turretDamageDictionary = new Dictionary<uint, int>();
        private const int MaxTurretHealth = 1500;

        [RPC]
        public void TurretBehaviour_SetTargetInRange(uint id, bool inRange)
        {
//            Logger.Log("TurretBehaviour_SetTargetInRange(" + id + ", " + inRange + ")");
            if (SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                var friendList = new RustPPExtension().FriendsOf(spawnableObjectBehaviour.SpawnableObject.SteamID);
                if (friendList != null && friendList.isFriendWith(FougeritePlayer.UID))
                {
//                    Logger.Log("Turret friend");
                    return;
                }

                var turretBehaviour = spawnableObjectBehaviour.GetComponent<TurretBehaviour>();
                turretBehaviour.SetTarget(FougeritePlayer, inRange);

                if (inRange)
                {
                    foreach (var rpcDictionaryValue in WorldEditorServer.RPCDictionary.Values)
                    {
                        rpcDictionaryValue.TurretBehaviour_SetTarget(id, FougeritePlayer.SteamID);
                    }

                    _turretTargetDictionary[id] = FougeritePlayer.SteamID;

                    TurretBehaviour_AddTurretID(id);

//                    Logger.Log("TurretBehaviour_SetTargetInRange(): Set target for everyone and added player to dict");
                }
                else
                {
                    foreach (var rpcDictionaryValue in WorldEditorServer.RPCDictionary.Values)
                    {
                        rpcDictionaryValue.TurretBehaviour_SetTarget(id, "");
                    }

                    _turretTargetDictionary.Remove(id);

//                    Logger.Log(
//                        "TurretBehaviour_SetTargetInRange(): Removed target for everyone and remove player from dict");
                }
            }
        }

        public void TurretBehaviour_SetTarget(uint id, string steamID)
        {
//            Logger.Log("TurretBehaviour_SetTarget(" + id + ", " + steamID + ")");

            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("TurretBehaviour_SetTarget", FougeritePlayer.NetworkPlayer, id, steamID);
        }

        public void TurretBehaviour_AddTurretID(uint id)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("TurretBehaviour_AddTurretID", FougeritePlayer.NetworkPlayer, id);
        }

        [RPC]
        public void TurretBehaviour_GetInfo(uint id)
        {
//            Logger.Log("TurretBehaviour_GetInfo(" + id + ")");
//
//            foreach (var keyValuePair in _turretTargetDictionary)
//            {
//                Logger.Log("_turretTargetDictionary[" + keyValuePair.Key + "]: " + keyValuePair.Value);
//            }

            if (_turretTargetDictionary.TryGetValue(id, out var targetID))
            {
                TurretBehaviour_SetTarget(id, targetID);

                if (targetID == FougeritePlayer.SteamID)
                {
                    if (SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
                    {
                        spawnableObjectBehaviour.GetComponent<TurretBehaviour>()
                            .SetTarget(FougeritePlayer, true);
                    }

                    foreach (var rpcDictionaryValue in WorldEditorServer.RPCDictionary.Values)
                    {
                        rpcDictionaryValue.TurretBehaviour_SetTarget(id, FougeritePlayer.SteamID);
                    }
                }
            }
        }

        [RPC]
        public void TurretBehaviour_Damage(uint id)
        {
            if (SpawnManager.SpawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                int currentHealth;
                if (_turretDamageDictionary.ContainsKey(id))
                {
                    currentHealth = _turretDamageDictionary[id];
                    _turretDamageDictionary[id] = currentHealth - 1;
                }
                else
                {
                    currentHealth = MaxTurretHealth - 1;
                    _turretDamageDictionary.Add(id, currentHealth);
                }

                if (currentHealth > 0)
                {
                    RustNotice("Health: " + currentHealth + "/" + MaxTurretHealth, 1);
                }
                else
                {
                    if (WorldEditorServer.TurretCount.TryGetValue(spawnableObjectBehaviour.SpawnableObject.SteamID,
                        out var count))
                    {
                        count--;
                        WorldEditorServer.TurretCount[spawnableObjectBehaviour.SpawnableObject.SteamID] = count;
                    }

                    SpawnManager.Spawn(new SpawnManager.SpawnableObject(0, "Server", "nr_Cxx", "Explosion",
                        spawnableObjectBehaviour.transform.position, Quaternion.identity, Vector3.one));
                    SpawnManager.RemoveObject(id, true);
                }
            }
        }

        [RPC]
        public void WorldEditorFinishDestroy(uint id)
        {
            var spawnableObjectBehaviour = SpawnManager.SpawnedObjects[id];
            if (spawnableObjectBehaviour.SpawnableObject.SteamID != FougeritePlayer.SteamID)
            {
                if (!FougeritePlayer.Admin)
                {
                    if (spawnableObjectBehaviour.SpawnableObject.Name != "RaidFlag")
                    {
                        FougeritePlayer.Notice("This object doesn't belong to you!");
                        return;
                    }
                }
            }

            SpawnManager.RemoveObject(id, true);
            FougeritePlayer.Notice("Destroyed!");
        }

        [RPC]
        public void WorldEditorSpawnObjects()
        {
            SpawnManager.SpawnObjects(FougeritePlayer);
        }

        [RPC]
        public void WorldEditorFinishPlacement(bool place, string assetName, string bundleName, Vector3 position,
            Quaternion rotation, Vector3 size, bool nearWater)
        {
            if (!place)
            {
                SpawnManager.InventoryItems.Remove(FougeritePlayer.SteamID);
                return;
            }

//            if (assetName != "SmallLadder" && assetName != "BigLadder")
//            {
            var structureComponents = new List<StructureComponent>();
            foreach (var collider in Facepunch.MeshBatch.MeshBatchPhysics.OverlapSphere(position, 2))
            {
                var structureComponent = collider.GetComponent<StructureComponent>();
                if (structureComponent != null)
                {
                    structureComponents.Add(structureComponent);
                }
            }

            foreach (var structureComponent in structureComponents)
            {
                if (structureComponent._master.creatorID.ToString() == FougeritePlayer.SteamID) continue;

                FougeritePlayer.Notice("You cannot place something near a structure from another player!");
                return;
            }
//            }
//            else
//            {
//                var nearStructure = false;
//
////                var structureComponents = new List<StructureComponent>();
//                foreach (var collider in Facepunch.MeshBatch.MeshBatchPhysics.OverlapSphere(position, 15))
//                {
//                    var structureComponent = collider.GetComponent<StructureComponent>();
//                    if (structureComponent != null)
//                    {
//                        nearStructure = true;
//                        break;
//                    }
//                }
//
//                if (!nearStructure)
//                {
//                    FougeritePlayer.Notice("You need to be close to a structure to place this!");
//                    return;
//                }
//            }

            if (assetName == "WaterBridge" && !nearWater)
            {
                FougeritePlayer.Notice("You need to be near the water to place this!");
                return;
            }

            if (assetName == "Turret" ||
                assetName == "Turret_Silencer" ||
                assetName == "Turret_Range" ||
                assetName == "Turret_Damage")
            {
                var nearStructure = false;
                var nearOtherPlayerStructure = false;

                foreach (var collider in Facepunch.MeshBatch.MeshBatchPhysics.OverlapSphere(position, 4))
                {
                    var structureComponent = collider.GetComponent<StructureComponent>();
                    if (structureComponent != null)
                    {
                        nearStructure = true;
                        if (structureComponent._master.creatorID.ToString() != FougeritePlayer.SteamID)
                        {
                            nearOtherPlayerStructure = true;
                            break;
                        }
                    }
                }

                if (!nearStructure || nearOtherPlayerStructure)
                {
                    FougeritePlayer.Notice("You need to be close to one of your own structures to place this!");
                    return;
                }
            }

            var inventoryItem = SpawnManager.InventoryItems[FougeritePlayer.SteamID];

            int num = 1;
            if (inventoryItem.Consume(ref num))
            {
                inventoryItem.inventory.RemoveItem(inventoryItem.slot);
            }
            else
            {
                inventoryItem.inventory.MarkSlotDirty(inventoryItem.slot);
            }

            SpawnManager.InventoryItems.Remove(FougeritePlayer.SteamID);

            var distance = Vector3.Distance(FougeritePlayer.Location, position);
            if (distance <= 6)
            {
                SpawnManager.Spawn(new SpawnManager.SpawnableObject(0, FougeritePlayer.SteamID, bundleName, assetName,
                    position, rotation, size));

                if (assetName == "Landmine")
                {
                    if (WorldEditorServer.LandmineCount.TryGetValue(FougeritePlayer.SteamID, out var count))
                    {
                        FougeritePlayer.Notice("Placed " + count + "/" + SpawnManager.MaxLandmineCount + " landmines!");
                    }
                }
                else if (assetName.StartsWith("Turret"))
                {
                    if (WorldEditorServer.TurretCount.TryGetValue(FougeritePlayer.SteamID, out var count))
                    {
                        FougeritePlayer.Notice("Placed " + count + "/" + SpawnManager.MaxTurretCount + " turrets!");
                    }
                }
            }
        }

        [RPC]
        public void DebugLog(string s)
        {
            Logger.Log("[ClientDebugLog]: " + s);
        }

        public void RustNotice(string s, float duration)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("RustNotice", FougeritePlayer.NetworkPlayer, s, duration);
        }

        public void PlayNukeSound()
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("PlayNukeSound", FougeritePlayer.NetworkPlayer);            
        }
    }
}