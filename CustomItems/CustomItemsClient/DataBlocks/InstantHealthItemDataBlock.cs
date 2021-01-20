using System.IO;
using System.Reflection;
using UnityEngine;

namespace CustomItemsClient.DataBlocks
{
    public class InstantHealthItemDataBlock : BasicHealthKitDataBlock
    {
        public float amountToHeal;
        
        public InstantHealthItemDataBlock()
        {
            name = "Adrenaline Rush";
            
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\CustomItems\\InstantHealth.png"));
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
            UsedSound = new WWW(
                    "file://" + RustBuster2016.API.Hooks.GameDirectory +
                    "\\RB_Data\\CustomItems\\InstantHealthUsed.ogg")
                .GetAudioClip(true);
            Combinations = new CombineRecipe[0];
            transientMode = 0;
            amountToHeal = 25;
            stopsBleeding = true;
            
            var type = GetType().BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481339);
        }

        public override void PopulateInfoWindow(ItemToolTip infoWindow, IInventoryItem tipItem)
        {
            infoWindow.AddItemTitle(this, tipItem, 0f);
            infoWindow.AddSectionTitle("Medical", 15f);
            infoWindow.AddBasicLabel("Instantly heals " + amountToHeal + " health and stops bleeding.", 15f);
            infoWindow.FinishPopulating();
        }
    }
}