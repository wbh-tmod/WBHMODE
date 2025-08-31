using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Armor.Silence
{
    [AutoloadEquip(EquipType.Body)]
    public class SilenceScalemail : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;

            // Common values for every boss mask
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 60);
            Item.vanity = true;
            Item.maxStack = 1;

            Item.defense = 7;
            Item.ArmorPenetration = 2;
        }
        public override void AddRecipes()
        {
#if DEBUG
            CreateRecipe().
                AddIngredient(ItemID.DirtBlock, 1).
                Register();
#endif
        }
    }
}
