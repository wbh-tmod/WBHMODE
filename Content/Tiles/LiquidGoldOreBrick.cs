using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Projectiles;


namespace WBHMODE.Content.Tiles
{
    public class LiquidGoldOreBrick : ModTile
    {
        public override void SetStaticDefaults () { 
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;

            TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
            TileID.Sets.GeneralPlacementTiles[Type] = false;
            TileID.Sets.ChecksForMerge[Type] = true;
            DustType = DustID.Stone;
            AddMapEntry(new Color(150, 150, 150));
        }
        public override bool HasWalkDust()
        {
            return Main.rand.NextBool(3);
        }
        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = DustID.Stone;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.3f;
            g = 0.3f;
            b = 0.3f;
        }
        //未做：较低频率生成粒子。粒子发出微弱的光，不受重力影响，初速度为0，有两种颜色，一种为白色，一种与物块颜色相同
    }
}
