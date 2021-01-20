//TODO: AutoSpawn
//using System.Collections;
//using System.Collections.Generic;
//using Fougerite;
//using UnityEngine;
//
//namespace WorldEditorServer
//{
//    public class AutoSpawnBehaviour : MonoBehaviour
//    {
//        public IEnumerator WaitAndSpawn(AutoSpawnManager.SpawnedObject objectToSpawn)
//        {
//            while (true)
//            {
////                Logger.Log("[WorldEditor] Spawn waiting");
//                yield return new WaitForSeconds(objectToSpawn.Seconds);
//                foreach (var player in Fougerite.Server.GetServer().Players)
//                {
////                    Logger.Log("[WorldEditor] Spawn RPC TryGetValue");
//                    if (WorldEditorServer.RPCDictionary.TryGetValue(player.SteamID, out var rpcBehaviour))
//                    {
//                        StartCoroutine(WaitAndDestroy(objectToSpawn.DestroySeconds, rpcBehaviour.WorldEditorSpawn(objectToSpawn)));
////                        Logger.Log("[WorldEditor] Spawned " + objectToSpawn.AssetName + " for " + player.SteamID);
//                    }
//                }
//            }
//        }
//        
//        public IEnumerator WaitAndSpawn(List<AutoSpawnManager.SpawnedObject> objectsToSpawn)
//        {
//            while (true)
//            {
////                Logger.Log("[WorldEditor] Spawn waiting");
//
////                int i = 0;
////                yield return new WaitForSeconds(objectsToSpawn[i].Seconds);
////                foreach (var player in Fougerite.Server.GetServer().Players)
////                {
////                    if (WorldEditorServer.RPCDictionary.TryGetValue(player.SteamID, out var rpcBehaviour))
////                    {
////                        var idOne = rpcBehaviour.WorldEditorSpawn(objectsToSpawn[i]);
////                        StartCoroutine(WaitAndDestroy(objectsToSpawn[i].DestroySeconds, idOne));
////                    }
////                }
//
//                for (int i = 0; i < objectsToSpawn.Count; i++)
//                {
//                    yield return new WaitForSeconds(objectsToSpawn[i].Seconds);
//                    foreach (var player in Fougerite.Server.GetServer().Players)
//                    {
//                        if (WorldEditorServer.RPCDictionary.TryGetValue(player.SteamID, out var rpcBehaviour))
//                        {
//                            var id = rpcBehaviour.WorldEditorSpawn(objectsToSpawn[i]);
//                            StartCoroutine(WaitAndDestroy(objectsToSpawn[i].DestroySeconds, id));
//                        }
//                    }
//                }
//                
//                
////                yield return new WaitForSeconds(objectToSpawn.Seconds);
////                foreach (var player in Fougerite.Server.GetServer().Players)
////                {
//////                    Logger.Log("[WorldEditor] Spawn RPC TryGetValue");
////                    if (WorldEditorServer.RPCDictionary.TryGetValue(player.SteamID, out var rpcBehaviour))
////                    {
////                        yield return new WaitForSeconds(objectToSpawn2.Seconds);
////                        var idTwo = rpcBehaviour.WorldEditorSpawn(objectToSpawn2);
////                        StartCoroutine(WaitAndDestroy(objectToSpawn2.DestroySeconds, idTwo));
//////                        Logger.Log("[WorldEditor] Spawned " + objectToSpawn.AssetName + " for " + player.SteamID);
////                    }
////                }
//            }
//        }
//
//        public IEnumerator WaitAndDestroy(float seconds, uint id)
//        {
////            Logger.Log("[WorldEditor] Destroy waiting");
//            yield return new WaitForSeconds(seconds);
//            foreach (var player in Fougerite.Server.GetServer().Players)
//            {
////                Logger.Log("[WorldEditor] Destroy RPC TryGetValue");
//                if (WorldEditorServer.RPCDictionary.TryGetValue(player.SteamID, out var rpcBehaviour))
//                {
//                    rpcBehaviour.WorldEditorDestroy(id);
////                    Logger.Log("[WorldEditor] Destroyed " + id + " for " + player.SteamID);
//                }
//            }
//        }
//    }
//}