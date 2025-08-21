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
    public class FlowingSandBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;

            // Set the SandgunAmmoProjectileData to your sandgun projectile with a bonus damage of 10
            ItemID.Sets.SandgunAmmoProjectileData[Type] = new(ModContent.ProjectileType<Projectiles.FlowingSandBallGunProjectile>(), 10);
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FlowingSand>());
            Item.width = 12;
            Item.height = 12;
            Item.ammo = AmmoID.Sand;
            // Item.shoot and Item.damage are not used for sand ammo by convention. They would result in undesireable item tooltips.
            // ItemID.Sets.SandgunAmmoProjectileData is used instead.
            Item.notAmmo = true;
        }
        public override void AddRecipes()
        {
            //CreateRecipe(10)
            //    .AddIngredient<ExampleItem>()
            //    .AddTile<Tiles.Furniture.ExampleWorkbench>()
            //    .Register();
#if DEBUG
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 合成配方
            recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.Register();
#endif
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FlowingSandBlock>(), 1);
            recipe.AddTile(TileID.ChlorophyteExtractinator);
            recipe.ReplaceResult(ItemID.SandBlock, 1);
            recipe.Register();
        }
    }
}
