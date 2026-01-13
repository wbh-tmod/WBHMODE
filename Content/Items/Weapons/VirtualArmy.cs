#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using WBHMODE.Content.Projectiles;

namespace WBHMODE.Content.Items.Weapons
{
    public class VirtualArmy : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 32;
            Item.width = 32;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Pink;
            Item.damage = 31;
            Item.value = Item.buyPrice(0, 54, 0, 0);
            Item.noMelee = true;
            Item.useTime = 32;
            Item.knockBack = 2f;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.mana = 10;
            Item.crit = 0;
            Item.staff[Item.type] = true;
            Item.UseSound = SoundID.Item44;
            Item.DamageType = DamageClass.Summon;
            //Item.buffType = ModContent.BuffType<GliderBuff>();
            //Item.buffTime = 3600;
            Item.shoot = ModContent.ProjectileType<VirtualSoldier>();
            Item.shootSpeed = 10f;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
            //return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
#if DEBUG
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 合成配方
            recipe.Register();
#endif
            //base.AddRecipes();
        }
    }
}
