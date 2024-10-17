
using Avantage.Classes;
using Avantage.Mods;
using static Avantage.Menu.Settings;

namespace Avantage.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // Main Mods
                new ButtonInfo { buttonText = "Settings", method =() => Main.buttonsType = 1, isTogglable = false},
                new ButtonInfo { buttonText = "OP", method =() => Main.buttonsType = 2, isTogglable = false},
                new ButtonInfo { buttonText = "Fun", method =() => Main.buttonsType = 3, isTogglable = false},
                new ButtonInfo { buttonText = "Movement", method =() => Main.buttonsType = 4, isTogglable = false},
                new ButtonInfo { buttonText = "Enviorment", method =() => Main.buttonsType = 5, isTogglable = false},
            },

            new ButtonInfo[] { // Settings
                new ButtonInfo { buttonText = "Right Hand", enableMethod =() => rightHanded = true, disableMethod =() => rightHanded = false},
                new ButtonInfo { buttonText = "Change Theme", enableMethod =() => Settings.ChangeTheme(), isTogglable = false},
                new ButtonInfo { buttonText = "AntiReport[OP]", enableMethod =() => Exploits.OpAntiReport(), enabled = true},
                new ButtonInfo { buttonText = "FirstPerson", enableMethod =() => Fun.FirstPerson(), enabled = true},
                new ButtonInfo { buttonText = "FPS Counter", enableMethod =() => fpsCounter = true, disableMethod =() => fpsCounter = false},
                new ButtonInfo { buttonText = "Toggle TOS [ONE TIME]", enableMethod =() => SettingsMods.DisableTOSShit(), disableMethod =() => SettingsMods.EnableTOSShit(), enabled = true},
            },

            new ButtonInfo[] { // OP
                new ButtonInfo { buttonText = "AntiBan[Dont use]", method =() => Exploits.AntiBan()},
                new ButtonInfo { buttonText = "BreakGamemode", method =() => Exploits.BreakGamemode()},
                new ButtonInfo { buttonText = "Lag Gun[UND]", method =() => Exploits.LagGun()},
                new ButtonInfo { buttonText = "Lag All[NW]", method =() => Exploits.LagAll()},
                new ButtonInfo { buttonText = "Kick Gun[UND]", method =() => Exploits.hsbdhnhsdnh()},
                new ButtonInfo { buttonText = "Kick All[UND]", method =() => Exploits.KickAll()},
                new ButtonInfo { buttonText = "Tag Gun", method =() => Exploits.TagGun(), disableMethod =()=> GorillaTagger.Instance.offlineVRRig.enabled = true},
                new ButtonInfo { buttonText = "Tag All", method =() => Exploits.TagAll(), disableMethod =()=> GorillaTagger.Instance.offlineVRRig.enabled = true},
                new ButtonInfo { buttonText = "SpazzCosmetics", method =() => Exploits.SpazzCosmetics()},
                new ButtonInfo { buttonText = "AllowCosmeticsOutsideofTryon", enableMethod =() => Exploits.AllowCosmeticsOutsideofTryon(), disableMethod =() => Exploits.RemoveTryonCosmetics(), isTogglable = false},
                new ButtonInfo { buttonText = "Lucy Grab Gun", method =() => Exploits.LucyPickUpGun()},
                new ButtonInfo { buttonText = "Lucy Grab All", method =() => Exploits.LucyPickUpAll()},
                new ButtonInfo { buttonText = "SpawnLucy", method =() => Exploits.SpawnLucy()},
                new ButtonInfo { buttonText = "DespawnLucy", method =() => Exploits.DespawnLucy()},
            },

            new ButtonInfo[] { // Fun
                new ButtonInfo { buttonText = "WaterHands[Player]", method =() => Fun.WaterHands()},
                new ButtonInfo { buttonText = "WaterGun", method =() => Fun.WaterGun()},
                new ButtonInfo { buttonText = "RandomSoundSpam", method =() => Fun.RandomSoundSpam()},
                new ButtonInfo { buttonText = "BassSoundSpam", method =() => Fun.BassSoundSpam()},
                new ButtonInfo { buttonText = "MetalSoundSpam", method =() => Fun.MetalSoundSpam()},
                new ButtonInfo { buttonText = "WolfSoundSpam", method =() => Fun.WolfSoundSpam()},
                new ButtonInfo { buttonText = "CatSoundSpam", method =() => Fun.CatSoundSpam()},
                new ButtonInfo { buttonText = "TurkeySoundSpam", method =() => Fun.TurkeySoundSpam()},
                new ButtonInfo { buttonText = "FrogSoundSpam", method =() => Fun.FrogSoundSpam()},
                new ButtonInfo { buttonText = "BeeSoundSpam", method =() => Fun.BeeSoundSpam()},
                new ButtonInfo { buttonText = "EarrapeSoundSpam", method =() => Fun.EarrapeSoundSpam()},
                new ButtonInfo { buttonText = "DingSoundSpam", method =() => Fun.DingSoundSpam()},
                new ButtonInfo { buttonText = "CrystalSoundSpam", method =() => Fun.CrystalSoundSpam()},
                new ButtonInfo { buttonText = "BigCrystalSoundSpam", method =() => Fun.BigCrystalSoundSpam()},
                new ButtonInfo { buttonText = "PanSoundSpam", method =() => Fun.PanSoundSpam()},
                new ButtonInfo { buttonText = "AK47SoundSpam", method =() => Fun.AK47SoundSpam()},
                new ButtonInfo { buttonText = "SqueakSoundSpam", method =() => Fun.SqueakSoundSpam()},
                new ButtonInfo { buttonText = "SirenSoundSpam", method =() => Fun.SirenSoundSpam()},
                new ButtonInfo { buttonText = "Jump Once", method =() => Movement.Jump(), isTogglable = false},

            },
            new ButtonInfo[] { // Movement
                new ButtonInfo { buttonText = "Fly", method =() => Movement.Fly()},
                new ButtonInfo { buttonText = "Fly[T]", method =() => Movement.TriggerFly()},
                new ButtonInfo { buttonText = "Spider Man", method =() => Movement.SpiderMan()},
            },

            new ButtonInfo[] { // Enviorment
                new ButtonInfo { buttonText = "Redoing Entire Catagory", method =() => Exploits.EnableFortuneTeller()},
            },
        };
    }
}
