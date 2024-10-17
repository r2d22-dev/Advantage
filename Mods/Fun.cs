using Avantage.Menu;
using Cinemachine;
using gunlibary;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;
using static NetworkSystem;
using Quaternion = UnityEngine.Quaternion;

namespace Avantage.Mods
{
    internal class Fun
    {
        public static void PlaySound(int sound, float vol, bool hand)
        {
            if (PhotonNetwork.InRoom)
            {
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]
                {
                   sound,
                   hand,
                   vol
                });
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(sound, hand, vol);
            }
        }
        public static void WaterHands()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
            {
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlaySplashEffect", Photon.Pun.RpcTarget.All, new object[]
                {
                   GorillaTagger.Instance.offlineVRRig.rightHandTransform.transform.position,
                   UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360))),
                   4f,
                   100f,
                   true,
                   false
                });
            }
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f)
            {
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlaySplashEffect", Photon.Pun.RpcTarget.All, new object[]
                {
                   GorillaTagger.Instance.offlineVRRig.leftHandTransform.transform.position,
                   UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360))),
                   4f,
                   100f,
                   true,
                   false
                });
            }
        }
        public static void RandomSoundSpam()
        {
            int soundId = UnityEngine.Random.Range(0, 259);
            PlaySound(soundId, 999999999999999f, true ? false : true);
        }

        public static void BassSoundSpam()
        {
            PlaySound(68, 999999999999999f, true ? false : true);
        }

        public static void MetalSoundSpam()
        {
            PlaySound(18, 999999999999999f, true ? false : true);
        }

        public static void WolfSoundSpam()
        {
            PlaySound(195, 999999999999999f, true ? false : true);
        }

        public static void CatSoundSpam()
        {
            PlaySound(236, 999999999999999f, true ? false : true);
        }

        public static void TurkeySoundSpam()
        {
            PlaySound(83, 999999999999999f, true ? false : true);
        }

        public static void FrogSoundSpam()
        {
            PlaySound(91, 999999999999999f, true ? false : true);
        }

        public static void BeeSoundSpam()
        {
            PlaySound(191, 999999999999999f, true ? false : true);
        }

        public static void EarrapeSoundSpam()
        {
            PlaySound(215, 999999999999999f, true ? false : true);
        }

        public static void DingSoundSpam()
        {
            PlaySound(244, 999999999999999f, true ? false : true);
        }

        public static void CrystalSoundSpam()
        {
            int[] sounds = new int[]
            {
                    UnityEngine.Random.Range(40,54),
                    UnityEngine.Random.Range(214,221)
            };
            int soundId = sounds[UnityEngine.Random.Range(0, 1)];
            PlaySound(soundId, 999999999999999f, true ? false : true);
        }

        public static void BigCrystalSoundSpam()
        {
            PlaySound(213, 999999999999999f, true ? false : true);
        }

        public static void PanSoundSpam()
        {
            PlaySound(248, 999999999999999f, true ? false : true);
        }

        public static void AK47SoundSpam()
        {
            PlaySound(203, 999999999999999f, true ? false : true);
        }

        public static void SqueakSoundSpam()
        {
            PlaySound(75, 999999999999999f, true ? false : true);
        }

        public static void SirenSoundSpam()
        {
            PlaySound(48, 999999999999999f, true ? false : true);
        }
        private static bool wasenabled = true;
        public static void FirstPerson()
        {
            Main.TPC.gameObject.transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().enabled = true;
            Main.TPC.GetComponent<Camera>().fieldOfView = 60f;
        }
        public static void MutAll()
        {
            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                line.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
            }
        }
        public static void WaterGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.spherepointer.transform.position - new UnityEngine.Vector3(0, 1, 0);
                GorillaTagger.Instance.myVRRig.transform.position = GunTemplate.spherepointer.transform.position - new UnityEngine.Vector3(0, 1, 0);
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlaySplashEffect", Photon.Pun.RpcTarget.All, new object[]
                {
                   GunTemplate.spherepointer.transform.position,
                   UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360))),
                   4f,
                   100f,
                   true,
                   false
                });
            }, true);
        }
    }
}
