using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;
using BepInEx;

namespace Avantage.Menu
{
    [BepInPlugin("siGMA.Console", "Console", "1.0")]
    public class GUI_Console : BaseUnityPlugin
    {
        // Token: 0x060000D0 RID: 208
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        // Token: 0x060000D1 RID: 209
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Token: 0x060000D2 RID: 210
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        // Token: 0x060000D3 RID: 211
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        private void Awake()
        {
            GUI_Console.AllocConsole();
            console = GUI_Console.GetConsoleWindow();
            GUI_Console.ShowWindow(console, 5);
            writer = new StreamWriter(Console.OpenStandardOutput());
            writer.AutoFlush = true;
            Console.Title = "Moon Client On Top";
            Console.SetOut(writer);
            Logo();
        }

        public void Logo()
        {
            GUI_Console.Log("  .     '     ,\r\n    _________\r\n _ /_|_____|_\\ _\r\n   '. \\   / .'\r\n     '.\\ /.'\r\n       '.'");
            try
            {
                PluginInfo versionJSON = JsonConvert.DeserializeObject<PluginInfo[]>(new WebClient().DownloadString(""))[0];
                if (int.Parse(versionJSON.Minor) > 1 || int.Parse(versionJSON.Major) > 2 || int.Parse(versionJSON.Revisions) > 5)
                {
                    GUI_Console.Log(string.Concat(new string[]
                    {
                        "Moon Client Version: ",
                        2.ToString(),
                        ".",
                        5.ToString(),
                        ".",
                        1.ToString(),
                        " Update Available: TRUE"
                    }));
                }
                else
                {
                    GUI_Console.Log(string.Concat(new string[]
                    {
                        "Moon Client Version: ",
                        2.ToString(),
                        ".",
                        5.ToString(),
                        ".",
                        1.ToString(),
                        " Update Available: FALSE"
                    }));
                }
            }
            catch (Exception ex)
            {
                GUI_Console.Log(ex.ToString());
            }
        }

        public static void Log(object message)
        {
            Console.WriteLine(message);
        }

        public static void LogERR(object message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void LogError(object message)
        {
        }

        private IntPtr console;

        private StreamWriter writer;
    }
}


