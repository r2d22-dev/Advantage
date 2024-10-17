using UnityEngine;
using static Avantage.Menu.Main;
using static Avantage.Menu.Settings;

namespace Avantage.Mods
{
    internal class SettingsMods
    {
        public static void DisableTOSShit()
        {
            GameObject root = GameObject.Find("Miscellaneous Scripts/PrivateUIRoom/Root");
            if (root != null) root.SetActive(false);
            GameObject root2 = GameObject.Find("Miscellaneous Scripts/PrivateUIRoom/root");
            if (root2 != null) root2.SetActive(false);
            GameObject Geode = GameObject.Find("Miscellaneous Scripts/PrivateUIRoom/ReportOccluder/Geode");
            if (Geode != null) Geode.SetActive(false);
            GameObject Canvas = GameObject.Find("Miscellaneous Scripts/MetaReporting/Canvas");
            if (Canvas != null) Canvas.SetActive(false);
            GameObject Geode2 = GameObject.Find("Miscellaneous Scripts/MetaReporting/ReportOccluder/Geode");
            if (Geode2 != null) Geode2.SetActive(false);  
        }

        public static void EnableTOSShit()
        {
            GameObject root = GameObject.Find("Miscellaneous Scripts/PrivateUIRoom/Root");
            if (root != null) root.SetActive(true);
            GameObject root2 = GameObject.Find("Miscellaneous Scripts/PrivateUIRoom/root");
            if (root2 != null) root2.SetActive(true);
            GameObject Geode = GameObject.Find("Miscellaneous Scripts/PrivateUIRoom/ReportOccluder/Geode");
            if (Geode != null) Geode.SetActive(true);
            GameObject Canvas = GameObject.Find("Miscellaneous Scripts/MetaReporting/Canvas");
            if (Canvas != null) Canvas.SetActive(true);
            GameObject Geode2 = GameObject.Find("Miscellaneous Scripts/MetaReporting/ReportOccluder/Geode");
            if (Geode2 != null) Geode2.SetActive(true);
        }
    }
}
