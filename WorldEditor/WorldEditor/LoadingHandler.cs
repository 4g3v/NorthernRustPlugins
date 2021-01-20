using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using RustBuster2016.API;
using uLink;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using WorldEditor.CustomStuff;

namespace WorldEditor
{
    public class LoadingHandler : MonoBehaviour
    {
        public delegate void FinishedEventHandler();

        public event FinishedEventHandler Finished;

        public static Dictionary<string, AssetBundle> bundleDictionary = new Dictionary<string, AssetBundle>();

        public GameObject ourobject;
        public LoadObjectFromBundle Spawner;
        public List<LoadObjectFromBundle> SetObjects = new List<LoadObjectFromBundle>();

        public bool _showSplash;
        private Texture2D _splash;
        private Texture2D _hudTexture;

        private int _loadedBundles = 0;
        private int _spawnedAssets = 0;
        private int _maxBundles = 0;
        private int _maxAssets = 0;

        private WWW _currentWWW;
//        private string _progressString;
        private bool isCached;
//        private float progress;

        private void Start()
        {
            var firstBundle = WorldEditor.Bundles[0].Split('|');

//            isCached = Caching.IsVersionCached(firstBundle[1], 1);
//            var s = "Downloading " + firstBundle[0] + ".unity3d - ";
//            _progressString = !isCached ? (s + "PERCENT%\n") : (s + "100%\n");

            _splash = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            _splash.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory +
                                                "\\RB_Data\\WorldEditor\\Splash.png"));
            
            _hudTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            _hudTexture.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory +
                                                    "\\RB_Data\\WorldEditor\\Hud.png"));

            _loadedBundles = 0;
            _spawnedAssets = 0;
            _showSplash = true;
        }

//        private void Update()
//        {
//            if (_currentWWW != null && !_currentWWW.isDone)
//            {
//                progress = _currentWWW.progress * 100;
//            }
//            else
//            {
//                progress = 0;
//            }
//        }

        private void OnGUI()
        {
            var h = Screen.height;

            if (_showSplash)
            {
                GUI.DrawTexture(new Rect(4, (h / 2) - (_splash.height / 2), _splash.width, _splash.height),
                    _splash);

                if (_maxBundles != 0)
                {
//                    Rect labelRect = GUILayoutUtility.GetRect(new GUIContent(_progressString), "label");

                    GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
                    boxStyle.normal.textColor = Color.green;
                    GUI.Box(new Rect(4, 4, 310, 24),
                        "Loading all needed components, please wait.", boxStyle);
//                    GUI.Box(new Rect(4, 4, 310, 24),
//                        "Loading all needed components, please wait. (" + _loadedBundles + "/" + _maxBundles + ")", boxStyle);

//                    labelRect.x = 8;
//                    labelRect.y = 26;
//
//                    GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
//                    labelStyle.normal.textColor = Color.yellow;
//
//                    GUI.Label(labelRect, _progressString.Replace("PERCENT", progress.ToString()), labelStyle);
                }
            }
            else
            {
                GUI.DrawTexture(new Rect(6, (h / 2) - _hudTexture.height / 2, _hudTexture.width, _hudTexture.height), _hudTexture);
            }
        }

        private void LoadAllSetObjects(string bundle)
        {
            if (!File.Exists(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\ClientSideAssets.txt"))
            {
                return;
            }

            var clientSideAssets =
                File.ReadAllLines(RustBuster2016.API.Hooks.GameDirectory +
                                  "\\RB_Data\\WorldEditor\\ClientSideAssets.txt");
            _maxAssets = clientSideAssets.Length;

            foreach (string line in clientSideAssets)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                try
                {
                    string[] pares = line.Split(':');

                    if (pares[0] != bundle)
                        continue;

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
                    LoadObjectFromBundle SpawnedObject =
                        TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();
                    SpawnedObject.Create(nombre, bundle, new Vector3(posx, posy, posz),
                        new Quaternion(quax, quay, quaz, quaw),
                        new Vector3(sizx, sizy, sizz));
                    SetObjects.Add(SpawnedObject);
                    UnityEngine.Object.DontDestroyOnLoad(TempGameObject);

                    _spawnedAssets++;
                }
                catch
                {
                    RustBuster2016.API.Hooks.LogData("Error", "Failure to load: " + line);
                }
            }
        }

        public class MoviePlay : MonoBehaviour
        {
            private void Start()
            {
                var movieTexture = ((MovieTexture)GetComponent<Renderer>().material.mainTexture);
                movieTexture.loop = true;
                movieTexture.Play();
            }
        }
        
        public IEnumerator LoadAsset()
        {
            _maxBundles = WorldEditor.Bundles.Count;

//            var stopwatch = new Stopwatch();
            foreach (var bundleString in WorldEditor.Bundles)
            {
                if (string.IsNullOrEmpty(bundleString))
                    continue;
                var bundles = bundleString.Split('|');
//                RustBuster2016.API.Hooks.LogData("WorldEditor", "Downloading and loading " + bundles[1]);

//                stopwatch.Start();
//                isCached = Caching.IsVersionCached(bundles[1], 1);
//                var s = "Downloading " + bundles[0] + ".unity3d - ";
//                _progressString += !isCached ? (s + "PERCENT%\n") : (s + "100%\n");

//                Directory.CreateDirectory(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\Test\\");
//                var fileName = RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\Test\\" + bundles[0] + ".unity3d";
//                new WebClient().DownloadFile(bundles[1], fileName);
//                stopwatch.Stop();
//                File.AppendAllText("WE.log", "Downloading took " + stopwatch.ElapsedMilliseconds + "ms\n");
//                stopwatch.Reset();
                
//                stopwatch.Start();
                _currentWWW = WWW.LoadFromCacheOrDownload(bundles[1], 1);
//                _currentWWW = WWW.LoadFromCacheOrDownload("file://" + fileName, 1);
                while (_currentWWW.progress < 1)
                {
                    yield return _currentWWW;
                }

                if (_currentWWW.error != null)
                {
                    RustBuster2016.API.Hooks.LogData("WorldEditor", "www: " + _currentWWW.error);
                    yield return null;
                }
//                stopwatch.Stop();
//                File.AppendAllText("WE.log", "LoadFromCacheOrDownload() took " + stopwatch.ElapsedMilliseconds + "ms\n");                
//                stopwatch.Reset();

//                _progressString = _progressString.Replace("PERCENT", progress != 0 ? progress.ToString() : "100");
//                _progressString += "Loading " + bundles[0] + ".unity3d\n";

                var bundle = _currentWWW.assetBundle;
                bundleDictionary.Add(bundles[0], bundle);

                _currentWWW.Dispose();
                _currentWWW = null;                
            }

            foreach (var bundleName in bundleDictionary.Keys)
            {
                var bundle = bundleDictionary[bundleName];
                
//                stopwatch.Start();
                ourobject = new GameObject();
                Spawner = ourobject.AddComponent<LoadObjectFromBundle>();
                UnityEngine.Object.DontDestroyOnLoad(ourobject);
                foreach (UnityEngine.Object item in bundle.LoadAll())
                {
                    if (item.name.Equals("NewSupplyPrefab"))
                    {
                        if (item is GameObject)
                        {
                            var _ObjectInstantiate = ((GameObject) item);
//                            _ObjectInstantiate = (GameObject) Instantiate(_ObjectInstantiate, Vector3.zero, Quaternion.identity);
                            var newSupplyDropPlane = _ObjectInstantiate.AddComponent<NewSupplyDropPlane>();
//                            newSupplyDropPlane.propellers = new GameObject[0];
                            newSupplyDropPlane.maxSpeed = 250;
                            newSupplyDropPlane.TEMP_numCratesToDrop = 3;
                            var newSupplyNetworkView = _ObjectInstantiate.AddComponent<uLinkNetworkView>();
                            newSupplyNetworkView.gameObject.name = "C130";
                            newSupplyNetworkView.observed = newSupplyDropPlane;
                            newSupplyNetworkView.rpcReceiver = RPCReceiver.ThisGameObject;
                            newSupplyNetworkView.prefabRoot = _ObjectInstantiate;
                            var newTransformInterpolator = _ObjectInstantiate.AddComponent<TransformInterpolator>();
                            newTransformInterpolator.allowDifference = 0.1f;
                            newTransformInterpolator.exterpolate = false;
                            newTransformInterpolator.target = _ObjectInstantiate.transform;
//                            var firstNetDoppler = _ObjectInstantiate.transform.FindChild("uh60").gameObject.AddComponent<NetDoppler>();
//                            firstNetDoppler.minPitch = 0.75f;
//                            var secondNetDoppler = _ObjectInstantiate.transform.FindChild("uh602").gameObject.AddComponent<NetDoppler>();
//                            secondNetDoppler.minPitch = 0.75f;

                            var allField = Type
                                .GetType(
                                    "NetCull+AutoPrefabs, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
                                .GetField("all");
                            Dictionary<string, uLinkNetworkView> allDictionary =
                                (Dictionary<string, uLinkNetworkView>) allField.GetValue(null);

                            allDictionary.Remove("C130");
                            allDictionary.Add("C130", newSupplyNetworkView);

                            allField.SetValue(null, allDictionary);
                            
                            NetworkInstantiator.Remove("C130");
                            NetworkInstantiator.AddPrefab(newSupplyNetworkView.gameObject);
                            
                            Hooks.LogData("WorldEditor", "Replaced supply.");
                        }

                        continue;
                    }

                    if (item.name.ToLower().Contains("movie"))
                    {
                        if (item is GameObject)
                        {
                            var movieGo = (GameObject) item;
                            movieGo.AddComponent<MoviePlay>();
//                            Debug.Log("Added MoviePlay");
                        }
                    }

                    if (item.name.ToLower().Contains("mat") || item.name.ToLower().Contains("avatar") ||
                        item.name.ToLower().Contains("img"))
                    {
                        continue;
                    }

//                    GameObject go = new GameObject();
//                    LoadObjectFromBundle lo = go.AddComponent<LoadObjectFromBundle>();
//
//                    Vector3 scale = new Vector3(1, 1, 1);
//                    if (item is GameObject)
//                    {
//                        var itemGo = item as GameObject;
//                        if (itemGo.transform != null)
//                        {
//                            scale = transform.localScale;
//                        }
//                    }
//
//                    bool b = lo.Create(item.name, bundleName, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), scale);
//                    if (!b)
//                    {
//                        UnityEngine.Object.Destroy(go);
//                        continue;
//                    }
//
//                    UnityEngine.Object.Destroy(lo.ObjectInstantiate);
//                    UnityEngine.Object.Destroy(go);

                    if (!WorldEditor.Instance.Prefabs.Contains(item.name))
                    {
                        WorldEditor.Instance.Prefabs.Add(item.name);
                    }

                    if (!WorldEditor.Instance.PrefabBundleDictionary.ContainsKey(item.name))
                        WorldEditor.Instance.PrefabBundleDictionary.Add(item.name, bundleName);
                }

//                stopwatch.Stop();
//                File.AppendAllText("WE.log", "Creating objects took " + stopwatch.ElapsedMilliseconds + "ms\n");
//                stopwatch.Reset();
                
//                stopwatch.Start();
                LoadAllSetObjects(bundleName);
//                stopwatch.Stop();
//                File.AppendAllText("WE.log", "LoadAllSetObjects() took " + stopwatch.ElapsedMilliseconds + "ms\n");
//                stopwatch.Reset();
                
//                _loadedBundles++;
            }
            
            WorldEditor.Instance.Editor = WorldEditor.Instance.MainHolder.AddComponent<Editor>();

            _showSplash = false;
            WorldEditor.Instance.SendMessageToServer("worldedit_finished");
            Finished?.Invoke();
            yield return null;
        }

        public class LoadObjectFromBundle : MonoBehaviour
        {
            private GameObject _ObjectInstantiate;
            private string _name;
            private string _bundleName;
            private Vector3 _pos;
            private Quaternion _rot;
            private Vector3 _siz;

            public bool Create(string name, string bundleName, Vector3 pos, Quaternion rot, Vector3 siz)
            {
                _name = name;
                _bundleName = bundleName;
                _pos = pos;
                _rot = rot;
                _siz = siz;
                try
                {
                    _ObjectInstantiate = bundleDictionary[_bundleName].Load(_name, typeof(GameObject)) as GameObject;
                    if (_ObjectInstantiate != null)
                    {
                        _ObjectInstantiate.transform.localScale = siz;
                        _ObjectInstantiate = (GameObject) Instantiate(_ObjectInstantiate, _pos, _rot);
                        _ObjectInstantiate.transform.localScale = siz;
                        WorldEditor.Instance.AllSpawnedObjects.Add(this);
                        //CustomObjectIdentifier id = _ObjectInstantiate.collider.gameObject.AddComponent<CustomObjectIdentifier>();
                        //id.BundleClass = this;
                        return true;
                    }
                }
                catch
                {
                    // ignore
                }

                return false;
            }

            public GameObject ObjectInstantiate
            {
                get { return _ObjectInstantiate; }
            }

            public string Name
            {
                get { return _name; }
            }

            public string BundleName
            {
                get { return _bundleName; }
            }

            public Vector3 Position
            {
                get { return _pos; }
            }

            public Quaternion Rotation
            {
                get { return _rot; }
            }

            public Vector3 Size
            {
                get { return _siz; }
            }
        }
    }
}