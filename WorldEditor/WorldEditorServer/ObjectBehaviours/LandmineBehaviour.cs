using Fougerite;
using UnityEngine;

namespace WorldEditorServer.ObjectBehaviours
{
    public class LandmineBehaviour : MonoBehaviour
    {
        private SpawnManager.SpawnableObjectBehaviour _spawnableObjectBehaviour;
        private float _damageValue;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnManager.SpawnableObjectBehaviour>();
            _damageValue = 100;
        }

        public void HandleCollision(Fougerite.Player player)
        {
            if (player.SteamID != _spawnableObjectBehaviour.SpawnableObject.SteamID)
            {
                foreach (var rpcBehaviour in WorldEditorServer.RPCDictionary.Values)
                    rpcBehaviour.LandmineBehaviour_PlayParticleSystem(_spawnableObjectBehaviour
                        .SpawnableObject.ID);

                var compHealth = player.Health - _damageValue;

                if (compHealth <= 0)
                    player.Kill();
                else
                {
                    player.Health = compHealth;
                    player.PlayerClient.gameObject.GetComponent<ClientVitalsSync>().SendClientItsHealth();
                }

                if (WorldEditorServer.LandmineCount.TryGetValue(_spawnableObjectBehaviour.SpawnableObject.SteamID, out var count))
                {
                    count--;
                    WorldEditorServer.LandmineCount[_spawnableObjectBehaviour.SpawnableObject.SteamID] = count;
                    
                    if (Server.Cache.TryGetValue(ulong.Parse(_spawnableObjectBehaviour.SpawnableObject.SteamID), out var creatorPlayer))
                    {
                        creatorPlayer.Notice("One of your landmines exploded! " + count + "/" + SpawnManager.MaxLandmineCount + " left!");
                    }
                }
                
                SpawnManager.RemoveObject(_spawnableObjectBehaviour.SpawnableObject.ID, false);
            }
        }
    }
}