using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using WBHMODE.Content.Biomes;

namespace WBHMODE.Common
{
    public static class ErodedLandCondition
    {
        public static Condition InErodedLandBiome = new Condition("Mods.WBHMODE.Conditions.InErodedLandBiome", () => Main.LocalPlayer.InModBiome<ErodedLandSurfaceBiome>() /*|| Main.LocalPlayer.InModBiome<ErodedLandUndergroundBiome>()*/);

    }
}
