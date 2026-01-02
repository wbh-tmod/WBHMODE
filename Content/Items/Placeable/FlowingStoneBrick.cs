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

namespace WBHMODE.Content.Items.Placeable
{
    public class FlowingStoneBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FlowingStoneBrick>());
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
#if DEBUG
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 合成配方
            recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.Register();
#endif
            // 熔炉 流动石块2:1流动石砖
            CreateRecipe().
                AddIngredient(ModContent.ItemType<FlowingStone>(), 2).
                AddTile(TileID.Furnaces).
                Register();
            // 工作台 流动石墙4:1流动石砖
            CreateRecipe().
                AddIngredient(ModContent.ItemType<FlowingStoneBrickWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
        //public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        //{ // Calls upon use of an extractinator. Below is the chance you will get ExampleOre from the extractinator.
        //if (Main.rand.NextBool(3))
        //{
        //    resultType = ModContent.ItemType<ExampleOre>();  // Get this from the extractinator with a 1 in 3 chance.
        //    if (Main.rand.NextBool(5))
        //    {
        //        resultStack += Main.rand.Next(2); // Add a chance to get more than one of ExampleOre from the extractinator.
        //    }
        //}
        //}
    }
}
