//using System.IO;
//using System.Reflection;
//using Fougerite;
//using UnityEngine;
//
//namespace CustomItems.DataBlocks
//{
//    public class TestItemDataBlock : ItemDataBlock
//    {
//        public TestItemDataBlock()
//        {
//            name = "TestItem";
//
//            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\test.png"));
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
//        public override IInventoryItem ConstructItem()
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
//            UseItem(item);
//            return InventoryItem.MenuItemResult.DoneOnServer;
//        }
//
//        public virtual void UseItem(IInventoryItem inventoryItem)
//        {
//            var player = new Fougerite.Player(inventoryItem.controller.playerClient);
//            WorldEditorServer.SpawnManager.StartPlacement(player, new WorldEditorServer.SpawnManager.SpawnableObject(0, player.SteamID, "test", "test",
//                player.Location, Quaternion.identity, new Vector3(1, 1, 1)), inventoryItem);
//        }
//
//        private new sealed class ITEM_TYPE : TestItem<TestItemDataBlock>, IInventoryItem
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
//
//        public override void OnBeltUse()
//        {
//            T tDatablock = this.datablock;
//            tDatablock.UseItem(iface);
//        }
//    }
//}