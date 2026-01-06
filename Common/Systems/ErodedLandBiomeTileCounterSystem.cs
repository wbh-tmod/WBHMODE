using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using WBHMODE.Content.Tiles;
namespace WBHMODE.Common.Systems
{
    public class ErodedLandBiomeTileCounterSystem : ModSystem
    {
        public int ErodedLandTiles = 0; // 消蚀之地

        public override void ResetNearbyTileEffects()
        {
            ErodedLandTiles = 0;
        }

        /*
        计数玩家附近的邪恶群系方块数
        在Content/Biomes/ErodedLandBiome.cs/IsBiomeActive中调用 
        计入方块：流动石块、流动沙块、硬化流动沙块、流动沙岩块、黑冰雪块
        尚未添加：多刺灌木、草、丛林草
        可选添加：腐化植物（猩红不计入、腐化计入）
        */
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            ErodedLandTiles = tileCounts[ModContent.TileType<FlowingStone>()] 
                + tileCounts[ModContent.TileType<FlowingSand>()] 
                + tileCounts[ModContent.TileType<HardenedFlowingSand>()]
                + tileCounts[ModContent.TileType<FlowingSandstone>()]
                + tileCounts[ModContent.TileType<BlackIce>()]; // 
        }
    }
}
