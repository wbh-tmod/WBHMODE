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
#if DEBUG
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 合成配方
            recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.ReplaceResult(ModContent.ItemType<LiquidGoldOre>(), 100);
            recipe.Register();
#endif
        }
    }
}
