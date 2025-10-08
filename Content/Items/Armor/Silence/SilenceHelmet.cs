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
using WBHMODE.Content.Items.Materials;

namespace WBHMODE.Content.Items.Armor.Silence
{
    [AutoloadEquip(EquipType.Head)]
    public class SilenceHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;

            // Common values for every boss mask
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 75);
            Item.vanity = true;
            Item.maxStack = 1;

            Item.defense = 6;
            Item.ArmorPenetration = 2;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SilenceScalemail>() && legs.type == ModContent.ItemType<SilenceGreaves>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.AddBuff(ModContent.BuffType<SilenceArmorBuff>(), 1);
        }
        public override void AddRecipes()
        {
#if DEBUG
            CreateRecipe().
                AddIngredient(ItemID.DirtBlock, 1).
                Register();
#endif
            CreateRecipe().
                AddIngredient(ModContent.ItemType<LiquidGoldBar>(), 15).
                AddIngredient(ModContent.ItemType<FlowingStonePhase>(), 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
