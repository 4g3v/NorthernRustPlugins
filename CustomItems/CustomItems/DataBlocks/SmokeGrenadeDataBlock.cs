//using System.IO;
//using System.Reflection;
//using Fougerite;
//using UnityEngine;
//
//namespace CustomItems
//{
//    public class SmokeGrenadeDataBlock : HandGrenadeDataBlock
//    {
//        public SmokeGrenadeDataBlock()
//        {
//            name = "Smoke Grenade";
//            
//            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\SmokeGrenade.png"));
//            iconTex = texture;
//            
//            _maxUses = 5;
//            _spawnUsesMin = 1;
//            _spawnUsesMax = 1;
//            _minUsesForDisplay = 1;
//            _maxCondition = 100;
//            _splittable = true;
//            _itemFlags = 0;
//            isResearchable = false;
//            isRepairable = false;
//            isRecycleable = false;
//            doesLoseCondition = false;
//            category = ItemCategory.Weapons;
//            Combinations = new CombineRecipe[0];
//            transientMode = 0;
////            _itemRepPrefab
////            _viewModelPrefab
////            deploySound
//            secondaryFireAims = false;
//            aimSensitivtyPercent = 0.400000006f;
//            attachmentPoint = "RArmHand";
//            animationGroupName = "hatchet";
//            isSemiAuto = false;
//            fireRate = 0.75f;
//            fireRateSecondary = 1f;
//            deployLength = 0.75f;
//            damageMin = 20000f;
//            damageMax = 20000f;
////            throwObjectPrefab
//            throwStrengthMin = 5f;
//            throwStrengthPerSec = 20f;
//            throwStrengthMax = 40;
////            pullPinSound
//            
//            var type = GetType().BaseType.BaseType.BaseType.BaseType.BaseType.BaseType;
//            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
//            uniqueIDField.SetValue(this, 134481342);
//        }
//    }
//}