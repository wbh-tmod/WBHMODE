#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using WBHMODE.Common.Systems;
using WBHMODE.Content.Items.Placeable;
using Microsoft.Xna.Framework;

namespace WBHMODE.Content.Biomes
{
    public class ErodedLandSurfaceBiome : ModBiome
    {
        // 设置该生物群落的水风格（自定义的ErodedLandWaterStyle）
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<ErodedLandWaterStyle>(); // Sets a water style for when inside this biome
        // 设置该生物群落的地表背景风格
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<ErodedLandSurfaceBackgroundStyle>();
        // 设置截图时的瓷砖颜色风格（这里用了血腥之地的风格）
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;
        // 设置该生物群落的背景音乐（加载模组内Assets/Music路径下的ErodedLandMusic）
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/ErodedLandMusic");

        public override int BiomeTorchItemType => ModContent.ItemType<ErodingTorch>();
        public override int BiomeCampfireItemType => ModContent.ItemType<ErodingCampfire>();

        // Populate the Bestiary Filter
        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;
        public override string MapBackground => BackgroundPath; // Re-uses Bestiary Background for Map Background

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
        //高优先级
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
    }
}
