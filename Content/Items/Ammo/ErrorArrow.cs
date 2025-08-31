#define DEBUG

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Items.Materials;
using WBHMODE.Content.Projectiles;

namespace WBHMODE.Content.Items.Ammo
{
    public class ErrorArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 36;

            Item.damage = 9; // Keep in mind that the arrow's final damage is combined with the bow weapon damage.
            Item.DamageType = DamageClass.Ranged;

            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(copper: 40);
            Item.shoot = ModContent.ProjectileType<ErrorArrowProjectile>(); // The projectile that weapons fire when using this item as ammunition.
            Item.shootSpeed = 10f; // The speed of the projectile.
            Item.ammo = AmmoID.Arrow; // The ammo class this ammo belongs to.
        }

        // For a more detailed explanation of recipe creation, please go to Content/ExampleRecipes.cs.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.WoodenArrow, 5); // 合成配方
            recipe.AddIngredient(ModContent.ItemType<WailingRock>(), 1);
            recipe.AddTile(TileID.Anvils); // 合成台
            recipe.Register();
#if DEBUG
            CreateRecipe(100)
                .AddIngredient(ItemID.DirtBlock, 1)
                .Register();
#endif
        }
        //public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        //{
        //    Vector2 delta = new Vector2(10, 10);
        //    //int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ErrorArrowProjectile>(), 8, 1f);
        //    //int p = Main.rand.Next(3);
        //    //if (p == 0)
        //    //{
        //        int proj = Projectile.NewProjectile(source, position, velocity + delta, ModContent.ProjectileType<ErrorArrowProjectile>(), 8, 1f);
        //        int proj2 = Projectile.NewProjectile(source, position, velocity - delta, ModContent.ProjectileType<ErrorArrowProjectile>(), 8, 1f);
        //    //}
        //    return true;
        //}
    }
}
