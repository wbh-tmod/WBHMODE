using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodChandelier : ModItem
	{
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodChandelier>());
        }

        public override void AddRecipes()
        {
            // ÌúÕè 4µñËÜÄ¾ + 4»ð°Ñ + 1Á´Ìõ -> µñËÜÄ¾µõµÆ
            CreateRecipe()
                .AddIngredient<SculptureWood>(4)
                .AddIngredient(ItemID.Torch, 4)
                .AddIngredient(ItemID.Chain)
                .AddTile(TileID.Anvils)
                .Register();
            
                
        }
    }
}