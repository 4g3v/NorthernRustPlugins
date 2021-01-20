using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Fougerite;
using Fougerite.Events;
using RustBuster2016Server;
using UnityEngine;
using Object = UnityEngine.Object;
using Server = Fougerite.Server;

namespace WorldEditorServer
{
    public class WorldEditorServer : Module
    {
        private bool _FoundRB;
        public GameObject MainHolder;
        public LoadingHandler Handler;

        public IniParser SettingsParser;
        public static List<string> Bundles = new List<string>();
        public static bool PerformanceLog;
        private List<Zone> _zones = new List<Zone>();

        public static Dictionary<string, RPCBehaviour> RPCDictionary = new Dictionary<string, RPCBehaviour>();

        public static Dictionary<string, Vector3> RespawnFlags = new Dictionary<string, Vector3>();
        public static List<string> RespawnFlagKillList = new List<string>();
        public static Dictionary<string, int> LandmineCount = new Dictionary<string, int>();
        public static Dictionary<string, int> TurretCount = new Dictionary<string, int>();
        public static RustPPExtension RustPP;

//        TODO: AutoSpawn
//        private GameObject _autoSpawnGameObject;

        public override string Name => "WorldEditorServer";
        public override string Author => "4g3v, DreTaX, Salva";
        public override string Description => "WorldEditorServer";
        public override Version Version => new Version("1.1.1");

        public override void Initialize()
        {
            RustPP = new RustPPExtension();

            var AssetPath_ = "file:///";
            Caching.expirationDelay = 1;
            Caching.CleanCache();

            Hooks.OnModulesLoaded += OnModulesLoaded;
            Hooks.OnPlayerConnected += OnPlayerConnected;
            Hooks.OnPlayerDisconnected += OnPlayerDisconnected;
            Hooks.OnPlayerSpawned += OnPlayerSpawned;
            Hooks.OnPlayerKilled += OnPlayerKilled;
            Hooks.OnCommand += OnCommand;

            #region Settings

            if (!File.Exists(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\ClientSideAssets.txt"))
            {
                File.Create(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\ClientSideAssets.txt").Dispose();
            }

            if (!File.Exists(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\Settings.ini"))
            {
                File.Create(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\Settings.ini").Dispose();
                SettingsParser = new IniParser(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\Settings.ini");
                SettingsParser.AddSetting("Settings", "AssetBundles");
                SettingsParser.AddSetting("Settings", "TimedSpawns");
                SettingsParser.AddSetting("Settings", "PerformanceLog", "false");
                SettingsParser.AddSetting("Settings", "BundlesToTransfer");
                SettingsParser.AddSetting("Settings", "BundleDownloadLink");
                SettingsParser.Save();
            }

            if (SettingsParser == null)
                SettingsParser = new IniParser(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\Settings.ini");

            #endregion

            #region AssetBundles

            var bundlesString = SettingsParser.GetSetting("Settings", "AssetBundles");
            if (!string.IsNullOrEmpty(bundlesString))
            {
                foreach (var bundle in bundlesString.Split(','))
                {
                    if (string.IsNullOrEmpty(bundle))
                        continue;

                    Bundles.Add(bundle + "|" + AssetPath_ + Util.GetRootFolder() + "\\Save\\WorldEditorServer\\" +
                                bundle + ".unity3d");
                }
            }

            #endregion

            PerformanceLog = SettingsParser.GetBoolSetting("Settings", "PerformanceLog");

            #region ZoneSpawnAndDestroy

            var s = SettingsParser.GetSetting("Settings", "ZoneSpawnAndDestroy");
            if (!string.IsNullOrEmpty(s))
            {
                var dasEntries = s.Split('|');
                foreach (var dasEntry in dasEntries)
                {
                    if (string.IsNullOrEmpty(dasEntry))
                        continue;
                    var dasEntrySplitted = dasEntry.Split('/');
                    Vector3 location = Utils.StringToVector3(dasEntrySplitted[0]);
                    int secondsToDestroy = int.Parse(dasEntrySplitted[1]);
                    int radius = int.Parse(dasEntrySplitted[2]);
                    string prefab = dasEntrySplitted[3];

                    _zones.Add(new Zone(location, radius, secondsToDestroy, prefab));
                }
            }

            #endregion

//            var bundlesToTransferString = SettingsParser.GetSetting("Settings", "BundlesToTransfer");
//            if (!string.IsNullOrEmpty(bundlesToTransferString))
//            {
//                var bundlesToTransfer = bundlesToTransferString.Split(',');
//                foreach (var bundleToTransfer in bundlesToTransfer)
//                {
//                    if (string.IsNullOrEmpty(bundleToTransfer))
//                        continue;

//                    RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("WorldEditor\\", Util.GetRootFolder() + "\\Save\\WorldEditorServer\\" + bundleToTransfer + ".unity3d"));
//                }
//            }

//            TODO: AutoSpawn
//            var autoSpawnString = SettingsParser.GetSetting("Settings", "AutoSpawn");
//            if (!string.IsNullOrEmpty(autoSpawnString))
//            {
//                _autoSpawnGameObject = new GameObject();
//                var spawnBehaviour = _autoSpawnGameObject.AddComponent<AutoSpawnBehaviour>();
//                Object.DontDestroyOnLoad(_autoSpawnGameObject);
//
//                foreach (var autoSpawnEntryString in autoSpawnString.Split('|'))
//                {
//                    if (string.IsNullOrEmpty(autoSpawnEntryString))
//                    {
//                        continue;
//                    }
//
//                    if (!autoSpawnEntryString.Contains("/"))
//                    {
//                        string[] autoSpawnEntrySplit = autoSpawnEntryString.Split(':');
//
//                        var seconds = float.Parse(autoSpawnEntrySplit[0]);
//                        var destroySeconds = float.Parse(autoSpawnEntrySplit[1]);
//                        var bundle = autoSpawnEntrySplit[2];
//                        var name = autoSpawnEntrySplit[3];
//
//                        var locationString = autoSpawnEntrySplit[4].Split(',');
//                        Vector3 location = new Vector3(float.Parse(locationString[0]), float.Parse(locationString[1]),
//                            float.Parse(locationString[2]));
//
//                        var rotationString = autoSpawnEntrySplit[5].Split(',');
//                        Quaternion rotation = new Quaternion(float.Parse(rotationString[0]),
//                            float.Parse(rotationString[1]),
//                            float.Parse(rotationString[2]), float.Parse(rotationString[3]));
//
//                        var sizeString = autoSpawnEntrySplit[6].Split(',');
//                        Vector3 size = new Vector3(float.Parse(sizeString[0]), float.Parse(sizeString[1]),
//                            float.Parse(sizeString[2]));
//
//                        spawnBehaviour.StartCoroutine(
//                            spawnBehaviour.WaitAndSpawn(new AutoSpawnManager.SpawnedObject(seconds, destroySeconds,
//                                name,
//                                bundle, location,
//                                rotation,
//                                size)));
//
//                        Logger.Log("[WorldEditor] Started AutoSpawn Coroutine for " + name);
//                    }
//                    else
//                    {
//                        List<AutoSpawnManager.SpawnedObject>
//                            objectsToSpawn = new List<AutoSpawnManager.SpawnedObject>();
//                        foreach (var multiEntry in autoSpawnEntryString.Split('/'))
//                        {
//                            string[] autoSpawnEntrySplit = multiEntry.Split(':');
//
//                            var seconds = float.Parse(autoSpawnEntrySplit[0]);
//                            var destroySeconds = float.Parse(autoSpawnEntrySplit[1]);
//                            var bundle = autoSpawnEntrySplit[2];
//                            var name = autoSpawnEntrySplit[3];
//
//                            var locationString = autoSpawnEntrySplit[4].Split(',');
//                            Vector3 location = new Vector3(float.Parse(locationString[0]),
//                                float.Parse(locationString[1]),
//                                float.Parse(locationString[2]));
//
//                            var rotationString = autoSpawnEntrySplit[5].Split(',');
//                            Quaternion rotation = new Quaternion(float.Parse(rotationString[0]),
//                                float.Parse(rotationString[1]),
//                                float.Parse(rotationString[2]), float.Parse(rotationString[3]));
//
//                            var sizeString = autoSpawnEntrySplit[6].Split(',');
//                            Vector3 size = new Vector3(float.Parse(sizeString[0]), float.Parse(sizeString[1]),
//                                float.Parse(sizeString[2]));
//
//                            objectsToSpawn.Add(new AutoSpawnManager.SpawnedObject(seconds, destroySeconds, name, bundle,
//                                location, rotation, size));
//                        }
//
//                        spawnBehaviour.StartCoroutine(spawnBehaviour.WaitAndSpawn(objectsToSpawn));
//
//                        Logger.Log("[WorldEditor] Started multi AutoSpawn Coroutine for " + objectsToSpawn.Count +
//                                   " objects");
//                    }
//                }
//            }

//            API.AddFileToDownload(new RBDownloadable("WorldEditor\\",
//                Util.GetRootFolder() + "\\Save\\WorldEditorServer\\Settings.ini"));
//            API.AddFileToDownload(new RBDownloadable("WorldEditor\\",
//                Util.GetRootFolder() + "\\Save\\WorldEditorServer\\ClientSideAssets.txt"));
//            API.AddFileToDownload(new RBDownloadable("WorldEditor\\",
//                Util.GetRootFolder() + "\\Save\\WorldEditorServer\\Splash.png"));
//            API.AddFileToDownload(new RBDownloadable("WorldEditor\\",
//                Util.GetRootFolder() + "\\Save\\WorldEditorServer\\Hud.png"));

            MainHolder = new GameObject();
            Handler = MainHolder.AddComponent<LoadingHandler>();
            Object.DontDestroyOnLoad(MainHolder);
            try
            {
                Handler.StartCoroutine(Handler.LoadAsset());
            }
            catch (Exception ex)
            {
                Logger.LogError("Couroutine failed. " + ex);
            }

            if (_FoundRB)
            {
                API.OnRustBusterUserMessage += OnRustBusterUserMessage;
            }
        }

        private void OnPlayerKilled(DeathEvent de)
        {
            var player = ((Fougerite.Player) de.Victim);

            if (RespawnFlags.ContainsKey(player.SteamID))
                RespawnFlagKillList.Add(player.SteamID);
        }

        private void OnPlayerSpawned(Fougerite.Player player, SpawnEvent se)
        {
            if (RespawnFlagKillList.Contains(player.SteamID))
            {
                player.TeleportTo(RespawnFlags[player.SteamID]);

                RespawnFlagKillList.Remove(player.SteamID);
            }
        }

        private void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "destroyc")
            {
                player.Notice("Look at the object you want to destroy and left click, right click to cancel!");
                RPCDictionary[player.SteamID].WorldEditorStartDestroy();
            }
            else if (cmd == "destroycall" && player.Admin)
            {
                var ids = new List<uint>();
                foreach (var id in SpawnManager.SpawnedObjects.Keys)
                    ids.Add(id);

                foreach (var id in ids)
                    SpawnManager.RemoveObject(id, true);
            }
            else if (cmd == "destroycinfo" && player.Admin)
            {
                var colliders = Physics.OverlapSphere(player.Location, 5);
                for (var i = 0; i < colliders.Length; i++)
                {
                    var collider = colliders[i];

                    var spawnableObjectBehaviour = collider.GetComponent<SpawnManager.SpawnableObjectBehaviour>();
                    if (spawnableObjectBehaviour != null)
                    {
                        Logger.Log("SpawnableObject[" + spawnableObjectBehaviour.SpawnableObject.ID + "]: Position: " +
                                   spawnableObjectBehaviour.SpawnableObject.GetPosition() + " | Distance: " +
                                   Vector3.Distance(player.Location,
                                       spawnableObjectBehaviour.SpawnableObject.GetPosition()) + " | SteamID: " +
                                   spawnableObjectBehaviour.SpawnableObject.SteamID);
                    }
                }
            }
            else if (cmd == "sospawn" && player.Admin)
            {
                if (args.Length != 5)
                {
                    player.MessageFrom("WorldEditor", "/sospawn (BUNDLE) (OBJECTNAME) (POSITION) (ROTATION) (SIZE)");
                    return;
                }
                
                var bundle = args[0];
                var objectName = args[1];
                var position = Utils.StringToVector3(args[2]);
                var rotation = Utils.StringToQuaternion(args[3]);
                var size = Utils.StringToVector3(args[4]);
                
                var spawnableObject = new SpawnManager.SpawnableObject(0, player.SteamID, bundle, objectName,
                    position, rotation, size);
                SpawnManager.Spawn(spawnableObject);
            }
        }

        private void OnPlayerDisconnected(Fougerite.Player player)
        {
            if (RPCDictionary.ContainsKey(player.SteamID))
                RPCDictionary.Remove(player.SteamID);
            if (SpawnManager.InventoryItems.ContainsKey(player.SteamID))
                SpawnManager.InventoryItems.Remove(player.SteamID);
        }

        private void OnPlayerConnected(Fougerite.Player player)
        {
            var playerRPCBehaviour = player.PlayerClient.gameObject.GetComponent<RPCBehaviour>();
            if (playerRPCBehaviour == null)
                playerRPCBehaviour = player.PlayerClient.gameObject.AddComponent<RPCBehaviour>();

            playerRPCBehaviour.FougeritePlayer = player;

            if (!RPCDictionary.ContainsKey(player.SteamID))
            {
                RPCDictionary[player.SteamID] = playerRPCBehaviour;
            }
        }

        private void OnPlayerMove(HumanController hc, Vector3 origin, int encoded, ushort stateflags,
            uLink.NetworkMessageInfo info, Util.PlayerActions action)
        {
            foreach (var zone in _zones)
            {
                var distance = Vector3.Distance(origin, zone.Location);
                if (distance < zone.Radius)
                {
                    Fougerite.Player player = new Fougerite.Player(hc.playerClient);
                    player.SendConsoleMessage("worldedit_timedspawn|" + zone.SecondsToDestroy + "|" +
                                              Base64Encode(zone.Prefab));
                }
            }
        }

        public class Zone
        {
            public Vector3 Location;
            public int SecondsToDestroy;
            public int Radius;
            public string Prefab;

            public Zone(Vector3 location, int radius, int secondsToDestroy, string prefab)
            {
                Location = location;
                SecondsToDestroy = secondsToDestroy;
                Radius = radius;
                Prefab = prefab;
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

//        private void OnPlayerConnected(Fougerite.Player player)
//        {
//            this._protectedPlayers.Add(player.SteamID);
//        }

//        private void OnPlayerDisconnected(Fougerite.Player player)
//        {
//            if (_protectedPlayers.Contains(player.SteamID))
//                _protectedPlayers.Remove(player.SteamID);
//        }

        public static void AddPerfLog(string s)
        {
            if (!File.Exists(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\PerformanceLog.txt"))
                File.Create(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\PerformanceLog.txt").Dispose();
            File.AppendAllText(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\PerformanceLog.txt", s + "\r\n");
        }

//        private void OnPlayerHurt(HurtEvent he)
//        {
//            if (!he.VictimIsPlayer) return;
//            if (he.Victim == null) return;

//            var heVictim = (Fougerite.Player) he.Victim;
//            if (_protectedPlayers.Contains(heVictim.SteamID))
//            {
//                he.DamageAmount = 0;

//                if (!he.AttackerIsPlayer) return;
//                var attacker = (Fougerite.Player) he.Attacker;
//                attacker.Notice("This player is entering Absolom Rust, wait a bit.");
//            }
//        }

//        private void OnEntityHurt(HurtEvent he)
//        {
//            if (!he.VictimIsPlayer) return;
//            if (he.Victim == null) return;

//            var heVictim = (Fougerite.Player) he.Victim;
//            if (this._protectedPlayers.Contains(heVictim.SteamID))
//                he.DamageAmount = 0;
//        }


        public override void DeInitialize()
        {
            Hooks.OnModulesLoaded -= OnModulesLoaded;
            Hooks.OnPlayerConnected -= OnPlayerConnected;
            Hooks.OnPlayerDisconnected -= OnPlayerDisconnected;
            Hooks.OnCommand -= OnCommand;

            if (_FoundRB)
            {
                API.OnRustBusterUserMessage -= OnRustBusterUserMessage;
            }

            Object.Destroy(MainHolder);
            foreach (var bundle in LoadingHandler.bundleDictionary.Values)
            {
                bundle.Unload(true);
            }

            Caching.expirationDelay = 1;
            Caching.CleanCache();

//            TODO: AutoSpawn
//            _autoSpawnGameObject.GetComponent<AutoSpawnBehaviour>().StopAllCoroutines();
//            Object.Destroy(_autoSpawnGameObject);

//            _protectedPlayers.Clear();

//            Hooks.OnPlayerConnected -= OnPlayerConnected;
//            Hooks.OnPlayerDisconnected -= OnPlayerDisconnected;
//            Hooks.OnPlayerHurt -= OnPlayerHurt;
//            Hooks.OnEntityHurt -= OnEntityHurt;
        }

        private void OnModulesLoaded()
        {
            foreach (var x in ModuleManager.Modules)
            {
                if (x.Plugin.Name.ToLower().Contains("rustbuster"))
                {
                    _FoundRB = true;
                    API.OnRustBusterUserMessage += OnRustBusterUserMessage;
                    break;
                }
            }
        }

        private void OnRustBusterUserMessage(API.RustBusterUserAPI user, Message msgc)
        {
            if (msgc.PluginSender == "WorldEditor")
            {
                if (msgc.MessageByClient == "worldeditadmin")
                {
                    msgc.ReturnMessage = user.Player.Admin ? "yes" : "no";
                }
                else if (msgc.MessageByClient == "worldedit_finished")
                {
//                    this._protectedPlayers.Remove(user.SteamID);
//                    user.Player.Notice("", "All custom objects loaded. You are no longer protected!", 10);
                    user.Player.Notice("", "All custom objects loaded.", 10);
                }
                else if (msgc.MessageByClient.Contains("worldedit_spawn"))
                {
                    var splitted = msgc.MessageByClient.Split('|');
                    foreach (var player in Server.GetServer().Players)
                    {
                        player.SendConsoleMessage("worldedit_spawn|" + splitted[1]);
                    }
                }
                else if (msgc.MessageByClient == "worldedit_kick")
                {
                    user.Player.Disconnect();
                }
            }
        }
    }
}