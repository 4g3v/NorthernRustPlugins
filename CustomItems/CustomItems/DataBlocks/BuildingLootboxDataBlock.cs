using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class BuildingLootboxDataBlock : ItemDataBlock
    {
        public BuildingLootboxDataBlock()
        {
            name = "Building Lootbox";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\BuildingLootbox.png"));
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
        
        public override IInventoryItem ConstructItem()
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
            UseItem(item);
            return InventoryItem.MenuItemResult.DoneOnServer;
        }

        public virtual void UseItem(IInventoryItem inventoryItem)
        {
            var player = new Fougerite.Player(inventoryItem.controller.playerClient);

            if (player.Inventory.FreeSlots >= 8)
            {
                player.Inventory.AddItem("Metal Ceiling", 20);
                player.Inventory.AddItem("Metal Doorway", 20);
                player.Inventory.AddItem("Metal Foundation", 20);
                player.Inventory.AddItem("Metal Pillar", 40);
                player.Inventory.AddItem("Metal Ramp", 10);
                player.Inventory.AddItem("Metal Stairs", 10);
                player.Inventory.AddItem("Metal Wall", 40);
                player.Inventory.AddItem("Metal Window", 10);
                
                int num = 1;
                if (inventoryItem.Consume(ref num))
                {
                    inventoryItem.inventory.RemoveItem(inventoryItem.slot);
                }
                else
                {
                    inventoryItem.inventory.MarkSlotDirty(inventoryItem.slot);
                }
            }
            else
            {
                player.Notice("You don't have enough space in your inventory!");
            }
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

        public override void OnBeltUse()
        {
            T tDatablock = this.datablock;
            tDatablock.UseItem(iface);
        }
    }
}