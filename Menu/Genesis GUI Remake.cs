using Avantage.Classes;
using Avantage.Mods;
using BepInEx;
using GorillaLocomotion;
using GorillaNetworking;
using Meta.WitAi.Lib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Avantage.Menu
{
    [BepInPlugin("siGMA.GUI", "GUI", "1.0")]
    public class UI : BaseUnityPlugin
    {
        public static int selectedtab;
        public static Rect windowRect = new Rect(10, 10, 650, 400);
        public static bool showGUI = true; 
        static string sigma;
        static bool sigma2;

        public static string[] Rahh = new string[]
        {
            "ez"
        };

        public static Vector2 scrollpos1 = Vector2.zero;
        public static Vector2 scrollpos2 = Vector2.zero;
        public static Color theme = Color.red;
        static Color theme2 = Color.black;
        private static Vector3 oldMousePos;

        public static void Keyboarding()
        {
            float currentSpeed = 5;
            Transform bodyTransform = Camera.main.transform;
            GorillaTagger.Instance.rigidbody.useGravity = false;
            GorillaTagger.Instance.rigidbody.velocity = Vector3.zero;
            if (UnityInput.Current.GetKey(KeyCode.LeftShift))
            {
                currentSpeed *= 2.5f;
            }
            if (UnityInput.Current.GetKey(KeyCode.W) || UnityInput.Current.GetKey(KeyCode.UpArrow))
            {
                bodyTransform.position += bodyTransform.forward * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.A) || UnityInput.Current.GetKey(KeyCode.LeftArrow))
            {
                bodyTransform.position += -bodyTransform.right * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.S) || UnityInput.Current.GetKey(KeyCode.DownArrow))
            {
                bodyTransform.position += -bodyTransform.forward * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.D) || UnityInput.Current.GetKey(KeyCode.RightArrow))
            {
                bodyTransform.position += bodyTransform.right * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.Space))
            {
                bodyTransform.position += bodyTransform.up * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.LeftControl))
            {
                bodyTransform.position += -bodyTransform.up * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetMouseButton(1))
            {
                Vector3 pos = UnityInput.Current.mousePosition - oldMousePos;
                float x = bodyTransform.localEulerAngles.x - pos.y * 0.3f;
                float y = bodyTransform.localEulerAngles.y + pos.x * 0.3f;
                bodyTransform.localEulerAngles = new Vector3(x, y, 0f);
            }
            oldMousePos = UnityInput.Current.mousePosition;
        }

        private float buttonCooldown = 0.1f; 
        private float lastButtonPressTime = 0f;
        private static Color purple = new Color32(0, 0, 0, 255);
        void Update()
        { 
            if (UnityInput.Current.GetKeyDown(KeyCode.F1) && Time.time >= lastButtonPressTime + buttonCooldown)
            {
                showGUI = !showGUI; 
                lastButtonPressTime = Time.time;
            }
        }

        void OnGUI()
        {
            if (showGUI)
            {
                GUI.backgroundColor = theme;
                windowRect = GUI.Window(0, windowRect, DrawMainWindow, $"Cosmo Nigger GUI | User : {PhotonNetwork.NickName.ToLower()} | Fps : {Mathf.RoundToInt(1.0f / Time.deltaTime)} ");
                if (GUI.Button(new Rect(Screen.width - 160, 10, 150, 30), "Disable GUI"))
                {
                    showGUI = false; 
                }
            }
            else
            {
                GUI.backgroundColor = theme;
                if (GUI.Button(new Rect(Screen.width - 160, 10, 150, 30), "Enable GUI"))
                {
                    showGUI = true; 
                }
            }
        }

        private GUIStyle GetButtonStyle(ButtonInfo module)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = (bool)module.enabled ? theme : Color.white;
            style.active.textColor = (bool)module.enabled ? theme : Color.white;
            style.focused.textColor = (bool)module.enabled ? theme : Color.white;
            style.hover.textColor = (bool)module.enabled ? theme : Color.white;
            return style;
        }

        private string searchQuery = "";

        private void DrawMainWindow(int windowID)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUI.backgroundColor = theme;

            string[] tabnames = new string[] { "Mods", "Computer", "Themes", "Console", "PlayerList" };
            selectedtab = GUILayout.Toolbar(selectedtab, tabnames);

            if (selectedtab == 0)
            {
                GUILayout.Label("Search:", GUI.skin.label);
                searchQuery = GUILayout.TextField(searchQuery, GUILayout.Width(windowRect.width - 30));

                scrollpos1 = GUILayout.BeginScrollView(scrollpos1, GUILayout.Width(windowRect.width - 30), GUILayout.Height(windowRect.height - 40));

                foreach (ButtonInfo[] btninfo in Buttons.buttons)
                {
                    foreach (ButtonInfo info in btninfo)
                    {
                        if (string.IsNullOrEmpty(searchQuery) || info.buttonText.ToLower().Contains(searchQuery.ToLower()))
                        {
                            GUILayout.BeginHorizontal();

                            if (GUILayout.Button(new GUIContent(info.buttonText), GetButtonStyle(info)))
                            {
                                GUI.backgroundColor = Color.black;
                                info.enabled = !info.enabled;
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                            }

                            GUILayout.EndHorizontal();
                        }
                    }
                }

                GUILayout.EndScrollView();
            }
            if (selectedtab == 1)
            {
                scrollpos1 = GUILayout.BeginScrollView(scrollpos1, GUILayout.Width(windowRect.width - 30), GUILayout.Height(windowRect.height - 40));

                sigma = GUILayout.TextField(sigma);
                sigma2 = GUILayout.Toggle(sigma2, "WASD");
                if (sigma2)
                    Keyboarding();
                if (GUILayout.Button("Join Room"))
                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(sigma, JoinType.Solo);
                if (GUILayout.Button("Set Name"))
                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(sigma, JoinType.Solo);
                if (GUILayout.Button("Disconnect"))
                {
                    PhotonNetwork.Disconnect();
                    PhotonNetworkController.Instance.OnDisconnected();
                }

                GUILayout.EndScrollView();
            }
            if (selectedtab == 2)
            {
                scrollpos1 = GUILayout.BeginScrollView(scrollpos1, GUILayout.Width(windowRect.width - 30), GUILayout.Height(windowRect.height - 40));
                if (GUILayout.Button("Default"))
                {
                    theme = purple;
                    theme2 = purple;
                }
                if (GUILayout.Button("Cyan"))
                {
                    theme = Color.magenta;
                    theme2 = Color.magenta;
                }
                if (GUILayout.Button("Red"))
                {
                    theme = Color.red;
                    theme2 = new Color32(140, 0, 0, 255);
                }
                if (GUILayout.Button("Green"))
                {
                    theme = Color.green;
                    theme2 = new Color32(0, 135, 0, 255);
                }
                if (GUILayout.Button("Blue"))
                {
                    theme = Color.blue;
                    theme2 = Color.cyan;
                }
                if (GUILayout.Button("cyan"))
                {
                    theme = Color.cyan;
                    theme2 = Color.blue;
                }
                if (GUILayout.Button("Yellow"))
                {
                    theme = Color.yellow;
                    theme2 = new Color32(188, 188, 0, 255);
                }
                if (GUILayout.Button("Gray"))
                {
                    theme = Color.gray;
                    theme2 = new Color32(81, 81, 81, 255);
                }

                GUILayout.EndScrollView();
            }
            if (selectedtab == 3)
            {
                GUILayout.Label("Soon");

            }
            if (selectedtab == 4)
            {

            }

            GUILayout.EndVertical();

            GUI.DragWindow();
        }
    }
}
