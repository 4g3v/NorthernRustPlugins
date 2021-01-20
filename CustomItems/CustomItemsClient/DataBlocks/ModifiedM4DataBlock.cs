using System.IO;
using System.Reflection;
using UnityEngine;

namespace CustomItemsClient.DataBlocks
{    
    public class ModifiedM4DataBlock : BulletWeaponDataBlock
    {
        public ModifiedM4DataBlock()
        {
            this.name = "AbsolomM4";
            
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\CustomItems\\AbsolomM4.png"));
            iconTex = texture;

            this.category = ItemCategory.Weapons;
            this.Combinations = new CombineRecipe[0];
            this._maxCondition = 200;
            this._maxUses = 50;
            this._splittable = false;
            this._spawnUsesMin = 1;
            this._spawnUsesMax = 1;
            this._minUsesForDisplay = 1;
            this._itemFlags = 0;
            this.isResearchable = false;
            this.isRepairable = false;
            this.isRecycleable = false;
            this.doesLoseCondition = true;
            this.secondaryFireAims = true;
            this.aimSensitivtyPercent = 0.400000006f;
            this.attachmentPoint = "RArmHand";
            this.animationGroupName = "rifle";
            this.isSemiAuto = false;
            this.fireRate = 0.075f;
            this.fireRateSecondary = 0.5f;
            this.deployLength = 0.7f;
            this.damageMin = 24;
            this.damageMax = 28;
            this.maxClipAmmo = 50;
            this.fireSoundRange = 300;
            this.bulletRange = 140;
            this.recoilPitchMin = 2;
            this.recoilPitchMax = 5;
            this.recoilYawMin = -3;
            this.recoilYawMax = 3;
            this.recoilDuration = 0.100000003f;
            this.aimingRecoilSubtract = 0.25f;
            this.crouchRecoilSubtract = 0.100000003f;
            this.reloadDuration = 1.5f;
            this.maxEligableSlots = 5;
            this.NoAimingAfterShot = false;
            this.aimSway = 0.5f;
            this.aimSwaySpeed = 1;

            var type = this.GetType().BaseType.BaseType.BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481337);
        }

        public override string GetItemDescription()
        {
            return "In loving memory of AVX.";
        }
    }
}