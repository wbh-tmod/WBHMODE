using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodWorkbench : ModItem
	{
        public override void SetDefaults()
        {
            // ModContent.TileType<Tiles.Furniture.ExampleWorkbench>() retrieves the id of the tile that this item should place when used.
            // DefaultToPlaceableTile handles setting various Item values that placeable items use
            // Hover over DefaultToPlaceableTile in Visual Studio to read the documentation!
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodWorkbench>());
            Item.width = 28; // The item texture's width
            Item.height = 14; // The item texture's height
            Item.value = 150;
        }

        // Í½ÊÖ 10µñËÜÄ¾ -> µñËÜÄ¾¹¤×÷Ì¨
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SculptureWood>(10)
                .Register();
                
        }
    }
}