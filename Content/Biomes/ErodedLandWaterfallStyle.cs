using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace WBHMODE.Content.Biomes
{
    public class ErodedLandWaterfallStyle : ModWaterfallStyle
    {
        // Makes the waterfall provide light
        // Learn how to make a waterfall: https://terraria.wiki.gg/wiki/Waterfall
        public override void AddLight(int i, int j) =>
            Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), Color.White.ToVector3() * 0.5f);
    }
}
