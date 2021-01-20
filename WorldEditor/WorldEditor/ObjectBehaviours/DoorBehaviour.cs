using UnityEngine;

namespace WorldEditor.ObjectBehaviours
{
    public class DoorBehaviour : MonoBehaviour
    {
        public RPCBehaviour RpcBehaviour;

        private SpawnableObjectBehaviour _spawnableObjectBehaviour;
        private GameObject _lockChild;
        private SphereCollider _sphereCollider;
        private readonly float _radius = 4;
        private bool _nearDoor;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnableObjectBehaviour>();

            _lockChild = transform.FindChild("Lock").gameObject;

            RpcBehaviour.DoorBehaviour_GetOpen(_spawnableObjectBehaviour.SpawnableObject.ID);
//            _sphereCollider = gameObject.AddComponent<SphereCollider>();
//            _sphereCollider.isTrigger = true;fla
//            _sphereCollider.radius = _radius;
        }

        private void Update()
        {
            if (_nearDoor)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    RpcBehaviour.DoorBehaviour_Use(_spawnableObjectBehaviour.SpawnableObject.ID);
                }
            }
        }

        public void SetNearDoor(bool b)
        {
            _nearDoor = b;
        }
        
        public void SetOpen(bool b)
        {
            _lockChild.SetActive(!b);
        }

        private void OnTriggerEnter(Collider other)
        {
//            Debug.Log("OnTriggerEnter");
            if (other.gameObject.name.Contains("TotemPole"))
            {
                _nearDoor = true;
            }
        }

//        private void OnTriggerStay(Collider other)
//        {
//            Debug.Log("OnTriggerStay");
//            if (other.gameObject.name.Contains("TotemPole"))
//            {
//                _nearDoor = true;
//            }
//        }

        private void OnTriggerExit(Collider other)
        {
//            Debug.Log("OnTriggerExit");
            if (other.gameObject.name.Contains("TotemPole"))
            {
                _nearDoor = false;
            }
        }
    }
}