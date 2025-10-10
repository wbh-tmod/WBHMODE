using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Buffs;
using WBHMODE.Content.Projectiles;

namespace WBHMODE.Content.Items.Weapons
{
    public class DirtMagic : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToStaff(ModContent.ProjectileType<DirtMagicShit>(), 7, 20, 1);
            Item.width = 34;
            Item.height = 40;
            Item.UseSound = SoundID.Item71;
            Item.SetWeaponValues(1, 2);
            Item.SetShopValues(ItemRarityColor.LightRed4, 10000);
            //Item.shoot = ModContent.ProjectileType<DirtMagicShit>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            //recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            // We can use ModifyManaCost to dynamically adjust the mana cost of this item, similar to how Space Gun works with the Meteor armor set.
            // See ExampleHood to see how accessories give the reduce mana cost effect.
            if (player.statLife < player.statLifeMax2 / 2)
            {
                mult *= 0.5f; // Half the mana cost when at low health. Make sure to use multiplication with the mult parameter.
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff(ModContent.BuffType<AcidEtchingDebuff>(), 600);
            target.AddBuff(ModContent.BuffType<HalfDeclineDebuff>(), 600);
        }
    }

}
