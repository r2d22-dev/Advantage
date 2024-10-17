
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Avantage.Classes;
using Avantage.Menu;
using UnityEngine;
using static Avantage.Menu.Main;
using Valve.VR.InteractionSystem;
using BepInEx;
using Avantage.Notifications;

namespace Avantage.Menu
{
    internal class Settings
    {
        public static ExtGradient backgroundColor = new ExtGradient { isRainbow = true };
        public static ExtGradient[] buttonColors = new ExtGradient[]
        {
            new ExtGradient{colors = GetSolidGradient(Main.Button) }, // Disabled
            new ExtGradient{colors = GetSolidGradient((new Color32(255, 0, 0, 255)))} // Enabled
        };
        public static Color[] textColors = new Color[]
        {
             Color.white, // Disabled
            Color.white // Enabled
        };
        public static Color32 enabled = new Color32(153, 50, 204, 255);
        public static float time = Time.time;
        public static void FlushRPCS()
        {
            if (PhotonNetwork.InRoom)
            {
                GorillaNot.instance.rpcErrorMax = int.MaxValue;
                GorillaNot.instance.rpcCallLimit = int.MaxValue;
                GorillaNot.instance.logErrorMax = int.MaxValue;
                PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
                PhotonNetwork.QuickResends = int.MaxValue;
                PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
                PhotonNetwork.OpCleanRpcBuffer(GorillaTagger.Instance.myVRRig.GetView);
                PhotonNetwork.RemoveBufferedRPCs(GorillaTagger.Instance.myVRRig.ViewID, null, null);
                PhotonNetwork.RemoveRPCsInGroup(int.MaxValue);
                PhotonNetwork.SendAllOutgoingCommands();
                GorillaNot.instance.OnPlayerLeftRoom(PhotonNetwork.LocalPlayer);
                time = Time.time + 0.1f;
            }
        }
        public static void StumpText()
        {
            for (int i = 0; i < 0; i++)
            {
                GameObject stumpertext = new GameObject("SlingaDing1");
                stumpertext.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                TextMesh textMesh = stumpertext.AddComponent<TextMesh>();
                textMesh.color = Color.white;
                textMesh.AddComponent<Renderer>();
                textMesh.fontSize = 24;
                textMesh.fontStyle = activeFontStyle;
                textMesh.characterSize = 0.1f;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignment.Center;
                textMesh.text = "<color=grey>[</color><color=blue>" + "Advantage" + "</color><color=grey>]</color>";
                stumpertext.transform.position = new Vector3(-66.6349f, 12.7573f, -82.5203f);
                stumpertext.transform.LookAt(Camera.main.transform.position);
                stumpertext.transform.Rotate(0f, 180f, 0f);
                UnityEngine.Object.Destroy(stumpertext, Time.deltaTime);
                GameObject stumpertext2 = new GameObject("SlingaDing2");
                stumpertext2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                TextMesh textMesh2 = stumpertext2.AddComponent<TextMesh>();
                textMesh2.color = Color.white;
                textMesh2.fontSize = 24;
                textMesh2.AddComponent<Renderer>();
                textMesh2.fontStyle = activeFontStyle;
                textMesh2.characterSize = 0.1f;
                textMesh2.anchor = TextAnchor.MiddleCenter;
                textMesh2.alignment = TextAlignment.Center;
                textMesh2.text = "THE ONLY MENU YOU WILL EVER NEED!";
                stumpertext2.transform.position = new Vector3(-66.6349f, 12.6573f, -82.5203f);
                stumpertext2.transform.LookAt(Camera.main.transform.position);
                stumpertext2.transform.Rotate(0f, 180f, 0f);
                UnityEngine.Object.Destroy(stumpertext2, Time.deltaTime);
                GameObject stumpertext22 = new GameObject("SlingaDing3");
                stumpertext22.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                TextMesh textMesh22 = stumpertext22.AddComponent<TextMesh>();
                textMesh22.color = Color.white;
                textMesh22.fontSize = 24;
                textMesh22.fontStyle = activeFontStyle;
                textMesh22.characterSize = 0.1f;
                textMesh22.anchor = TextAnchor.MiddleCenter;
                textMesh22.AddComponent<Renderer>();
                textMesh22.alignment = TextAlignment.Center;
                textMesh22.text = "THE MENU IS CURRENTLY " + PluginInfo.MenuStatus;
                stumpertext22.transform.position = new Vector3(-66.6349f, 12.5573f, -82.5203f);
                stumpertext22.transform.LookAt(Camera.main.transform.position);
                stumpertext22.transform.Rotate(0f, 180f, 0f);
                UnityEngine.Object.Destroy(stumpertext22, Time.deltaTime);
                GameObject stumpertext221 = new GameObject("SlingaDing4");
                stumpertext221.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                TextMesh textMesh221 = stumpertext221.AddComponent<TextMesh>();
                textMesh221.color = Color.white;
                textMesh221.fontSize = 24;
                textMesh221.AddComponent<Renderer>();
                textMesh221.fontStyle = activeFontStyle;
                textMesh221.characterSize = 0.1f;
                textMesh221.anchor = TextAnchor.MiddleCenter;
                textMesh221.alignment = TextAlignment.Center;
                textMesh221.text = "YOU ARE USING V:" + PluginInfo.Version;
                stumpertext221.transform.position = new Vector3(-66.6349f, 12.4573f, -82.5203f);
                stumpertext221.transform.LookAt(Camera.main.transform.position);
                stumpertext221.transform.Rotate(0f, 180f, 0f);
                UnityEngine.Object.Destroy(stumpertext221, Time.deltaTime);
                GameObject stumpertext2211 = new GameObject("SlingaDing5");
                stumpertext221.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                TextMesh textMesh2211 = stumpertext2211.AddComponent<TextMesh>();
                textMesh2211.color = Color.white;
                textMesh2211.fontSize = 24;
                textMesh2211.AddComponent<Renderer>();
                textMesh2211.fontStyle = activeFontStyle;
                textMesh2211.characterSize = 0.1f;
                textMesh2211.anchor = TextAnchor.MiddleCenter;
                textMesh2211.alignment = TextAlignment.Center;
                textMesh2211.text = "I CHA554  IS NOT RESPONSIBLE FOR ANY BANS WHILE USING THIS MENU ˞˞˞˞˞˞˞˞˞˞˞˞˞˞˞˞˞˞˞˞ IS!";
                stumpertext2211.transform.position = new Vector3(-66.6349f, 12.3573f, -82.5203f);
                stumpertext2211.transform.LookAt(Camera.main.transform.position);
                stumpertext2211.transform.Rotate(0f, 180f, 0f);
                UnityEngine.Object.Destroy(stumpertext2211, Time.deltaTime);
            }
        }
            public static FontStyle activeFontStyle = FontStyle.Italic;
        //w
        public static int reel = 0;
        public static void ChangeTheme()
        {
            reel++;
            if (reel > 5)
            {
                reel = 0;
            }
            if (reel == 1)
            {
                Main.GetIndex("Change Theme").buttonText = "Change Theme : Blue";
                Main.Background = Color.blue;
            }
            if (reel == 2)
            {
                Main.GetIndex("Change Theme : Blue").buttonText = "Change Theme : Red";
                Main.Background = Color.red;
            }
            if (reel == 3)
            {
                Main.GetIndex("Change Theme : Red").buttonText = "Change Theme : Green";
                Main.Background = Color.green;
            }
            if (reel == 4)
            {
                Main.GetIndex("Change Theme : Green").buttonText = "Change Theme : Cyan";
                Main.Background = Color.cyan;
            }
            if (reel == 5)
            {
                Main.GetIndex("Change Theme : Green").buttonText = "Change Theme : Magenta";
                Main.Background = Color.magenta;
            }
            Main.RecreateMenu();
        }

        public static Font currentFont = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);

        public static bool fpsCounter = true;
        public static bool disconnectButton = true;
        public static bool rightHanded = false;
        public static bool disableNotifications = false;

        public static KeyCode keyboardButton = KeyCode.Q;

        public static Vector3 menuSize = new Vector3(0.1f, 1f, 0.8f); // Depth, Width, Height
        public static int buttonsPerPage = 6;
    }
}
