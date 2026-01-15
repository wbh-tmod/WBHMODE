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
    public class SculptureWoodWall: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<Walls.SculptureWoodWall>());
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            // 工作台 1雕塑木 -> 4雕塑木墙
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<SculptureWood>(), 1).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
