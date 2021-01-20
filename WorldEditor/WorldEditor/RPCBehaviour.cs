using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RustBuster2016.API;
using UnityEngine;
using WorldEditor.ObjectBehaviours;
using Debug = UnityEngine.Debug;

namespace WorldEditor
{
    public class RPCBehaviour : MonoBehaviour
    {
        public uLink.NetworkView NetworkView;

        private Dictionary<uint, SpawnableObjectBehaviour> _spawnedObjects =
            new Dictionary<uint, SpawnableObjectBehaviour>();

        private bool _destroyMode;
        private AudioClip _airdropTowerAudioClip;
        private AudioClip _nukeAudioClip;
        private List<SpawnableObjectBehaviour> _turrets = new List<SpawnableObjectBehaviour>();

        private void Start()
        {
            NetworkView = gameObject.GetComponent<uLink.NetworkView>();
            _airdropTowerAudioClip = new WWW(
                    "file://" + Hooks.GameDirectory +
                    "\\RB_Data\\WorldEditor\\AirdropTower.ogg")
                .GetAudioClip(true);
            _nukeAudioClip = new WWW(
                    "file://" + Hooks.GameDirectory +
                    "\\RB_Data\\WorldEditor\\Nuke.ogg")
                .GetAudioClip(true);

            StartCoroutine(WaitTillStuffIsLoaded());
        }

        private IEnumerator WaitTillStuffIsLoaded()
        {
            while (WorldEditor.Instance.Handler._showSplash)
            {
                yield return new WaitForSeconds(1);
            }

            WorldEditorSpawnObjects();
        }

        [RPC]
        public void WorldEditorSpawn(uint id, string steamID, string assetName, string bundleName, Vector3 position,
            Quaternion rotation, Vector3 size)
        {
            var TempGameObject = new GameObject();
            var SpawnedObject = TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();

            SpawnedObject.Create(assetName, bundleName, position,
                rotation, size);

            var spawnableObject = new SpawnableObject(id, steamID, bundleName, assetName, position, rotation, size);
            var spawnableObjectBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<SpawnableObjectBehaviour>();
            spawnableObjectBehaviour.SpawnableObject = spawnableObject;
            spawnableObjectBehaviour.LoaderObject = TempGameObject;

            UnityEngine.Object.DontDestroyOnLoad(TempGameObject);
            UnityEngine.Object.DontDestroyOnLoad(SpawnedObject.ObjectInstantiate);

            switch (spawnableObject.Name)
            {
                case "Landmine":
                {
                    var landmineBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<LandmineBehaviour>();
                    landmineBehaviour.RpcBehaviour = this;
                    break;
                }

                case "Teleporter":
                {
                    var teleporterBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TeleporterBehaviour>();
                    teleporterBehaviour.RpcBehaviour = this;
                    break;
                }

                case "Door":
                {
                    var doorBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<DoorBehaviour>();
                    doorBehaviour.RpcBehaviour = this;
                    break;
                }

                case "ImageZone":
                {
                    var imageZoneBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<ImageZoneBehaviour>();
                    imageZoneBehaviour.RpcBehaviour = this;
                    break;
                }

                case "Turret":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.RpcBehaviour = this;
                    turretBehaviour.Range = 8;
                    break;
                }

                case "Turret2":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.RpcBehaviour = this;
                    turretBehaviour.Range = 11;
                    break;
                }

                case "Turret_Silencer":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.RpcBehaviour = this;
                    turretBehaviour.Range = 11;
                    break;
                }

                case "Turret_Range":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.RpcBehaviour = this;
                    turretBehaviour.Range = 21;
                    break;
                }

                case "Turret_Damage":
                {
                    var turretBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<TurretBehaviour>();
                    turretBehaviour.RpcBehaviour = this;
                    turretBehaviour.Range = 11;
                    break;
                }

                case "AirdropTower":
                {
                    var airdropTowerBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<AirdropTowerBehaviour>();
                    airdropTowerBehaviour.RpcBehaviour = this;
                    break;
                }
                
                case "RadZone":
                {
                    var radZoneBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<RadZoneBehaviour>();
                    radZoneBehaviour.RpcBehaviour = this;
                    break;
                }
            }

            _spawnedObjects[id] = spawnableObjectBehaviour;

            if (assetName.StartsWith("Turret"))
                _turrets.Add(spawnableObjectBehaviour);
        }

        [RPC]
        public void WorldEditorDestroy(uint id)
        {
            var spawnableObjectBehaviour = _spawnedObjects[id];

            if (spawnableObjectBehaviour.SpawnableObject.Name.StartsWith("Turret"))
            {
                if (_turrets.Remove(spawnableObjectBehaviour))
                {
                    Debug.Log("Removed turret from list!");
                }
            }

            UnityEngine.Object.Destroy(spawnableObjectBehaviour.LoaderObject);
            UnityEngine.Object.Destroy(spawnableObjectBehaviour.gameObject);

            _spawnedObjects.Remove(id);
        }

        [RPC]
        public void WorldEditorStartPlacement(string assetName, string bundleName, Quaternion rotation, Vector3 size)
        {
            var TempGameObject = new GameObject();
            var SpawnedObject = TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();

            SpawnedObject.Create(assetName, bundleName, PlayerClient.GetLocalPlayer().transform.position,
                rotation, size);

            var placementBehaviour = SpawnedObject.ObjectInstantiate.AddComponent<PlacementBehaviour>();
            placementBehaviour.SpawnableObject =
                new SpawnableObject(0, null, bundleName, assetName, Vector3.zero, rotation, size);
            placementBehaviour.LoaderObject = TempGameObject;
            placementBehaviour.RPCBehaviour = this;

            foreach (var collider in SpawnedObject.ObjectInstantiate.GetComponents<Collider>())
            {
                collider.enabled = false;
            }

            foreach (var collider in SpawnedObject.ObjectInstantiate.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            UnityEngine.Object.DontDestroyOnLoad(TempGameObject);
            UnityEngine.Object.DontDestroyOnLoad(SpawnedObject.ObjectInstantiate);
        }

        [RPC]
        public void WorldEditorStartDestroy()
        {
            _destroyMode = true;
        }

        [RPC]
        public void LandmineBehaviour_PlayParticleSystem(uint id)
        {
            if (!_spawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour)) return;

            var landmineBehaviour = spawnableObjectBehaviour.GetComponent<LandmineBehaviour>();
            if (landmineBehaviour != null)
                landmineBehaviour.PlayParticleSystem();
        }

        public void LandmineBehaviour_HandleCollision(uint id)
        {
            NetworkView.RPC("LandmineBehaviour_HandleCollision", uLink.NetworkPlayer.server, id);
        }

        [RPC]
        public void TeleporterBehaviour_SetRadius(uint id, float radius)
        {
            if (_spawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                var teleporterBehaviour = spawnableObjectBehaviour.GetComponent<TeleporterBehaviour>();
                teleporterBehaviour.SetRadius(radius);
            }
        }

        public void TeleporterBehaviour_Teleport(uint id)
        {
            NetworkView.RPC("TeleporterBehaviour_Teleport", uLink.NetworkPlayer.server, id);
        }

        public void TeleporterBehaviour_GetRadius(uint id)
        {
            NetworkView.RPC("TeleporterBehaviour_GetRadius", uLink.NetworkPlayer.server, id);
        }

        public void ImageZoneBehaviour_GetInfo(uint id)
        {
            NetworkView.RPC("ImageZoneBehaviour_GetInfo", uLink.NetworkPlayer.server, id);
        }
        
        public void RadZoneBehaviour_GetInfo(uint id)
        {
            NetworkView.RPC("RadZoneBehaviour_GetInfo", uLink.NetworkPlayer.server, id);
        }
        
        public void RadZoneBehaviour_AddRads(uint id)
        {
            NetworkView.RPC("RadZoneBehaviour_AddRads", uLink.NetworkPlayer.server, id);
        }
        
        [RPC]
        public void RadZoneBehaviour_SetInfo(uint id, float radius)
        {
            if (_spawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                var radZoneBehaviour = spawnableObjectBehaviour.GetComponent<RadZoneBehaviour>();
                radZoneBehaviour.SetInfo(radius);
            }
        }

        [RPC]
        public void ImageZoneBehaviour_SetInfo(uint id, float radius, string image)
        {
            if (_spawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                var imageZoneBehaviour = spawnableObjectBehaviour.GetComponent<ImageZoneBehaviour>();
                imageZoneBehaviour.SetInfo(radius, image);
            }
        }

        [RPC]
        public void DoorBehaviour_SetNearDoor(uint id, bool b)
        {
            if (_spawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
//                Debug.Log("Setting NearDoor to " + b);
                var doorBehaviour = spawnableObjectBehaviour.GetComponent<DoorBehaviour>();
                doorBehaviour.SetNearDoor(b);
//                Debug.Log("Set.");
            }
        }

        [RPC]
        public void AirdropTowerBehaviour_SetNearTower(uint id, bool b)
        {
            if (_spawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                var airdropTowerBehaviour = spawnableObjectBehaviour.GetComponent<AirdropTowerBehaviour>();
                airdropTowerBehaviour.SetNearTower(b);
            }
        }

        [RPC]
        public void AirdropTowerBehaviour_PlaySound()
        {
            AudioSource.PlayClipAtPoint(_airdropTowerAudioClip,
                PlayerClient.localPlayerClient.controllable.transform.position);
        }

        [RPC]
        public void DoorBehaviour_SetOpen(uint id, bool b)
        {
            if (_spawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                var doorBehaviour = spawnableObjectBehaviour.GetComponent<DoorBehaviour>();
                doorBehaviour.SetOpen(b);
            }
        }

        public void DoorBehaviour_Use(uint id)
        {
            NetworkView.RPC("DoorBehaviour_Use", uLink.NetworkPlayer.server, id);
        }

        public void DoorBehaviour_GetOpen(uint id)
        {
            NetworkView.RPC("DoorBehaviour_GetOpen", uLink.NetworkPlayer.server, id);
        }

        public void TurretBehaviour_SetTargetInRange(uint id, bool inRange)
        {
            NetworkView.RPC("TurretBehaviour_SetTargetInRange", uLink.NetworkPlayer.server, id, inRange);
        }

        public void TurretBehaviour_GetInfo(uint id)
        {
//            Debug.Log("TurretBehaviour_GetInfo()");
            NetworkView.RPC("TurretBehaviour_GetInfo", uLink.NetworkPlayer.server, id);
        }

        [RPC]
        public void TurretBehaviour_SetTarget(uint id, string steamID)
        {
//            Debug.Log("TurretBehaviour_SetTarget()");
            if (_spawnedObjects.TryGetValue(id, out var spawnableObjectBehaviour))
            {
                var turretBehaviour = spawnableObjectBehaviour.GetComponent<TurretBehaviour>();

                if (string.IsNullOrEmpty(steamID))
                {
//                    Debug.Log("TurretBehaviour_SetTarget(): Null target");
                    turretBehaviour.SetTarget(null);
                    return;
                }

//                Debug.Log("TurretBehaviour_SetTarget(): Searching for target");
                foreach (var character in GameObject.FindObjectsOfType<Character>())
                {
                    if (character != null && character.playerClient != null)
                    {
                        if (character.playerClient.userID.ToString() != steamID)
                        {
                            continue;
                        }

                        turretBehaviour.SetTarget(character.transform);
//                        Debug.Log("TurretBehaviour_SetTarget(): Found and set target");
                        return;
                    }
                }

//                Debug.Log("TurretBehaviour_SetTarget(): Couldn't find target, probably offline");
            }
        }

        [RPC]
        public void TurretBehaviour_AddTurretID(uint id)
        {
            WorldEditor.LastTurretIDs.Add(id);
        }

        public void TurretBehaviour_Damage(uint id)
        {
            NetworkView.RPC("TurretBehaviour_Damage", uLink.NetworkPlayer.server, id);
        }

        public void AirdropTowerBehaviour_Use(uint id)
        {
            NetworkView.RPC("AirdropTowerBehaviour_Use", uLink.NetworkPlayer.server, id);
        }

        public void WorldEditorFinishDestroy(uint id)
        {
            NetworkView.RPC("WorldEditorFinishDestroy", uLink.NetworkPlayer.server, id);
        }

        private void Update()
        {
            if (_destroyMode)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                    Physics.Raycast(ray, out var hit);

                    if (hit.transform != null)
                    {
                        var spawnableObjectBehaviour =
                            hit.transform.GetComponent<SpawnableObjectBehaviour>();

                        if (spawnableObjectBehaviour == null)
                        {
                            spawnableObjectBehaviour = hit.transform.GetComponentInParent<SpawnableObjectBehaviour>();

                            if (spawnableObjectBehaviour == null)
                            {
                                spawnableObjectBehaviour =
                                    hit.transform.GetComponentInChildren<SpawnableObjectBehaviour>();

                                if (spawnableObjectBehaviour == null)
                                {
                                    Rust.Notice.Popup("", "You need to look at one of our new objects!");
                                    return;
                                }
                            }
                        }

                        WorldEditorFinishDestroy(spawnableObjectBehaviour.SpawnableObject.ID);
                    }
                    else
                    {
                        Rust.Notice.Popup("", "You need to look at something!");
                    }
                }
                else if (Input.GetButtonDown("Fire2"))
                {
                    Rust.Notice.Popup("", "Cancelled!");
                    _destroyMode = false;
                }
            }

//            var stopwatch = new Stopwatch();
//            stopwatch.Start();
            if (PlayerClient.localPlayerClient == null)
                return;
            if (_spawnedObjects.Values.Count == 0)
                return;

            var controllable = PlayerClient.localPlayerClient.controllable;
            foreach (var so in _spawnedObjects.Values)
            {
                if (so.SpawnableObject.Name == "AirdropTower")
                {
                    continue;
                }
                if (so.SpawnableObject.Name.Contains("Meiler"))
                {
                    continue;
                }
                if (so.SpawnableObject.Name == "NUKE")
                {
                    continue;
                }
                
                var distance = Vector3.Distance(so.transform.position, controllable.transform.position);
                
                var renderers = so.GetComponentsInChildren<MeshRenderer>().ToList();
                renderers.AddRange(so.GetComponents<MeshRenderer>());
                
                if (distance >= 75)
                {
                    foreach (var renderer in renderers)
                    {
                        if (renderer.enabled)
                        {
                            renderer.enabled = false;
                        }
                    }
                }
                else
                {
                    foreach (var renderer in renderers)
                    {
                        if (!renderer.enabled)
                        {
                            renderer.enabled = true;
                        }
                    }
                }
            }

//            stopwatch.Stop();
//            Debug.Log("Distance check took " + stopwatch.ElapsedMilliseconds + "ms");
//            stopwatch.Reset();
        }

        public void WorldEditorFinishPlacement(bool place, string assetName, string bundleName, Vector3 position,
            Quaternion rotation, Vector3 size, bool nearWater)
        {
            NetworkView.RPC("WorldEditorFinishPlacement", uLink.NetworkPlayer.server, place, assetName, bundleName,
                position, rotation, size, nearWater);
        }

        public void DebugLog(string s)
        {
            NetworkView.RPC("DebugLog", uLink.NetworkPlayer.server, s);
        }

        [RPC]
        public void RustNotice(string s, float duration)
        {
            Rust.Notice.Popup("", s, duration);
        }

        [RPC]
        public void PlayNukeSound()
        {
            AudioSource.PlayClipAtPoint(_nukeAudioClip,
                PlayerClient.localPlayerClient.controllable.transform.position);            
        }
        
        private class PlacementBehaviour : MonoBehaviour
        {
            public SpawnableObject SpawnableObject;
            public GameObject LoaderObject;
            public RPCBehaviour RPCBehaviour;

            private Vector3 _position;

            private void Start()
            {
                _position = new Vector3(0.5f, 0.5f, 2.25f);
            }

            private void Update()
            {
                var point = Camera.main.ViewportToWorldPoint(_position);
                transform.position = point;

                if (Input.GetButtonDown("Fire1"))
                {
                    var nearWater = false;
                    if (PlayerClient.localPlayerClient != null)
                    {
                        var controllable = PlayerClient.localPlayerClient.controllable;
                        var findObjectsOfType = GameObject.FindObjectsOfType<GameObject>();

                        foreach (var o in findObjectsOfType)
                        {
                            if (o.name == "RustWaterReflectionScene Camera")
                            {
                                if (Vector3.Distance(controllable.transform.position, o.transform.position) < 10)
                                {
//                                    Debug.Log(o.name);
                                    nearWater = true;
                                    break;
                                }
                            }
                        }
                    }

                    RPCBehaviour.WorldEditorFinishPlacement(true, SpawnableObject.Name, SpawnableObject.BundleName,
                        point, transform.rotation, transform.localScale, nearWater);
                    Cleanup();
                }
                else if (Input.GetButtonDown("Fire2"))
                {
                    RPCBehaviour.WorldEditorFinishPlacement(false, SpawnableObject.Name, SpawnableObject.BundleName,
                        point, transform.rotation, transform.localScale, false);
                    Cleanup();
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Rotate(Vector3.down);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    transform.Rotate(Vector3.up);
                }
            }

            private void Cleanup()
            {
                UnityEngine.Object.Destroy(LoaderObject);
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        public void WorldEditorSpawnObjects()
        {
            NetworkView.RPC("WorldEditorSpawnObjects", uLink.NetworkPlayer.server);
        }
    }
}