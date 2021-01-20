//using Fougerite;
//using UnityEngine;
//
//namespace CustomItems
//{
//    public class SmokeGrenade : RigidObj
//    {
//        private float fuseLength = 3f;
//        private float lastBounceTime;
//        public AudioClip bounceSound;
//        public GameObject explosionEffect;
//        
//        public SmokeGrenade() : base(FeatureFlags.StreamInitialVelocity |
//                                     FeatureFlags.StreamOwnerViewID |
//                                     FeatureFlags.ServerCollisions)
//        {
//        }
//        
//        private new void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
//        {
//            base.uLink_OnNetworkInstantiate(info);
//            ServerHelper.SetupForServer(gameObject);
//            Invoke("TimeIsUp", fuseLength);
//        }
//        
//        protected override void OnHide()
//        {
//            if (renderer)
//            {
//                renderer.enabled = false;
//            }
//        }
//
//        protected override void OnShow()
//        {
//            if (renderer)
//            {
//                renderer.enabled = true;
//            }
//        }
//
//        protected override void OnDone()
//        {
////            UnityEngine.Object obj = UnityEngine.Object.Instantiate(explosionEffect, transform.position, Quaternion.LookRotation(Vector3.up));
////            float amount = 60f;
////            UnityEngine.Object.Destroy(obj, amount);
//        }
//        
//        private void TimeIsUp()
//        {
//            MakeDoneAndDestroy(this);
//        }
//        
//        protected override void OnServerCollisionEnter(Collision collision)
//        {
//            if (Time.time - lastBounceTime < 0.25f)
//            {
//                return;
//            }
//            lastBounceTime = Time.time;
//            if (collision.relativeVelocity.sqrMagnitude > 0.0225f)
//            {
//                networkView.RPC("ClientBounce", uLink.RPCMode.Others);
//            }
//        }
//    }
//}