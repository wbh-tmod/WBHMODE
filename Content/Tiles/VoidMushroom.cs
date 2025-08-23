using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace WBHMODE.Content.Tiles
{
    public class VoidMushroom : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileCut[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;

            //DustType = ModContent.DustType<Sparkle>();

            AddMapEntry(new Color(200, 255, 255));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        //public override void ChangeWaterfallStyle(ref int style)
        //{
        //    style = ModContent.GetInstance<ExampleWaterfallStyle>().Slot;
        //}
    }
}
