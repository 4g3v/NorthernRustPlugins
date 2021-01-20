using System.IO;
using System.Reflection;
using RustBuster2016.API;
using UnityEngine;

namespace CustomItemsClient.DataBlocks
{
    public class DrugOneDataBlock : ItemDataBlock
    {
        public DrugOneDataBlock()
        {
            name = "Amphetamine";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Hooks.GameDirectory + "\\RB_Data\\CustomItems\\Amphetamine.png"));
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
            category = ItemCategory.Medical;
            UsedSound = new WWW(
                    "file://" + Hooks.GameDirectory +
                    "\\RB_Data\\CustomItems\\AmphetamineUsed.ogg")
                .GetAudioClip(true);
            Combinations = new CombineRecipe[0];
            transientMode = 0;
            
            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481355);
        }

        protected override IInventoryItem ConstructItem()
        {
            return new ITEM_TYPE(this);
        }
        
        public override int RetreiveMenuOptions(IInventoryItem item, InventoryItem.MenuItem[] results, int offset)
        {
            offset = base.RetreiveMenuOptions(item, results, offset);
            if (item.isInLocalInventory)
            {
                results[offset++] = InventoryItem.MenuItem.Use;
            }
            return offset;
        }
        
        public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
        {
            if (option != InventoryItem.MenuItem.Use)
                return base.ExecuteMenuOption(option, item);
            return InventoryItem.MenuItemResult.DoneOnServer;
        }
        
        public override string GetItemDescription()
        {
            return "Makes you go super fast. But be careful, don't overdose!";
        }
        
        private sealed class ITEM_TYPE : DrugOne<DrugOneDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(DrugOneDataBlock datablock) : base(datablock)
            {
            }
        }
    }
    
    public abstract class DrugOne<T> : InventoryItem<T> where T : DrugOneDataBlock
    {
        protected DrugOne(T datablock) : base(datablock)
        {
        }
    }
}