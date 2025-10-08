#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Materials
{
    public class Neuro : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(copper: 2); // 售价
            Item.rare = ItemRarityID.White;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.consumable = false;
        }
        public override void AddRecipes()
        {
#if DEBUG
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 合成配方
            recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.ReplaceResult(ModContent.ItemType<Neuro>(), 100);
            recipe.Register();
#endif
            return;
        }
    }
}
