using System.IO;
using System.Reflection;
using UnityEngine;

namespace CustomItemsClient.DataBlocks
{
    public class AVXDollarDataBlock : ItemDataBlock
    {
        public AVXDollarDataBlock()
        {
            name = "NR Dollar";
            itemDescriptionOverride = "You can use the NR Dollars in the shops.";
            
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\CustomItems\\AVXDollar.png"));
            iconTex = texture;

            _maxUses = 250;
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
            category = ItemCategory.Misc;
            Combinations = new CombineRecipe[0];
            transientMode = 0;
            
            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481350);
        }
    }
}