using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Items.Materials;

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

            Item.damage = 8; // Keep in mind that the arrow's final damage is combined with the bow weapon damage.
            Item.DamageType = DamageClass.Ranged;

            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(copper: 40);
            Item.shoot = ModContent.ProjectileType<Projectiles.ErrorArrow>(); // The projectile that weapons fire when using this item as ammunition.
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
        }
    }
}
