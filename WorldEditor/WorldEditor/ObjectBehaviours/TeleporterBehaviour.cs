using UnityEngine;

namespace WorldEditor.ObjectBehaviours
{
    public class TeleporterBehaviour : MonoBehaviour
    {
        public RPCBehaviour RpcBehaviour;

        private SpawnableObjectBehaviour _spawnableObjectBehaviour;
        private SphereCollider _sphereCollider;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnableObjectBehaviour>();

            _sphereCollider = gameObject.AddComponent<SphereCollider>();
            _sphereCollider.isTrigger = true;
            _sphereCollider.radius = 0.5f;

//            Debug.Log("TeleporterBehaviour.Start(): added collider");

            GetRadius();
        }

        public void SetRadius(float radius)
        {
//            Debug.Log("TeleporterBehaviour.SetRadius()");
            _sphereCollider.radius = radius;
        }

        public void GetRadius()
        {
//            Debug.Log("TeleporterBehaviour.GetRadius()");
            RpcBehaviour.TeleporterBehaviour_GetRadius(_spawnableObjectBehaviour.SpawnableObject.ID);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                RpcBehaviour.TeleporterBehaviour_Teleport(_spawnableObjectBehaviour.SpawnableObject.ID);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                RpcBehaviour.TeleporterBehaviour_Teleport(_spawnableObjectBehaviour.SpawnableObject.ID);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                RpcBehaviour.TeleporterBehaviour_Teleport(_spawnableObjectBehaviour.SpawnableObject.ID);
            }
        }
    }
}