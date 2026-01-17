using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodCandle : ModItem
	{
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodCandle>());
            Item.width = 28;
            Item.height = 20;
            Item.value = 0;
        }

        
        public override void AddRecipes()
        {
            // π§◊˜Ã® 4µÒÀ‹ƒæ + 1ª∞— -> 1µÒÀ‹ƒæ¿Ø÷Ú
            CreateRecipe()
                .AddIngredient<SculptureWood>(4)
                .AddIngredient(ItemID.Torch)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}