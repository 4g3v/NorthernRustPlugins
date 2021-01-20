//using Fougerite;
//using UnityEngine;
//
//namespace WorldEditorServer.ObjectBehaviours
//{
//    public class TeleporterBehaviour : MonoBehaviour
//    {
//        private SpawnManager.SpawnableObjectBehaviour _spawnableObjectBehaviour;
//        private Teleporter _teleporter;
//
//        private void Start()
//        {
//            _spawnableObjectBehaviour = GetComponent<SpawnManager.SpawnableObjectBehaviour>();
//            TeleporterSettings.Load();
//
//            _teleporter =
//                TeleporterSettings.Teleporters.GetTeleporterFromID(_spawnableObjectBehaviour.SpawnableObject.ID);
//
//            var sphereCollider = gameObject.AddComponent<SphereCollider>();
//            sphereCollider.isTrigger = true;
//            sphereCollider.radius = _teleporter.Radius;
//        }
//
//        private void OnTriggerEnter(Collider other)
//        {
//            Logger.Log("OnTriggerEnter");
//            var ccMotor = (CCMotor) other.gameObject.GetComponent<CCDesc>().Tag;
//            var character = ccMotor.idMain.gameObject.GetComponent<Character>();
//
//            if (character != null)
//            {
//                new Fougerite.Player(character.playerClient).TeleportTo(_teleporter.GetPosition());
//            }
//        }
//    }
//}