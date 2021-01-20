using System;
using System.Collections;
using Fougerite;
using UnityEngine;

namespace WorldEditorServer.ObjectBehaviours
{
    public class TurretBehaviour : MonoBehaviour
    {
        private SpawnManager.SpawnableObjectBehaviour _spawnableObjectBehaviour;
        private Fougerite.Player _target;
        private bool _inRange;
        public float Damage;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnManager.SpawnableObjectBehaviour>();

            StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                if (!_inRange)
                {
                    yield return new WaitForSeconds(1.5f);
                    continue;
                }

                if (_target == null)
                {
                    _inRange = false;
                    continue;
                }

//                Logger.Log("_target: " + _target);

                try
                {
                    if (!_target.IsOnline)
                    {
                        _inRange = false;
                        continue;
                    }

                    if (!RPCBehaviour._turretTargetDictionary.ContainsKey(_spawnableObjectBehaviour.SpawnableObject.ID))
                    {
                        _inRange = false;
                        continue;
                    }

//                    Logger.Log("Shooting at " + _target.Name);

                    var compHealth = _target.Health - Damage;
                    if (compHealth <= 0)
                    {
                        if (_target.PlayerClient.controllable != null)
                        {
                            _target.Kill();
                            _inRange = false;
                            RPCBehaviour._turretTargetDictionary.Remove(_spawnableObjectBehaviour.SpawnableObject.ID);
//                            Logger.Log("Killed.");
                        }
                    }
                    else
                    {
                        _target.Health = compHealth;
                        _target.Character.controllable.GetComponent<ClientVitalsSync>().SendClientItsHealth();
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("TurretBehaviour: Catched " + e);
                    _inRange = false;
                }

                yield return new WaitForSeconds(1f);
            }
        }

        public void SetTarget(Fougerite.Player target, bool inRange)
        {
            _target = !inRange ? null : target;
            _inRange = inRange;
        }
    }
}