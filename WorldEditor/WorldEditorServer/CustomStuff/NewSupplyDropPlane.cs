using Fougerite;
using UnityEngine;

namespace WorldEditorServer.CustomStuff
{
    public class NewSupplyDropPlane : SupplyDropPlane
    {
//        public new float maxSpeed = 250f;
//        protected new float lastDist = float.PositiveInfinity;
//        protected new bool approachingTarget = true;

//        public new int TEMP_numCratesToDrop = 3;

//        public GameObject[] propellers;
//        public new Vector3 startPos;
//        public new Vector3 dropTargetPos;
//        public new Quaternion startAng;
//        private bool passedTarget;
//        protected new Vector3 targetPos;
//        protected new float targetReachedTime;
//        protected new bool droppedPayload;
//        protected new TransformInterpolator _interp;
//        protected new double _lastMoveTime;

//        public NewSupplyDropPlane()
//            : this(IDFlags.Unknown)
//        {
//        }

//        protected NewSupplyDropPlane(IDFlags idFlags)
//            : base(idFlags)
//        {
//        }

        private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
        {
            transform.FindChild("uh60").gameObject.SetActive(true);
            transform.FindChild("uh602").gameObject.SetActive(true);
            this._interp = this.GetComponent<TransformInterpolator>();
            this._lastMoveTime = NetCull.localTime;
            this.InvokeRepeating("DoNetwork", 0.0f, NetCull.sendIntervalF);
            this._interp.running = false;
            // ReSharper disable once InvocationIsSkipped
            ServerHelper.SetupForServer(this.gameObject);
        }

//        public new void Update()
//        {
//            base.Update();
//        }

        public new void TargetReached()
        {
            base.TargetReached();
            Logger.Log("Airdrop at: " + transform.position + " reached target!");
//            if (this.droppedPayload)
//                return;
//            this.droppedPayload = true;
//            int num = Random.Range(1, this.TEMP_numCratesToDrop + 1);
//            float time = 0.0f;
//            for (int index = 0; index < num; ++index)
//            {
//                this.Invoke("DropCrate", time);
//                time += Random.RandomRange(0.3f, 0.6f);
//            }

//            this.targetPos += this.transform.forward * this.maxSpeed * 30f;
//            this.targetPos.y += 800f;
//            this.Invoke("NetDestroy", 20f);
        }

//        public new void NetDestroy()
//        {
//            base.NetDestroy();
//            this.CancelInvoke();
//            NetCull.Destroy(this.gameObject);
//        }

//        public new void DropCrate()
//        {
//            base.DropCrate();
//            GameObject gameObject = NetCull.InstantiateClassic("SupplyCrate",
//                this.transform.position - this.transform.forward * 50f,
//                Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360f), 0.0f)), 0);
//            gameObject.rigidbody.centerOfMass = new Vector3(0.0f, -1.5f, 0.0f);
//            gameObject.rigidbody.AddForceAtPosition(-this.transform.forward * 50f,
//                gameObject.transform.position - new Vector3(0.0f, 1f, 0.0f));
//            Logger.Log("Crate position: " + gameObject.transform.position);
//            Logger.Log("Crate localPosition: " + gameObject.transform.localPosition);
//        }

//        public new void SetDropTarget(Vector3 pos)
//        {
//            base.SetDropTarget(pos);
//            this.dropTargetPos = pos;
//            this.targetPos = this.dropTargetPos;
//        }

//        public new void DoMovement()
//        {
//            base.DoMovement();
//        }

        public new void DoNetwork()
        {
            base.DoMovement();
            Vector3 rot = transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
//            transform.rotation = ;
            this.networkView.RPC("GetNetworkUpdate", uLink.RPCMode.OthersExceptOwner, new object[2]
            {
                (object) this.transform.position,
                (object) Quaternion.Euler(rot)
            });
        }
    }
}