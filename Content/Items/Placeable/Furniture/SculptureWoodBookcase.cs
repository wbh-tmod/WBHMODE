using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodBookcase : ModItem
	{
        public override void SetDefaults()
        {
            // ModContent.TileType<Tiles.Furniture.ExampleWorkbench>() retrieves the id of the tile that this item should place when used.
            // DefaultToPlaceableTile handles setting various Item values that placeable items use
            // Hover over DefaultToPlaceableTile in Visual Studio to read the documentation!
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodBookcase>());
            Item.width = 28; // The item texture's width
            Item.height = 20; // The item texture's height
            Item.value = 300;
        }

        
        public override void AddRecipes()
        {
            // æ‚ƒæª˙ 20µÒÀ‹ƒæ + 10 È -> 1µÒÀ‹ƒæ Èº‹
            CreateRecipe()
                .AddIngredient<SculptureWood>(20)
                .AddIngredient(ItemID.Book, 10)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}