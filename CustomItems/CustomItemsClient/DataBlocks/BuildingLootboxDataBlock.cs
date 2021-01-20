using System.IO;
using System.Reflection;
using RustBuster2016.API;
using UnityEngine;

namespace CustomItemsClient.DataBlocks
{
    public class BuildingLootboxDataBlock : ItemDataBlock
    {
        public BuildingLootboxDataBlock()
        {
            name = "Building Lootbox";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(
                File.ReadAllBytes(Hooks.GameDirectory + "\\RB_Data\\CustomItems\\BuildingLootbox.png"));
            iconTex = texture;

            category = ItemCategory.Misc;
            Combinations = new CombineRecipe[0];
            _maxCondition = 100;
            _maxUses = 5;
            _splittable = true;

            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481423);
        }

        public override string GetItemDescription()
        {
            return "Use it and you'll get building items.";
        }

        protected override IInventoryItem ConstructItem()
        {
            return new ITEM_TYPE(this);
        }

        public override int RetreiveMenuOptions(IInventoryItem item, InventoryItem.MenuItem[] results, int offset)
        {
            offset = base.RetreiveMenuOptions(item, results, offset);
            if (item.isInLocalInventory)
                results[offset++] = InventoryItem.MenuItem.Use;
            return offset;
        }

        public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option,
            IInventoryItem item)
        {
            if (option != InventoryItem.MenuItem.Use)
                return base.ExecuteMenuOption(option, item);
            return InventoryItem.MenuItemResult.DoneOnServer;
        }

        private sealed class ITEM_TYPE : BuildingLootbox<BuildingLootboxDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(BuildingLootboxDataBlock datablock) : base(datablock)
            {
            }
        }
    }

    public abstract class BuildingLootbox<T> : InventoryItem<T> where T : BuildingLootboxDataBlock
    {
        protected BuildingLootbox(T datablock) : base(datablock)
        {
        }
    }
}