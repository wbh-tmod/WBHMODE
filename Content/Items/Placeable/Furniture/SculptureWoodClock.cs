using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodClock : ModItem
	{
        public override void SetDefaults()
        {
            // ModContent.TileType<Tiles.Furniture.ExampleWorkbench>() retrieves the id of the tile that this item should place when used.
            // DefaultToPlaceableTile handles setting various Item values that placeable items use
            // Hover over DefaultToPlaceableTile in Visual Studio to read the documentation!
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodClock>());
            Item.width = 26; // The item texture's width
            Item.height = 22; // The item texture's height
            Item.value = 500;
        }

        
        public override void AddRecipes()
        {
            // æ‚ƒæª˙ 10µÒÀ‹ƒæ + 6≤£¡ß + 3Ã˙∂ß/«¶∂ß -> 1µÒÀ‹ƒæ ±÷”
            CreateRecipe()
                .AddIngredient<SculptureWood>(10)
                .AddIngredient(ItemID.Glass, 6)
                .AddIngredient(ItemID.IronBar, 3)
                .AddTile(TileID.Sawmill)
                .Register();
            CreateRecipe()
                .AddIngredient<SculptureWood>(10)
                .AddIngredient(ItemID.Glass, 6)
                .AddIngredient(ItemID.LeadBar, 3)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}