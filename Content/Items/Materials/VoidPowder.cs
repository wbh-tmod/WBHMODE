//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Items.Placeable;

namespace WBHMODE.Content.Items.Materials
{
    public class VoidPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
            ItemID.Sets.SortingPriorityMaterials[Type] = 69;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.PurificationPowder; // Shimmer transform
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(silver: 1); // 售价
            Item.rare = ItemRarityID.White;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
        }
        public override bool? UseItem(Player player)
        {
            return true;
        }
    }
}
