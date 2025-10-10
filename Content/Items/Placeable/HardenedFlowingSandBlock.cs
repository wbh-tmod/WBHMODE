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
    public class HardenedFlowingSandBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.HardenedFlowingSand>());
            Item.width = 12;
            Item.height = 12;
            Item.ammo = AmmoID.Sand;
        }
        public override void AddRecipes()
        {
#if DEBUG
            CreateRecipe(100).
                AddIngredient(ItemID.DirtBlock, 1).
                AddTile(TileID.WorkBenches).
                Register();
#endif
            // 硬化流动沙块-硬化沙块
            CreateRecipe().
                AddIngredient(ModContent.ItemType<HardenedFlowingSandBlock>(), 1).
                AddTile(TileID.ChlorophyteExtractinator).
                Register().
                ReplaceResult(ItemID.HardenedSand);
            // 硬化流动沙块-沙漠火把
            CreateRecipe(3).
                AddIngredient(ModContent.ItemType<HardenedFlowingSandBlock>(), 1).
                AddIngredient(ItemID.Torch, 3).
                Register().
                ReplaceResult(ItemID.DesertTorch);
        }
    }
}
