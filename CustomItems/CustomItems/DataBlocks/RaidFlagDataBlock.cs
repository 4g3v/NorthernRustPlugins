//using System.IO;
//using System.Reflection;
//using Fougerite;
//using UnityEngine;
//
//namespace CustomItems.DataBlocks
//{
//    public class RaidFlagDataBlock : ItemDataBlock
//    {
//        public RaidFlagDataBlock()
//        {
//            name = "Raid Flag";
//
//            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\RaidFlag.png"));
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
//            WorldEditorServer.SpawnManager.StartPlacement(player, new WorldEditorServer.SpawnManager.SpawnableObject(0,
//                player.SteamID, "nr_B2x", "RaidFlag",
//                player.Location, Quaternion.identity, new Vector3(1f, 1f, 1f)), inventoryItem);
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
//
//        public override void OnBeltUse()
//        {
//            T tDatablock = this.datablock;
//            tDatablock.UseItem(iface);
//        }
//    }
//}