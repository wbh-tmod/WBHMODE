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
    public class FlowingSand : ModTile
    {
        public override void SetStaticDefaults () { 
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;

            // Sand specific properties
            Main.tileSand[Type] = true;
            TileID.Sets.Conversion.Sand[Type] = true; // Allows Clentaminator solutions to convert this tile to their respective Sand tiles.
            TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true; // Allows Sandshark enemies to "swim" in this sand.
            TileID.Sets.CanBeDugByShovel[Type] = true;
            TileID.Sets.Falling[Type] = true;
            TileID.Sets.Suffocate[Type] = true;
            TileID.Sets.FallingBlockProjectile[Type] = new TileID.Sets.FallingBlockProjectileInfo(ModContent.ProjectileType<FlowingSandBallFallingProjectile>(), 10); // Tells which falling projectile to spawn when the tile should fall.
            
            TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
            TileID.Sets.GeneralPlacementTiles[Type] = false;
            TileID.Sets.ChecksForMerge[Type] = true;

            MineResist = 0.5f; // Sand tile typically require half as many hits to mine.
            DustType = DustID.Stone;
            AddMapEntry(new Color(150, 150, 150));
        }
        public override bool HasWalkDust()
        {
            return Main.rand.NextBool(3);
        }
        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = DustID.Sand;
        }
    }
}
