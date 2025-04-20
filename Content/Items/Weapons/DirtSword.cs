using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Weapons
{
    public class DirtSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 1; // 伤害
            Item.DamageType = DamageClass.Melee; // 上海类型
            Item.width = 50; // 宽
            Item.height = 50; // 高
            //  Weapons usually have equal <see cref="useAnimation"/> and useTime, unequal values for these two results in multiple attacks per click.
            Item.useTime = 20; // 攻速(帧)
            Item.useAnimation = 20; // 攻速(帧)
            Item.useStyle = ItemUseStyleID.Swing; // 使用方式
            Item.knockBack = 4; // 击退
            Item.value = Item.buyPrice(copper: 1); // 售价
            Item.rare = ItemRarityID.White; // 稀有度
            Item.UseSound = SoundID.Item1; // 音效
            Item.autoReuse = true; // 自动攻击
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 合成配方
            //recipe.AddTile(TileID.WorkBenches); // 合成台
            recipe.Register();
        }
    }

}
