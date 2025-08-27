//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Items.Materials;

namespace WBHMODE.Content.Items.Weapons
{
    public class PhantomBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 58;
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(silver: 36);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5;
            //Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 6.7f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback) => knockback = new StatModifier(1f, 1f);
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LiquidGoldBar>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
#if DEBUG
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LiquidGoldBar>(), 8)
                .Register();
#endif
        }
    }
}
