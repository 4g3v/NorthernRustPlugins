using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using CustomItemsClient.DataBlocks;
using RustBuster2016.API;
using RustBuster2016.API.Events;
using uLink;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace CustomItemsClient
{
    public class CustomItemsClient : RustBusterPlugin
    {
        private string AAAAAAAA =>
            "Fuck off Autori and everyone else dumping this plugin. " +
            "I bet you aren't able to properly implement the server " +
            "side stuff anyway. If you would be able to you wouldn't just " +
            "want to copy my stuff (to anyone actually involved in the making of one, " +
            "like Dretax or Salva, you guys could have just asked me), that's a dickmove imo   ~Sincerely 4g3v";

        public override string Name => "CustomItems";
        public override string Author => "4g3v";
        public override Version Version => new Version(1, 0);
        public static CustomItemsClient Instance;

        private int _oldAllLength;
        private ItemDataBlock[] _all;
        private Dictionary<string, int> _dataBlocks;
        private Dictionary<int, int> _dataBlocksByUniqueID;

        public List<ItemDataBlock> NewItems = new List<ItemDataBlock>();

        private GameObject _waitObject;
        private RPCBehaviour _rpcBehaviour;
        public bool Finished;

//        private GameObject _loadObject;

        public override void Initialize()
        {
            Instance = this;

            if (!Directory.Exists(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\CustomItems"))
            {
                Directory.CreateDirectory(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\CustomItems");
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var webClient = new WebClient();
            var filesTxt = webClient.DownloadString("http://absolomrust.ddnss.org/CustomItems.txt");
//            var filesTxt = webClient.DownloadString("http://192.168.0.199/CustomItems.txt");
            foreach (var s in filesTxt.Split(new[] {Environment.NewLine}, StringSplitOptions.None))
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                
                var fileAndOverwrite = s.Split('|');

                var fileName = RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\CustomItems\\" +
                               Path.GetFileName(fileAndOverwrite[0]);
                
                if (File.Exists(fileName) && !bool.Parse(fileAndOverwrite[1]))
                    continue;

                webClient.DownloadFile(fileAndOverwrite[0], fileName);
            }

            stopwatch.Stop();
            Debug.Log("Downloading CustomItems files took " + stopwatch.ElapsedMilliseconds + " ms");

            Hooks.OnRustBusterClientPluginsLoaded += OnRustBusterClientPluginsLoaded;
            Hooks.OnRustBusterClientDeathScreen += OnRustBusterClientDeathScreen;
        }

        private void OnRustBusterClientDeathScreen(DeathScreenEvent deathScreenEvent)
        {
            _rpcBehaviour.DisableParachute();
        }

        public override void DeInitialize()
        {
            if (_rpcBehaviour != null)
            {
                Object.Destroy(_rpcBehaviour);
            }

            if (_waitObject != null)
            {
                Object.Destroy(_waitObject);
            }

            Hooks.OnRustBusterClientPluginsLoaded -= OnRustBusterClientPluginsLoaded;

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

            DatablockDictionary._dataBlocks = _dataBlocks;
            DatablockDictionary._dataBlocksByUniqueID = _dataBlocksByUniqueID;
            DatablockDictionary._all = _newAll;

            NewItems = new List<ItemDataBlock>();
            _all = null;
            _dataBlocks = null;
            _dataBlocksByUniqueID = null;

//            UnityEngine.Object.Destroy(_loadObject);

            Hooks.LogData(Name, "Removed all custom items!");
        }

        public void AddRPC()
        {
            if (_waitObject != null)
            {
                Object.Destroy(_waitObject);
            }

            _rpcBehaviour = PlayerClient.GetLocalPlayer().gameObject.AddComponent<RPCBehaviour>();
        }

        private void OnRustBusterClientPluginsLoaded()
        {
            _waitObject = new GameObject();
            _waitObject.AddComponent<CharacterWaiter>();
            Object.DontDestroyOnLoad(_waitObject);

            var datablockDictionaryType = typeof(DatablockDictionary);

            _all = DatablockDictionary._all;
            _oldAllLength = _all.Length;

            _dataBlocks = DatablockDictionary._dataBlocks;

            _dataBlocksByUniqueID = DatablockDictionary._dataBlocksByUniqueID;

            var modifiedM4DataBlock = ScriptableObject.CreateInstance<ModifiedM4DataBlock>();
            modifiedM4DataBlock.ammoType = (AmmoItemDataBlock) DatablockDictionary.GetByName("9mm Ammo");
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
            AddItem(modifiedM4DataBlock);

//            LoadWeaponStuff();

            var instantHealthItemDataBlock = ScriptableObject.CreateInstance<InstantHealthItemDataBlock>();
            NewItems.Add(instantHealthItemDataBlock);
            AddItem(instantHealthItemDataBlock);

            var instantHealthBlueprintDataBlock = ScriptableObject.CreateInstance<InstantHealthBlueprintDataBlock>();
            NewItems.Add(instantHealthBlueprintDataBlock);
            AddItem(instantHealthBlueprintDataBlock);

            var avxDollarDataBlock = ScriptableObject.CreateInstance<AVXDollarDataBlock>();
            NewItems.Add(avxDollarDataBlock);
            AddItem(avxDollarDataBlock);

            var drugOneDataBlock = ScriptableObject.CreateInstance<DrugOneDataBlock>();
            NewItems.Add(drugOneDataBlock);
            AddItem(drugOneDataBlock);

            var tpOneDataBlock = ScriptableObject.CreateInstance<TPOneDataBlock>();
            NewItems.Add(tpOneDataBlock);
            AddItem(tpOneDataBlock);

            var tpTwoDataBlock = ScriptableObject.CreateInstance<TPTwoDataBlock>();
            NewItems.Add(tpTwoDataBlock);
            AddItem(tpTwoDataBlock);

            var tpThreeDataBlock = ScriptableObject.CreateInstance<TPThreeDataBlock>();
            NewItems.Add(tpThreeDataBlock);
            AddItem(tpThreeDataBlock);

            var tpFourDataBlock = ScriptableObject.CreateInstance<TPFourDataBlock>();
            NewItems.Add(tpFourDataBlock);
            AddItem(tpFourDataBlock);

//            var testItemDataBlock = ScriptableObject.CreateInstance<TestItemDataBlock>();
//            NewItems.Add(testItemDataBlock);
//            AddItem(testItemDataBlock);

            var parachuteDataBlock = ScriptableObject.CreateInstance<ParachuteDataBlock>();
            NewItems.Add(parachuteDataBlock);
            AddItem(parachuteDataBlock);

            var electronicJunkDataBlock = ScriptableObject.CreateInstance<ElectronicJunkDataBlock>();
            NewItems.Add(electronicJunkDataBlock);
            AddItem(electronicJunkDataBlock);

            var copperDataBlock = ScriptableObject.CreateInstance<CopperDataBlock>();
            NewItems.Add(copperDataBlock);
            AddItem(copperDataBlock);

            var batteriesDataBlock = ScriptableObject.CreateInstance<BatteriesDataBlock>();
            NewItems.Add(batteriesDataBlock);
            AddItem(batteriesDataBlock);

            var ironShardsDataBlock = ScriptableObject.CreateInstance<IronShardsDataBlock>();
            NewItems.Add(ironShardsDataBlock);
            AddItem(ironShardsDataBlock);

            var healingStationDataBlock = ScriptableObject.CreateInstance<HealingStationDataBlock>();
            NewItems.Add(healingStationDataBlock);
            AddItem(healingStationDataBlock);

            var healingStationBlueprintDataBlock = ScriptableObject.CreateInstance<HealingStationBlueprintDataBlock>();
            NewItems.Add(healingStationBlueprintDataBlock);
            AddItem(healingStationBlueprintDataBlock);

            var lootboxDataBlock = ScriptableObject.CreateInstance<LootboxDataBlock>();
            NewItems.Add(lootboxDataBlock);
            AddItem(lootboxDataBlock);

            var lootboxBlueprintDataBlock = ScriptableObject.CreateInstance<LootboxBlueprintDataBlock>();
            NewItems.Add(lootboxBlueprintDataBlock);
            AddItem(lootboxBlueprintDataBlock);

            var bigLootboxDataBlock = ScriptableObject.CreateInstance<BigLootboxDataBlock>();
            NewItems.Add(bigLootboxDataBlock);
            AddItem(bigLootboxDataBlock);

            var bigLootboxBlueprintDataBlock = ScriptableObject.CreateInstance<BigLootboxBlueprintDataBlock>();
            NewItems.Add(bigLootboxBlueprintDataBlock);
            AddItem(bigLootboxBlueprintDataBlock);

            var landmineDataBlock = ScriptableObject.CreateInstance<LandmineDataBlock>();
            NewItems.Add(landmineDataBlock);
            AddItem(landmineDataBlock);

            var landmineBlueprintDataBlock = ScriptableObject.CreateInstance<LandmineBlueprintDataBlock>();
            NewItems.Add(landmineBlueprintDataBlock);
            AddItem(landmineBlueprintDataBlock);

            var respawnFlagDataBlock = ScriptableObject.CreateInstance<RespawnFlagDataBlock>();
            NewItems.Add(respawnFlagDataBlock);
            AddItem(respawnFlagDataBlock);

            var respawnFlagBlueprintDataBlock = ScriptableObject.CreateInstance<RespawnFlagBlueprintDataBlock>();
            NewItems.Add(respawnFlagBlueprintDataBlock);
            AddItem(respawnFlagBlueprintDataBlock);

            var metalBarricadeDataBlock = ScriptableObject.CreateInstance<MetalBarricadeDataBlock>();
            NewItems.Add(metalBarricadeDataBlock);
            AddItem(metalBarricadeDataBlock);

            var metalBarricadeBlueprintDataBlock = ScriptableObject.CreateInstance<MetalBarricadeBlueprintDataBlock>();
            NewItems.Add(metalBarricadeBlueprintDataBlock);
            AddItem(metalBarricadeBlueprintDataBlock);

//            var portalRadioDataBlock = ScriptableObject.CreateInstance<PortalRadioDataBlock>();
//            NewItems.Add(portalRadioDataBlock);
//            AddItem(portalRadioDataBlock);
//
//            var portalRadioBlueprintDataBlock = ScriptableObject.CreateInstance<PortalRadioBlueprintDataBlock>();
//            NewItems.Add(portalRadioBlueprintDataBlock);
//            AddItem(portalRadioBlueprintDataBlock);

//            var bookshelfDataBlock = ScriptableObject.CreateInstance<BookshelfDataBlock>();
//            NewItems.Add(bookshelfDataBlock);
//            AddItem(bookshelfDataBlock);
//
//            var bookshelfBlueprintDataBlock = ScriptableObject.CreateInstance<BookshelfBlueprintDataBlock>();
//            NewItems.Add(bookshelfBlueprintDataBlock);
//            AddItem(bookshelfBlueprintDataBlock);

            var absolomM4BlueprintDataBlock = ScriptableObject.CreateInstance<AbsolomM4BlueprintDataBlock>();
            NewItems.Add(absolomM4BlueprintDataBlock);
            AddItem(absolomM4BlueprintDataBlock);

//            var raidFlagDataBlock = ScriptableObject.CreateInstance<RaidFlagDataBlock>();
//            NewItems.Add(raidFlagDataBlock);
//            AddItem(raidFlagDataBlock);
//
//            var raidFlagBlueprintDataBlock = ScriptableObject.CreateInstance<RaidFlagBlueprintDataBlock>();
//            NewItems.Add(raidFlagBlueprintDataBlock);
//            AddItem(raidFlagBlueprintDataBlock);

//            var ironBaseDataBlock = ScriptableObject.CreateInstance<IronBaseDataBlock>();
//            NewItems.Add(ironBaseDataBlock);
//            AddItem(ironBaseDataBlock);
//
//            var ironBaseBlueprintDataBlock = ScriptableObject.CreateInstance<IronBaseBlueprintDataBlock>();
//            NewItems.Add(ironBaseBlueprintDataBlock);
//            AddItem(ironBaseBlueprintDataBlock);
            
//            var smallLadderDataBlock = ScriptableObject.CreateInstance<SmallLadderDataBlock>();
//            NewItems.Add(smallLadderDataBlock);
//            AddItem(smallLadderDataBlock);    
//            
//            var bigLadderDataBlock = ScriptableObject.CreateInstance<BigLadderDataBlock>();
//            NewItems.Add(bigLadderDataBlock);
//            AddItem(bigLadderDataBlock);
//            
//            var smallLadderBlueprintDataBlock = ScriptableObject.CreateInstance<SmallLadderBlueprintDataBlock>();
//            NewItems.Add(smallLadderBlueprintDataBlock);
//            AddItem(smallLadderBlueprintDataBlock);
//
//            var bigLadderBlueprintDataBlock = ScriptableObject.CreateInstance<BigLadderBlueprintDataBlock>();
//            NewItems.Add(bigLadderBlueprintDataBlock);
//            AddItem(bigLadderBlueprintDataBlock);
            
            var shelfDataBlock = ScriptableObject.CreateInstance<ShelfDataBlock>();
            NewItems.Add(shelfDataBlock);
            AddItem(shelfDataBlock);
            
            var shelfBlueprintDataBlock = ScriptableObject.CreateInstance<ShelfBlueprintDataBlock>();
            NewItems.Add(shelfBlueprintDataBlock);
            AddItem(shelfBlueprintDataBlock);
            
            var oneManBunkerDataBlock = ScriptableObject.CreateInstance<OneManBunkerDataBlock>();
            NewItems.Add(oneManBunkerDataBlock);
            AddItem(oneManBunkerDataBlock);

            var oneManBunkerBlueprintDataBlock = ScriptableObject.CreateInstance<OneManBunkerBlueprintDataBlock>();
            NewItems.Add(oneManBunkerBlueprintDataBlock);
            AddItem(oneManBunkerBlueprintDataBlock);
            
//            var waterBridgeDataBlock = ScriptableObject.CreateInstance<WaterBridgeDataBlock>();
//            NewItems.Add(waterBridgeDataBlock);
//            AddItem(waterBridgeDataBlock);
//            
//            var waterBridgeBlueprintDataBlock = ScriptableObject.CreateInstance<WaterBridgeBlueprintDataBlock>();
//            NewItems.Add(waterBridgeBlueprintDataBlock);
//            AddItem(waterBridgeBlueprintDataBlock);
//            
//            var woodFactoryDataBlock = ScriptableObject.CreateInstance<WoodFactoryDataBlock>();
//            NewItems.Add(woodFactoryDataBlock);
//            AddItem(woodFactoryDataBlock);
//
//            var woodFactoryBlueprintDataBlock = ScriptableObject.CreateInstance<WoodFactoryBlueprintDataBlock>();
//            NewItems.Add(woodFactoryBlueprintDataBlock);
//            AddItem(woodFactoryBlueprintDataBlock);
            
            var flagDataBlock = ScriptableObject.CreateInstance<FlagDataBlock>();
            NewItems.Add(flagDataBlock);
            AddItem(flagDataBlock);
            
            var keycardADataBlock = ScriptableObject.CreateInstance<KeycardADataBlock>();
            NewItems.Add(keycardADataBlock);
            AddItem(keycardADataBlock);
            
            var keycardBDataBlock = ScriptableObject.CreateInstance<KeycardBDataBlock>();
            NewItems.Add(keycardBDataBlock);
            AddItem(keycardBDataBlock);
            
            var keycardCDataBlock = ScriptableObject.CreateInstance<KeycardCDataBlock>();
            NewItems.Add(keycardCDataBlock);
            AddItem(keycardCDataBlock);
            
            var keycardC1DataBlock = ScriptableObject.CreateInstance<KeycardC1DataBlock>();
            NewItems.Add(keycardC1DataBlock);
            AddItem(keycardC1DataBlock);
            
            var diamondsDataBlock = ScriptableObject.CreateInstance<DiamondsDataBlock>();
            NewItems.Add(diamondsDataBlock);
            AddItem(diamondsDataBlock);
            
            var buildingLootboxDataBlock = ScriptableObject.CreateInstance<BuildingLootboxDataBlock>();
            NewItems.Add(buildingLootboxDataBlock);
            AddItem(buildingLootboxDataBlock);
            
            var buildingLootboxBlueprintDataBlock = ScriptableObject.CreateInstance<BuildingLootboxBlueprintDataBlock>();
            NewItems.Add(buildingLootboxBlueprintDataBlock);
            AddItem(buildingLootboxBlueprintDataBlock);

            var turretDataBlock = ScriptableObject.CreateInstance<TurretDataBlock>();
            NewItems.Add(turretDataBlock);
            AddItem(turretDataBlock);

            var turretBlueprintDataBlock = ScriptableObject.CreateInstance<TurretBlueprintDataBlock>();
            NewItems.Add(turretBlueprintDataBlock);
            AddItem(turretBlueprintDataBlock);
            
            var turretSilencerDataBlock = ScriptableObject.CreateInstance<TurretSilencerDataBlock>();
            NewItems.Add(turretSilencerDataBlock);
            AddItem(turretSilencerDataBlock);
            
            var turretRangeExtensionDataBlock = ScriptableObject.CreateInstance<TurretRangeExtensionDataBlock>();
            NewItems.Add(turretRangeExtensionDataBlock);
            AddItem(turretRangeExtensionDataBlock);
            
            var turretDamageExtension = ScriptableObject.CreateInstance<TurretDamageExtension>();
            NewItems.Add(turretDamageExtension);
            AddItem(turretDamageExtension);

            var silencedTurretDataBlock = ScriptableObject.CreateInstance<SilencedTurretDataBlock>();
            NewItems.Add(silencedTurretDataBlock);
            AddItem(silencedTurretDataBlock);

            var silencedTurretBlueprintDataBlock = ScriptableObject.CreateInstance<SilencedTurretBlueprintDataBlock>();
            NewItems.Add(silencedTurretBlueprintDataBlock);
            AddItem(silencedTurretBlueprintDataBlock);
            
            var rangeTurretDataBlock = ScriptableObject.CreateInstance<RangeTurretDataBlock>();
            NewItems.Add(rangeTurretDataBlock);
            AddItem(rangeTurretDataBlock);

            var rangeTurretBlueprintDataBlock = ScriptableObject.CreateInstance<RangeTurretBlueprintDataBlock>();
            NewItems.Add(rangeTurretBlueprintDataBlock);
            AddItem(rangeTurretBlueprintDataBlock);
            
            var damageTurretDataBlock = ScriptableObject.CreateInstance<DamageTurretDataBlock>();
            NewItems.Add(damageTurretDataBlock);
            AddItem(damageTurretDataBlock);

            var damageTurretBlueprintDataBlock = ScriptableObject.CreateInstance<DamageTurretBlueprintDataBlock>();
            NewItems.Add(damageTurretBlueprintDataBlock);
            AddItem(damageTurretBlueprintDataBlock);
            
            var jointDataBlock = ScriptableObject.CreateInstance<JointDataBlock>();
            NewItems.Add(jointDataBlock);
            AddItem(jointDataBlock);
            
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
//            smokeGrenadeDataBlock._itemRepPrefab = Facepunch.Bundling.Load<ItemRepresentation>("content/item/f1/f1.irp");
//            smokeGrenadeDataBlock._viewModelPrefab = Facepunch.Bundling.Load<ViewModel>("content/item/f1/VM-F1");
//            smokeGrenadeDataBlock.deploySound = Facepunch.Bundling.Load<AudioClip>("content/shared/sfx/draw_generic_rustle");
//            smokeGrenadeDataBlock.pullPinSound = Facepunch.Bundling.Load<AudioClip>("content/item/sfx/grenade_arm");
//            smokeGrenadeDataBlock.throwObjectPrefab =
//                Facepunch.Bundling.Load<GameObject>("content/item/f1/SupplySignalGrenade");
//
//            UnityEngine.Object.Destroy(smokeGrenadeDataBlock.throwObjectPrefab.GetComponent<SignalGrenade>());
//            var smokeGrenade = smokeGrenadeDataBlock.throwObjectPrefab.AddComponent<SmokeGrenade>();
//            smokeGrenade.bounceSound = Facepunch.Bundling.Load<AudioClip>("content/item/sfx/grenade_bounce_1");
//            smokeGrenade.explosionEffect = Facepunch.Bundling.Load<GameObject>("rust/effects/particles/smoke/SignalSmokeGreen");
//            
//            var smokeView = smokeGrenadeDataBlock.throwObjectPrefab.GetComponent<uLinkNetworkView>();
//            smokeView.observed = smokeGrenade;
//            
//            _newItems.Add(smokeGrenadeDataBlock);
//            AddItem(smokeGrenadeDataBlock);

//            _newItems.Add(ScriptableObject.CreateInstance<TestItem>());

            #endregion

            Finished = true;
            Hooks.LogData(Name, "Loaded all custom items!");
        }

//        private void LoadWeaponStuff()
//        {
//            _loadObject = new GameObject();
//            _loadObject.AddComponent<BundleBehaviour>();
//            Object.DontDestroyOnLoad(_loadObject);
//        }

        public void AddItem(ItemDataBlock itemDataBlock)
        {
            Hooks.LogData(Name, "Adding: " + itemDataBlock.name);
            _dataBlocks.Add(itemDataBlock.name, _all.Length);
            _dataBlocksByUniqueID.Add(itemDataBlock.uniqueID, _all.Length);

            ItemDataBlock[] _newAll = new ItemDataBlock[_all.Length + 1];
            for (var i = 0; i < _all.Length; i++)
            {
                _newAll[i] = _all[i];
            }

            _newAll[_newAll.Length - 1] = itemDataBlock;

            _all = _newAll;

            DatablockDictionary._dataBlocks = _dataBlocks;
            DatablockDictionary._dataBlocksByUniqueID = _dataBlocksByUniqueID;
            DatablockDictionary._all = _all;
//            _dataBlocksField.SetValue(null, _dataBlocks);
//            _datablocksByUniqueIDField.SetValue(null, _dataBlocksByUniqueID);
//            _allField.SetValue(null, _all);
        }
    }
}