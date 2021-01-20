using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{    
    public class ModifiedM4DataBlock : BulletWeaponDataBlock
    {
        public ModifiedM4DataBlock()
        {
            name = "AbsolomM4";
            
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Util.GetRootFolder() + "\\Save\\CustomItems\\AbsolomM4.png"));
            iconTex = texture;
            
            category = ItemCategory.Weapons;
            Combinations = new CombineRecipe[0];
            _maxCondition = 200;
            _maxUses = 50;
            _splittable = false;
            _spawnUsesMin = 1;
            _spawnUsesMax = 1;
            _minUsesForDisplay = 1;
            _itemFlags = 0;
            isResearchable = false;
            isRepairable = false;
            isRecycleable = false;
            doesLoseCondition = true;
            secondaryFireAims = true;
            aimSensitivtyPercent = 0.400000006f;
            attachmentPoint = "RArmHand";
            animationGroupName = "rifle";
            isSemiAuto = false;
            fireRate = 0.075f;
            fireRateSecondary = 0.5f;
            deployLength = 0.7f;
            damageMin = 24;
            damageMax = 28;
            maxClipAmmo = 50;
            fireSoundRange = 300;
            bulletRange = 140;
            recoilPitchMin = 2;
            recoilPitchMax = 5;
            recoilYawMin = -3;
            recoilYawMax = 3;
            recoilDuration = 0.100000003f;
            aimingRecoilSubtract = 0.25f;
            crouchRecoilSubtract = 0.100000003f;
            reloadDuration = 1.5f;
            maxEligableSlots = 5;
            NoAimingAfterShot = false;
            aimSway = 0.5f;
            aimSwaySpeed = 1;

            var type = this.GetType().BaseType.BaseType.BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481337);
        }
    }
}