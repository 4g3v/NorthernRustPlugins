using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;

namespace WorldEditor.ObjectBehaviours
{
    public class ImageZoneBehaviour : MonoBehaviour
    {
        public RPCBehaviour RpcBehaviour;

        private SpawnableObjectBehaviour _spawnableObjectBehaviour;
        private SphereCollider _sphereCollider;
        private bool _showImage;
        private Texture2D _texture;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnableObjectBehaviour>();

            _sphereCollider = gameObject.AddComponent<SphereCollider>();
            _sphereCollider.isTrigger = true;
            _sphereCollider.radius = 0.5f;

            GetInfo();
        }

        public void SetInfo(float radius, string image)
        {
            _sphereCollider.radius = radius;
            _texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            _texture.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory +
                                                 "\\RB_Data\\WorldEditor\\" + image));
        }

        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            if (_showImage)
            {
                GUI.DrawTexture(
                    new Rect((w / 2) - (_texture.width / 2), (h / 2) - (_texture.height / 2), _texture.width,
                        _texture.height), _texture);
            }
        }

        public void GetInfo()
        {
            RpcBehaviour.ImageZoneBehaviour_GetInfo(_spawnableObjectBehaviour.SpawnableObject.ID);
            StartCoroutine(CheckCoroutine());
        }

        private IEnumerator CheckCoroutine()
        {
            Controllable controllable;
            while (true)
            {
                if (PlayerClient.GetLocalPlayer() != null)
                {
                    controllable = PlayerClient.GetLocalPlayer().controllable;
                    if (controllable != null)
                        break;
                }

                yield return new WaitForSeconds(1);
            }

            while (true)
            {
                if (_showImage)
                {
                    var distance = Vector3.Distance(controllable.transform.position, transform.position);
                    if (distance > _sphereCollider.radius)
                    {
                        _showImage = false;
                    }
                }

                yield return new WaitForSeconds(2);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                _showImage = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole"))
            {
                _showImage = false;
            }
        }
    }
}