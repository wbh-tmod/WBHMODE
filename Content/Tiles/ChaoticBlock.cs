using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Projectiles;

namespace WBHMODE.Content.Tiles
{
    public class ChaoticBlock : ModTile
    {
        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = DustID.SnowBlock;
            AddMapEntry(new Color(150, 150, 150));
        }
    }
}
