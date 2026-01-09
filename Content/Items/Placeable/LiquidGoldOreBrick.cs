//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Items.Materials;

namespace WBHMODE.Content.Items.Placeable
{
    public class LiquidGoldOreBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
            Item.value = Item.sellPrice(0); // 售价
        }
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LiquidGoldOreBrick>());
            Item.width = 12;
            Item.height = 12;
        }
        public override void AddRecipes() {
            // 熔炉 1液金矿 + 5流动石块 -> 5液金矿砖
            CreateRecipe(5).
                AddIngredient(ModContent.ItemType<LiquidGoldOre>(), 1).
                AddIngredient(ModContent.ItemType<FlowingStone>(), 5).
                AddTile(TileID.Furnaces).
                Register();
            // 工作台 4液金矿砖墙 -> 1液金矿砖
            CreateRecipe().
                AddIngredient(ModContent.ItemType<LiquidGoldOreBrickWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
