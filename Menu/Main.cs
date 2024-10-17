using BepInEx;
using HarmonyLib;
using Photon.Pun;
using Avantage.Classes;
using Avantage.Mods;
using Avantage.Notifications;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Avantage.Menu.Buttons;
using static Avantage.Menu.Settings;
using Fusion;
using ExitGames.Client.Photon.StructWrapping;
using System.Net;
using static GorillaTelemetry;

namespace Avantage.Menu
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    public class Main : MonoBehaviour
    {
        // Constant
        public static void Prefix()
        {
            // Initialize Menu
            try
            {
                bool toOpen = (!rightHanded && ControllerInputPoller.instance.leftControllerSecondaryButton) || (rightHanded && ControllerInputPoller.instance.rightControllerSecondaryButton);
                bool keyboardOpen = UnityInput.Current.GetKey(keyboardButton);

                if (menu == null)
                {
                    if (toOpen || keyboardOpen)
                    {
                        CreateMenu();
                        RecenterMenu(rightHanded, keyboardOpen);
                        if (reference == null)
                        {
                            CreateReference(rightHanded);
                        }
                    }
                }
                else
                {
                    if ((toOpen || keyboardOpen))
                    {
                        RecenterMenu(rightHanded, keyboardOpen);
                    }
                    else
                    {
                        Rigidbody comp = menu.AddComponent(typeof(Rigidbody)) as Rigidbody;
                        if (rightHanded)
                        {
                            comp.velocity = GorillaLocomotion.Player.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                        }
                        else
                        {
                            comp.velocity = GorillaLocomotion.Player.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                        }

                        UnityEngine.Object.Destroy(menu, 2);
                        menu = null;

                        UnityEngine.Object.Destroy(reference);
                        reference = null;
                    }
                }
            }
            catch (Exception exc)
            {
                UnityEngine.Debug.LogError(string.Format("{0} // Error initializing at {1}: {2}", PluginInfo.Name, exc.StackTrace, exc.Message));
            }

            // Constant
            try
            {
                try
                {
                    if (TPC == null)
                    {
                        try
                        {
                            TPC = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera").GetComponent<Camera>();
                        }
                        catch
                        {
                            TPC = GameObject.Find("Shoulder Camera").GetComponent<Camera>();
                        }
                    }
                }
                catch { }
                // Pre-Execution
                if (fpsObject != null)
                {
                    fpsObject.text = "FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();
                }

                // Execute Enabled mods
                foreach (ButtonInfo[] buttonlist in buttons)
                {
                    foreach (ButtonInfo v in buttonlist)
                    {
                        if (v.enabled)
                        {
                            if (v.method != null)
                            {
                                try
                                {
                                    v.method.Invoke();
                                }
                                catch (Exception exc)
                                {
                                    UnityEngine.Debug.LogError(string.Format("{0} // Error with mod {1} at {2}: {3}", PluginInfo.Name, v.buttonText, exc.StackTrace, exc.Message));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                UnityEngine.Debug.LogError(string.Format("{0} // Error with executing mods at {1}: {2}", PluginInfo.Name, exc.StackTrace, exc.Message));
            }
        }
        public static Material loadimagefromurl(string url)
        {
            Material m;
            WebClient c = new WebClient();
            byte[] array = c.DownloadData(url);
            m = new Material(Shader.Find("GorillaTag/UberShader"));
            m.shaderKeywords = new string[]
            {
                   "_USE_TEXTURE"
            };
            string text = Application.dataPath;
            text = text.Replace("/Gorilla Tag_Data", "");
            Texture2D texture2D = new Texture2D(4096, 4096);
            ImageConversion.LoadImage(texture2D, array);
            m.mainTexture = texture2D;
            texture2D.Apply();
            return m;
        }
        // Functions
        public static Color32 Background = new Color32(4, 4, 4, 255);
        public static Color32 Button = new Color32(255, 0, 50, 255);
        private static GameObject titleObj = null;
        public static float j = 0f;
        public static float k = 0.2f;
        public static void CreateMenu()
        {
            // Menu Holder
            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
            menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * GorillaLocomotion.Player.Instance.scale;

            // Menu Background
            menuBackground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menuBackground.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menuBackground.GetComponent<BoxCollider>());
            menuBackground.transform.parent = menu.transform;
            menuBackground.transform.rotation = Quaternion.identity;
            menuBackground.transform.localScale = menuSize;
            menuBackground.GetComponent<Renderer>().material.color = Background;
            menuBackground.transform.position = new Vector3(0.05f, 0f, 0f);
            // Canvas
            canvasObject = new GameObject();
            canvasObject.transform.parent = menu.transform;
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScaler.dynamicPixelsPerUnit = 1000f;

            // Title and FPS
            Text text = new GameObject
            {
                transform =
                    {
                        parent = canvasObject.transform
                    }
            }.AddComponent<Text>();
            titleObj = new GameObject();
            titleObj.transform.parent = canvasObject.transform;
            titleObj.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            text.font = currentFont;
            text.fontSize = 1;
            text.text = "Advantage";
            text.color = textColors[0];
            text.supportRichText = true;
            text.fontStyle = FontStyle.Italic;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.15f, 0.025f);
            component.position = new Vector3(0.06f, 0f, 0.142f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            // Buttons
            // Disconnect

            //
            if (buttonsType == 1 || buttonsType == 2 || buttonsType == 3 || buttonsType == 4 || buttonsType == 5 || buttonsType == 6)
            {
                GameObject disconnectbutton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (!UnityInput.Current.GetKey(KeyCode.Q))
                {
                    disconnectbutton.layer = 2;
                }
                //me?
                UnityEngine.Object.Destroy(disconnectbutton.GetComponent<Rigidbody>());
                disconnectbutton.GetComponent<BoxCollider>().isTrigger = true;
                disconnectbutton.transform.parent = menu.transform;
                disconnectbutton.transform.rotation = Quaternion.identity;
                disconnectbutton.transform.localScale = new Vector3(0.02f, 0.9f, 0.08f);
                disconnectbutton.transform.localPosition = new Vector3(0.56f, 0f, -0.4455f);
                disconnectbutton.GetComponent<Renderer>().material.color = Button;
                disconnectbutton.AddComponent<Classes.Button>().relatedText = "Return";

                Text discontext = new GameObject
                {
                    transform =
                {
                    parent = canvasObject.transform
                }
                }.AddComponent<Text>();
                discontext.text = "Return";
                discontext.font = currentFont;
                discontext.fontSize = -1;
                discontext.color = textColors[0];
                discontext.alignment = TextAnchor.MiddleCenter;
                discontext.resizeTextForBestFit = true;
                discontext.resizeTextMinSize = 0;

                RectTransform rectt = discontext.GetComponent<RectTransform>();
                rectt.localPosition = Vector3.zero;
                rectt.sizeDelta = new Vector2(0.2f, 0.03f);
                rectt.localPosition = new Vector3(disconnectbutton.transform.position.x + 0.0046f, disconnectbutton.transform.position.y, disconnectbutton.transform.position.z);
                rectt.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

                GameObject RPCBUTTON = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (!UnityInput.Current.GetKey(KeyCode.Q))
                {
                    disconnectbutton.layer = 2;
                }
                //me?
                UnityEngine.Object.Destroy(RPCBUTTON.GetComponent<Rigidbody>());
                RPCBUTTON.GetComponent<BoxCollider>().isTrigger = true;
                RPCBUTTON.transform.parent = menu.transform;
                RPCBUTTON.transform.rotation = Quaternion.identity;
                RPCBUTTON.transform.localScale = new Vector3(0.02f, 0.4f, 0.08f);
                RPCBUTTON.transform.localPosition = new Vector3(0.56f, -0.30f, 0.4455f);
                RPCBUTTON.GetComponent<Renderer>().material.color = Button;
                RPCBUTTON.AddComponent<Classes.Button>().relatedText = "RPC";

                Text rpctext = new GameObject
                {
                    transform =
                {
                    parent = canvasObject.transform
                }
                }.AddComponent<Text>();
                rpctext.text = "FlushRPCS";
                rpctext.font = currentFont;
                rpctext.fontSize = -1;
                rpctext.color = textColors[0];
                rpctext.alignment = TextAnchor.MiddleCenter;
                rpctext.resizeTextForBestFit = true;
                rpctext.resizeTextMinSize = 0;

                RectTransform somecool = rpctext.GetComponent<RectTransform>();
                somecool.localPosition = Vector3.zero;
                somecool.sizeDelta = new Vector2(0.2f, 0.03f);
                somecool.localPosition = new Vector3(RPCBUTTON.transform.position.x + 0.0046f, RPCBUTTON.transform.position.y, RPCBUTTON.transform.position.z);
                somecool.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));


            }
            if ((ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f || UnityInput.Current.GetKeyDown(KeyCode.R)) && menu != null && Time.time - j >= k)
            {
                pageNumber--;
                if (pageNumber < 0)
                    pageNumber = (buttons[buttonsType].Length + buttonsPerPage - 1) / buttonsPerPage - 1;
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, true, 0.4f);
                RecreateMenu();
                j = Time.time;
            }
            if ((ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f || UnityInput.Current.GetKeyDown(KeyCode.T)) && menu != null && Time.time - j >= k)
            {
                pageNumber++;
                if (pageNumber > (buttons[buttonsType].Length + buttonsPerPage - 1) / buttonsPerPage - 1)
                    pageNumber = 0;
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.4f);
                RecreateMenu();
                j = Time.time;
            }

            // Mod Buttons
            ButtonInfo[] activeButtons = buttons[buttonsType].Skip(pageNumber * buttonsPerPage).Take(buttonsPerPage).ToArray();
            for (int i = 0; i < activeButtons.Length; i++)
            {
                CreateButton(i * 0.1f, activeButtons[i]);
            }
        }

        public static void CreateButton(float offset, ButtonInfo method)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (!UnityInput.Current.GetKey(KeyCode.Q))
            {
                gameObject.layer = 2;
            }
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.02f, 0.1f, 0.08f);
            gameObject.transform.localPosition = new Vector3(0.56f, 0.4f, 0.28f - offset);
            gameObject.AddComponent<Classes.Button>().relatedText = method.buttonText;
            ColorChanger colorChanger = gameObject.AddComponent<ColorChanger>();
            if (method.enabled)
            {
                colorChanger.colorInfo = buttonColors[1];
            }
            else
            {
                colorChanger.colorInfo = buttonColors[0];
            }
            colorChanger.Start();
            Text text = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            text.font = currentFont;
            text.text = method.buttonText;
            if (method.overlapText != null)
            {
                text.text = method.overlapText;
            }
            text.supportRichText = true;
            text.fontSize = -1;
            if (method.enabled)
            {
                text.color = textColors[1];
            }
            else
            {
                text.color = textColors[0];
            }
            text.alignment = TextAnchor.MiddleCenter;
            text.fontStyle = FontStyle.Italic;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(.2f, .03f);
            component.localPosition = new Vector3(.064f, 0, .111f - offset / 2.6f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        public static void RecreateMenu()
        {
            if (menu != null)
            {
                UnityEngine.Object.Destroy(menu);
                menu = null;

                CreateMenu();
                RecenterMenu(rightHanded, UnityInput.Current.GetKey(keyboardButton));
            }
        }

        public static void RecenterMenu(bool isRightHanded, bool isKeyboardCondition)
        {
            if (!isKeyboardCondition)
            {
                if (!isRightHanded)
                {
                    menu.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    menu.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                }
                else
                {
                    menu.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    Vector3 rotation = GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles;
                    rotation += new Vector3(0f, 0f, 180f);
                    menu.transform.rotation = Quaternion.Euler(rotation);
                }
            }
            else
            {
                try
                {
                    TPC = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera").GetComponent<Camera>();
                }
                catch { }
                if (TPC != null)
                {
                    TPC.transform.position = new Vector3(-63.245f, 3.8434f, - 63.2187f);
                    TPC.transform.rotation = Quaternion.identity;
                    GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    bg.transform.localScale = new Vector3(10f, 10f, 0.01f);
                    bg.transform.transform.position = TPC.transform.position + TPC.transform.forward;
                    bg.GetComponent<Renderer>().material.color = Color.clear;
                    GameObject.Destroy(bg, Time.deltaTime);
                    menu.transform.parent = TPC.transform;
                    menu.transform.position = (TPC.transform.position + (Vector3.Scale(TPC.transform.forward, new Vector3(0.5f, 0.5f, 0.5f)))) + (Vector3.Scale(TPC.transform.up, new Vector3(-0.02f, -0.02f, -0.02f)));
                    Vector3 rot = TPC.transform.rotation.eulerAngles;
                    rot = new Vector3(rot.x - 90, rot.y + 90, rot.z);
                    menu.transform.rotation = Quaternion.Euler(rot);

                    if (reference != null)
                    {
                        if (Mouse.current.leftButton.isPressed)
                        {
                            Ray ray = TPC.ScreenPointToRay(Mouse.current.position.ReadValue());
                            RaycastHit hit;
                            bool worked = Physics.Raycast(ray, out hit, 100);
                            if (worked)
                            {
                                Classes.Button collide = hit.transform.gameObject.GetComponent<Classes.Button>();
                                if (collide != null)
                                {
                                    collide.OnTriggerEnter(buttonCollider);
                                }
                            }
                        }//kys
                        else
                        {
                            reference.transform.position = new Vector3(999f, -999f, -999f);
                        }
                    }
                }
            }
        }

        public static void CreateReference(bool isRightHanded)
        {
            reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (isRightHanded)
            {
                reference.transform.parent = GorillaTagger.Instance.leftHandTransform;
            }
            else
            {
                reference.transform.parent = GorillaTagger.Instance.rightHandTransform;
            }
            reference.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
            reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            buttonCollider = reference.GetComponent<SphereCollider>();

            ColorChanger colorChanger = reference.AddComponent<ColorChanger>();
            colorChanger.colorInfo = backgroundColor;
            colorChanger.Start();
        }

        public static void Toggle(string buttonText)
        {
            int lastPage = ((buttons[buttonsType].Length + buttonsPerPage - 1) / buttonsPerPage) - 1;
            if (buttonText == "PreviousPage")
            {
                pageNumber--;
                if (pageNumber < 0)
                {
                    pageNumber = lastPage;
                }
            }
            else
            {
                if (buttonText == "NextPage")
                {
                    pageNumber++;
                    if (pageNumber > lastPage)
                    {
                        pageNumber = 0;
                    }
                }
                if (buttonText == "Return")
                {
                    Global.ReturnHome();
                }
                if (buttonText == "Disconnect")
                {
                    PhotonNetwork.Disconnect();
                }
                if(buttonText == "RPC")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        FlushRPCS();
                    }
                    NotifiLib.SendNotification($"RPC Flushed : {RpcInfo.FromLocal(PhotonNetwork.LocalPlayer.Get<NetworkRunner>(), RpcChannel.Reliable, RpcHostMode.SourceIsHostPlayer).ToString()}");
                }
                else
                {
                    ButtonInfo target = GetIndex(buttonText);
                    if (target != null)
                    {
                        if (target.isTogglable)
                        {
                            target.enabled = !target.enabled;
                            if (target.enabled)
                            {
                                if (target.enableMethod != null)
                                {
                                    try { target.enableMethod.Invoke(); } catch { }
                                }
                            }
                            else
                            {
                                if (target.disableMethod != null)
                                {
                                    try { target.disableMethod.Invoke(); } catch { }
                                }
                            }
                        }
                        else
                        {
                            if (target.method != null)
                            {
                                try { target.method.Invoke(); } catch { }
                            }
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError(buttonText + " does not exist");
                    }
                }
            }
            RecreateMenu();
        }

        public static GradientColorKey[] GetSolidGradient(Color color)
        {
            return new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
        }

        public static ButtonInfo GetIndex(string buttonText)
        {
            foreach (ButtonInfo[] buttons in Menu.Buttons.buttons)
            {
                foreach (ButtonInfo button in buttons)
                {
                    if (button.buttonText == buttonText)
                    {
                        return button;
                    }
                }
            }

            return null;
        }
        public static GameObject menu;
        public static GameObject menuBackground;
        public static GameObject reference;
        public static GameObject canvasObject;
        public static SphereCollider buttonCollider;
        public static Camera TPC;
        public static Text fpsObject;
        public static int pageNumber = 0;
        public static int buttonsType = 0;
    }
}