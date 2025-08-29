#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable
{
    public class BlackIceBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            //ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<BlackSnow>();
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.createTile = ModContent.TileType<Tiles.BlackIce>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
#if DEBUG
            CreateRecipe(100).
                AddIngredient(ItemID.DirtBlock,1).
                Register();
#endif
        }
    }
}
