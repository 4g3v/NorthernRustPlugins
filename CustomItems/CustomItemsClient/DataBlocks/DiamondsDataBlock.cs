using System.IO;
using System.Reflection;
using RustBuster2016.API;
using UnityEngine;

namespace CustomItemsClient.DataBlocks
{
    public class DiamondsDataBlock : ItemDataBlock
    {
        public DiamondsDataBlock()
        {
            name = "Diamonds";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Hooks.GameDirectory + "\\RB_Data\\CustomItems\\Diamonds.png"));
            iconTex = texture;

            category = ItemCategory.Tools;
            Combinations = new CombineRecipe[0];
            _maxCondition = 100;
            _maxUses = 250;
            _splittable = true;

            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481422);
        }

        public override string GetItemDescription()
        {
            return "You need them to craft the unique building parts lootboxes.";
        }
    }
}