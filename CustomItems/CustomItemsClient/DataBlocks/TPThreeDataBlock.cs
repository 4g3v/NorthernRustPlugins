using System.IO;
using System.Reflection;
using UnityEngine;

namespace CustomItemsClient.DataBlocks
{
    public class TPThreeDataBlock : ItemDataBlock
    {
        public TPThreeDataBlock()
        {
            name = "PocketWarp 3";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\CustomItems\\TPThree.png"));
            iconTex = texture;
            
            _maxUses = 5;
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
            category = ItemCategory.Tools;
            Combinations = new CombineRecipe[0];
            transientMode = 0;
            
            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481358);
        }

        public override string GetItemDescription()
        {
            return "Instantly teleports you to BigRad!";
        }

        public override int RetreiveMenuOptions(IInventoryItem item, InventoryItem.MenuItem[] results, int offset)
        {
            offset = base.RetreiveMenuOptions(item, results, offset);
            if (item.isInLocalInventory)
                results[offset++] = InventoryItem.MenuItem.Use;
            return offset;
        }

        public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
        {
            if (option != InventoryItem.MenuItem.Use)
                return base.ExecuteMenuOption(option, item);
            return InventoryItem.MenuItemResult.DoneOnServer;
        }

        private sealed class ITEM_TYPE : TPThree<TPThreeDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(TPThreeDataBlock datablock) : base(datablock)
            {
            }
        }
    }
    
    public abstract class TPThree<T> : InventoryItem<T> where T : TPThreeDataBlock
    {
        protected TPThree(T datablock) : base(datablock)
        {
        }
    }
}