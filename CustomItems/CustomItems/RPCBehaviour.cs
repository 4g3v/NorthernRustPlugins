using System.Collections.Generic;
using System.IO;
using CustomItems.DataBlocks;
using Fougerite;
using UnityEngine;

namespace CustomItems
{
    public class RPCBehaviour : MonoBehaviour
    {
        public Fougerite.Player FougeritePlayer;

        public bool CanUseDrugOne;
        public bool CanUseJoint;
        public static List<string> JointUsers = new List<string>();

        public void MotorChange()
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("MotorChange", FougeritePlayer.NetworkPlayer);
        }
        public void MotorChangeJoint()
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("MotorChangeJoint", FougeritePlayer.NetworkPlayer);
        }

        public void PrepareUseAmphetamine()
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("PrepareUseAmphetamine", FougeritePlayer.NetworkPlayer);
        }
        
        public void PrepareUseJoint()
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("PrepareUseJoint", FougeritePlayer.NetworkPlayer);
        }
        
        public void SetDisplayOverlay(bool b)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("SetDisplayOverlay", FougeritePlayer.NetworkPlayer, b);
        }
        public void SetDisplayJointOverlay(bool b)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("SetDisplayJointOverlay", FougeritePlayer.NetworkPlayer, b);
        }

        public void WorldEditorSpawn(string assetName, string bundleName, Vector3 position, Quaternion rotation,
            Vector3 size)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView)
                .RPC("WorldEditorSpawn", FougeritePlayer.NetworkPlayer, assetName, bundleName, position, rotation, size);
        }

        public void UseParachute()
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView).RPC("UseParachute", FougeritePlayer.NetworkPlayer);   
        }

        public void ShowParachute(string steamID, bool show)
        {
            uLink.NetworkView.Get(FougeritePlayer.PlayerClient.networkView).RPC("ShowParachute", FougeritePlayer.NetworkPlayer, steamID, show);            
        }

        [RPC]
        public void GetParachutes()
        {
            foreach (var steamID in ParachuteDataBlock.ParachuteDictionary.Keys)
            {
                ShowParachute(steamID, ParachuteDataBlock.ParachuteDictionary[steamID]);
            }
        }

        [RPC]
        public void SetCanUseDrugOne(bool canUse)
        {
            CanUseDrugOne = canUse;
        }
        
        [RPC]
        public void SetCanUseJoint(bool canUse)
        {
            CanUseJoint = canUse;
        }
        
        [RPC]
        public void RemoveFromJointList()
        {
            JointUsers.Remove(FougeritePlayer.SteamID);
        }

        [RPC]
        public void UseDrugOne()
        {
            if (CanUseDrugOne)
            {
                var inventory = FougeritePlayer.Inventory.InternalInventory;
                var inventoryItem = inventory.FindItem("Amphetamine");
                
                if (inventoryItem != null)
                {
                    int num = 1;
                    if (inventoryItem.Consume(ref num))
                    {
                        inventory.RemoveItem(inventoryItem.slot);
                    }
                    else
                    {
                        inventory.MarkSlotDirty(inventoryItem.slot);
                    }   
                    inventoryItem.FireClientSideItemEvent(InventoryItem.ItemEvent.Used);
                }
                
                var compHealth = FougeritePlayer.Health - 35;
                if (compHealth >= 1)
                {
                    FougeritePlayer.Health = compHealth;
                    FougeritePlayer.PlayerClient.controllable.GetLocal<ClientVitalsSync>().SendClientItsHealth();
                }
                else
                {
                    FougeritePlayer.Kill();
                    SetDisplayOverlay(false);
                }

                MotorChange();
                CanUseDrugOne = false;
            }
        }
        
        [RPC]
        public void UseJoint()
        {
            if (CanUseJoint)
            {
                var inventory = FougeritePlayer.Inventory.InternalInventory;
                var inventoryItem = inventory.FindItem("Joint");
                
                if (inventoryItem != null)
                {
                    int num = 1;
                    if (inventoryItem.Consume(ref num))
                    {
                        inventory.RemoveItem(inventoryItem.slot);
                    }
                    else
                    {
                        inventory.MarkSlotDirty(inventoryItem.slot);
                    }   
                    inventoryItem.FireClientSideItemEvent(InventoryItem.ItemEvent.Used);
                }
                
                FougeritePlayer.AdjustCalorieLevel(-(FougeritePlayer.CalorieLevel * 0.5f));

                MotorChangeJoint();
                CanUseJoint = false;
                JointUsers.Add(FougeritePlayer.SteamID);
            }
        }

        [RPC]
        public void Log(string log)
        {
            File.AppendAllText(Util.GetRootFolder() + "\\Save\\CustomItems\\Log.txt", FougeritePlayer.SteamID + ": " + log + "\r\n");
        }

        [RPC]
        public void InvGetNetworkUpdate()
        {
            uLink.BitStream stream = new uLink.BitStream(false);
            FougeritePlayer.Inventory.InternalInventory.GenerateInvBitStream(ref stream, false);
            NetCull.RPC(FougeritePlayer.Inventory.InternalInventory, "GNUP", FougeritePlayer.NetworkPlayer, stream.GetDataByteArray());
        }
    }
}