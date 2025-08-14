using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Ammo
{
    public class DirtArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 36;

            Item.damage = 1; // Keep in mind that the arrow's final damage is combined with the bow weapon damage.
            Item.DamageType = DamageClass.Ranged;

            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(copper: 1);
            Item.shoot = ModContent.ProjectileType<Projectiles.DirtArrow>(); // The projectile that weapons fire when using this item as ammunition.
            Item.shootSpeed = 3f; // The speed of the projectile.
            Item.ammo = AmmoID.Arrow; // The ammo class this ammo belongs to.
        }

        // For a more detailed explanation of recipe creation, please go to Content/ExampleRecipes.cs.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(99);
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 合成配方
            recipe.AddIngredient(ItemID.Wood, 1);
            //recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.Register();
        }
    }
}
