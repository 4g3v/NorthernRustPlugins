//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using uLink;
//using UnityEngine;
//using MonoBehaviour = UnityEngine.MonoBehaviour;
//
//namespace CustomItemsClient
//{
//    public class BundleBehaviour : MonoBehaviour
//    {
//        private void Start()
//        {
//            StartCoroutine("LoadBundle");
//        }
//
//        private IEnumerator LoadBundle()
//        {
//            WWW www = WWW.LoadFromCacheOrDownload(@"file://C:\Users\Vega\Desktop\weapon.unity3d", 1);
//            yield return www;
//            var bundle = www.assetBundle;
//            www.Dispose();
//
//            var modifiedM4DataBlock = ScriptableObject.CreateInstance<ModifiedM4DataBlock>();
//            modifiedM4DataBlock.ammoType = (AmmoItemDataBlock) DatablockDictionary.GetByName("9mm Ammo");
//            modifiedM4DataBlock.deploySound = Facepunch.Bundling.Load<AudioClip>("content/item/m4/sfx/m4_deploy");
//            modifiedM4DataBlock.dryFireSound = Facepunch.Bundling.Load<AudioClip>("content/shared/sfx/dryfire_rifle-1");
//            modifiedM4DataBlock.fireSound = Facepunch.Bundling.Load<AudioClip>("content/item/pistol/sfx/pistol_fire_4");
//            modifiedM4DataBlock.fireSound_Far = Facepunch.Bundling.Load<AudioClip>("content/effect/sfx/rifle_far");
//            modifiedM4DataBlock.fireSound_Silenced =
//                Facepunch.Bundling.Load<AudioClip>("content/effect/sfx/silenced_shot");
//            modifiedM4DataBlock.muzzleflashVM = Facepunch.Bundling.Load<GameObject>("content/effect/Flash");
//            modifiedM4DataBlock.muzzleFlashWorld = Facepunch.Bundling.Load<GameObject>("content/effect/flash_tp");
//            modifiedM4DataBlock.reloadSound = Facepunch.Bundling.Load<AudioClip>("content/item/sfx/gsr1911_reload-1");
//            modifiedM4DataBlock.shotBob = Facepunch.Bundling.Load<BobEffect>("content/headbob/effect/Gun Kickback");
//            modifiedM4DataBlock.tracerPrefab = Facepunch.Bundling.Load<GameObject>("content/effect/TracerPrefab");
//
//            var itemRepresentationObject = ((GameObject) bundle.Load("notreallym4.irp"));
//            var itemRepresentation =
//                itemRepresentationObject.AddComponent(
//                    Facepunch.Bundling.Load<ItemRepresentation>("content/item/m4/m4.irp"));
//            itemRepresentation.name = "notreallym4.irp";
//
//            var viewModelObject = new GameObject("VM-notreallym4");
//            var viewModel = viewModelObject.AddComponentWithInit<ViewModel>(vm =>
//                {
//                    vm = vm.GetCopyOf(Facepunch.Bundling.Load<ViewModel>("content/item/m4/VM-M4"));
//                });
//            viewModel.name = "VM-notreallym4";
////            var viewModel = viewModelObject.AddComponent(Facepunch.Bundling.Load<ViewModel>("content/item/m4/VM-M4"));
//
//            Debug.Log(viewModel.root);
//
//            var allField = Type
//                .GetType(
//                    "NetCull+AutoPrefabs, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
//                ?.GetField("all");
//            Dictionary<string, uLinkNetworkView> allDictionary =
//                (Dictionary<string, uLinkNetworkView>) allField.GetValue(null);
//            allDictionary.Add("notreallym4.irp", itemRepresentation.GetComponent<uLinkNetworkView>());
//            allField.SetValue(null, allDictionary);
//
//            NetworkInstantiator.AddPrefab(itemRepresentationObject);
//
//            modifiedM4DataBlock._itemRepPrefab = itemRepresentation;
//            modifiedM4DataBlock._viewModelPrefab = viewModel;
//            CustomItemsClient.Instance.NewItems.Add(modifiedM4DataBlock);
//            CustomItemsClient.Instance.AddItem(modifiedM4DataBlock);
//
//            Debug.Log("LoadBundle() finished");
//
////            var viewModel = viewModelObject.AddComponent<ViewModel>();
////            viewModel.animation = null;
////            viewModel.caps = origVM.caps;
////            viewModel.item = (IHeldItem) modifiedM4DataBlock.CreateItem();
////            viewModel.muzzle = origVM.muzzle;
////            viewModel.offset = origVM.offset;
////            viewModel.optics = origVM.optics;
////            viewModel.pivot = origVM.pivot;
////            viewModel.pivot2 = origVM.pivot2;
////            viewModel.root = origVM.root;
////            viewModel.rotate = origVM.rotate;
////            viewModel.sight = origVM.sight;
////            viewModel.aimMask = origVM.aimMask;
////            viewModel.barrelAiming = origVM.barrelAiming;
////            viewModel.barrelLimit = origVM.barrelLimit;
////            viewModel.barrelPivot = origVM.barrelPivot;
////            viewModel.barrelRotation = origVM.barrelRotation;
////            viewModel.bowAllowed = origVM.bowAllowed;
////            viewModel.bowCurve = origVM.bowCurve;
////            viewModel.bowPivot = origVM.bowPivot;
////            viewModel.crosshairColor = origVM.crosshairColor;
////            viewModel.crosshairOutline = origVM.crosshairOutline;
////            viewModel.crosshairTexture = origVM.crosshairTexture;
////            viewModel.dotTexture = origVM.dotTexture;
////            viewModel.itemRep = itemRepresentation;
////            viewModel.lazyAngle = origVM.lazyAngle;
////            viewModel.punchScalar = origVM.punchScalar;
////            viewModel.zoomCurve = origVM.zoomCurve;
////            viewModel.zoomOffset = origVM.zoomOffset;
////            viewModel.zoomPunch = origVM.zoomPunch;
////            viewModel.zoomRotate = origVM.zoomRotate;
////            viewModel.barrelWhileBowing = origVM.barrelWhileBowing;
////            viewModel.barrelWhileZoom = origVM.barrelWhileZoom;
////            viewModel.bowEnterDuration = origVM.bowEnterDuration;
////            viewModel.bowExitDuration = origVM.bowExitDuration;
////            viewModel.bowOffsetAngles = origVM.bowOffsetAngles;
////            viewModel.bowOffsetPoint = origVM.bowOffsetPoint;
////            viewModel.deployAnimName = origVM.deployAnimName;
////            viewModel.fireAnimName = origVM.fireAnimName;
////            viewModel.noHitPlane = origVM.noHitPlane;
////            viewModel.perspectiveAspectOverride = origVM.perspectiveAspectOverride;
////            viewModel.perspectiveFarOverride = origVM.perspectiveFarOverride;
////            viewModel.perspectiveNearOverride = origVM.perspectiveNearOverride;
////            viewModel.reloadAnimName = origVM.reloadAnimName;
////            viewModel.showCrosshairZoom = origVM.showCrosshairZoom;
////            viewModel.zoomInDuration = origVM.zoomInDuration;
////            viewModel.zoomOutDuration = origVM.zoomOutDuration;
////            viewModel.barrelAngleMaxSpeed = origVM.barrelAngleMaxSpeed;
////            viewModel.barrelAngleSmoothDamp = origVM.barrelAngleSmoothDamp;
////            viewModel.barrelLimitOffsetFactor = origVM.barrelLimitOffsetFactor;
////            viewModel.barrelLimitPivotFactor = origVM.barrelLimitPivotFactor;
////            viewModel.bowCurveIs01Fraction = origVM.bowCurveIs01Fraction;
////            viewModel.fireAnimScaleSpeed = origVM.fireAnimScaleSpeed;
////            viewModel.headBobOffsetScale = origVM.headBobOffsetScale;
////            viewModel.headBobRotationScale = origVM.headBobRotationScale;
////            viewModel.showCrosshairNotZoomed = origVM.showCrosshairNotZoomed;
////            viewModel.zoomFieldOfView = origVM.zoomFieldOfView;
////            viewModel.perspectiveFOVOverride = origVM.perspectiveFOVOverride;
//
////            var uLinkNetworkView = notReallyM4Object.AddComponent<uLinkNetworkView>();
//        }
//    }
//}