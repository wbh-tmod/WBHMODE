#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using WBHMODE.Common.Systems;

namespace WBHMODE.Content.Biomes
{
    public class ErodedLandSurfaceBiome : ModBiome
    {
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<ErodedLandWaterStyle>(); // Sets a water style for when inside this biome
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<ErodedLandSurfaceBackgroundStyle>();
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/ErodedLandMusic");

        public override bool IsBiomeActive(Player player)
        {
            bool b1 = ModContent.GetInstance<ErodedLandBiomeTileCounterSystem>().ErodedLandTiles >= 300;
#if DEBUG
            b1 = ModContent.GetInstance<ErodedLandBiomeTileCounterSystem>().ErodedLandTiles >= 20;
#endif
            // b2 没写
            bool b3 = player.ZoneSkyHeight || player.ZoneOverworldHeight;

            return b1 && b3;
        }
    }
}
