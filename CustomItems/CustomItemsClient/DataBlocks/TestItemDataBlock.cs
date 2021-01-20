//using System.IO;
//using System.Reflection;
//using RustBuster2016.API;
//using UnityEngine;
//
//namespace CustomItemsClient.DataBlocks
//{
//    public class TestItemDataBlock : ItemDataBlock
//    {
//        public TestItemDataBlock()
//        {
//            name = "TestItem";
//
//            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//            texture.LoadImage(
//                File.ReadAllBytes(Hooks.GameDirectory + "\\RB_Data\\CustomItems\\test.png"));
//            iconTex = texture;
//
//            category = ItemCategory.Resource;
//            Combinations = new CombineRecipe[0];
//            _maxCondition = 100;
//            _maxUses = 0x000000FA;
//            _splittable = true;
//
//            var type = GetType().BaseType.BaseType;
//            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
//            uniqueIDField.SetValue(this, 134481338);
//        }
//
//        public override string GetItemDescription()
//        {
//            return "Does something.";
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
//        public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
//        {
//            if (option != InventoryItem.MenuItem.Use)
//                return base.ExecuteMenuOption(option, item);
//            return InventoryItem.MenuItemResult.DoneOnServer;
//        }
//
//        private sealed class ITEM_TYPE : TestItem<TestItemDataBlock>, IInventoryItem
//        {
//            public ITEM_TYPE(TestItemDataBlock datablock) : base(datablock)
//            {
//            }
//        }
//    }
//    
//    public abstract class TestItem<T> : InventoryItem<T> where T : TestItemDataBlock
//    {
//        protected TestItem(T datablock) : base(datablock)
//        {
//        }
//    }
//}