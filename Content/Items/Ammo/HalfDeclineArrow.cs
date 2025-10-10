using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using WBHMODE.Content.Items.Materials;

namespace WBHMODE.Content.Items.Ammo
{
    public class HalfDeclineArrow : ModItem
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

            Item.rare = ItemRarityID.Orange;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(copper: 8);
            Item.shoot = ModContent.ProjectileType<Projectiles.HalfDeclineArrowProjectile>(); // The projectile that weapons fire when using this item as ammunition.
            Item.shootSpeed = 3f; // The speed of the projectile.
            Item.ammo = AmmoID.Arrow; // The ammo class this ammo belongs to.
        }
        public override void AddRecipes()
        {
#if DEBUG
            CreateRecipe(150)
                .AddIngredient(ItemID.DirtBlock, 1)
                .Register();
#endif
        }
    }
}
