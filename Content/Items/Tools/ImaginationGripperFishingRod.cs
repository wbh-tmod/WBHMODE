using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using WBHMODE.Content.Items.Materials;
using WBHMODE.Content.Projectiles;

namespace WBHMODE.Content.Items.Tools
{
    public class ImaginationGripperFishingRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanFishInLava[Item.type] = true; // Allows the pole to fish in lava
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodFishingPole);

            Item.value = Item.sellPrice(gold: 2, silver: 64); // 售价
            Item.rare = ItemRarityID.Blue;
            Item.fishingPole = 24; // Sets the poles fishing power
            Item.shootSpeed = 14f; // Sets the speed in which the bobbers are launched. Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f.
            Item.shoot = ModContent.ProjectileType<Projectiles.ImaginationGripperBobber>(); // The bobber projectile. Note that this will be overridden by Fishing Bobber accessories if present, so don't assume the bobber spawned is the specified projectile. https://terraria.wiki.gg/wiki/Fishing_Bobbers
        }
        public override void HoldItem(Player player)
        {
            player.accFishingLine = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //int bobberAmount = Main.rand.Next(3, 6); // 3 to 5 bobbers
            int bobberAmount = 1;
            float spreadAmount = 75f; // how much the different bobbers are spread out.
            Vector2 bobberSpeed = velocity + new Vector2(Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f, Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f);
            // Generate new bobbers
            Projectile.NewProjectile(source, position, bobberSpeed, type, 0, 0f, player.whoAmI);
            return false;
        }
        public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            // Change these two values in order to change the origin of where the line is being drawn.
            // This will make it draw 43 pixels right and 30 pixels up from the player's center, while they are looking right and in normal gravity.
            lineOriginOffset = new Vector2(43, -30);

            // Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
            if (bobber.ModProjectile is ImaginationGripperBobber imaginationGripperBobber)
            {
                // ExampleBobber has custom code to decide on a line color.
                lineColor = imaginationGripperBobber.FishingLineColor;
            }
            else
            {
                // If the bobber isn't ExampleBobber, a Fishing Bobber accessory is in effect and we use DiscoColor instead.
                lineColor = Main.DiscoColor;
            }
        }
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
