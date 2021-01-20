using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class TPOneDataBlock : ItemDataBlock
    {
        public TPOneDataBlock()
        {
            name = "PocketWarp 1";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\TPOne.png"));
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
            uniqueIDField.SetValue(this, 134481356);
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
            player.TeleportTo(new Vector3(6077.474f, 376.276f, -3578.096f));
            
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

        private new sealed class ITEM_TYPE : TPOne<TPOneDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(TPOneDataBlock datablock) : base(datablock)
            {
            }
        }
    }
    
    public abstract class TPOne<T> : InventoryItem<T> where T : TPOneDataBlock
    {
        protected TPOne(T datablock) : base(datablock)
        {
        }

        public override void OnBeltUse()
        {
            T tDatablock = this.datablock;
            tDatablock.UseItem(iface);
        }
    }
}