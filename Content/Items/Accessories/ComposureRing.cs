#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Common.Players;
using WBHMODE.Content.Buffs;

namespace WBHMODE.Content.Items.Accessories
{
    public class ComposureRing : ModItem
    {
        public override void SetDefaults() {
            Item.accessory = true; // 将物品标记为饰品，会让这个物品可以装备在饰品栏，同时物品的介绍上也会显示“可装备”字样
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<ComposureRingBuff>(), -1);
        }
        public override void AddRecipes()
        {
#if DEBUG
            CreateRecipe(100).
                AddIngredient(ItemID.DirtBlock, 1).
                Register();
#endif
        }
    }
}
