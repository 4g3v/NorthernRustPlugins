using System.Collections;
using UnityEngine;

namespace WorldEditorServer.ObjectBehaviours
{
    public class HealingStationBehaviour : MonoBehaviour
    {
        private SpawnManager.SpawnableObjectBehaviour _spawnableObjectBehaviour;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnManager.SpawnableObjectBehaviour>();
            
            StartCoroutine(CheckCoroutine());
        }

        private IEnumerator CheckCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                foreach (var collider in Physics.OverlapSphere(transform.position, 5))
                {
                    if (collider.gameObject.name.Contains("Player"))
                    {
                        var character = collider.GetComponent<Character>();
                        if (character != null)
                        {
                            var player = new Fougerite.Player(character.playerClient);
                            
                            if (_spawnableObjectBehaviour.SpawnableObject.SteamID == player.SteamID)
                            {
                                var compHealth = player.Health + 1f;
                                if (compHealth > 200)
                                    compHealth = 200;
                                player.Health = compHealth;

                                collider.GetComponent<HumanBodyTakeDamage>().Bandage(1000);
                                collider.GetComponent<ClientVitalsSync>().SendClientItsHealth();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}