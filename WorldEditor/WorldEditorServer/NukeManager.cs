using System;
using System.Collections;
using UnityEngine;
using WorldEditorServer.ObjectBehaviours;
using Object = UnityEngine.Object;

namespace WorldEditorServer
{
    public class NukeManager
    {
        private static GameObject _nukeTimerObject;

        public static void Start()
        {
            _nukeTimerObject = new GameObject();
            _nukeTimerObject.AddComponent<NukeManagerBehaviour>();
            Object.DontDestroyOnLoad(_nukeTimerObject);
        }

        public static void Stop()
        {
            _nukeTimerObject.GetComponent<NukeManagerBehaviour>().Stop();
            Object.Destroy(_nukeTimerObject);
        }

        public class NukeManagerBehaviour : MonoBehaviour
        {
            private void Start()
            {
                StartCoroutine(HandleCoroutine());
            }

            public void Stop()
            {
                StopAllCoroutines();
            }

            private IEnumerator HandleCoroutine()
            {
                var nuclearPilePosition = new Vector3(2342.776f, 848.0834f, -4209.329f);
                var nuclearPileRotation = Quaternion.identity;
                var nuclearPileScale = new Vector3(1, 1, 1);

                while (true)
                {
                    var nuclearPile = SpawnManager.Spawn(new SpawnManager.SpawnableObject(0, "Server", "dl_powerplant",
                        "Meiler",
                        nuclearPilePosition, nuclearPileRotation, nuclearPileScale), false);

                    yield return new WaitForSeconds(4*60); //Start time

                    foreach (var rpcBehaviour in WorldEditorServer.RPCDictionary.Values)
                    {
                        rpcBehaviour.PlayNukeSound();
                    }

                    yield return new WaitForSeconds(23);

                    var nuke = SpawnManager.Spawn(new SpawnManager.SpawnableObject(0, "Server", "dl_powerplant", "NUKE",
                        new Vector3(2737.199f, 560.3943f, -3928.984f), Quaternion.identity, new Vector3(2, 2, 2)), false);
                    
                    var radZone = SpawnManager.Spawn(new SpawnManager.SpawnableObject(0, "Server", "dl_powerplant", "RadZone",
                        new Vector3(2342.776f, 515.2609f, -3915.802f), nuclearPileRotation, nuclearPileScale), false);
                    var radZoneBehaviour = SpawnManager.SpawnedObjects[radZone].GetComponent<RadZoneBehaviour>();
                    radZoneBehaviour.Radius = 1600;
                    radZoneBehaviour.Rads = 20;
                    
                    yield return new WaitForSeconds(1.5f);
                    SpawnManager.RemoveObject(nuclearPile, true, false);
                    var brokenPowerPlant = SpawnManager.Spawn(new SpawnManager.SpawnableObject(0, "Server",
                        "dl_powerplant", "BrokenMeiler", nuclearPilePosition, nuclearPileRotation, nuclearPileScale), false);

                    yield return new WaitForSeconds(14);
                    SpawnManager.RemoveObject(nuke, true, false);
                    yield return new WaitForSeconds(60*5); //End waiting time

                    SpawnManager.RemoveObject(brokenPowerPlant, true, false);
                    SpawnManager.RemoveObject(radZone, true, false);
                }
            }
        }
    }
}