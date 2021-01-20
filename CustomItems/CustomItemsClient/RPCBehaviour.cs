using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Cache;
using RustBuster2016.API;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace CustomItemsClient
{
    public class RPCBehaviour : MonoBehaviour
    {
        public uLink.NetworkView NetworkView;
        private float normalMaxForwardSpeed;
        private float normalMaxSidewaysSpeed;
        private float normalMaxBackwardsSpeed;
        private float normalMaxGroundAcceleration;
        private float normalMaxAirAcceleration;

        private float normalMaxAirHorizontalSpeed;

//        private float normalInputAirVelocityRatio;
        private float normalMaxFallSpeed;
        private Texture2D _image;
        private Texture2D _jointImage;
        private Texture2D _parachuteImage;
        private bool _displayOverlay;
        private bool _displayJointOverlay;
        private CCMotor _ccMotor;
        private bool _useParachute;

        private GameObject _parachute;
//        public float ParachuteMaxAirAcceleration;
//        public float ParachuteMaxAirHorizontalSpeed;
//        public float ParachuteInputAirVelocityRatio;
//        public float ParachuteMaxFallSpeed;

        private void Start()
        {
            NetworkView = gameObject.GetComponent<uLink.NetworkView>();

            var humanController = (HumanController) PlayerClient.GetLocalPlayer().controllable.character.controller;
            _ccMotor = humanController.ccmotor;

            normalMaxForwardSpeed = _ccMotor.movement.setup.maxForwardSpeed;
            normalMaxSidewaysSpeed = _ccMotor.movement.setup.maxSidewaysSpeed;
            normalMaxBackwardsSpeed = _ccMotor.movement.setup.maxBackwardsSpeed;
            normalMaxGroundAcceleration = _ccMotor.movement.setup.maxGroundAcceleration;
            normalMaxAirAcceleration = _ccMotor.movement.setup.maxAirAcceleration;
            normalMaxAirHorizontalSpeed = _ccMotor.movement.setup.maxAirHorizontalSpeed;
//            normalInputAirVelocityRatio = _ccMotor.movement.setup.inputAirVelocityRatio;
            normalMaxFallSpeed = _ccMotor.movement.setup.maxFallSpeed;

//            Debug.Log("normalMaxAirAcceleration: " + normalMaxAirAcceleration);
//            Debug.Log("normalMaxAirHorizontalSpeed: " + normalMaxAirHorizontalSpeed);
//            Debug.Log("normalInputAirVelocityRatio: " + normalInputAirVelocityRatio);
//            Debug.Log("normalMaxFallSpeed: " + normalMaxFallSpeed);

//            ParachuteMaxAirAcceleration = normalMaxAirAcceleration;
//            ParachuteMaxAirHorizontalSpeed = normalMaxAirHorizontalSpeed;
//            ParachuteInputAirVelocityRatio = normalInputAirVelocityRatio;
//            ParachuteMaxFallSpeed = normalMaxFallSpeed;

            _image = new Texture2D(2, 2);
            _image.LoadImage(File.ReadAllBytes(Hooks.GameDirectory + "\\RB_Data\\CustomItems\\AmphetamineOverlay.png"));
            _jointImage = new Texture2D(2, 2);
            _jointImage.LoadImage(File.ReadAllBytes(Hooks.GameDirectory + "\\RB_Data\\CustomItems\\JointOverlay.png"));
            _parachuteImage = new Texture2D(2, 2);
            _parachuteImage.LoadImage(
                File.ReadAllBytes(Hooks.GameDirectory + "\\RB_Data\\CustomItems\\ParachuteOverlay.png"));

//            Log(Environment.UserName + "||" + Environment.MachineName);

            StartCoroutine(WaitTillItemsLoaded());
            StartCoroutine(LoadParachuteBundle());
        }

        private void GetParachutes()
        {
            NetworkView.RPC("GetParachutes", uLink.NetworkPlayer.server);
        }

        private IEnumerator LoadParachuteBundle()
        {
            var www = WWW.LoadFromCacheOrDownload("http://absolomrust.ddnss.org/nr_pc.unity3d", 1);
            while (www.progress < 1)
            {
                yield return www;
            }

            var bundle = www.assetBundle;
            www.Dispose();

            _parachute = (GameObject) bundle.Load("Parachute");
//            Debug.Log("Loaded and set parachute object");
            
            GetParachutes();
        }

        private IEnumerator WaitTillItemsLoaded()
        {
            while (!CustomItemsClient.Instance.Finished)
            {
                yield return new WaitForSeconds(1);
            }

            InvGetNetworkUpdate();
        }

        private void InvGetNetworkUpdate()
        {
            NetworkView.RPC("InvGetNetworkUpdate", uLink.NetworkPlayer.server);
        }
        
        private void RemoveFromJointList()
        {
            NetworkView.RPC("RemoveFromJointList", uLink.NetworkPlayer.server);
        }

        private void OnGUI()
        {
            if (_displayOverlay)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _image, ScaleMode.StretchToFill);
            if (_displayJointOverlay)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _jointImage, ScaleMode.StretchToFill);

            if (_useParachute)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _parachuteImage, ScaleMode.StretchToFill);
        }

        [RPC]
        public void PrepareUseAmphetamine()
        {
            var humanController = (HumanController) PlayerClient.GetLocalPlayer().controllable.character.controller;
            var ccMotor = humanController.ccmotor;

            var canUse = ccMotor.movement.setup.maxForwardSpeed == normalMaxForwardSpeed;
            NetworkView.RPC("SetCanUseDrugOne", uLink.NetworkPlayer.server, canUse);
            NetworkView.RPC("UseDrugOne", uLink.NetworkPlayer.server);
            if (canUse)
                _displayOverlay = true;
        }
        
        [RPC]
        public void PrepareUseJoint()
        {
            var humanController = (HumanController) PlayerClient.GetLocalPlayer().controllable.character.controller;
            var ccMotor = humanController.ccmotor;

            var canUse = ccMotor.movement.setup.maxForwardSpeed == normalMaxForwardSpeed;
            NetworkView.RPC("SetCanUseJoint", uLink.NetworkPlayer.server, canUse);
            NetworkView.RPC("UseJoint", uLink.NetworkPlayer.server);
            if (canUse)
                _displayJointOverlay = true;
        }

        [RPC]
        public void MotorChange()
        {
            var humanController = (HumanController) PlayerClient.GetLocalPlayer().controllable.character.controller;
            _ccMotor = humanController.ccmotor;
            
            _ccMotor.movement.setup.maxForwardSpeed *= 3;
            _ccMotor.movement.setup.maxSidewaysSpeed *= 3;
            _ccMotor.movement.setup.maxBackwardsSpeed *= 3;
            _ccMotor.movement.setup.maxGroundAcceleration *= 3;
            _ccMotor.movement.setup.maxAirAcceleration *= 3;
            _ccMotor.movement.setup.maxAirHorizontalSpeed *= 3;

            StartCoroutine(GetSober());
        }
        [RPC]
        public void MotorChangeJoint()
        {
            var humanController = (HumanController) PlayerClient.GetLocalPlayer().controllable.character.controller;
            _ccMotor = humanController.ccmotor;
            
            _ccMotor.movement.setup.maxForwardSpeed *= 0.5f;
            _ccMotor.movement.setup.maxSidewaysSpeed *= 0.5f;
            _ccMotor.movement.setup.maxBackwardsSpeed *= 0.5f;
            _ccMotor.movement.setup.maxGroundAcceleration *= 0.5f;
            _ccMotor.movement.setup.maxAirAcceleration *= 0.5f;
            _ccMotor.movement.setup.maxAirHorizontalSpeed *= 0.5f;

            StartCoroutine(GetSoberJoint());
        }

        [RPC]
        public void SetDisplayOverlay(bool b)
        {
            _displayOverlay = b;
        }
        
        [RPC]
        public void SetDisplayJointOverlay(bool b)
        {
            _displayJointOverlay = b;
        }

        [RPC]
        public void UseParachute()
        {
            _useParachute = !_useParachute;
            
            var humanController = (HumanController) PlayerClient.GetLocalPlayer().controllable.character.controller;
            _ccMotor = humanController.ccmotor;
            
            if (_useParachute)
            {
                _ccMotor.movement.setup.maxFallSpeed = 2;
            }
            else
            {
                _ccMotor.movement.setup.maxFallSpeed = normalMaxFallSpeed;
            }
        }

        private Dictionary<string, GameObject> _parachuteDictionary = new Dictionary<string, GameObject>();

        [RPC]
        public void ShowParachute(string steamID, bool show)
        {
            if (_parachuteDictionary.ContainsKey(steamID) && !show)
            {
                GameObject.Destroy(_parachuteDictionary[steamID]);
                _parachuteDictionary.Remove(steamID);
//                Debug.Log("Removed parachute from dict");
                return;
            }

            if (!show)
                return;

            var parachute = (GameObject) GameObject.Instantiate(_parachute);

            foreach (var character in GameObject.FindObjectsOfType<Character>())
            {
                if (character != null && character.playerClient != null)
                {
                    if (character.playerClient.userID.ToString() != steamID)
                    {
                        continue;
                    }

                    var parachuteBehaviour = parachute.AddComponent<ParachuteBehaviour>();
                    parachuteBehaviour.Position = character.transform;
                    
//                    Debug.Log("Set parachute player transform");
                    break;
                }
            }

            _parachuteDictionary[steamID] = parachute;
//            Debug.Log("Set dict stuff");
        }

        public class ParachuteBehaviour : MonoBehaviour
        {
            public Transform Position;

            private void Update()
            {
                var pos = Position.position;
                pos.y += 3.5f;
                transform.position = pos;
            }
        }

        public void DisableParachute()
        {
            _useParachute = false;
            _ccMotor.movement.setup.maxFallSpeed = normalMaxFallSpeed;
        }

        private IEnumerator GetSober()
        {
            yield return new WaitForSeconds(15);

            _ccMotor.movement.setup.maxForwardSpeed = normalMaxForwardSpeed;
            _ccMotor.movement.setup.maxSidewaysSpeed = normalMaxSidewaysSpeed;
            _ccMotor.movement.setup.maxBackwardsSpeed = normalMaxBackwardsSpeed;
            _ccMotor.movement.setup.maxGroundAcceleration = normalMaxGroundAcceleration;
            _ccMotor.movement.setup.maxAirAcceleration = normalMaxAirAcceleration;
            _ccMotor.movement.setup.maxAirHorizontalSpeed = normalMaxAirHorizontalSpeed;

            _displayOverlay = false;
        }
        private IEnumerator GetSoberJoint()
        {
            yield return new WaitForSeconds(30);

            _ccMotor.movement.setup.maxForwardSpeed = normalMaxForwardSpeed;
            _ccMotor.movement.setup.maxSidewaysSpeed = normalMaxSidewaysSpeed;
            _ccMotor.movement.setup.maxBackwardsSpeed = normalMaxBackwardsSpeed;
            _ccMotor.movement.setup.maxGroundAcceleration = normalMaxGroundAcceleration;
            _ccMotor.movement.setup.maxAirAcceleration = normalMaxAirAcceleration;
            _ccMotor.movement.setup.maxAirHorizontalSpeed = normalMaxAirHorizontalSpeed;

            _displayJointOverlay = false;
            RemoveFromJointList();
        }

        public void Log(string log)
        {
            NetworkView.RPC("Log", uLink.NetworkPlayer.server, log);
        }
    }
}