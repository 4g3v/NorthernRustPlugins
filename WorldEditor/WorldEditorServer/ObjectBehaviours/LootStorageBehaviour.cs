using System;
using System.Collections;
using System.Collections.Generic;
using Fougerite;
using UnityEngine;

namespace WorldEditorServer.ObjectBehaviours
{
    public class LootStorageBehaviour : MonoBehaviour
    {
        private SpawnManager.SpawnableObjectBehaviour _spawnableObjectBehaviour;

        private void Start()
        {
            _spawnableObjectBehaviour = GetComponent<SpawnManager.SpawnableObjectBehaviour>();
            StartCoroutine(RefillAndSpawnCoroutine(
                LootStorageSettings.GetLootStorageFromID(_spawnableObjectBehaviour.SpawnableObject.ID)));
        }

        private void FillWithItems(LootableObject lootableObject, List<Item> items)
        {
            foreach (var item in items)
            {
                lootableObject._inventory.AddItem(DatablockDictionary.GetByName(item.ItemName),
                    Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default),
                    Inventory.Uses.Quantity.Manual(item.Amount));
            }
        }
        
        private IEnumerator RefillAndSpawnCoroutine(LootStorage lootStorage)
        {
            foreach (var collider in Facepunch.MeshBatch.MeshBatchPhysics.OverlapSphere(
                transform.position, 2))
            {
                if (collider.name.Contains("WoodBoxLarge"))
                {
                    NetCull.Destroy(collider.gameObject);
                }
            }

            var loot = NetCull.InstantiateStatic(";deploy_wood_storage_large", transform.position, Quaternion.identity);
            var lootableObject = loot.GetComponent<LootableObject>();

            FillWithItems(lootableObject, lootStorage.GetItems());

            while (true)
            {
                var foundBox = false;

                foreach (var collider in Facepunch.MeshBatch.MeshBatchPhysics.OverlapSphere(
                    transform.position, 2))
                {
                    if (collider.name.Contains("WoodBoxLarge"))
                    {
                        foundBox = true;
                        break;
                    }
                }

                if (!foundBox)
                {
                    loot = NetCull.InstantiateStatic(";deploy_wood_storage_large", transform.position, Quaternion.identity);
                    lootableObject = loot.GetComponent<LootableObject>();
                }
                
                lootableObject._inventory.Clear();
                FillWithItems(lootableObject, lootStorage.GetItems());

                yield return new WaitForSeconds(lootStorage.RefillSeconds);
            }
        }
    }
}