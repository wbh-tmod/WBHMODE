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
    public class ErodedLandBiome : ModBiome
    {
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<ErodedLandWaterStyle>(); // Sets a water style for when inside this biome
        //public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<ErodedSurfaceBackgroundStyle>();
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/ErodedLandMusic");

        public override bool IsBiomeActive(Player player)
        {
            bool b1 = ModContent.GetInstance<ErodedLandBiomeTileCounterSystem>().ErodedLandTiles >= 20;
            // b2 没写
            bool b3 = player.ZoneSkyHeight || player.ZoneOverworldHeight;

            return b1 && b3;
        }
    }
}
