using UnityEngine;

namespace WorldEditor.CustomStuff
{
    public class NewSupplyDropPlane : SupplyDropPlane
    {
        public new float maxSpeed = 250f;
        protected new float lastDist = float.PositiveInfinity;
        protected new bool approachingTarget = true;
        public new int TEMP_numCratesToDrop = 3;
//        public GameObject[] propellers;
        public new Vector3 startPos;
        public new Vector3 dropTargetPos;
        public new Quaternion startAng;
        private bool passedTarget;
        protected new Vector3 targetPos;
        protected new float targetReachedTime;
        protected new bool droppedPayload;
        protected new TransformInterpolator _interp;
        protected new double _lastMoveTime;

        public NewSupplyDropPlane()
            : this(IDFlags.Unknown)
        {
        }

        protected NewSupplyDropPlane(IDFlags idFlags)
            : base(idFlags)
        {
        }

        private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
        {
            transform.FindChild("uh60").gameObject.SetActive(true);
            transform.FindChild("uh602").gameObject.SetActive(true);
            this._interp = this.GetComponent<TransformInterpolator>();
            this._interp.running = true;
        }

//        public void Update()
//        {
//            foreach (GameObject propeller in this.propellers)
//                propeller.transform.RotateAroundLocal(Vector3.forward, 12f * Time.deltaTime);
//        }

        [RPC]
        public new void GetNetworkUpdate(Vector3 pos, Quaternion rot, uLink.NetworkMessageInfo info)
        {
            this._interp.SetGoals(pos, rot, info.timestamp);
        }
    }
}