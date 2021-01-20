using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldEditor.ObjectBehaviours
{
    public class LandmineBehaviour : MonoBehaviour
    {
        public RPCBehaviour RpcBehaviour;
        private ParticleSystem _particleSystem;
        private Dictionary<string, AudioSource> _audioDict;

        private void Start()
        {
            _particleSystem = transform.FindChild("Effect").GetComponent<ParticleSystem>();
            
            _audioDict = new Dictionary<string, AudioSource>();
            foreach (var audioSource in transform.FindChild("Effect").GetComponents<AudioSource>())
            {
                _audioDict[audioSource.clip.name] = audioSource;
            }
        }

        public void PlayParticleSystem()
        {
            _particleSystem.Play();

            _audioDict["TickSound"].Stop();
            _audioDict["BombSound"].Play();

            StartCoroutine(WaitAndDestroy());
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                RpcBehaviour.LandmineBehaviour_HandleCollision(GetComponent<SpawnableObjectBehaviour>().SpawnableObject.ID);
//                var ccMotor = (CCMotor) other.gameObject.GetComponent<CCDesc>().Tag;
//                var character = ccMotor.idMain.gameObject.GetComponent<Character>();
                
//                if (character != null)
//                {
//                    RpcBehaviour.LandmineBehaviour_HandleCollision(GetComponent<SpawnableObjectBehaviour>().SpawnableObject.ID);
//                }
            }
        }
    }
}