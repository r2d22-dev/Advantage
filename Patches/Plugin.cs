using BepInEx;
using System.ComponentModel;

namespace Avantage.Patches
{
    [Description(Avantage.PluginInfo.Description)]
    [BepInPlugin(Avantage.PluginInfo.GUID, Avantage.PluginInfo.Name, Avantage.PluginInfo.Version)]
    public class HarmonyPatches : BaseUnityPlugin
    {
        private void OnEnable()
        {
            Menu.ApplyHarmonyPatches();
        }

        private void OnDisable()
        {
            Menu.RemoveHarmonyPatches();
        }
    }
}
