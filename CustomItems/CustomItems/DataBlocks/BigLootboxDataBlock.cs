using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class BigLootboxDataBlock : ItemDataBlock
    {
        public BigLootboxDataBlock()
        {
            name = "Big Lootbox";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\BigLootbox.png"));
            iconTex = texture;

            category = ItemCategory.Misc;
            Combinations = new CombineRecipe[0];
            _maxCondition = 100;
            _maxUses = 5;
            _splittable = true;

            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481384);
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

            if (player.Inventory.FreeSlots >= 9)
            {
                player.Inventory.AddItem("M4", 1);
                player.Inventory.AddItem("Shotgun", 1);
                player.Inventory.AddItem("Kevlar Helmet", 1);
                player.Inventory.AddItem("Kevlar Pants", 1);
                player.Inventory.AddItem("Kevlar Boots", 1);
                player.Inventory.AddItem("Kevlar Vest", 1);
                player.Inventory.AddItem("Large Medkit", 2);
                player.Inventory.AddItem("556 Ammo", 100);
                player.Inventory.AddItem("Shotgun Shells", 100);
                
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

        private sealed class ITEM_TYPE : BigLootbox<BigLootboxDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(BigLootboxDataBlock datablock) : base(datablock)
            {
            }
        }
    }
    
    public abstract class BigLootbox<T> : InventoryItem<T> where T : BigLootboxDataBlock
    {
        protected BigLootbox(T datablock) : base(datablock)
        {
        }

        public override void OnBeltUse()
        {
            T tDatablock = this.datablock;
            tDatablock.UseItem(iface);
        }
    }
}