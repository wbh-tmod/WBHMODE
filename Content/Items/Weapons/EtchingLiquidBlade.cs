#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Buffs;
using WBHMODE.Content.Items.Materials;

namespace WBHMODE.Content.Items.Weapons
{
    public class EtchingLiquidBlade : ModItem
    {
        //public int attackType = 0; // keeps track of which attack it is
        //public int comboExpireTimer = 0; // we want the attack pattern to reset if the weapon is not used for certain period of time
        public override void SetDefaults()
        {
            Item.damage = 13; // 伤害
            Item.scale = 2f;
            Item.DamageType = DamageClass.Melee; // 伤害类型
            Item.width = 40; // 宽
            Item.height = 40; // 高
            //  Weapons usually have equal <see cref="useAnimation"/> and useTime, unequal values for these two results in multiple attacks per click.
            Item.useTime = 8; // 攻速(帧)
            Item.useAnimation = 8; // 攻速(帧)
            Item.useStyle = ItemUseStyleID.Swing; // 使用方式
            Item.knockBack = 0; // 击退
            Item.value = Item.buyPrice(silver: 27); // 售价
            Item.rare = ItemRarityID.White; // 稀有度
            Item.UseSound = SoundID.Item1; // 音效
            Item.autoReuse = true; // 自动攻击
            //Item.useTurn = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<LiquidGoldBar>(), 10); // 合成配方
            recipe.AddTile(TileID.Anvils); // 合成台
            recipe.Register();
#if DEBUG
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<LiquidGoldBar>(), 10); // 合成配方
            //recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.Register();
#endif
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int value = Main.rand.Next(5);
            if (value == 0)
            {
                target.AddBuff(ModContent.BuffType<AcidEtchingDebuff>(), 150);
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.IceTorch);
        }
    }
}
