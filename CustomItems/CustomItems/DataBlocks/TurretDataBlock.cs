using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class TurretDataBlock : ItemDataBlock
    {
        public TurretDataBlock()
        {
            name = "Turret";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\Turret.png"));
            iconTex = texture;

            category = ItemCategory.Weapons;
            Combinations = new CombineRecipe[0];
            _maxCondition = 100;
            _maxUses = 1;
            _splittable = true;

            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481425);
        }

        public override string GetItemDescription()
        {
            return "Spawns a Turret. If a player gets too close to it, it will target and shoot that player.";
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

        public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option,
            IInventoryItem item)
        {
            if (option != InventoryItem.MenuItem.Use)
                return base.ExecuteMenuOption(option, item);
            UseItem(item);
            return InventoryItem.MenuItemResult.DoneOnServer;
        }

        public virtual void UseItem(IInventoryItem inventoryItem)
        {
            var player = new Fougerite.Player(inventoryItem.controller.playerClient);
            WorldEditorServer.SpawnManager.StartPlacement(player, new WorldEditorServer.SpawnManager.SpawnableObject(0,
                player.SteamID, "nr_Cxx", "Turret",
                player.Location, Quaternion.identity, new Vector3(1, 1, 1)), inventoryItem);
        }

        private sealed class ITEM_TYPE : Turret<TurretDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(TurretDataBlock datablock) : base(datablock)
            {
            }
        }
    }

    public abstract class Turret<T> : InventoryItem<T> where T : TurretDataBlock
    {
        protected Turret(T datablock) : base(datablock)
        {
        }

        public override void OnBeltUse()
        {
            T tDatablock = this.datablock;
            tDatablock.UseItem(iface);
        }
    }
}