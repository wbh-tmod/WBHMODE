#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace WBHMODE.Content.Items.Weapons
{
    public class ImaginationGun : ModItem
    {
        public override void SetDefaults() {
            Item.width = 62; // Hitbox width of the item.
            Item.height = 32; // Hitbox height of the item.
            Item.crit = 8;
            Item.scale = 0.75f;
            Item.rare = ItemRarityID.Blue; // The color that the item's name will be in-game.
            Item.useTime = 80; // The item's use time in ticks (60 ticks == 1 second.)
            Item.useAnimation = 80; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.
            Item.UseSound = SoundID.Splash;
            Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
            Item.damage = 3; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            Item.knockBack = 1f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            Item.noMelee = true; // So the item's animation doesn't do damage.
            Item.shoot = ProjectileID.PurificationPowder; // For some reason, all the guns in the vanilla source have this.
            Item.shootSpeed = 10f; // The speed of the projectile (measured in pixels per frame.) This value equivalent to Handgun
            Item.useAmmo = AmmoID.Bullet; // The "ammo Id" of the ammo item that this weapon uses. Ammo IDs are magic numbers that usually correspond to the item id of one item that most commonly represent the ammo type.
        }
        public override void AddRecipes()
        {
#if DEBUG
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LiquidGoldBar>(), 1)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .Register()
                .ReplaceResult(ItemID.MusketBall, 100);
#endif
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, -2f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			float numberProjectiles = 8; // 3, 4, or 5 shots
			float rotation = MathHelper.ToRadians(45);

			position += Vector2.Normalize(velocity) * 45f;
			velocity *= 0.2f; // Slow the projectile down to 1/5th speed so we can see it. This is only here because this example shares ModItem.SetDefaults code with other examples. If you are making your own weapon just change Item.shootSpeed as normal.

			for (int i = 0; i < numberProjectiles; i++) {
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, (i / 5f + 2.8f) / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}

			return false; // return false to stop vanilla from calling Projectile.NewProjectile.
		}
    }
}
