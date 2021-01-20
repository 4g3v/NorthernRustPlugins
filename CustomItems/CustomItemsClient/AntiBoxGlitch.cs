//using System.Collections;
//using UnityEngine;
//
//namespace CustomItemsClient
//{
//    public class AntiBoxGlitch : MonoBehaviour
//    {
//        private void Start()
//        {
//            foreach (var characterController in GameObject.FindObjectsOfType<CharacterController>())
//            {
//                if (characterController.ToString().ToLower().Contains("totempole"))
//                {
//                    _controller = characterController;
//                    break;
//                }
//            }
//            
////            StartCoroutine(Check());
//        }
//
//        private CharacterController _controller;
//        
//        private void Update()
//        {
//            if (_controller.velocity.y >= 10)
//            {
//                Rust.Notice.Popup("", "No box jump for you (" + _controller.velocity.y + ")");
//            }
//        }
//
////        private IEnumerator Check()
////        {
////            Debug.Log("Check()");
////            foreach (var characterController in GameObject.FindObjectsOfType<CharacterController>())
////            {
////                if (characterController.ToString().ToLower().Contains("totempole"))
////                {
////                    _controller = characterController;
////                    break;
////                }
////            }
////            
////            while (true)
////            {
////                if (_controller.velocity.y >= 10)
////                {
////                    Rust.Notice.Popup("", "No box jump for you (" + _controller.velocity.y + ")");
//////                    controller.Move(Vector3.down * 50);
////                }
//////                Debug.Log("Velocity: " + controller.rigidbody.velocity);
////                yield return new WaitForSeconds(0.1f);
////            }
////        }
//    }
//}