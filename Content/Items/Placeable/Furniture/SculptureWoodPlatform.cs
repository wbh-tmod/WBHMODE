using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodPlatform : ModItem
	{
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 200;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodPlatform>());
            Item.width = 8;
            Item.height = 10;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient<SculptureWood>()
                .Register();
        }
    }
}