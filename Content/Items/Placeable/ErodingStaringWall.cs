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
    public class ErodingStaringWall: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<Walls.ErodingStaringWall>());
      
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
            // 墓地工作台 流动石块1:4
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<FlowingStone>(), 1).
                AddTile(TileID.WorkBenches).
                AddCondition(Condition.InGraveyard).
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
