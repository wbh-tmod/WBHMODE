using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable
{
    public class ErodingTorch : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.Torches[Item.type] = true;
            ItemID.Sets.SingleUseInGamepad[Type] = true;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ShimmerTorch;
        }

        public override void SetDefaults()
        {
            //Item.width = 14;
            //Item.height = 16;
            //Item.maxStack = 9999;
            //Item.holdStyle = 1;
            //Item.noWet = true;
            //Item.useTurn = true;
            //Item.autoReuse = true;
            //Item.useAnimation = 15;
            //Item.useTime = 10;
            //Item.useStyle = ItemUseStyleID.Swing;
            //Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.ErodingTorch>();
            //Item.flame = true;
            Item.value = 50;
        }

        public override void HoldItem(Player player)
        {
            // This torch cannot be used in water, so it shouldn't spawn particles or light either
            if (player.wet)
            {
                return;
            }
            // Randomly spawn sparkles when the torch is held. Bigger chance to spawn them when swinging the torch.
            if (Main.rand.NextBool(player.itemAnimation > 0 ? 7 : 30))
            {
                Dust dust = Dust.NewDustDirect(new Vector2(player.itemLocation.X + (player.direction == -1 ? -16f : 6f), player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<Sparkle>(), 0f, 0f, 100);
                if (!Main.rand.NextBool(3))
                {
                    dust.noGravity = true;
                }

                dust.velocity *= 0.3f;
                dust.velocity.Y -= 1.5f;
                dust.position = player.RotatedRelativePoint(dust.position);
            }
            
            //bool killTorch = Collision.DrownCollision(player.position, player.width, player.height, player.gravDir) || Item.wet;
            //if (!killTorch && Main.rand.NextBool(player.itemAnimation > 0 ? 10 : 20))
            //{
            //    Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, DustID.Torch);
            //}
            Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
            
            //if (!killTorch)
            Lighting.AddLight(position, 0.9f, 0.9f, 1.1f);
        }

        public override void PostUpdate()
        {
            if (!Item.wet)
                Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 1, 1, 1);
        }

        public override void AddRecipes()
        {
            // 徒手 火把3+流动石块/黑冰雪块/硬化流动沙块1->消蚀火把3
            CreateRecipe(3).
                AddIngredient(ItemID.Torch, 3).
                AddIngredient(ModContent.ItemType<FlowingStone>(), 1).
                Register();
            CreateRecipe(3).
                AddIngredient(ItemID.Torch, 3).
                AddIngredient(ModContent.ItemType<BlackIceBlock>(), 1).
                Register();
            CreateRecipe(3).
                AddIngredient(ItemID.Torch, 3).
                AddIngredient(ModContent.ItemType<HardenedFlowingSandBlock>(), 1).
                Register();
        }
    }
}