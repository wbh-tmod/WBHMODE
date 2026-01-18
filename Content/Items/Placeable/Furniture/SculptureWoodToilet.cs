using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodToilet : ModItem
	{
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodToilet>());
            Item.width = 16;
            Item.height = 24;
            Item.value = 150;
        }

        
        public override void AddRecipes()
        {
            // ¾âÄ¾»ú 6µñËÜÄ¾ -> 1µñËÜÄ¾ÂíÍ°
            CreateRecipe()
                .AddIngredient<SculptureWood>(6)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}