//using UnityEngine;
//
//namespace CustomItemsClient
//{
//    public class SmokeGrenade : RigidObj
//    {
//        public GameObject explosionEffect;
//        public AudioClip bounceSound;
//
//        [RPC]
//        private void ClientBounce(uLink.NetworkMessageInfo info)
//        {
////            global::InterpTimedEvent.Queue(this, "bounce", ref info);
//        }
//
//        private new void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
//        {
//            base.uLink_OnNetworkInstantiate(info);
//        }
//        
//        protected override void OnHide()
//        {
//            if (base.renderer)
//            {
//                base.renderer.enabled = false;
//            }
//        }
//
//        protected override void OnShow()
//        {
//            if (base.renderer)
//            {
//                base.renderer.enabled = true;
//            }
//        }
//
//        protected override void OnDone()
//        {
//            UnityEngine.Object obj = UnityEngine.Object.Instantiate(explosionEffect, transform.position,
//                Quaternion.LookRotation(Vector3.up));
//            float amount = 60f;
//            UnityEngine.Object.Destroy(obj, amount);
//        }
//
//        public SmokeGrenade() : base(FeatureFlags.StreamInitialVelocity |
//                                     FeatureFlags.StreamOwnerViewID |
//                                     FeatureFlags.ServerCollisions)
//        {
//        }
//    }
//}