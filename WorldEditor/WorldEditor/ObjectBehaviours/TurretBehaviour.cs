using System;
using System.Collections;
using UnityEngine;

namespace WorldEditor.ObjectBehaviours
{
    public class TurretBehaviour : MonoBehaviour
    {
        public RPCBehaviour RpcBehaviour;
        public int Range;

        private SpawnableObjectBehaviour _spawnableObjectBehaviour;
        private Transform _playerTransform;
        private Transform _rotateTransform;
        private ParticleSystem _muzzleParticleSystem;
        private AudioSource _shootSound;
        private Vector3 _fixedRotationVector;
        private Controllable _controllable;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnableObjectBehaviour>();
            _rotateTransform = transform.FindChild("GameObject");
            _muzzleParticleSystem = _rotateTransform.FindChild("Muzzle").GetComponent<ParticleSystem>();
            _shootSound = _rotateTransform.GetComponent<AudioSource>();
            _fixedRotationVector = new Vector3(-90, 0, 0);

            RpcBehaviour.TurretBehaviour_GetInfo(_spawnableObjectBehaviour.SpawnableObject.ID);
            StartCoroutine(CheckCoroutine());
        }

        private IEnumerator CheckCoroutine()
        {
//            Controllable controllable;
//            while (true)
//            {
//                if (PlayerClient.GetLocalPlayer() != null)
//                {
//                    controllable = PlayerClient.GetLocalPlayer().controllable;
//                    if (controllable != null)
//                        break;
//                }
//
//                yield return new WaitForSeconds(1);
//            }

            while (true)
            {
                if (_playerTransform != null)
                {
                    var character = _playerTransform.GetComponent<Character>();
                    if (character == null)
                    {
//                        Debug.Log("CheckCoroutine(): Character is null");
                        yield return new WaitForSeconds(1);
                        continue;
                    }

                    try
                    {
                        if (character.playerClient.userID ==
                            PlayerClient.GetLocalPlayer().userID)
                        {
                            var distance = Vector3.Distance(_playerTransform.position, transform.position);
//                            Debug.Log("CheckCoroutine(): Distance: " + distance);
                            if (distance > Range)
                            {
                                _playerTransform = null;
                                _controllable = null;
                                WorldEditor.LastTurretIDs.Remove(_spawnableObjectBehaviour.SpawnableObject.ID);
                                RpcBehaviour.TurretBehaviour_SetTargetInRange(
                                    _spawnableObjectBehaviour.SpawnableObject.ID,
                                    false);
                                StopShootingEffects();
//                                Debug.Log("CheckCoroutine(): Outside of range");
                            }
                        }
//                        else
//                        {
//                            Debug.Log("CheckCoroutine(): Turret isn't targeting local player");
//                        }
                    }
                    catch { }
                }

                yield return new WaitForSeconds(2);
            }
        }

        public void SetTarget(Transform target)
        {
            _playerTransform = target;

            if (target != null)
            {
                _controllable = _playerTransform.GetComponent<Character>().controllable;
                StartShootingEffects();
            }
            else
            {
                _controllable = null;
                StopShootingEffects();
            }
        }

        private void StartShootingEffects()
        {
            _muzzleParticleSystem.Play();
            _shootSound.Play();
        }

        private void StopShootingEffects()
        {
            _muzzleParticleSystem.Stop();
            _shootSound.Stop();
        }

        private void Update()
        {
            if (_playerTransform != null && _controllable != null)
            {
                _rotateTransform.LookAt(_controllable.eyesOrigin);
                _rotateTransform.Rotate(_fixedRotationVector);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_playerTransform == null)
            {
                if (other.gameObject.name.Contains("TotemPole"))
                {
                    if (PlayerClient.GetLocalPlayer().userID.ToString() !=
                        _spawnableObjectBehaviour.SpawnableObject.SteamID)
                    {
//                        _playerTransform = other.gameObject.transform;
//                        _controllable = PlayerClient.GetLocalPlayer().controllable;
                        
//                        WorldEditor.LastTurretIDs.Add(_spawnableObjectBehaviour.SpawnableObject.ID);

                        RpcBehaviour.TurretBehaviour_SetTargetInRange(_spawnableObjectBehaviour.SpawnableObject.ID,
                            true);
//                        StartShootingEffects();
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.Contains("TotemPole") && _playerTransform != null)
            {
                var character = _playerTransform.GetComponent<Character>();
                if (character == null)
                {
//                    Debug.Log("OnTriggerExit(): Character is null");
                    return;
                }

                var localPlayerSteamID = PlayerClient.GetLocalPlayer().userID;
                if (character.playerClient.userID ==
                    localPlayerSteamID)
                {
                    if (localPlayerSteamID.ToString() != _spawnableObjectBehaviour.SpawnableObject.SteamID)
                    {
                        _playerTransform = null;
                        _controllable = null;
                        WorldEditor.LastTurretIDs.Remove(_spawnableObjectBehaviour.SpawnableObject.ID);
                        RpcBehaviour.TurretBehaviour_SetTargetInRange(_spawnableObjectBehaviour.SpawnableObject.ID,
                            false);
                        StopShootingEffects();
                    }
                }
                else
                {
//                    Debug.Log(
//                        "OnTriggerExit(): Not setting _playerTransform to null because it's targeting someone else");
                }
            }
        }
    }
}