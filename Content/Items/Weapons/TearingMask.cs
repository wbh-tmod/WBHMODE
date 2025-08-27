#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Items.Materials;
using WBHMODE.Content.Projectiles;

namespace WBHMODE.Content.Items.Weapons
{
    public class TearingMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.damage = 1;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(gold: 1, silver: 50); 
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TearingMaskProjectile>();
            Item.shootSpeed = 25f;
        }
        public override void AddRecipes()
        {
#if DEBUG
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LiquidGoldBar>(), 1)
                .Register();
#endif
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 plrToMouse = Main.MouseWorld - player.Center;
            // 计算玩家到鼠标的向量弧度
            float r = (float)Math.Atan2(plrToMouse.Y, plrToMouse.X);
            float distance = Vector2.Distance(Main.MouseWorld, player.Center);
            Vector2 shootVel = r.ToRotationVector2();
            if (distance > 200f) {
                shootVel *= 25;
            } else {
                shootVel *= distance / 8;
            }
            Projectile.NewProjectile(source, position, shootVel, type, damage, knockback);
            return false;
        }
    }
}
