//using System.IO;
//using System.Reflection;
//using UnityEngine;
//
//namespace CustomItemsClient.DataBlocks
//{
//    public class RaidFlagDataBlock : ItemDataBlock
//    {
//        public RaidFlagDataBlock()
//        {
//            name = "Raid Flag";
//
//            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//            texture.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory +
//                                                "\\RB_Data\\CustomItems\\RaidFlag.png"));
//            iconTex = texture;
//
//            category = ItemCategory.Tools;
//            Combinations = new CombineRecipe[0];
//            _maxCondition = 100;
//            _maxUses = 5;
//            _splittable = true;
//
//            var type = GetType().BaseType.BaseType;
//            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
//            uniqueIDField.SetValue(this, 134481401);
//        }
//
//        public override string GetItemDescription()
//        {
//            return "Spawns a Raid Flag so that your enemy notices that he was raided.";
//        }
//
//        protected override IInventoryItem ConstructItem()
//        {
//            return new ITEM_TYPE(this);
//        }
//
//        public override int RetreiveMenuOptions(IInventoryItem item, InventoryItem.MenuItem[] results, int offset)
//        {
//            offset = base.RetreiveMenuOptions(item, results, offset);
//            if (item.isInLocalInventory)
//                results[offset++] = InventoryItem.MenuItem.Use;
//            return offset;
//        }
//
//        public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option,
//            IInventoryItem item)
//        {
//            if (option != InventoryItem.MenuItem.Use)
//                return base.ExecuteMenuOption(option, item);
//            return InventoryItem.MenuItemResult.DoneOnServer;
//        }
//
//        private sealed class ITEM_TYPE : RaidFlag<RaidFlagDataBlock>, IInventoryItem
//        {
//            public ITEM_TYPE(RaidFlagDataBlock datablock) : base(datablock)
//            {
//            }
//        }
//    }
//
//    public abstract class RaidFlag<T> : InventoryItem<T> where T : RaidFlagDataBlock
//    {
//        protected RaidFlag(T datablock) : base(datablock)
//        {
//        }
//    }
//}