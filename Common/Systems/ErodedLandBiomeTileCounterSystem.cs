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

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            ErodedLandTiles = tileCounts[ModContent.TileType<FlowingStone>()]; // 流动石相超过300
        }
    }
}
