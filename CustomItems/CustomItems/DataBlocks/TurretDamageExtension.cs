using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class TurretDamageExtension : ItemDataBlock
    {
        public TurretDamageExtension()
        {
            name = "Turret Damage Extension";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\TurretDamageExtension.png"));
            iconTex = texture;

            category = ItemCategory.Tools;
            Combinations = new CombineRecipe[0];
            _maxCondition = 100;
            _maxUses = 10;
            _splittable = true;

            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481435);
        }

        public override string GetItemDescription()
        {
            return "Crafting ingredient for the Heavy Turret.";
        }
    }
}