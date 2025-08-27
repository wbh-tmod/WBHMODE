//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Items.Placeable;
using WBHMODE.Content.Tiles;

namespace WBHMODE.Content.Items.Materials
{
    public class LiquidGoldBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 69; // Hellstone
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.createTile = ModContent.TileType<LiquidGoldBarTile>();
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 33); // 售价
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<LiquidGoldBar>(), 1).
                AddTile(TileID.Furnaces).
                Register();
#if DEBUG
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 3); // 合成配方
            recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.ReplaceResult(ModContent.ItemType<LiquidGoldBar>(), 100);
            recipe.Register();
#endif

        }
    }
}
