using System;
using System.Collections;
using System.Collections.Generic;
using RustBuster2016.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WorldEditor
{
    public class TimedSpawns : MonoBehaviour
    {
        public List<TimedSpawn> TimedObjects = new List<TimedSpawn>();
        private string _timedSpawns;

        private void Start()
        {
            _timedSpawns = WorldEditor.Instance.SettingsParser.GetSetting("Settings", "TimedSpawns");
            if (string.IsNullOrEmpty(_timedSpawns))
                return;

            WorldEditor.Instance.Handler.Finished += HandlerOnFinished;
        }

        private void HandlerOnFinished()
        {
            foreach (var timedSpawnEntryString in _timedSpawns.Split('/'))
            {
                if (string.IsNullOrEmpty(timedSpawnEntryString))
                    continue;
                
                var timedSpawnEntry = timedSpawnEntryString.Split(':');
                
                int index = int.Parse(timedSpawnEntry[0]);
                float seconds = float.Parse(timedSpawnEntry[1]);
                bool destroy = bool.Parse(timedSpawnEntry[2]);
                
                var timedSpawn = new TimedSpawn(WorldEditor.Instance.Handler.SetObjects[index - 1], seconds, destroy);
                TimedObjects.Add(timedSpawn);

                StartCoroutine(SpawnTimed(timedSpawn));
            }
        }

        private IEnumerator DestroyAfterSeconds(WorldEditor.DestroyAfterSecondsClass destroyAfterSecondsClass)
        {
            yield return new WaitForSeconds(destroyAfterSecondsClass.Seconds);
            UnityEngine.Object.Destroy(destroyAfterSecondsClass.GameObject.ObjectInstantiate);
            UnityEngine.Object.Destroy(destroyAfterSecondsClass.GameObject);

            destroyAfterSecondsClass.StillAlive = false;
//            Debug.Log("Destroyed.");
        }

        private void OnDisable()
        {
            StopAllCoroutines();
//            WorldEditor.Instance.Handler.Finished -= HandlerOnFinished;
        }

        private IEnumerator SpawnTimed(TimedSpawn objectToSpawn)
        {
            while (true)
            {
//                Hooks.LogData("WorldEditor", "SpawnTimed(): before yield");
                yield return new WaitForSeconds(objectToSpawn.Seconds);
//                Hooks.LogData("WorldEditor", "SpawnTimed(): after yield");
            
                WorldEditor.Instance.AllSpawnedObjects.Remove(objectToSpawn.ObjectToSpawn);

                if (objectToSpawn.Destroy_)
                {
                    UnityEngine.Object.Destroy(objectToSpawn.ObjectToSpawn.ObjectInstantiate);
//                    Hooks.LogData("WorldEditor", "SpawnTimed(): Destroyed object");
                }
            
//                Hooks.LogData("WorldEditor", "SpawnTimed(): ObjectToSpawn: " + objectToSpawn.ObjectToSpawn.Name);
//                Hooks.LogData("WorldEditor", "SpawnTimed(): ObjectToSpawn: " + objectToSpawn.ObjectToSpawn.BundleName);
//                Hooks.LogData("WorldEditor", "SpawnTimed(): ObjectToSpawn: " + objectToSpawn.ObjectToSpawn.Position);
//                Hooks.LogData("WorldEditor", "SpawnTimed(): ObjectToSpawn: " + objectToSpawn.ObjectToSpawn.Rotation);
//                Hooks.LogData("WorldEditor", "SpawnTimed(): ObjectToSpawn: " + objectToSpawn.ObjectToSpawn.Size);

                var b = objectToSpawn.ObjectToSpawn.Create(objectToSpawn.ObjectToSpawn.Name, objectToSpawn.ObjectToSpawn.BundleName,
                    objectToSpawn.ObjectToSpawn.Position, objectToSpawn.ObjectToSpawn.Rotation,
                    objectToSpawn.ObjectToSpawn.Size);
//                Hooks.LogData("WorldEditor", "SpawnTimed(): ObjectInstantiate: " + objectToSpawn.ObjectToSpawn.ObjectInstantiate.transform.position);
//                Hooks.LogData("WorldEditor", "SpawnTimed(): ObjectInstantiate: " + objectToSpawn.ObjectToSpawn.ObjectInstantiate.transform.rotation);

//                Hooks.LogData("WorldEditor", "SpawnTimed(): Create returned: " + b);
            }
        }

        public class TimedSpawn
        {
            public LoadingHandler.LoadObjectFromBundle ObjectToSpawn;
            public float Seconds;
            public bool Destroy_;

            public TimedSpawn(LoadingHandler.LoadObjectFromBundle objectToSpawn, float seconds, bool destroy)
            {
                ObjectToSpawn = objectToSpawn;
                Seconds = seconds;
                Destroy_ = destroy;
            }
        }
    }
}