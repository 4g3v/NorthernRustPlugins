using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class KeycardCDataBlock : ItemDataBlock
    {
        public KeycardCDataBlock()
        {
            name = "Keycard C";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\KeycardC.png"));
            iconTex = texture;

            category = ItemCategory.Tools;
            Combinations = new CombineRecipe[0];
            _maxCondition = 100;
            _maxUses = 1;
            _splittable = true;

            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481420);
        }

        public override string GetItemDescription()
        {
            return "Use the Keycards to open the locked laser doors in the sewer.";
        }
    }
}