using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomItems.DataBlocks;
using Fougerite;
using Fougerite.Events;
using RustBuster2016Server;
using UnityEngine;
using Module = Fougerite.Module;

namespace CustomItems
{
    public class CustomItems : Module
    {
        public override string Name => "CustomItems";
        public override string Author => "4g3v";
        public override Version Version => new Version(1, 0);
        public static CustomItems Instance;

        private bool _reloaded = false;

        private ItemDataBlock[] _all;
        private int _oldAllLength;
        private FieldInfo _allField;

        private Dictionary<string, int> _dataBlocks;
        private FieldInfo _dataBlocksField;

        private Dictionary<int, int> _dataBlocksByUniqueID;
        private FieldInfo _datablocksByUniqueIDField;

        public List<ItemDataBlock> NewItems = new List<ItemDataBlock>();

        private AVXDollarDataBlock _avxDollarDataBlock;

        public Dictionary<string, RPCBehaviour> RPCDictionary = new Dictionary<string, RPCBehaviour>();
        public static ItemsBlocks Items;

//        private GameObject _loadObject;

        public override void Initialize()
        {
            Instance = this;

            Hooks.OnItemsLoaded += OnItemsLoaded;

            Hooks.OnServerLoaded += OnServerLoaded;
            Hooks.OnPlayerConnected += OnPlayerConnected;
            Hooks.OnPlayerKilled += OnPlayerKilled;
            Hooks.OnPlayerDisconnected += OnPlayerDisconnected;
            Hooks.OnPlayerHurt += OnPlayerHurt;

//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\test.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\BreachGrenade.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\SmokeGrenade.png"));

//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\AbsolomM4.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\InstantHealth.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\InstantHealthUsed.ogg"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\AVXDollar.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\Amphetamine.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\AmphetamineOverlay.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\AmphetamineUsed.ogg"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\TPOne.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\TPTwo.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\TPThree.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\TPFour.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\Parachute.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\ParachuteOverlay.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\HealingStation.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\Lootbox.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\BigLootbox.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\Landmine.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\RespawnFlag.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\MetalBarricade.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\ElectronicJunk.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\Copper.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\Batteries.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\PortalRadio.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\Bookshelf.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\IronShards.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\RaidFlag.png"));
//            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("CustomItems\\",
//                Util.GetRootFolder() + "\\Save\\CustomItems\\IronBase.png"));

            if (_reloaded)
                OnServerLoaded();
        }

        private void OnPlayerHurt(HurtEvent he)
        {
            if (he.VictimIsPlayer && !he.VictimIsSleeper)
            {
                var victimPlayer = (Fougerite.Player) he.Victim;
                if (RPCBehaviour.JointUsers.Contains(victimPlayer.SteamID))
                {
                    he.DamageAmount *= 0.5f;
                }
            }
        }

        private void OnPlayerKilled(DeathEvent de)
        {
            var player = (Fougerite.Player) de.Victim;

            if (ParachuteDataBlock.ParachuteDictionary.ContainsKey(player.SteamID))
            {
                ParachuteDataBlock.ParachuteDictionary.Remove(player.SteamID);
            }

            foreach (var rpcDictionaryValue in RPCDictionary.Values)
            {
                rpcDictionaryValue.ShowParachute(player.SteamID, false);
            }
        }

        private void OnItemsLoaded(ItemsBlocks items)
        {
            Items = items;

            #region Items

//            var testItemDataBlock = ScriptableObject.CreateInstance<TestItemDataBlock>();
//            NewItems.Add(testItemDataBlock);
//            AddItem(items, testItemDataBlock);

            var modifiedM4DataBlock = ScriptableObject.CreateInstance<ModifiedM4DataBlock>();
            modifiedM4DataBlock.ammoType = (AmmoItemDataBlock) items.Find(idb => idb.name == "9mm Ammo");
            modifiedM4DataBlock.deploySound = Facepunch.Bundling.Load<AudioClip>("content/item/m4/sfx/m4_deploy");
            modifiedM4DataBlock.dryFireSound = Facepunch.Bundling.Load<AudioClip>("content/shared/sfx/dryfire_rifle-1");
            modifiedM4DataBlock.fireSound = Facepunch.Bundling.Load<AudioClip>("content/item/pistol/sfx/pistol_fire_4");
            modifiedM4DataBlock.fireSound_Far = Facepunch.Bundling.Load<AudioClip>("content/effect/sfx/rifle_far");
            modifiedM4DataBlock.fireSound_Silenced =
                Facepunch.Bundling.Load<AudioClip>("content/effect/sfx/silenced_shot");
            modifiedM4DataBlock.muzzleflashVM = Facepunch.Bundling.Load<GameObject>("content/effect/Flash");
            modifiedM4DataBlock.muzzleFlashWorld = Facepunch.Bundling.Load<GameObject>("content/effect/flash_tp");
            modifiedM4DataBlock.reloadSound = Facepunch.Bundling.Load<AudioClip>("content/item/sfx/gsr1911_reload-1");
            modifiedM4DataBlock.shotBob = Facepunch.Bundling.Load<BobEffect>("content/headbob/effect/Gun Kickback");
            modifiedM4DataBlock.tracerPrefab = Facepunch.Bundling.Load<GameObject>("content/effect/TracerPrefab");
            modifiedM4DataBlock._itemRepPrefab = Facepunch.Bundling.Load<ItemRepresentation>("content/item/m4/m4.irp");
            modifiedM4DataBlock._viewModelPrefab = Facepunch.Bundling.Load<ViewModel>("content/item/m4/VM-M4");
            NewItems.Add(modifiedM4DataBlock);
            AddItem(items, modifiedM4DataBlock);

//            LoadWeaponStuff();

            var instantHealthItemDataBlock = ScriptableObject.CreateInstance<InstantHealthItemDataBlock>();
            NewItems.Add(instantHealthItemDataBlock);
            AddItem(items, instantHealthItemDataBlock);

            var instantHealthBlueprintDataBlock = ScriptableObject.CreateInstance<InstantHealthBlueprintDataBlock>();
            NewItems.Add(instantHealthBlueprintDataBlock);
            AddItem(items, instantHealthBlueprintDataBlock);

            _avxDollarDataBlock = ScriptableObject.CreateInstance<AVXDollarDataBlock>();
            NewItems.Add(_avxDollarDataBlock);
            AddItem(items, _avxDollarDataBlock);

            var drugOneDataBlock = ScriptableObject.CreateInstance<DrugOneDataBlock>();
            NewItems.Add(drugOneDataBlock);
            AddItem(items, drugOneDataBlock);

            var tpOneDataBlock = ScriptableObject.CreateInstance<TPOneDataBlock>();
            NewItems.Add(tpOneDataBlock);
            AddItem(items, tpOneDataBlock);

            var tpTwoDataBlock = ScriptableObject.CreateInstance<TPTwoDataBlock>();
            NewItems.Add(tpTwoDataBlock);
            AddItem(items, tpTwoDataBlock);

            var tpThreeDataBlock = ScriptableObject.CreateInstance<TPThreeDataBlock>();
            NewItems.Add(tpThreeDataBlock);
            AddItem(items, tpThreeDataBlock);

            var tpFourDataBlock = ScriptableObject.CreateInstance<TPFourDataBlock>();
            NewItems.Add(tpFourDataBlock);
            AddItem(items, tpFourDataBlock);

            var parachuteDataBlock = ScriptableObject.CreateInstance<ParachuteDataBlock>();
            NewItems.Add(parachuteDataBlock);
            AddItem(items, parachuteDataBlock);

            var electronicJunkDataBlock = ScriptableObject.CreateInstance<ElectronicJunkDataBlock>();
            NewItems.Add(electronicJunkDataBlock);
            AddItem(items, electronicJunkDataBlock);

            var copperDataBlock = ScriptableObject.CreateInstance<CopperDataBlock>();
            NewItems.Add(copperDataBlock);
            AddItem(items, copperDataBlock);

            var batteriesDataBlock = ScriptableObject.CreateInstance<BatteriesDataBlock>();
            NewItems.Add(batteriesDataBlock);
            AddItem(items, batteriesDataBlock);

            var ironShardsDataBlock = ScriptableObject.CreateInstance<IronShardsDataBlock>();
            NewItems.Add(ironShardsDataBlock);
            AddItem(items, ironShardsDataBlock);

            var healingStationDataBlock = ScriptableObject.CreateInstance<HealingStationDataBlock>();
            NewItems.Add(healingStationDataBlock);
            AddItem(items, healingStationDataBlock);

            var healingStationBlueprintDataBlock = ScriptableObject.CreateInstance<HealingStationBlueprintDataBlock>();
            NewItems.Add(healingStationBlueprintDataBlock);
            AddItem(items, healingStationBlueprintDataBlock);

            var lootboxDataBlock = ScriptableObject.CreateInstance<LootboxDataBlock>();
            NewItems.Add(lootboxDataBlock);
            AddItem(items, lootboxDataBlock);

            var lootboxBlueprintDataBlock = ScriptableObject.CreateInstance<LootboxBlueprintDataBlock>();
            NewItems.Add(lootboxBlueprintDataBlock);
            AddItem(items, lootboxBlueprintDataBlock);

            var bigLootboxDataBlock = ScriptableObject.CreateInstance<BigLootboxDataBlock>();
            NewItems.Add(bigLootboxDataBlock);
            AddItem(items, bigLootboxDataBlock);

            var bigLootboxBlueprintDataBlock = ScriptableObject.CreateInstance<BigLootboxBlueprintDataBlock>();
            NewItems.Add(bigLootboxBlueprintDataBlock);
            AddItem(items, bigLootboxBlueprintDataBlock);

            var landmineDataBlock = ScriptableObject.CreateInstance<LandmineDataBlock>();
            NewItems.Add(landmineDataBlock);
            AddItem(items, landmineDataBlock);

            var landmineBlueprintDataBlock = ScriptableObject.CreateInstance<LandmineBlueprintDataBlock>();
            NewItems.Add(landmineBlueprintDataBlock);
            AddItem(items, landmineBlueprintDataBlock);

            var respawnFlagDataBlock = ScriptableObject.CreateInstance<RespawnFlagDataBlock>();
            NewItems.Add(respawnFlagDataBlock);
            AddItem(items, respawnFlagDataBlock);

            var respawnFlagBlueprintDataBlock = ScriptableObject.CreateInstance<RespawnFlagBlueprintDataBlock>();
            NewItems.Add(respawnFlagBlueprintDataBlock);
            AddItem(items, respawnFlagBlueprintDataBlock);

            var metalBarricadeDataBlock = ScriptableObject.CreateInstance<MetalBarricadeDataBlock>();
            NewItems.Add(metalBarricadeDataBlock);
            AddItem(items, metalBarricadeDataBlock);

            var metalBarricadeBlueprintDataBlock = ScriptableObject.CreateInstance<MetalBarricadeBlueprintDataBlock>();
            NewItems.Add(metalBarricadeBlueprintDataBlock);
            AddItem(items, metalBarricadeBlueprintDataBlock);

//            var portalRadioDataBlock = ScriptableObject.CreateInstance<PortalRadioDataBlock>();
//            NewItems.Add(portalRadioDataBlock);
//            AddItem(items, portalRadioDataBlock);
//
//            var portalRadioBlueprintDataBlock = ScriptableObject.CreateInstance<PortalRadioBlueprintDataBlock>();
//            NewItems.Add(portalRadioBlueprintDataBlock);
//            AddItem(items, portalRadioBlueprintDataBlock);

//            var bookshelfDataBlock = ScriptableObject.CreateInstance<BookshelfDataBlock>();
//            NewItems.Add(bookshelfDataBlock);
//            AddItem(items, bookshelfDataBlock);
//
//            var bookshelfBlueprintDataBlock = ScriptableObject.CreateInstance<BookshelfBlueprintDataBlock>();
//            NewItems.Add(bookshelfBlueprintDataBlock);
//            AddItem(items, bookshelfBlueprintDataBlock);

            var absolomM4BlueprintDataBlock = ScriptableObject.CreateInstance<AbsolomM4BlueprintDataBlock>();
            NewItems.Add(absolomM4BlueprintDataBlock);
            AddItem(items, absolomM4BlueprintDataBlock);

//            var raidFlagDataBlock = ScriptableObject.CreateInstance<RaidFlagDataBlock>();
//            NewItems.Add(raidFlagDataBlock);
//            AddItem(items, raidFlagDataBlock);
//
//            var raidFlagBlueprintDataBlock = ScriptableObject.CreateInstance<RaidFlagBlueprintDataBlock>();
//            NewItems.Add(raidFlagBlueprintDataBlock);
//            AddItem(items, raidFlagBlueprintDataBlock);

//            var ironBaseDataBlock = ScriptableObject.CreateInstance<IronBaseDataBlock>();
//            NewItems.Add(ironBaseDataBlock);
//            AddItem(items, ironBaseDataBlock);
//
//            var ironBaseBlueprintDataBlock = ScriptableObject.CreateInstance<IronBaseBlueprintDataBlock>();
//            NewItems.Add(ironBaseBlueprintDataBlock);
//            AddItem(items, ironBaseBlueprintDataBlock);

//            var smallLadderDataBlock = ScriptableObject.CreateInstance<SmallLadderDataBlock>();
//            NewItems.Add(smallLadderDataBlock);
//            AddItem(items, smallLadderDataBlock);
//
//            var bigLadderDataBlock = ScriptableObject.CreateInstance<BigLadderDataBlock>();
//            NewItems.Add(bigLadderDataBlock);
//            AddItem(items, bigLadderDataBlock);
//
//            var smallLadderBlueprintDataBlock = ScriptableObject.CreateInstance<SmallLadderBlueprintDataBlock>();
//            NewItems.Add(smallLadderBlueprintDataBlock);
//            AddItem(items, smallLadderBlueprintDataBlock);
//
//            var bigLadderBlueprintDataBlock = ScriptableObject.CreateInstance<BigLadderBlueprintDataBlock>();
//            NewItems.Add(bigLadderBlueprintDataBlock);
//            AddItem(items, bigLadderBlueprintDataBlock);

            var shelfDataBlock = ScriptableObject.CreateInstance<ShelfDataBlock>();
            NewItems.Add(shelfDataBlock);
            AddItem(items, shelfDataBlock);

            var shelfBlueprintDataBlock = ScriptableObject.CreateInstance<ShelfBlueprintDataBlock>();
            NewItems.Add(shelfBlueprintDataBlock);
            AddItem(items, shelfBlueprintDataBlock);

            var oneManBunkerDataBlock = ScriptableObject.CreateInstance<OneManBunkerDataBlock>();
            NewItems.Add(oneManBunkerDataBlock);
            AddItem(items, oneManBunkerDataBlock);

            var oneManBunkerBlueprintDataBlock = ScriptableObject.CreateInstance<OneManBunkerBlueprintDataBlock>();
            NewItems.Add(oneManBunkerBlueprintDataBlock);
            AddItem(items, oneManBunkerBlueprintDataBlock);

//            var waterBridgeDataBlock = ScriptableObject.CreateInstance<WaterBridgeDataBlock>();
//            NewItems.Add(waterBridgeDataBlock);
//            AddItem(items, waterBridgeDataBlock);
//            
//            var waterBridgeBlueprintDataBlock = ScriptableObject.CreateInstance<WaterBridgeBlueprintDataBlock>();
//            NewItems.Add(waterBridgeBlueprintDataBlock);
//            AddItem(items, waterBridgeBlueprintDataBlock);
//            
//            var woodFactoryDataBlock = ScriptableObject.CreateInstance<WoodFactoryDataBlock>();
//            NewItems.Add(woodFactoryDataBlock);
//            AddItem(items, woodFactoryDataBlock);
//
//            var woodFactoryBlueprintDataBlock = ScriptableObject.CreateInstance<WoodFactoryBlueprintDataBlock>();
//            NewItems.Add(woodFactoryBlueprintDataBlock);
//            AddItem(items, woodFactoryBlueprintDataBlock);

            var flagDataBlock = ScriptableObject.CreateInstance<FlagDataBlock>();
            NewItems.Add(flagDataBlock);
            AddItem(items, flagDataBlock);

            var keycardADataBlock = ScriptableObject.CreateInstance<KeycardADataBlock>();
            NewItems.Add(keycardADataBlock);
            AddItem(items, keycardADataBlock);

            var keycardBDataBlock = ScriptableObject.CreateInstance<KeycardBDataBlock>();
            NewItems.Add(keycardBDataBlock);
            AddItem(items, keycardBDataBlock);

            var keycardCDataBlock = ScriptableObject.CreateInstance<KeycardCDataBlock>();
            NewItems.Add(keycardCDataBlock);
            AddItem(items, keycardCDataBlock);

            var keycardC1DataBlock = ScriptableObject.CreateInstance<KeycardC1DataBlock>();
            NewItems.Add(keycardC1DataBlock);
            AddItem(items, keycardC1DataBlock);

            var diamondsDataBlock = ScriptableObject.CreateInstance<DiamondsDataBlock>();
            NewItems.Add(diamondsDataBlock);
            AddItem(items, diamondsDataBlock);

            var buildingLootboxDataBlock = ScriptableObject.CreateInstance<BuildingLootboxDataBlock>();
            NewItems.Add(buildingLootboxDataBlock);
            AddItem(items, buildingLootboxDataBlock);

            var buildingLootboxBlueprintDataBlock =
                ScriptableObject.CreateInstance<BuildingLootboxBlueprintDataBlock>();
            NewItems.Add(buildingLootboxBlueprintDataBlock);
            AddItem(items, buildingLootboxBlueprintDataBlock);

            var turretDataBlock = ScriptableObject.CreateInstance<TurretDataBlock>();
            NewItems.Add(turretDataBlock);
            AddItem(items, turretDataBlock);

            var turretBlueprintDataBlock = ScriptableObject.CreateInstance<TurretBlueprintDataBlock>();
            NewItems.Add(turretBlueprintDataBlock);
            AddItem(items, turretBlueprintDataBlock);

            var turretSilencerDataBlock = ScriptableObject.CreateInstance<TurretSilencerDataBlock>();
            NewItems.Add(turretSilencerDataBlock);
            AddItem(items, turretSilencerDataBlock);

            var turretRangeExtensionDataBlock = ScriptableObject.CreateInstance<TurretRangeExtensionDataBlock>();
            NewItems.Add(turretRangeExtensionDataBlock);
            AddItem(items, turretRangeExtensionDataBlock);

            var turretDamageExtension = ScriptableObject.CreateInstance<TurretDamageExtension>();
            NewItems.Add(turretDamageExtension);
            AddItem(items, turretDamageExtension);

            var silencedTurretDataBlock = ScriptableObject.CreateInstance<SilencedTurretDataBlock>();
            NewItems.Add(silencedTurretDataBlock);
            AddItem(items, silencedTurretDataBlock);

            var silencedTurretBlueprintDataBlock = ScriptableObject.CreateInstance<SilencedTurretBlueprintDataBlock>();
            NewItems.Add(silencedTurretBlueprintDataBlock);
            AddItem(items, silencedTurretBlueprintDataBlock);

            var rangeTurretDataBlock = ScriptableObject.CreateInstance<RangeTurretDataBlock>();
            NewItems.Add(rangeTurretDataBlock);
            AddItem(items, rangeTurretDataBlock);

            var rangeTurretBlueprintDataBlock = ScriptableObject.CreateInstance<RangeTurretBlueprintDataBlock>();
            NewItems.Add(rangeTurretBlueprintDataBlock);
            AddItem(items, rangeTurretBlueprintDataBlock);

            var damageTurretDataBlock = ScriptableObject.CreateInstance<DamageTurretDataBlock>();
            NewItems.Add(damageTurretDataBlock);
            AddItem(items, damageTurretDataBlock);

            var damageTurretBlueprintDataBlock = ScriptableObject.CreateInstance<DamageTurretBlueprintDataBlock>();
            NewItems.Add(damageTurretBlueprintDataBlock);
            AddItem(items, damageTurretBlueprintDataBlock);

            var jointDataBlock = ScriptableObject.CreateInstance<JointDataBlock>();
            NewItems.Add(jointDataBlock);
            AddItem(items, jointDataBlock);

            #endregion

            Logger.Log("[CustomItems] Loaded all items.");
        }

        private void OnPlayerDisconnected(Fougerite.Player player)
        {
            if (RPCDictionary.ContainsKey(player.SteamID))
                RPCDictionary.Remove(player.SteamID);

            if (ParachuteDataBlock.ParachuteDictionary.ContainsKey(player.SteamID))
            {
                ParachuteDataBlock.ParachuteDictionary.Remove(player.SteamID);
            }

            foreach (var rpcDictionaryValue in RPCDictionary.Values)
            {
                rpcDictionaryValue.ShowParachute(player.SteamID, false);
            }
        }

        private void OnPlayerConnected(Fougerite.Player player)
        {
            var playerRPCBehaviour = player.PlayerClient.gameObject.GetComponent<RPCBehaviour>();
            if (playerRPCBehaviour == null)
                playerRPCBehaviour = player.PlayerClient.gameObject.AddComponent<RPCBehaviour>();

            playerRPCBehaviour.FougeritePlayer = player;

            if (!RPCDictionary.ContainsKey(player.SteamID))
            {
                RPCDictionary[player.SteamID] = playerRPCBehaviour;
            }
        }

        public override void DeInitialize()
        {
            Hooks.OnItemsLoaded -= OnItemsLoaded;

            Hooks.OnServerLoaded -= OnServerLoaded;
            Hooks.OnPlayerConnected -= OnPlayerConnected;
            Hooks.OnPlayerKilled -= OnPlayerKilled;
            Hooks.OnPlayerDisconnected -= OnPlayerDisconnected;
            Hooks.OnPlayerHurt -= OnPlayerHurt;


            foreach (var itemDataBlock in NewItems)
            {
                _dataBlocks.Remove(itemDataBlock.name);
                _dataBlocksByUniqueID.Remove(itemDataBlock.uniqueID);
            }

            ItemDataBlock[] _newAll = new ItemDataBlock[_oldAllLength];
            for (var i = 0; i < _oldAllLength; i++)
            {
                _newAll[i] = _all[i];
            }

            _dataBlocksField.SetValue(null, _dataBlocks);
            _datablocksByUniqueIDField.SetValue(null, _dataBlocksByUniqueID);
            _allField.SetValue(null, _newAll);

            NewItems.Clear();
            _all = null;
            _dataBlocks = null;
            _dataBlocksByUniqueID = null;

            _reloaded = true;

//            UnityEngine.Object.Destroy(_loadObject);

            Logger.Log("Removed all custom items!");
        }

        private void AddLootEntry(string list, LootSpawnList.LootWeightedEntry entry)
        {
            var lootList = DatablockDictionary._lootSpawnLists[list];

            var lootWeightedEntries = lootList.LootPackages.ToList();
            lootWeightedEntries.Add(entry);

            lootList.LootPackages = lootWeightedEntries.ToArray();

            Logger.Log("[CustomItems] Added loot entry \"" + entry.obj.name + "\" to LootTable " + list);
        }

        private void OnServerLoaded()
        {
            var datablockDictionaryType = typeof(DatablockDictionary);

            _allField = datablockDictionaryType.GetField("_all", BindingFlags.NonPublic | BindingFlags.Static);
            _all = (ItemDataBlock[]) _allField.GetValue(null);
            _oldAllLength = _all.Length;

            _dataBlocksField =
                datablockDictionaryType.GetField("_dataBlocks", BindingFlags.NonPublic | BindingFlags.Static);
            _dataBlocks = (Dictionary<string, int>) _dataBlocksField.GetValue(null);

            _datablocksByUniqueIDField = datablockDictionaryType.GetField("_dataBlocksByUniqueID",
                BindingFlags.NonPublic | BindingFlags.Static);
            _dataBlocksByUniqueID = (Dictionary<int, int>) _datablocksByUniqueIDField.GetValue(null);

            #region LootTables

            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 10, amountMax = 20, weight = 30, obj = _avxDollarDataBlock
            });
            AddLootEntry("SupplyDropA", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 50, amountMax = 125, weight = 85, obj = _avxDollarDataBlock
            });
            AddLootEntry("SupplyDropB", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 50, amountMax = 125, weight = 85, obj = _avxDollarDataBlock
            });
            AddLootEntry("SupplyDropC", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 50, amountMax = 125, weight = 85, obj = _avxDollarDataBlock
            });
            AddLootEntry("SupplyDropD", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 50, amountMax = 125, weight = 85, obj = _avxDollarDataBlock
            });
            AddLootEntry("SupplyDropE", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 50, amountMax = 125, weight = 1, obj = _avxDollarDataBlock
            });
            AddLootEntry("MedicalSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 23, obj = Items.Find(i => i.name == "Adrenaline Rush")
            });
            AddLootEntry("WeaponSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "AbsolomM4")
            });
            AddLootEntry("MedicalSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 10, obj = Items.Find(i => i.name == "Amphetamine")
            });
            AddLootEntry("MedicalSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 10, obj = Items.Find(i => i.name == "Joint")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "PocketWarp 1")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "PocketWarp 2")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "PocketWarp 3")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "PocketWarp 4")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Lootbox")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 2, weight = 7, obj = Items.Find(i => i.name == "Electronic Junk")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 2, weight = 5, obj = Items.Find(i => i.name == "Copper")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 7, obj = Items.Find(i => i.name == "Batteries")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 20, amountMax = 40, weight = 23, obj = Items.Find(i => i.name == "Iron Shards")
            });
//            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Raid Flag")
//            });
//            AddLootEntry("SupplyDropA", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Iron Base")
//            });
//            AddLootEntry("SupplyDropB", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Iron Base")
//            });
//            AddLootEntry("SupplyDropC", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Iron Base")
//            });
//            AddLootEntry("SupplyDropD", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Iron Base")
//            });
//            AddLootEntry("SupplyDropE", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Iron Base")
//            });
            AddLootEntry("MedicalSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 8, obj = Items.Find(i => i.name == "Adrenaline Rush Blueprint")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Adrenaline Rush Blueprint")
            });
            AddLootEntry("MedicalSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 8, obj = Items.Find(i => i.name == "Healing Station Blueprint")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Lootbox Blueprint")
            });
            AddLootEntry("SupplyDropE", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Big Lootbox Blueprint")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Landmine Blueprint")
            });
            AddLootEntry("WeaponSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1.5f, obj = Items.Find(i => i.name == "Landmine Blueprint")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Respawn Flag Blueprint")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 2, weight = 3, obj = Items.Find(i => i.name == "Metal Barricade Blueprint")
            });
//            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Portal Radio Blueprint")
//            });
//            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Bookshelf Blueprint")
//            });
            AddLootEntry("WeaponSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "AbsolomM4 Blueprint")
            });
//            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Raid Flag Blueprint")
//            });
//            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Iron Base Blueprint")
//            });
//            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Small Ladder Blueprint")
//            });
//            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Big Ladder Blueprint")
//            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Shelf Blueprint")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "One Man Bunker Blueprint")
            });
//            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Water Bridge Blueprint")
//            });
//            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
//            {
//                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Wood Factory Blueprint")
//            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Building Lootbox Blueprint")
            });
            AddLootEntry("JunkSpawnList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Turret Blueprint")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Keycard A")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Keycard B")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Keycard C")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Keycard C1")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 1, obj = Items.Find(i => i.name == "Parachute")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Turret Silencer")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Turret Range Extension")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Turret Damage Extension")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Silenced Turret Blueprint")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Range Turret Blueprint")
            });
            AddLootEntry("AILootList", new LootSpawnList.LootWeightedEntry
            {
                amountMin = 1, amountMax = 1, weight = 3, obj = Items.Find(i => i.name == "Heavy Turret Blueprint")
            });

            #endregion

            #region fucked

//            var breachGrenadeDataBlock = ScriptableObject.CreateInstance<C4GrenadeDataBlock>();
//            breachGrenadeDataBlock._itemRepPrefab =
//                Facepunch.Bundling.Load<ItemRepresentation>("content/item/f1/f1.irp");
//            breachGrenadeDataBlock._viewModelPrefab = Facepunch.Bundling.Load<ViewModel>("content/item/f1/VM-F1");
//            breachGrenadeDataBlock.deploySound =
//                Facepunch.Bundling.Load<AudioClip>("content/shared/sfx/draw_generic_rustle");
//            
//            var newThrowObject =
//                (GameObject) UnityEngine.Object.Instantiate(
//                    Facepunch.Bundling.Load<GameObject>("content/item/f1/F1GrenadeWorld"));
//            newThrowObject.active = false;
//            newThrowObject.name = "BreachGrenadeWorld";
//
//            var timedGrenadeComponent = newThrowObject.GetComponent<TimedGrenade>();
//            timedGrenadeComponent.damage = 5000f;
//
//            UnityEngine.Object.DestroyImmediate(newThrowObject.GetComponent<RigidbodyInterpolator>());
//            UnityEngine.Object.DestroyImmediate(newThrowObject.GetComponent<uLinkNetworkView>());
//
//            var breachView = newThrowObject.AddComponent<uLinkNetworkView>();
//            breachView.stateSynchronization = uLink.NetworkStateSynchronization.Off;
//            breachView.SetViewID(new uLink.NetworkViewID(0), uLink.NetworkPlayer.server);
//            breachView.observed = timedGrenadeComponent;
//            breachView.rpcReceiver = RPCReceiver.OnlyObservedComponent;
//            breachView.prefabRoot = newThrowObject;
//            var nvBase = breachView.GetType().BaseType.BaseType.BaseType;
//            var childIndexField = nvBase.GetField("_childIndex", BindingFlags.NonPublic | BindingFlags.Static);
//            childIndexField.SetValue(breachView, 0);
//
//            var newRigidbodyInterpolator = newThrowObject.AddComponent<RigidbodyInterpolator>();
//            newRigidbodyInterpolator.target = newThrowObject.rigidbody;
//
//            var allField = Type
//                .GetType(
//                    "NetCull+AutoPrefabs, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
//                .GetField("all");
//            Dictionary<string, uLinkNetworkView> allDictionary =
//                (Dictionary<string, uLinkNetworkView>) allField.GetValue(null);
//            
//            allDictionary.Add("BreachGrenadeWorld", breachView);
//            allField.SetValue(null, allDictionary);
//
////            NetworkInstantiator.AddPrefab(newThrowObject);
//
//            var defaultCreator = NetworkInstantiator.CreateDefaultCreator(newThrowObject);
//            var uLinkCreator = new uLinkCreator();
//            uLinkCreator.networkView_0 = breachView;
//            NetworkInstantiator.Add("BreachGrenadeWorld", new NetworkInstantiator.Creator(uLinkCreator.method_0), NetworkInstantiator.defaultDestroyer);
//            
//            breachGrenadeDataBlock.throwObjectPrefab = newThrowObject;
//            
//            breachGrenadeDataBlock.pullPinSound = Facepunch.Bundling.Load<AudioClip>("content/item/sfx/grenade_arm");
//            _newItems.Add(breachGrenadeDataBlock);
//            AddItem(breachGrenadeDataBlock);
//
//            var smokeGrenadeDataBlock = ScriptableObject.CreateInstance<SmokeGrenadeDataBlock>();
//            smokeGrenadeDataBlock._itemRepPrefab =
//                Facepunch.Bundling.Load<ItemRepresentation>("content/item/f1/f1.irp");
//            smokeGrenadeDataBlock._viewModelPrefab = Facepunch.Bundling.Load<ViewModel>("content/item/f1/VM-F1");
//            smokeGrenadeDataBlock.deploySound =
//                Facepunch.Bundling.Load<AudioClip>("content/shared/sfx/draw_generic_rustle");
//            smokeGrenadeDataBlock.pullPinSound = Facepunch.Bundling.Load<AudioClip>("content/item/sfx/grenade_arm");
//            smokeGrenadeDataBlock.throwObjectPrefab =
//                (GameObject) UnityEngine.Object.Instantiate(
//                    Facepunch.Bundling.Load<GameObject>("content/item/f1/SupplySignalGrenade"));
//
//            smokeGrenadeDataBlock.throwObjectPrefab.name =
//                smokeGrenadeDataBlock.throwObjectPrefab.name.Replace("(Clone)", "");
//
//            UnityEngine.Object.Destroy(smokeGrenadeDataBlock.throwObjectPrefab.GetComponent<SignalGrenade>());
//            var smokeGrenade = smokeGrenadeDataBlock.throwObjectPrefab.AddComponent<SmokeGrenade>();
//            smokeGrenade.bounceSound = Facepunch.Bundling.Load<AudioClip>("content/item/sfx/grenade_bounce_1");
//            smokeGrenade.explosionEffect =
//                Facepunch.Bundling.Load<GameObject>("rust/effects/particles/smoke/SignalSmokeGreen");
//
//            var smokeView = smokeGrenadeDataBlock.throwObjectPrefab.GetComponent<uLinkNetworkView>();
//            smokeView.observed = smokeGrenade;
//
//            _newItems.Add(smokeGrenadeDataBlock);
//            AddItem(smokeGrenadeDataBlock);

//            _newItems.Add(ScriptableObject.CreateInstance<TestItem>());

            #endregion
        }

//        private void LoadWeaponStuff()
//        {
//            _loadObject = new GameObject();
//            _loadObject.AddComponent<BundleBehaviour>();
//            UnityEngine.Object.DontDestroyOnLoad(_loadObject);
//        }

//        public void AddItem(ItemDataBlock itemDataBlock)
//        {
//            Logger.Log("Adding: " + itemDataBlock.name);
//            _dataBlocks.Add(itemDataBlock.name, _all.Length);
//            _dataBlocksByUniqueID.Add(itemDataBlock.uniqueID, _all.Length);
//
//            ItemDataBlock[] _newAll = new ItemDataBlock[_all.Length + 1];
//            for (var i = 0; i < _all.Length; i++)
//            {
//                _newAll[i] = _all[i];
//            }
//
//            _newAll[_newAll.Length - 1] = itemDataBlock;
//
//            _all = _newAll;
//
//            _dataBlocksField.SetValue(null, _dataBlocks);
//            _datablocksByUniqueIDField.SetValue(null, _dataBlocksByUniqueID);
//            _allField.SetValue(null, _all);
//        }

        public void AddItem(ItemsBlocks items, ItemDataBlock itemDataBlock)
        {
            Logger.Log("[CustomItems] Adding: " + itemDataBlock.name);
            items.Add(itemDataBlock);
        }
    }
}