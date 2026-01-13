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
    public class SculptureWood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Wood;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SculptureWood>());
            Item.width = 12;
            Item.height = 12;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            // 工作台 4雕塑木墙 -> 1雕塑木
            CreateRecipe()
                .AddIngredient<SculptureWoodWall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();
            // 工作台 4雕塑木栅栏 -> 1雕塑木
            CreateRecipe()
                .AddIngredient<SculptureWoodFence>(4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
