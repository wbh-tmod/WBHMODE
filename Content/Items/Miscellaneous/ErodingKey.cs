#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Miscellaneous
{
    public class ErodingKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(copper: 0); // 售价
            Item.rare = ItemRarityID.Yellow;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.consumable = false;
        }
    }
}
