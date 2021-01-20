using System.IO;
using System.Reflection;
using RustBuster2016.API;
using UnityEngine;

namespace CustomItemsClient.DataBlocks
{
    public class JointDataBlock : ItemDataBlock
    {
        public JointDataBlock()
        {
            name = "Joint";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Hooks.GameDirectory + "\\RB_Data\\CustomItems\\Joint.png"));
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
                    "\\RB_Data\\CustomItems\\JointUsed.ogg")
                .GetAudioClip(true);
            Combinations = new CombineRecipe[0];
            transientMode = 0;
            
            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481436);
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

        public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
        {
            if (option != InventoryItem.MenuItem.Use)
                return base.ExecuteMenuOption(option, item);
            return InventoryItem.MenuItemResult.DoneOnServer;
        }
        
        public override string GetItemDescription()
        {
            return "Makes you go slow and care about pain less.";
        }

        private new sealed class ITEM_TYPE : Joint<JointDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(JointDataBlock datablock) : base(datablock)
            {
            }
        }
    }

    public abstract class Joint<T> : InventoryItem<T> where T : JointDataBlock
    {
        protected Joint(T datablock) : base(datablock)
        {
        }
    }
}