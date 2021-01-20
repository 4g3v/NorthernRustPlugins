//using System.IO;
//using System.Reflection;
//using UnityEngine;
//
//namespace CustomItemsClient.DataBlocks
//{
//    public class PortalRadioDataBlock : ItemDataBlock
//    {
//        public PortalRadioDataBlock()
//        {
//            name = "Portal Radio";
//
//            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//            texture.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\CustomItems\\PortalRadio.png"));
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
//            uniqueIDField.SetValue(this, 134481395);
//        }
//
//        public override string GetItemDescription()
//        {
//            return "Spawns Valve's Portal Radio and let's you listen to the theme.";
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
//        private sealed class ITEM_TYPE : PortalRadio<PortalRadioDataBlock>, IInventoryItem
//        {
//            public ITEM_TYPE(PortalRadioDataBlock datablock) : base(datablock)
//            {
//            }
//        }
//    }
//
//    public abstract class PortalRadio<T> : InventoryItem<T> where T : PortalRadioDataBlock
//    {
//        protected PortalRadio(T datablock) : base(datablock)
//        {
//        }
//    }
//}