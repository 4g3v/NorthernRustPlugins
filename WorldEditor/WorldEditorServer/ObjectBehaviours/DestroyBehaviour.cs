using System.Collections;
using UnityEngine;

namespace WorldEditorServer.ObjectBehaviours
{
    public class DestroyBehaviour : MonoBehaviour
    {
        public int Seconds;

        public void StartWaitAndDestroy()
        {
            StartCoroutine(WaitAndDestroy());
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(Seconds);
            SpawnManager.RemoveObject(GetComponent<SpawnManager.SpawnableObjectBehaviour>().SpawnableObject.ID, true);
        }
    }
}