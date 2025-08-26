#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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
    }
}
