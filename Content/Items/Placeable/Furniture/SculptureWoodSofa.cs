using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodSofa : ModItem
	{
        public override void SetDefaults()
        {
            // ModContent.TileType<Tiles.Furniture.ExampleWorkbench>() retrieves the id of the tile that this item should place when used.
            // DefaultToPlaceableTile handles setting various Item values that placeable items use
            // Hover over DefaultToPlaceableTile in Visual Studio to read the documentation!
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodSofa>());
            Item.width = 26; // The item texture's width
            Item.height = 26; // The item texture's height
            Item.value = 300;
        }

        
        public override void AddRecipes()
        {
            // ¾âÄ¾»ú 5µñËÜÄ¾ + 2Ë¿³ñ -> 1µñËÜÄ¾É³·¢
            CreateRecipe()
                .AddIngredient<SculptureWood>(5)
                .AddIngredient(ItemID.Silk, 2)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}