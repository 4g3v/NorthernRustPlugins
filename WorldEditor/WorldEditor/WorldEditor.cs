using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using RustBuster2016;
using RustBuster2016.API;
using RustBuster2016.API.Events;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Hooks = RustBuster2016.API.Hooks;
using Object = UnityEngine.Object;

namespace WorldEditor
{
    public class WorldEditor : RustBuster2016.API.RustBusterPlugin
    {
        private string AAAAAAAA =>
            "Fuck off Autori and everyone else dumping this plugin. " +
            "I bet you aren't able to properly implement the server " +
            "side stuff anyway. If you would be able to you wouldn't just " +
            "want to copy my stuff (to anyone actually involved in the making of one, " +
            "like Dretax or Salva, you guys could have just asked me), that's a dickmove imo   ~Sincerely 4g3v";

        public override string Name => "WorldEditor";
        public override string Author => "4g3v, DreTaX & Salva";
        public override Version Version => new Version("4.2.0");

        private static WorldEditor _inst;
        public static WorldEditor Instance => _inst;

        private bool IsAdmin = false;
        internal bool Enabled = false;

        public IniParser SettingsParser;
        public static List<string> Bundles = new List<string>();

        public GameObject MainHolder;
        public LoadingHandler Handler;
        public TimedSpawns TimedSpawns_;
        public Editor Editor;
        public List<string> Prefabs = new List<string>();
        public Dictionary<string, string> PrefabBundleDictionary = new Dictionary<string, string>();

        public List<LoadingHandler.LoadObjectFromBundle> AllSpawnedObjects =
            new List<LoadingHandler.LoadObjectFromBundle>();

        public string SavedObjectsPath;
        private string _bundleLink;
        private DestroyAfterSecondsClass _dasc;

        private GameObject _waitObject;
        private RPCBehaviour _rpcBehaviour;
        private string _worldeditorTxtURL;

        public static List<uint> LastTurretIDs = new List<uint>();

//        private Dictionary<string, string> _missingBundlesDict = new Dictionary<string, string>();
//        private List<string> _missingBundles;
//        private bool _openedLinks;


        public override void DeInitialize()
        {
            try
            {
                IsAdmin = false;
                Enabled = false;
                RustBuster2016.API.Hooks.OnRustBusterClientPluginsLoaded -= OnRustBusterClientPluginsLoaded;
                RustBuster2016.API.Hooks.OnRustBusterClientChat -= OnRustBusterClientChat;
                foreach (var x in AllSpawnedObjects)
                {
                    if (x.ObjectInstantiate != null)
                    {
                        UnityEngine.Object.Destroy(x.ObjectInstantiate);
                    }
                }

                UnityEngine.Object.Destroy(MainHolder);
                AllSpawnedObjects.Clear();
                TimedSpawns_.TimedObjects.Clear();
                PrefabBundleDictionary.Clear();
//                _missingBundleDict.Clear();
//                _missingBundles.Clear();
                foreach (var bundle in LoadingHandler.bundleDictionary.Values)
                {
                    bundle.Unload(true);
                }

                Bundles.Clear();
                LoadingHandler.bundleDictionary.Clear();
//                Caching.expirationDelay = 1;
//                Caching.CleanCache();
            }
            catch
            {
            }
        }

        public override void Initialize()
        {
//            var AssetPath_ = "file:///";
            _inst = this;
            if (!Directory.Exists(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor"))
            {
                Directory.CreateDirectory(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor");
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var webClient = new WebClient();
            _worldeditorTxtURL =
                true ? "http://absolomrust.ddnss.org/WorldEditor.txt" : "http://192.168.0.199/WorldEditor.txt";

            var filesTxt = webClient.DownloadString(_worldeditorTxtURL);
            foreach (var s in filesTxt.Split(new[] {Environment.NewLine}, StringSplitOptions.None))
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                var fileAndOverwrite = s.Split('|');

                var fileName = RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\" +
                               Path.GetFileName(fileAndOverwrite[0]);

                if (File.Exists(fileName) && !bool.Parse(fileAndOverwrite[1]))
                    continue;

                webClient.DownloadFile(fileAndOverwrite[0], fileName);
            }

            stopwatch.Stop();
            Debug.Log("Downloading WorldEditor files took " + stopwatch.ElapsedMilliseconds + " ms");

            SavedObjectsPath = RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\Objects.txt";
            if (!File.Exists(SavedObjectsPath))
            {
                File.Create(SavedObjectsPath).Dispose();
            }

            RustBuster2016.API.Hooks.OnRustBusterClientPluginsLoaded += OnRustBusterClientPluginsLoaded;
            RustBuster2016.API.Hooks.OnRustBusterClientChat += OnRustBusterClientChat;
            RustBuster2016.API.Hooks.OnRustBusterClientConsole += OnRustBusterClientConsole;
            Hooks.OnRustBusterWeaponFire += OnRustBusterWeaponFire;
            Hooks.OnRustBusterClientDeathScreen += OnRustBusterClientDeathScreen;

            SettingsParser =
                new IniParser(@RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\Settings.ini");

//            _missingBundles = new List<string>();
//            var bundlesString = SettingsParser.GetSetting("Settings", "AssetBundles");
            _bundleLink = SettingsParser.GetSetting("Settings", "BundleDownloadLink");

            var bundleLinkSplitted = _bundleLink.Split('|');
            foreach (var bundleLinkEntryString in bundleLinkSplitted)
            {
                if (string.IsNullOrEmpty(bundleLinkEntryString))
                    continue;
                var bundleLinkEntry = bundleLinkEntryString.Split('§');
                Bundles.Add(bundleLinkEntry[0] + "|" + bundleLinkEntry[1]);
            }

//            if (!string.IsNullOrEmpty(bundlesString))
//            {
//                foreach (var bundle in bundlesString.Split(','))
//                {
//                    if (string.IsNullOrEmpty(bundle))
//                        continue;

//                    var bundlePath = (AssetPath_ + @RustBuster2016.API.Hooks.GameDirectory +
//                                     "\\RB_Data\\WorldEditor\\" + bundle + ".unity3d");
//                    if (!File.Exists(bundlePath.Substring(8)))
//                    {
//                        _missingBundles.Add(bundle);
//                        continue;
//                    }
//                    Bundles.Add(bundle + "|" + bundlePath);
//                }
//            }

//            if (_missingBundles.Count != 0)
//            {
//                Rust.Notice.Popup("", "The following bundles are missing: " + string.Join(", ", _missingBundles.ToArray()), 30);
//                Hooks.OnRustBusterClientMove += HooksOnOnRustBusterClientMove;
//                return;
//            }

            MainHolder = new GameObject();
            Handler = MainHolder.AddComponent<LoadingHandler>();
            TimedSpawns_ = MainHolder.AddComponent<TimedSpawns>();
            UnityEngine.Object.DontDestroyOnLoad(MainHolder);
        }

        private void OnRustBusterClientDeathScreen(DeathScreenEvent deathScreenEvent)
        {
            foreach (var lastTurretID in LastTurretIDs)
            {
                _rpcBehaviour.TurretBehaviour_SetTargetInRange(lastTurretID, false);
            }

            LastTurretIDs.Clear();
        }

        private void OnRustBusterWeaponFire(BulletWeaponFireEvent bulletWeaponFireEvent)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            Physics.Raycast(ray, out var hit);
            if (hit.transform == null)
                return;

            var spawnableObjectBehaviour = hit.transform.GetComponent<SpawnableObjectBehaviour>();

            if (spawnableObjectBehaviour == null)
            {
                spawnableObjectBehaviour = hit.transform.GetComponentInParent<SpawnableObjectBehaviour>();

                if (spawnableObjectBehaviour == null)
                {
                    spawnableObjectBehaviour = hit.transform.GetComponentInChildren<SpawnableObjectBehaviour>();

                    if (spawnableObjectBehaviour == null)
                        return;
                }
            }

            if (spawnableObjectBehaviour.SpawnableObject.Name == "Turret" ||
                spawnableObjectBehaviour.SpawnableObject.Name == "Turret_Silencer" ||
                spawnableObjectBehaviour.SpawnableObject.Name == "Turret_Range" ||
                spawnableObjectBehaviour.SpawnableObject.Name == "Turret_Damage")
            {
                _rpcBehaviour.TurretBehaviour_Damage(spawnableObjectBehaviour.SpawnableObject.ID);
            }
        }

        public void AddRPC()
        {
            if (_waitObject != null)
            {
                Object.Destroy(_waitObject);
            }

            try
            {
                Handler.StartCoroutine(Handler.LoadAsset());
            }
            catch (Exception ex)
            {
                RustBuster2016.API.Hooks.LogData("WorldEditor", "Exception: " + ex);
            }

            _rpcBehaviour = PlayerClient.GetLocalPlayer().gameObject.AddComponent<RPCBehaviour>();
        }

//        private void HooksOnOnRustBusterClientMove(HumanController hc, Character ch, int num)
//        {
//            if (!_openedLinks)
//            {
//                foreach (var missingBundle in _missingBundles)
//                {
//                    Process.Start(_missingBundleDict[missingBundle]);
//                }

//                _openedLinks = true;
//            }

//            SendMessageToServer("worldedit_kick");
//        }

        private void OnRustBusterClientConsole(string msg)
        {
            if (msg.Contains("worldedit_spawn"))
            {
                var splitted = msg.Split('|');
                splitted[1] = Base64Decode(splitted[1]);

                string[] pares = splitted[1].Split(':');

                var nombre = pares[1];
                var loc = pares[2];
                var qua = pares[3];
                var siz = pares[4];

                // Position
                string[] locsplit = loc.ToString().Split(',');
                float posx = float.Parse(locsplit[0]);
                float posy = float.Parse(locsplit[1]);
                float posz = float.Parse(locsplit[2]);

                // Quaternion
                string[] quasplit = qua.ToString().Split(',');
                float quax = float.Parse(quasplit[0]);
                float quay = float.Parse(quasplit[1]);
                float quaz = float.Parse(quasplit[2]);
                float quaw = float.Parse(quasplit[3]);

                // Size
                string[] sizsplit = siz.ToString().Split(',');
                float sizx = float.Parse(sizsplit[0]);
                float sizy = float.Parse(sizsplit[1]);
                float sizz = float.Parse(sizsplit[2]);

                GameObject TempGameObject = new GameObject();
                LoadingHandler.LoadObjectFromBundle SpawnedObject =
                    TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();
                SpawnedObject.Create(nombre, pares[0], new Vector3(posx, posy, posz),
                    new Quaternion(quax, quay, quaz, quaw),
                    new Vector3(sizx, sizy, sizz));
                UnityEngine.Object.DontDestroyOnLoad(TempGameObject);
            }
            else if (msg.Contains("worldedit_timedspawn"))
            {
                if (_dasc != null)
                {
                    if (_dasc.StillAlive)
                        return;
                }

                var splitted = msg.Split('|');
                splitted[2] = Base64Decode(splitted[2]);

                string[] pares = splitted[2].Split(':');

                var nombre = pares[1];
                var loc = pares[2];
                var qua = pares[3];
                var siz = pares[4];

                // Position
                string[] locsplit = loc.ToString().Split(',');
                float posx = float.Parse(locsplit[0]);
                float posy = float.Parse(locsplit[1]);
                float posz = float.Parse(locsplit[2]);

                // Quaternion
                string[] quasplit = qua.ToString().Split(',');
                float quax = float.Parse(quasplit[0]);
                float quay = float.Parse(quasplit[1]);
                float quaz = float.Parse(quasplit[2]);
                float quaw = float.Parse(quasplit[3]);

                // Size
                string[] sizsplit = siz.ToString().Split(',');
                float sizx = float.Parse(sizsplit[0]);
                float sizy = float.Parse(sizsplit[1]);
                float sizz = float.Parse(sizsplit[2]);

                GameObject TempGameObject = new GameObject();
                LoadingHandler.LoadObjectFromBundle SpawnedObject =
                    TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();
                SpawnedObject.Create(nombre, pares[0], new Vector3(posx, posy, posz),
                    new Quaternion(quax, quay, quaz, quaw),
                    new Vector3(sizx, sizy, sizz));
                UnityEngine.Object.DontDestroyOnLoad(TempGameObject);
//                Debug.Log("Spawned.");

                _dasc = new DestroyAfterSecondsClass(SpawnedObject, int.Parse(splitted[1]));
                TimedSpawns_.StartCoroutine("DestroyAfterSeconds",
                    _dasc);
                _dasc.StillAlive = true;
            }
        }

        public class DestroyAfterSecondsClass
        {
            public LoadingHandler.LoadObjectFromBundle GameObject;
            public int Seconds;
            public bool StillAlive = false;

            public DestroyAfterSecondsClass(LoadingHandler.LoadObjectFromBundle gameObject, int seconds)
            {
                this.GameObject = gameObject;
                this.Seconds = seconds;
            }
        }

        private void OnRustBusterClientChat(ChatEvent ce)
        {
            var text = ce.ChatUI.textInput.Text;
            if (text.Contains("/worldedit"))
            {
                if (!IsAdmin)
                {
                    return;
                }

                if (text == "/worldedit")
                {
                    Enabled = !Enabled;
                    Rust.Notice.Inventory("", "WorldEdit Toggled: " + Enabled);
                }
                else
                {
                    var splitted = text.Split(' ');
                    switch (splitted[1])
                    {
                        case "spawn":
                            SendMessageToServer("worldedit_spawn|" + Base64Encode(splitted[2]));
                            break;
                    }
                }
            }
            else if (text.StartsWith("/fly") && IsAdmin)
            {
                Editor._fly = !Editor._fly;
                if (Editor._fly)
                {
                    Editor.localPlayerClient = PlayerClient.GetLocalPlayer();
                    Editor.controllable = Editor.localPlayerClient.controllable;
                    Editor.localCharacter = Editor.controllable.character;
                    Editor.localController = Editor.localCharacter.controller as HumanController;
                }
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void OnRustBusterClientPluginsLoaded()
        {
            _waitObject = new GameObject();
            _waitObject.AddComponent<CharacterWaiter>();
            Object.DontDestroyOnLoad(_waitObject);

            IsAdmin = this.SendMessageToServer("worldeditadmin").Contains("yes");
        }
    }
}