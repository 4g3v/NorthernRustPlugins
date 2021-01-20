using System;
using System.Collections;
using UnityEngine;

namespace WorldEditor.ObjectBehaviours
{
    public class RadZoneBehaviour : MonoBehaviour
    {
        public RPCBehaviour RpcBehaviour;

        private SpawnableObjectBehaviour _spawnableObjectBehaviour;
        private SphereCollider _sphereCollider;
        private bool _inZone;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnableObjectBehaviour>();

            _sphereCollider = gameObject.AddComponent<SphereCollider>();
            _sphereCollider.isTrigger = true;
            _sphereCollider.radius = 0.5f;

            GetInfo();
            Debug.Log("Called GetInfo()");
        }

        public void SetInfo(float radius)
        {
            _sphereCollider.radius = radius;
            StartCoroutine(CheckCoroutine());
            Debug.Log("Got info and started coroutine");
        }
        
        private void GetInfo()
        {
            RpcBehaviour.RadZoneBehaviour_GetInfo(_spawnableObjectBehaviour.SpawnableObject.ID);
        }

        private IEnumerator CheckCoroutine()
        {
            Controllable controllable;
            while (true)
            {
                if (PlayerClient.GetLocalPlayer() != null)
                {
                    controllable = PlayerClient.GetLocalPlayer().controllable;
                    if (controllable != null)
                        break;
                }

                yield return new WaitForSeconds(1);
            }

            while (true)
            {
                if (_inZone)
                {
                    var distance = Vector3.Distance(controllable.transform.position, transform.position);
                    if (distance > _sphereCollider.radius)
                    {
                        _inZone = false;
                        
                        yield return new WaitForSeconds(2);
                        continue;
                    }
                    
                    Debug.Log("Adding rads");
                    RpcBehaviour.RadZoneBehaviour_AddRads(_spawnableObjectBehaviour.SpawnableObject.ID);
                }

                yield return new WaitForSeconds(2);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                _inZone = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                _inZone = false;
            }
        }
    }
}