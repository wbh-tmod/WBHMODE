//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WBHMODE.Content.Items.Placeable.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace WBHMODE.Content.Walls
{
    public class ErodingHandsWall: ModWall
    {
        public override void SetStaticDefaults()
        {
            DustType = DustID.Stone;
            AddMapEntry(new Color(170, 170, 170));
        }
    }
}
