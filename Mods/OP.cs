
using ExitGames.Client.Photon.StructWrapping;
using ExitGames.Client.Photon;
using GorillaLocomotion.Gameplay;
using GorillaNetworking;
using GorillaTag.CosmeticSystem;
using Mono.Security.Interface;
using NetSynchrony;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using static Avantage.Classes.RigManager;
using System.Linq;
using System.Reflection;
using System.Text;
using static GorillaNetworking.CosmeticsController;
using UnityEngine.UIElements;
using UnityEngine;
using Avantage.Notifications;
using HarmonyLib;
using System.Diagnostics;
using Avantage.Menu;
using System.Net;
using static NetworkSystem;
using UnityEngine.InputSystem;
using Constants;
using GorillaTagScripts;
using Oculus.Platform;
using PlayFab.ClientModels;
using PlayFab;
using System.Text.RegularExpressions;
using GorillaGameModes;
using GorillaTag;
using gunlibary;
using Fusion;
using ModIO;

namespace Avantage.Mods
{
    public class TimeUtil
    {
        public static void StartDelay(Action action, float time)
        {
            if (Time.time >= TimeUtil.Delay + time)
            {
                TimeUtil.Delay = Time.time;
                action();
            }
        }

        public static float Delay;
    }
    public class Globals
    {
        public static Player __instance;

        public static VRRig __vrrig;
    }
    internal class Exploits
    {
        public static void GetOwnership(PhotonView view)
        {
            if (!view.AmOwner)
            {
                view.OwnerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                view.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                view.RequestOwnership();
                view.TransferOwnership(PhotonNetwork.LocalPlayer);
                if (view.GetComponent<RequestableOwnershipGuard>() != null)
                {
                    view.GetComponent<RequestableOwnershipGuard>().actualOwner = NetworkSystem.Instance.LocalPlayer;
                    view.GetComponent<RequestableOwnershipGuard>().currentOwner = NetworkSystem.Instance.LocalPlayer;
                    view.GetComponent<RequestableOwnershipGuard>().RequestTheCurrentOwnerFromAuthority();
                    view.GetComponent<RequestableOwnershipGuard>().TransferOwnership(NetworkSystem.Instance.LocalPlayer, "");
                    view.GetComponent<RequestableOwnershipGuard>().TransferOwnershipFromToRPC(PhotonNetwork.LocalPlayer, view.GetComponent<RequestableOwnershipGuard>().ownershipRequestNonce, default(PhotonMessageInfo));
                    view.GetComponent<RequestableOwnershipGuard>().GetAuthoritativePlayer();
                    view.GetComponent<RequestableOwnershipGuard>().RequestTheCurrentOwnerFromAuthority();
                    view.GetComponent<RequestableOwnershipGuard>().giveCreatorAbsoluteAuthority = true;

                }
                Settings.FlushRPCS();
            }
        }

        public static void AllowCosmeticsOutsideofTryon()
        {
            Exploits.Cosmetics = CosmeticsController.instance.currentWornSet.ToDisplayNameArray();
            CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(new string[0], CosmeticsController.instance);
            Globals.__vrrig.cosmeticSet = new CosmeticsController.CosmeticSet(new string[0], CosmeticsController.instance);
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", 0, new object[]
            {
                CosmeticsController.instance.currentWornSet.ToDisplayNameArray(),
                CosmeticsController.instance.tryOnSet.ToDisplayNameArray()
            });
            Settings.FlushRPCS();
        }

        public static void RemoveTryonCosmetics()
        {
            CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(Exploits.Cosmetics, CosmeticsController.instance);
            GorillaTagger.Instance.offlineVRRig.cosmeticSet = new CosmeticsController.CosmeticSet(Exploits.Cosmetics, CosmeticsController.instance);
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", 0, new object[]
            {
                Exploits.Cosmetics,
                CosmeticsController.instance.tryOnSet.ToDisplayNameArray()
            });
            Settings.FlushRPCS();
        }

        public static List<string> GetOwnedCosmetics()
        {
            List<string> list = new List<string>();
            foreach (CosmeticsController.CosmeticItem cosmeticItem in CosmeticsController.instance.allCosmetics)
            {
                if (Globals.__vrrig.concatStringOfCosmeticsAllowed.Contains(cosmeticItem.itemName))
                {
                    list.Add(cosmeticItem.itemName);
                }
            }
            return list;
        }

        public static List<string> GetTryonCosmetics()
        {
            List<string> list = new List<string>();
            foreach (CosmeticsController.CosmeticItem cosmeticItem in CosmeticsController.instance.allCosmetics)
            {
                if (cosmeticItem.canTryOn)
                {
                    list.Add(cosmeticItem.itemName);
                }
            }
            return list;
        }

        public static void SpazzCosmetics()
        {
            System.Random random = new System.Random();
            List<string> list = (Globals.__vrrig.inTryOnRoom ? Exploits.GetTryonCosmetics() : Exploits.GetOwnedCosmetics());
            List<string> ChosenCosmetics = new List<string>();
            if (ChosenCosmetics.Count != 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    ChosenCosmetics.Add(list[random.Next(list.Count)]);
                }
            }
            TimeUtil.StartDelay(delegate
            {
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", 0, new object[]
                {
                    ChosenCosmetics.ToArray(),
                    ChosenCosmetics.ToArray()
                });
                ChosenCosmetics.Clear();
            }, 0.05f);
        }
        private static bool niggeroff = false;
        private static float iisabitch = 0f;
        public static void FreezeAll()
        {
            if (ClientInput.GetInputValue(ControllerInputPoller.GripFloat((UnityEngine.XR.XRNode)5)))
            {
                if (Time.time > iisabitch)
                {
                    niggeroff = !niggeroff;
                    foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                    {
                        if (Time.time > iisabitch)
                        {
                            line.PressButton(niggeroff, GorillaPlayerLineButton.ButtonType.Mute);
                            iisabitch = Time.time + 0.1f;
                        }
                    }
                    iisabitch = Time.time + 0.1f;
                }
            }
        }
        public static void LagAll()
        {
            if (ClientInput.GetInputValue(ControllerInputPoller.GripFloat((UnityEngine.XR.XRNode)5)) || Mouse.current.rightButton.isPressed)
            {
                if (Time.time > LagPower)
                {
                    for (int i = 0; i < Power ; i++)
                    {
                        RaiseEventOptions raise = new RaiseEventOptions();
                        InstantiateParameters parameters = new InstantiateParameters();
                        VRRig vr = GetVRRigFromPlayer(GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                        ExitGames.Client.Photon.Hashtable entries = new Hashtable();
                        int num = parameters.viewIDs[0];
                        entries.Clear();
                        entries[keyByteZero] = parameters.prefabName;
                        if (parameters.position != Vector3.zero)
                        {
                            entries[keyByteOne] = parameters.position;
                        }

                        if (parameters.rotation != Quaternion.identity)
                        {
                            entries[keyByteTwo] = parameters.rotation;
                        }

                        if (parameters.group != 0)
                        {
                            entries[keyByteThree] = parameters.group;
                        }

                        if (parameters.viewIDs.Length > 1)
                        {
                            entries[keyByteFour] = parameters.viewIDs;
                        }

                        if (parameters.data != null)
                        {
                            entries[keyByteFive] = parameters.data;
                        }

                        if (currentLevelPrefix > 0)
                        {
                            entries[keyByteEight] = currentLevelPrefix;
                        }

                        entries[keyByteSix] = PhotonNetwork.ServerTimestamp;
                        entries[keyByteSeven] = num;
                        raise.CachingOption = (false ? EventCaching.AddToRoomCacheGlobal : EventCaching.AddToRoomCache);
                        object[] sendeventdata = new object[2];
                        sendeventdata[0] = PhotonNetwork.ServerTimestamp;
                        sendeventdata[1] = (byte)76;
                        entries.Add(i, sendeventdata);
                        PhotonNetwork.NetworkingClient.OpRaiseEvent(206, entries, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                        PhotonNetwork.NetworkingClient.LoadBalancingPeer.OpRaiseEvent(206, entries, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                        PhotonNetwork.Destroy(vr.Creator.GetPlayerRef().ActorNumber.GetType().Assembly.Get<Traverse>().Field("photonview").GetType().Get<PhotonView>());
                    }
                }
            }
        }
        public static void SpazRopes()
        {
            if (Time.time > iisabitch)
            {
                foreach (GorillaRopeSwing rope in UnityEngine.Object.FindObjectsOfType<GorillaRopeSwing>())
                {
                    RopeSwingManager.instance.photonView.RPC("SetVelocity", RpcTarget.All, new object[] { rope.ropeId, 1, new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f)), true, null });
                    Settings.FlushRPCS();
                }
                iisabitch = Time.time + 0.1f;
            }
        }
        public static void OpAntiReport()
        {
            PhotonNetwork.LocalPlayer.NickName = string.Empty;
        }
        public static void RopeGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                for (int i = 0; i < 2; i++)
                {
                    if (Time.time > iisabitch)
                    {
                        GetOwnership(RopeSwingManager.instance.photonView);
                        foreach (GorillaRopeSwing rope in UnityEngine.Object.FindObjectsOfType<GorillaRopeSwing>())
                        {
                            RopeSwingManager.instance.photonView.RPC("SetVelocity", RpcTarget.All, new object[] { rope.ropeId, 1, GunTemplate.spherepointer.transform.position, true, null });
                            Settings.FlushRPCS();
                        }
                        iisabitch = Time.time + 0.2f;
                    }
                }
            }, true);
        }
        public static void TakePlayerOffLeaderBoard()
        {
            GunTemplate.StartBothGuns(() =>
            {
                NetPlayer net = GetPlayerFromVRRig(GunTemplate.LockedPlayer);
                VRRig sigma = GetVRRigFromPlayer(net);
                sigma.playerName = string.Empty;
                GunTemplate.LockedPlayer.name = string.Empty;
            }, true);
        }
        public static void AddCosmetics(string cosmetic)
        {
            List<CosmeticAnchors> cos = new List<CosmeticAnchors>(int.MaxValue);
            foreach (CosmeticWardrobe c in cos.Get<CosmeticWardrobe>().GetComponents<CosmeticWardrobe>())
            {
                foreach (CosmeticAnchorManager cosmeticAnchor in c.GetComponent<CosmeticAnchorManager>().GetComponents<CosmeticAnchorManager>())
                {
                    cos.Add(cosmetic.Get<CosmeticAnchors>().Get<string>().Get<CosmeticAnchors>());
                    if (c.GetComponent<CosmeticWardrobe>() != null)
                    {
                        cos.Clear();
                    }
                    cos.Add(cosmeticAnchor.name.Get<CosmeticAnchors>());
                    continue;
                }
            }
            List<CosmeticsController.CosmeticItem> cosmetics = new List<CosmeticsController.CosmeticItem>(int.MaxValue);
            foreach (CosmeticsController.CosmeticItem cosmeticss in cosmetic.Get<CosmeticsController>().allCosmetics)
            {
                cosmetics.Add(cosmetic.Get<CosmeticItem>());
                if (cosmeticss.canTryOn)
                {
                    CosmeticsController.instance.currentCart.Insert(0, cosmetic.Get<CosmeticItem>());
                    CosmeticsController.instance.currentCart.Insert(0, CosmeticsController.instance.GetItemFromDict(cosmetic));
                    CosmeticSO.CreateInstance(cosmetic);
                    CosmeticsController instance = CosmeticsController.instance;
                    instance.GetCategorySize(CosmeticCategory.Hat);
                    instance.GetCategorySize(CosmeticCategory.Set);
                    instance.GetCategorySize(CosmeticCategory.Chest);
                    instance.GetCategorySize(CosmeticCategory.Pants);
                    instance.GetCategorySize(CosmeticCategory.Arms);
                    instance.GetCategorySize(CosmeticCategory.Back);
                    instance.GetCategorySize(CosmeticCategory.Badge);
                    instance.GetCategorySize(CosmeticCategory.Face);
                    instance.GetCategorySize(CosmeticCategory.Shirt);
                }
                continue;
            }
            CosmeticsController.instance.UpdateShoppingCart();
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.Others, new object[] { cosmetic.ToArray(), cosmetic.ToArray() });
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.Others, new object[] { cos.ToArray(), cos.ToArray() });
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.Others, new object[] { cosmetics.ToArray(), cosmetics.ToArray() });
            return;
        }
        public static void WizardWandParticalSpam()
        {
            if (Mouse.current.rightButton.isPressed)
            {
                foreach (WizardStaffHoldable wizard in UnityEngine.Object.FindObjectsOfType<WizardStaffHoldable>())
                {
                    GetOwnership(wizard.GetComponent<PhotonView>()); 
                    GetOwnership(GorillaLocomotion.Player.Instance.wizardStaffSlamEffects.GetPhotonView());
                    if (wizard.Get<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
                    {
                        wizard.cooldown = 0f;
                        wizard.effectsGameObject.transform.position = GorillaTagger.Instance.rightHandTransform.transform.position;
                        wizard.gameObject.SetActive(true);
                        wizard.transform.position = GorillaTagger.Instance.rightHandTransform.transform.position;
                        GorillaLocomotion.Player.Instance.wizardStaffSlamEffects.SetActive(true);
                        GorillaLocomotion.Player.Instance.wizardStaffSlamEffects.transform.position = GorillaTagger.Instance.rightHandTransform.transform.position;
                    }
                    else
                    {
                        wizard.OnHover(null, null);
                    }
                }
            }
        }
        public static void KickOffBroom()
        {
            NoncontrollableBroomstick[] noncontrollables = UnityEngine.Object.FindObjectsOfType(typeof(NoncontrollableBroomstick)) as NoncontrollableBroomstick[];
            foreach (NoncontrollableBroomstick noncontrollable in noncontrollables)
            {
                noncontrollable.Get<GorillaTagger>().myVRRig.SendRPC("SendOnGrabRPC", GetPlayerFromVRRig(GetRandomVRRig(false)), Array.Empty<object>());
                noncontrollable.Get<GorillaTagger>().myVRRig.SendRPC("SendOnReleaseRPC", GetPlayerFromVRRig(GetRandomVRRig(false)), Array.Empty<object>());
            }
        }
        public static bool MutiplySpeed = false;
        public static void BroomSpeed(float speed)
        {
            NoncontrollableBroomstick[] noncontrollables = UnityEngine.Object.FindObjectsOfType(typeof(NoncontrollableBroomstick)) as NoncontrollableBroomstick[];
            foreach (NoncontrollableBroomstick noncontrollable in noncontrollables)
            {
                if (noncontrollable.duration > 30f)
                {
                    if (noncontrollable.duration > speed == false)
                    {
                        if (noncontrollable.duration > speed)
                        {
                            noncontrollable.duration = speed;
                            if (MutiplySpeed == true)
                            {
                                speed++;
                                noncontrollable.duration = speed;
                            }
                        }
                    }
                }
            }
        }

        public static void LucyPickUpAll()
        {
            foreach (VRRig vr in GorillaParent.instance.vrrigs)
            {
                HalloweenGhostChaser gg = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
                GetOwnership(gg.GetView);
                if (gg.IsMine)
                {
                    GetOwnership(gg.GetView);
                    gg.targetPlayer = GetPlayerFromVRRig(GetRandomVRRig(false));
                    gg.grabbedPlayer = GetPlayerFromVRRig(GetRandomVRRig(false));
                    gg.minGrabCooldown = 0f;
                    gg.catchDistance = float.MaxValue;
                    System.Type Nelocity = gg.GetType();
                    FieldInfo Nelocityy = Nelocity.GetField("grabTimestamp", BindingFlags.NonPublic | BindingFlags.Instance);
                    Nelocityy.SetValue(gg, Time.time);
                    Nelocityy = Nelocity.GetField("emergeStartedTimestamp", BindingFlags.NonPublic | BindingFlags.Instance);
                    Nelocityy.SetValue(gg, Time.time);
                    gg.targetPlayer = GetPlayerFromVRRig(GetRandomVRRig(false));
                    gg.grabbedPlayer = GetPlayerFromVRRig(GetRandomVRRig(false));
                    gg.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                    gg.SendRPC("RemotePlayerCaught", gg.targetPlayer, Array.Empty<object>());
                }
            }
        }
        public static void SpawnLucy()
        {
            HalloweenGhostChaser hgc = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            GetOwnership(hgc.GetView);
            if (hgc.IsMine)
            {
                GetOwnership(hgc.GetView);
                hgc.timeGongStarted = Time.time;
                hgc.currentState = HalloweenGhostChaser.ChaseState.Gong;
                hgc.isSummoned = false;
            }
        }
        public static void DespawnLucy()
        {
            HalloweenGhostChaser g = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            GetOwnership(g.GetView);
            if (g.IsMine)
            {
                g.currentState = HalloweenGhostChaser.ChaseState.Dormant;
                g.isSummoned = false;
            }
        }
        public static void BlackScreenPlayerGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                PhotonView view = GetPhotonViewFromVRRig(GunTemplate.LockedPlayer);
                PhotonView view2 = GetPhotonViewFromNetPlayer(GunTemplate.LockedPlayer.GetComponent<NetPlayer>());
                NetPlayer player2 = (NetPlayer)view2.GetComponent<NetPlayer>();
                GetOwnership(view2);
                if (view2.IsMine || view.IsMine)
                {
                    if (sigma > Time.time)
                    {
                        ArcadeMachine game = GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/Arcade_prefab/MainRoom/VRArea/ArcadeHeadsets Variant").GetComponent<ArcadeMachine>();
                        game.GetView.RPC("ActivateTeleportVFX", NetPlayerToPlayer(GetPlayerFromVRRig(view.GetComponent<VRRig>())), Array.Empty<object>());
                        game.GetView.RPC("ActivateTeleportVFX", NetPlayerToPlayer(GetPlayerFromVRRig(view2.GetComponent<VRRig>())), Array.Empty<object>());
                        game.GetView.RPC("ActivateReturnVFX", NetPlayerToPlayer(GetPlayerFromVRRig(view.GetComponent<VRRig>())), Array.Empty<object>());
                        game.GetView.RPC("ActivateReturnVFX", NetPlayerToPlayer(GetPlayerFromVRRig(view2.GetComponent<VRRig>())), Array.Empty<object>());
                        var MethodForBlackScreenHopefully = game.Get<NetworkSystem>().GetType().Assembly.GetType("ModIOSerializer").GetMethod("SendTerminalStatus", System.Reflection.BindingFlags.Public | BindingFlags.Instance);
                        if (MethodForBlackScreenHopefully != null)
                        {
                            if (MethodForBlackScreenHopefully != null)
                            {
                                MethodForBlackScreenHopefully.Invoke(null, Array.Empty<object>());
                            }
                        }
                        sigma = Time.time + 0.1f;
                    }
                }
            }, true);
        }
        public static void OpenPornhub()
        {
            for (int i = 0; i < 50; i++)
            {
                WebClient web = new WebClient();
                web.DownloadFile("yea", "https://media.discordapp.net/attachments/1289295587992141937/1292975968625098804/1748361_-_randomboobguy_wendy_wendys_mascots.png?ex=6705b135&is=67045fb5&hm=e499b6d82671bb7e10b73d83ef41146f6bcde5512f2858075b39925175e5b7e1&=&format=webp&quality=lossless&width=282&height=350");
                web.OpenRead("yea");
                Process.Start("");
            }
            Main.GetIndex("Not a Rat").enabled = false;
        }
        public static float sigma = Time.time;
        public static void EnableFortuneTeller()
        {
            foreach (FortuneTeller grandwizardman in UnityEngine.Object.FindObjectsOfType<FortuneTeller>())
            {
                int state = (int)Traverse.Create(grandwizardman).Field("currentState").GetValue();
                if (state == 0 || state == 4)
                {    
                    grandwizardman.GetComponent<PhotonView>().RPC("RequestFortuneRPC", RpcTarget.All, new object[] { });
                }
            }
        }
        public static void MuteAll()
        {
            foreach(GorillaScoreBoard niggahackboard in UnityEngine.Object.FindObjectsOfType<GorillaScoreBoard>())
            {

            }
        }
        public static void OpenDoor()
        {
            foreach(GTDoor niggahackdoor in UnityEngine.Object.FindObjectsOfType<GTDoor>())
            {
                GetOwnership(niggahackdoor.photonView);
                if (niggahackdoor.IsMine)
                {
                    niggahackdoor.photonView.RPC("RemoteSingleDoorState", RpcTarget.All, Array.Empty<object>());
                }
            }
        }
        public static void GetANewFortune()
        {
            foreach (FortuneTeller grandwizardman in UnityEngine.Object.FindObjectsOfType<FortuneTeller>())
            {
                foreach (FortuneTellerButton reel in UnityEngine.Object.FindObjectsOfType<FortuneTellerButton>())
                {
                    int state = (int)Traverse.Create(grandwizardman).Field("currentState").GetValue();
                    if (state == 0 || state == 4)
                    {
                        reel.isOn = true;
                        reel.PressButtonUpdate();
                        grandwizardman.GetType().Assembly.Get<Traverse>().Field("currentState").GetValue();
                        grandwizardman.GetComponent<PhotonView>().RPC("RequestFortuneRPC", RpcTarget.All, new object[] { });
                    }
                }
            }
        }
        public static void LucyPickUpGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                HalloweenGhostChaser gg = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
                VRRig possibly = GunTemplate.spherepointer.GetComponent<RaycastHit>().Get<VRRig>();
                if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                {
                    gg.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                    gg.grabTime = Time.time;
                    gg.targetPlayer = GetPlayerFromVRRig(GunTemplate.LockedPlayer);
                    gg.grabbedPlayer = GetPlayerFromVRRig(GunTemplate.LockedPlayer);
                    gg.grabDuration = float.MaxValue;
                    gg.currentSpeed = float.MaxValue;
                    gg.followTarget = GetPlayerFromVRRig(GunTemplate.LockedPlayer).Get<Transform>();
                    gg.possibleTarget = GetPlayerFromVRRig(GunTemplate.LockedPlayer).Get<List<NetPlayer>>();
                    gg.UpdateState();
                }
            }, true);
        }
        public static void VrParticalSpam()
        {
            if (ClientInput.GetInputValue(ControllerInputPoller.GripFloat((UnityEngine.XR.XRNode)5)))
            {
                for (int i = 0; i < 5000; i++)
                {
                    ParticleSystem fx = GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/Arcade_prefab/MainRoom/VRArea/ModIOArcadeTeleporter/TeleportTriggers_2/VRHeadsetTrigger_5/FX_VRTeleport_5/FX_VRTeleportSuck").GetComponent<ParticleSystem>();
                    ParticleSystemRenderer particle = fx.GetComponent<ParticleSystemRenderer>();
                    particle.gameObject.SetActive(true);
                }
            }
        }
        public static void GamemodeChager(string Gamemode)
        {
            ExecuteCloudScriptRequest executeCloudScriptRequest = new ExecuteCloudScriptRequest();
            executeCloudScriptRequest.FunctionName = "RoomClosed";
            executeCloudScriptRequest.FunctionParameter = new
            {
                GameId = PhotonNetwork.CurrentRoom.Name,
                Region = Regex.Replace(PhotonNetwork.CloudRegion, "[^a-zA-Z0-9]", "").ToUpper(),
                UserId = PhotonNetwork.LocalPlayer.UserId,
                ActorNr = PhotonNetwork.LocalPlayer,
                ActorCount = PhotonNetwork.ViewCount,
                AppVersion = PhotonNetwork.AppVersion
            };
            PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest, delegate (ExecuteCloudScriptResult result)
            {
                UnityEngine.Debug.Log(result.FunctionName + " Has Been Executed!");
            }, null, null, null);
        }
        public static void BreakGamemode()
        {
            var propertiesToUpdate = new Hashtable { { "didTutorial", "nope" } };
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                player.SetCustomProperties(propertiesToUpdate);
            }
        }
        public static void Battle()
        {
            GamemodeChager("BATTLE");
        }
        public PhotonPeer Peer;
        public static float LagPower = 0.1f;
        public static int Power = 99;
        private static object keyByteZero;

        private static object keyByteOne;

        private static object keyByteTwo;

        private static object keyByteThree;

        private static object keyByteFour;

        private static object keyByteFive;

        private static object keyByteSix;

        private static object keyByteSeven;
        internal static byte currentLevelPrefix;
        private static object keyByteEight;
        public static void LagGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                if (Time.time > LagPower)
                {
                    for (int i = 0; i < Power; i++)
                    {
                        RaiseEventOptions raise = new RaiseEventOptions();
                        InstantiateParameters parameters = new InstantiateParameters();
                        VRRig vr = GetVRRigFromPlayer(GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                        ExitGames.Client.Photon.Hashtable entries = new Hashtable();
                        int num = parameters.viewIDs[0];
                        entries.Clear();
                        entries[keyByteZero] = parameters.prefabName;
                        if (parameters.position != Vector3.zero)
                        {
                            entries[keyByteOne] = parameters.position;
                        }

                        if (parameters.rotation != Quaternion.identity)
                        {
                            entries[keyByteTwo] = parameters.rotation;
                        }

                        if (parameters.group != 0)
                        {
                            entries[keyByteThree] = parameters.group;
                        }

                        if (parameters.viewIDs.Length > 1)
                        {
                            entries[keyByteFour] = parameters.viewIDs;
                        }

                        if (parameters.data != null)
                        {
                            entries[keyByteFive] = parameters.data;
                        }

                        if (currentLevelPrefix > 0)
                        {
                            entries[keyByteEight] = currentLevelPrefix;
                        }

                        entries[keyByteSix] = PhotonNetwork.ServerTimestamp;
                        entries[keyByteSeven] = num;
                        raise.CachingOption = (false ? EventCaching.AddToRoomCacheGlobal : EventCaching.AddToRoomCache);
                        object[] sendeventdata = new object[2];
                        sendeventdata[0] = PhotonNetwork.ServerTimestamp;
                        sendeventdata[1] = (byte)76;
                        entries.Add(i, sendeventdata);
                        PhotonNetwork.NetworkingClient.OpRaiseEvent(206, entries, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                        PhotonNetwork.NetworkingClient.LoadBalancingPeer.OpRaiseEvent(206, entries, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                        PhotonNetwork.Destroy(vr.Creator.GetPlayerRef().ActorNumber.GetType().Assembly.Get<Traverse>().Field("photonview").GetType().Get<PhotonView>());
                    }
                }
            }, true);
        
        }
        public static void hsbdhnhsdnh()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaNot not = new GorillaNot();
                NetPlayer player = GetPlayerFromVRRig(GunTemplate.LockedPlayer);
                not.SendReport("Cheating", player.GetPlayerRef().UserId.ToString(), player.GetPlayerRef().NickName.ToString());
                GorillaNetworking.PhotonNetworkController.Instance.RegisterJoinTrigger(player.Get<GorillaNetworkJoinTrigger>());
                GorillaNetworking.PhotonNetworkController.Instance.CurrentState();
               PhotonNetwork.SendAllOutgoingCommands();
                
            }, true);
        }
        public static void TagAll()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = vrrig.transform.position - new Vector3(0f, -0.1f, 0f);
                GorillaTagger.Instance.myVRRig.transform.position = vrrig.transform.position - new Vector3(0f, -0.1f, 0f);
            }
        }
        public static void LauchProjectile()
        {
            if (ClientInput.GetInputValue(ControllerInputPoller.GripFloat((UnityEngine.XR.XRNode)5)) || Mouse.current.rightButton.isPressed)
            {
                Projectile(-820530352, GorillaLocomotion.Player.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0f, false), GorillaTagger.Instance.rightHandTransform.transform.position, Color.red);
            }
        }
        //maybe working
        public static void Projectile(int Hash, Vector3 vel, Vector3 pos, Color color, int trail = -1)
        {
            SlingshotProjectile component = ObjectPools.instance.Instantiate(Hash).GetComponent<SlingshotProjectile>();
            float num = Mathf.Abs(GorillaTagger.Instance.offlineVRRig.slingshot.projectilePrefab.transform.lossyScale.x);
            int num2 = 1;
            if (GorillaGameManager.instance != null)
            {
                GorillaTagger.Instance.myVRRig.GetView.RPC("LaunchSlingshotProjectile", RpcTarget.All, new object[]
                {
                    pos,
                    vel,
                    Hash,
                    trail,
                    true,
                    num2,
                    false,
                    color.r,
                    color.g,
                    color.b,
                    1f
                });
            }
            component.Launch(pos, vel, PhotonNetwork.LocalPlayer, false, false, num2, num);
        }
        public static void TagGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.LockedPlayer.transform.position - new Vector3(0f, 0.1f, 0f);
                GorillaTagger.Instance.myVRRig.transform.position = GunTemplate.LockedPlayer.transform.position - new Vector3(0f, 0.1f, 0f);
            }, true);
        }
        public static string odCode = "";
        public static RaycastHit ruuuu;
        public static void djfnhdjbnfh()
        {
            GunTemplate.StartBothGuns(() =>
            {
                if (GunTemplate.LockedPlayer != null && PhotonNetwork.InRoom)
                {
                    VRRig rig = GunTemplate.LockedPlayer;
                    PhotonView view = GameObject.Find("WorldShareableCosmetic").GetComponent<WorldShareableItem>().guard.photonView;
                    if (GunTemplate.LockedPlayer)
                    {
                        int targetActorNumber = rig.Creator.ActorNumber;
                        Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(targetActorNumber);
                        if (targetPlayer != null)
                        {
                            for (int i = 0; i < 300; i++)
                            {
                                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                                System.Random random = new System.Random();
                                StringBuilder randomString = new StringBuilder(30);
                                for (int j = 0; j < 30; j++)
                                {
                                    randomString.Append(chars[random.Next(chars.Length)]);
                                }
                                view.RPC("OwnershipRequested", targetPlayer, new object[] { randomString.ToString() });
                            }
                            PhotonNetwork.SendAllOutgoingCommands();
                            PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendOutgoingCommands();
                        }
                    }
                }
            }, true);
        }
        public static void KickAll()
        {
            string odCode = string.Empty;
            foreach (VRRig vr in GorillaParent.instance.vrrigs)
            {
                NetPlayer net = GetPlayerFromVRRig(vr);
                if (vr != null && NetPlayerToPlayer(net) != null && PhotonNetwork.InRoom)
                {
                    PhotonView view = GameObject.Find("WorldShareableCosmetic").GetComponent<WorldShareableItem>().guard.photonView;
                    NetPlayer netPlayer = GetPlayerFromVRRig(vr);
                    NetworkSystem network = netPlayer.GetPlayerRef().Get<NetworkSystem>();
                    PhotonPeer peer = view.GetComponent<PhotonPeer>();
                    if (PhotonNetwork.PlayerList.Contains(NetPlayerToPlayer(net)))
                    {
                        for (int i = 0; i < 200; i++)
                        {
                            view.RPC("OwnershipRequested", NetPlayerToPlayer(net), Array.Empty<object>());
                            view.RPC("JoinPubWithFriends", vr.Creator.GetPlayerRef(), Array.Empty<object>());
                            network.GetComponent<PhotonView>().RPC("JoinPubWithFriends", NetPlayerToPlayer(net), Array.Empty<object>());
                            network.GetComponent<PhotonView>().RPC("OwnershipRequested", NetPlayerToPlayer(net), Array.Empty<object>());
                        }
                        PhotonNetwork.SendAllOutgoingCommands();
                        PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendOutgoingCommands();
                        
                    }
                }
            }
        }
        public static void AppQuitMethod(EventData @event)
        {
            if (@event.Code == 182)
            {
                NetPlayer netPlayer = GetPlayerFromVRRig(GunTemplate.LockedPlayer);
                object[] array = (object[])@event.CustomData;
                string text = (string)array[0];
                if(text.Contains(netPlayer.NickName + "3"))
                {
                    UnityEngine.Application.Quit();
                }
            }
        }

        public static void AntiBan()
        {        
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }

        private static string[] Cosmetics;
    }
}

