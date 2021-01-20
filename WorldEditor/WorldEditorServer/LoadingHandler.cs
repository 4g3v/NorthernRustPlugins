using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Fougerite;
using uLink;
using UnityEngine;
using WorldEditorServer.CustomStuff;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace WorldEditorServer
{
    public class LoadingHandler : MonoBehaviour
    {
        public static Stopwatch Stopwatch = new Stopwatch();
        public static Dictionary<string, AssetBundle> bundleDictionary = new Dictionary<string, AssetBundle>();

        private void LoadAllSetObjects(string bundle)
        {
            Stopwatch watch = null;
            if (WorldEditorServer.PerformanceLog)
                watch = new Stopwatch();

            foreach (string line in File.ReadAllLines(Util.GetRootFolder() +
                                                      "\\Save\\WorldEditorServer\\ClientSideAssets.txt"))
            {
                if (WorldEditorServer.PerformanceLog)
                    watch.Start();

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
                    UnityEngine.Object.DontDestroyOnLoad(TempGameObject);

                    if (WorldEditorServer.PerformanceLog)
                    {
                        watch.Stop();
                        WorldEditorServer.AddPerfLog("Spawning asset \"" + nombre + "\" took " +
                                                     watch.ElapsedMilliseconds / 1000f + "s");
                        watch.Reset();
                    }
                }
                catch
                {
                    Logger.LogError("[WorldEditorServer] Failure to load: " + line);
                }
            }
        }

        public IEnumerator LoadAsset()
        {
            foreach (var bundleString in WorldEditorServer.Bundles)
            {
                if (string.IsNullOrEmpty(bundleString))
                    continue;
                var bundles = bundleString.Split('|');

                if (WorldEditorServer.PerformanceLog)
                {
                    Stopwatch.Start();
                }

                WWW www = WWW.LoadFromCacheOrDownload(bundles[1], 1);
                yield return www;
                if (www.error != null)
                {
                    Logger.LogError("[WorldEditorServer] Failure to load www: " + www.error);
                }

                var bundle = www.assetBundle;
                bundleDictionary.Add(bundles[0], bundle);

                www.Dispose();

                foreach (UnityEngine.Object item in bundle.LoadAll())
                {
                    if (item.name.Equals("NewSupplyPrefab"))
                    {
                        if (item is GameObject)
                        {
                            Logger.Log(item.name);
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

                            Logger.Log("Set new Supply.");
                        }
                    }
                }

                if (WorldEditorServer.PerformanceLog)
                {
                    Stopwatch.Stop();
                    WorldEditorServer.AddPerfLog("Loading bundle \"" + bundles[0] + "\" took " +
                                                 Stopwatch.ElapsedMilliseconds / 1000f + "s");
                    Stopwatch.Reset();
                    Stopwatch.Start();
                }

                LoadAllSetObjects(bundles[0]);

                if (WorldEditorServer.PerformanceLog)
                {
                    Stopwatch.Stop();
                    WorldEditorServer.AddPerfLog("Spawning assets took " + Stopwatch.ElapsedMilliseconds / 1000f + "s");
                    Stopwatch.Reset();
                }
            }

            SpawnManager.LoadSpawnedObjects();
            NukeManager.Start();

            Logger.Log("Loaded all custom objects.");
        }

        public class LoadObjectFromBundle : MonoBehaviour
        {
            private GameObject _ObjectInstantiate;
            private string _name;
            private Vector3 _pos;
            private Quaternion _rot;
            private Vector3 _siz;

            public bool Create(string name, string bundleName, Vector3 pos, Quaternion rot, Vector3 siz)
            {
                _name = name;
                _pos = pos;
                _rot = rot;
                _siz = siz;
                var assetBundle = bundleDictionary[bundleName];
                _ObjectInstantiate = assetBundle.Load(_name, typeof(GameObject)) as GameObject;
                if (_ObjectInstantiate != null)
                {
                    _ObjectInstantiate.transform.localScale = siz;
                    _ObjectInstantiate = (GameObject) Instantiate(_ObjectInstantiate, _pos, _rot);
                    return true;
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