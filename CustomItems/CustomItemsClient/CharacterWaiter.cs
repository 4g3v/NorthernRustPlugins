using UnityEngine;

namespace CustomItemsClient
{
    public class CharacterWaiter : MonoBehaviour
    {
        public void Update()
        {
            if (PlayerClient.GetLocalPlayer() == null || PlayerClient.GetLocalPlayer().controllable == null) return;
            var player = PlayerClient.GetLocalPlayer().controllable.GetComponent<Character>();

            if (player == null) return;
            enabled = false;
            CustomItemsClient.Instance.AddRPC();
            GameObject gameObject = GameObject.Find("BrandingPanel");
            if (gameObject != null)
            {
//                Debug.Log("Destroyed BrandingPanel");
                Destroy(gameObject);
            }

//            PlayerClient.GetLocalPlayer().controllable.gameObject.AddComponent<AntiBoxGlitch>();
        }
    }
}