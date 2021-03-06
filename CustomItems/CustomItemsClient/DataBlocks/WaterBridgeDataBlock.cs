//using System.IO;
//using System.Reflection;
//using UnityEngine;
//
//namespace CustomItemsClient.DataBlocks
//{
//    public class WaterBridgeDataBlock : ItemDataBlock
//    {
//        public WaterBridgeDataBlock()
//        {
//            name = "Water Bridge";
//
//            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//            texture.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\CustomItems\\WaterBridge.png"));
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
//            uniqueIDField.SetValue(this, 134481413);
//        }
//
//        public override string GetItemDescription()
//        {
//            return "Spawns a Water Bridge which will disappear after 20 minutes. You can only place this near the water.";
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
//        private sealed class ITEM_TYPE : WaterBridge<WaterBridgeDataBlock>, IInventoryItem
//        {
//            public ITEM_TYPE(WaterBridgeDataBlock datablock) : base(datablock)
//            {
//            }
//        }
//    }
//    
//    public abstract class WaterBridge<T> : InventoryItem<T> where T : WaterBridgeDataBlock
//    {
//        protected WaterBridge(T datablock) : base(datablock)
//        {
//        }
//    }
//}