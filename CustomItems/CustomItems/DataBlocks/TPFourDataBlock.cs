using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class TPFourDataBlock : ItemDataBlock
    {
        public TPFourDataBlock()
        {
            name = "PocketWarp 4";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\TPFour.png"));
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
            uniqueIDField.SetValue(this, 134481359);
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

        public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
        {
            if (option != InventoryItem.MenuItem.Use)
                return base.ExecuteMenuOption(option, item);
            UseItem(item);
            return InventoryItem.MenuItemResult.DoneOnServer;
        }
        
        public virtual void UseItem(IInventoryItem inventoryItem)
        {
            var player = new Fougerite.Player(inventoryItem.controller.playerClient);
            player.TeleportTo(new Vector3(6660.546f, 353.1123f, -3852.007f));
            
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

        private new sealed class ITEM_TYPE : TPFour<TPFourDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(TPFourDataBlock datablock) : base(datablock)
            {
            }
        }
    }
    
    public abstract class TPFour<T> : InventoryItem<T> where T : TPFourDataBlock
    {
        protected TPFour(T datablock) : base(datablock)
        {
        }

        public override void OnBeltUse()
        {
            T tDatablock = this.datablock;
            tDatablock.UseItem(iface);
        }
    }
}