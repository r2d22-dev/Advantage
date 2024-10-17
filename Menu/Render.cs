using System;
using System.Collections.Generic;
using System.Text;
using ExitGames.Client.Photon.StructWrapping;
using ImGuiNET;
using UnityEngine;

namespace Avantage.Menu
{
    public class somthingcool : MonoBehaviour
    {
        public static float sliderVal = 0;
        public static int inputval = 50;
        public static bool checkboxval = false;
        public static System.Numerics.Vector4 colorval = new System.Numerics.Vector4(1,1,1,1);

        public static void Spacethenigger()
        {
            int i = colorval.Get<int>();
            ImGui.Begin("Sigma GUI");
            ImGui.TextColored(colorval, "Sigma");
            ImGui.SliderFloat("slider", ref sliderVal, 0, 15);
            ImGui.Checkbox("space is a nigger", ref checkboxval);
            ImGui.InputInt("color", ref i);
            ImGui.End();
               
        }
    }
}
