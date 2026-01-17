using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodBed : ModItem
	{
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodBed>());
            Item.width = 28;
            Item.height = 20;
            Item.value = 2000;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            // ¾âÄ¾»ú 15µñËÜÄ¾ + 5Ë¿³ñ -> 1µñËÜÄ¾´²
            CreateRecipe()
                .AddIngredient<SculptureWood>(15)
                .AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}