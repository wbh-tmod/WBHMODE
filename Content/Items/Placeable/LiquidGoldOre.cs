#define DEBUG

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
    public class LiquidGoldOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;

            // This ore can spawn in slime bodies like other pre-boss ores. (copper, tin, iron, etch)
            // It will drop in amount from 3 to 13.
            ItemID.Sets.OreDropsFromSlime[Type] = (3, 13);
        }
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LiquidGoldOre>());
            Item.width = 12;
            Item.height = 12;
            Item.value = 3000;
        }
        public override void AddRecipes() {
            // 魔矿1:1
            CreateRecipe().
                AddIngredient(ItemID.DemoniteOre, 1).
                AddTile(TileID.ChlorophyteExtractinator).
                Register();
            // 1:1魔矿
            CreateRecipe().
                AddIngredient(ModContent.ItemType<LiquidGoldOre>(), 1).
                AddTile(TileID.ChlorophyteExtractinator).
                Register().
                ReplaceResult(ItemID.DemoniteOre, 1);
            // 血腥矿1:1
            CreateRecipe().
                AddIngredient(ItemID.CrimtaneOre, 1).
                AddTile(TileID.ChlorophyteExtractinator).
                Register();
            // 1:1血腥矿
            CreateRecipe().
                AddIngredient(ModContent.ItemType<LiquidGoldOre>(), 1).
                AddTile(TileID.ChlorophyteExtractinator).
                Register().
                ReplaceResult(ItemID.CrimtaneOre, 1);
            // 鹿华
            CreateRecipe().
                AddIngredient(ModContent.ItemType<LiquidGoldOre>(), 5).
                AddIngredient(ItemID.FlinxFur, 3).
                AddIngredient(ItemID.Lens, 1).
                AddTile(TileID.DemonAltar).
                Register().
                ReplaceResult(ItemID.DeerThing, 1);

            Recipe recipe = CreateRecipe();
#if DEBUG
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 合成配方
            recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.ReplaceResult(ModContent.ItemType<LiquidGoldOre>(), 100);
            recipe.Register();

            //recipe = CreateRecipe();
            //recipe.AddIngredient(ItemID.DemoniteOre, 1); // 合成配方
            //recipe.AddTile(TileID.WorkBenches); // 合成台
            //recipe.ReplaceResult(ModContent.ItemType<LiquidGoldOre>(), 100);
            //recipe.Register();

            //recipe = CreateRecipe();
            //recipe.AddIngredient(ModContent.ItemType<LiquidGoldOre>(), 1); // 合成配方
            //recipe.AddTile(TileID.WorkBenches); // 合成台
            //recipe.ReplaceResult(ItemID.DemoniteOre, 100);
            //recipe.Register();
#endif
        }
    }
}
