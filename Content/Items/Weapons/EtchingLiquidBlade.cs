#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Buffs;

namespace WBHMODE.Content.Items.Weapons
{
    public class EtchingLiquidBlade : ModItem
    {
        //public int attackType = 0; // keeps track of which attack it is
        //public int comboExpireTimer = 0; // we want the attack pattern to reset if the weapon is not used for certain period of time
        public override void SetDefaults()
        {
            Item.damage = 11; // 伤害
            Item.scale = 2f;
            Item.DamageType = DamageClass.Melee; // 伤害类型
            Item.width = 50; // 宽
            Item.height = 50; // 高
            //  Weapons usually have equal <see cref="useAnimation"/> and useTime, unequal values for these two results in multiple attacks per click.
            Item.useTime = 8; // 攻速(帧)
            Item.useAnimation = 8; // 攻速(帧)
            Item.useStyle = ItemUseStyleID.Swing; // 使用方式
            Item.knockBack = 0; // 击退
            Item.value = Item.buyPrice(copper: 1); // 售价
            Item.rare = ItemRarityID.White; // 稀有度
            Item.UseSound = SoundID.Item1; // 音效
            Item.autoReuse = true; // 自动攻击
        }
        public override void AddRecipes()
        {
#if DEBUG
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 合成配方
            //recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.Register();
#endif
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AcidEtchingBuff>(), 600);
        }
    }
}
