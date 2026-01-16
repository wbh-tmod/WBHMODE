using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodBath : ModItem
	{
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodBath>());
            Item.width = 28; // The item texture's width
            Item.height = 20; // The item texture's height
            Item.value = 300;
        }

        
        public override void AddRecipes()
        {
            // ¾âÄ¾»ú 14µñËÜÄ¾ -> 1µñËÜÄ¾Ô¡¸×
            CreateRecipe()
                .AddIngredient<SculptureWood>(14)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}