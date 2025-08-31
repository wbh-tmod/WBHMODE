//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using WBHMODE.Content.Tiles;

namespace WBHMODE.Content.Items.Materials
{
    public class WailingRock: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 69; // Hellstone
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(copper: 100); // 售价
            Item.rare = ItemRarityID.White;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.consumable = false;
        }
        public override void AddRecipes()
        {
            return;
        }
    }
}
