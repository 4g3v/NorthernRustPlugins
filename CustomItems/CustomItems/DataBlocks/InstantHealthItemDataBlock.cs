using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class InstantHealthItemDataBlock : BasicHealthKitDataBlock
    {
        public float amountToHeal;
        
        public InstantHealthItemDataBlock()
        {
            name = "Adrenaline Rush";
            
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\InstantHealth.png"));
            iconTex = texture;

            _maxUses = 8;
            _spawnUsesMin = 1;
            _spawnUsesMax = 1;
            _minUsesForDisplay = 1;
            _maxCondition = 100;
            _splittable = true;
            _itemFlags = 0;
            isResearchable = false;
            isRepairable = false;
            isRecycleable = false;
            doesLoseCondition = false;
            category = ItemCategory.Medical;
            Combinations = new CombineRecipe[0];
            transientMode = 0;
            amountToHeal = 25;
            stopsBleeding = true;
            
            var type = GetType().BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481339);
        }

        public override void UseItem(IBasicHealthKit hk)
        {
            int slot = hk.slot;
            var inventory = hk.inventory;
            var humanBodyTakeDamage = inventory.GetLocal<HumanBodyTakeDamage>();
            
            if (!humanBodyTakeDamage)
            {
                return;
            }
            
            var metabolism = inventory.GetLocal<Metabolism>();
            
            if (!metabolism)
            {
                return;
            }
            
            if (stopsBleeding)
            {
                humanBodyTakeDamage.Bandage(1000f);
            }

            var oldHealth = humanBodyTakeDamage.health;
            var compHealth = oldHealth + amountToHeal;
            if (compHealth > 200)
                compHealth = 200;
            humanBodyTakeDamage.health = compHealth;
            inventory.inventoryHolder.playerClient.controllable.GetLocal<ClientVitalsSync>().SendClientItsHealth();

            if (oldHealth != 200)
            {
                int consumedCount = 1;
                bool flag = hk.Consume(ref consumedCount);
                if (consumedCount == 0)
                {
                    inventory.MarkSlotDirty(slot);
                    hk.FireClientSideItemEvent(InventoryItem.ItemEvent.Used);
                }
                if (flag)
                {
                    inventory.RemoveItem(slot);
                }   
            }
        }
    }
}