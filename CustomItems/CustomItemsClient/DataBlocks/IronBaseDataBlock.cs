//using System.IO;
//using System.Reflection;
//using RustBuster2016.API;
//using UnityEngine;
//
//namespace CustomItemsClient.DataBlocks
//{
//    public class IronBaseDataBlock : ItemDataBlock
//    {
//        public IronBaseDataBlock()
//        {
//            name = "Iron Base";
//
//            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//            texture.LoadImage(File.ReadAllBytes(Hooks.GameDirectory + "\\RB_Data\\CustomItems\\IronBase.png"));
//
//            iconTex = texture;
//
//            category = ItemCategory.Tools;
//            Combinations = new CombineRecipe[0];
//            _maxCondition = 100;
//            _maxUses = 1;
//            _splittable = true;
//
//            var type = GetType().BaseType.BaseType;
//            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
//            uniqueIDField.SetValue(this, 134481403);
//        }
//
//        public override string GetItemDescription()
//        {
//            return "Spawns an Iron Base.";
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
//        private sealed class ITEM_TYPE : IronBase<IronBaseDataBlock>, IInventoryItem
//        {
//            public ITEM_TYPE(IronBaseDataBlock datablock) : base(datablock)
//            {
//            }
//        }
//    }
//
//    public abstract class IronBase<T> : InventoryItem<T> where T : IronBaseDataBlock
//    {
//        protected IronBase(T datablock) : base(datablock)
//        {
//        }
//    }
//}