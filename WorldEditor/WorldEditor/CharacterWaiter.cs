using UnityEngine;

namespace WorldEditor
{
    public class CharacterWaiter : MonoBehaviour
    {
        public void Update()
        {
            if (PlayerClient.GetLocalPlayer() == null || PlayerClient.GetLocalPlayer().controllable == null) return;
            var player = PlayerClient.GetLocalPlayer().controllable.GetComponent<Character>();
            
            if (player == null) return;
            enabled = false;
            WorldEditor.Instance.AddRPC();
        }
    }
}