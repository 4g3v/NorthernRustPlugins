using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class LootboxDataBlock : ItemDataBlock
    {
        public LootboxDataBlock()
        {
            name = "Lootbox";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\Lootbox.png"));
            iconTex = texture;

            category = ItemCategory.Misc;
            Combinations = new CombineRecipe[0];
            _maxCondition = 100;
            _maxUses = 5;
            _splittable = true;

            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481382);
        }

        public override string GetItemDescription()
        {
            return "Use it and you'll get a few items.";
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

            if (player.Inventory.FreeSlots >= 7)
            {
                player.Inventory.AddItem("M4", 1);
                player.Inventory.AddItem("556 Ammo", 100);
                player.Inventory.AddItem("Leather Vest", 1);
                player.Inventory.AddItem("Leather Pants", 1);
                player.Inventory.AddItem("Leather Helmet", 1);
                player.Inventory.AddItem("Leather Boots", 1);
                player.Inventory.AddItem("Small Medkit", 2);
                
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

        private sealed class ITEM_TYPE : Lootbox<LootboxDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(LootboxDataBlock datablock) : base(datablock)
            {
            }
        }
    }
    
    public abstract class Lootbox<T> : InventoryItem<T> where T : LootboxDataBlock
    {
        protected Lootbox(T datablock) : base(datablock)
        {
        }

        public override void OnBeltUse()
        {
            T tDatablock = this.datablock;
            tDatablock.UseItem(iface);
        }
    }
}