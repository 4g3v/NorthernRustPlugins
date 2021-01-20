using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class ParachuteDataBlock : ItemDataBlock
    {
        public static Dictionary<string, bool> ParachuteDictionary = new Dictionary<string, bool>();
        
        public ParachuteDataBlock()
        {
            name = "Parachute";
            itemDescriptionOverride = "Use it to toggle the parachute!";

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\Parachute.png"));
            iconTex = texture;

            _maxUses = 1;
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
            category = ItemCategory.Tools;
            Combinations = new CombineRecipe[0];
            transientMode = 0;

            var type = GetType().BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481360);
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

        public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
        {
            if (option != InventoryItem.MenuItem.Use)
                return base.ExecuteMenuOption(option, item);
            UseItem(item);
            return InventoryItem.MenuItemResult.DoneOnServer;
        }

        public virtual void UseItem(IInventoryItem inventoryItem)
        {
            var player = new Fougerite.Player(inventoryItem.controller.playerClient);
            
            if (ParachuteDictionary.ContainsKey(player.SteamID))
            {
                ParachuteDictionary[player.SteamID] = !ParachuteDictionary[player.SteamID];
            }
            else
            {
                ParachuteDictionary[player.SteamID] = true;
            }
            
            CustomItems.Instance.RPCDictionary[player.SteamID].UseParachute();
            
            foreach (var rpcDictionaryValue in CustomItems.Instance.RPCDictionary.Values)
            {
                rpcDictionaryValue.ShowParachute(player.SteamID, ParachuteDictionary[player.SteamID]);
            }
        }
        
        private new sealed class ITEM_TYPE : Parachute<ParachuteDataBlock>, IInventoryItem
        {
            public ITEM_TYPE(ParachuteDataBlock datablock) : base(datablock)
            {
            }
        }
    }
    
    public abstract class Parachute<T> : InventoryItem<T> where T : ParachuteDataBlock
    {
        protected Parachute(T datablock) : base(datablock)
        {
        }

        public override void OnBeltUse()
        {
            var db = datablock;
            db.UseItem(iface);
        }
    }
}