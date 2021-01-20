using UnityEngine;

namespace WorldEditor.ObjectBehaviours
{
    public class AirdropTowerBehaviour : MonoBehaviour
    {
        public RPCBehaviour RpcBehaviour;

        private SpawnableObjectBehaviour _spawnableObjectBehaviour;
        private bool _nearTower;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnableObjectBehaviour>();
        }

        private void Update()
        {
            if (_nearTower)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    RpcBehaviour.AirdropTowerBehaviour_Use(_spawnableObjectBehaviour.SpawnableObject.ID);
                }
            }
        }

        public void SetNearTower(bool b)
        {
            _nearTower = b;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                _nearTower = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                _nearTower = false;
            }
        }
    }
}