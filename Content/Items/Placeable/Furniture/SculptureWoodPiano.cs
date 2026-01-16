using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodPiano : ModItem
	{
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodPiano>());
            Item.width = 28; // The item texture's width
            Item.height = 20; // The item texture's height
            Item.value = 300;
        }

        
        public override void AddRecipes()
        {
            // ¾âÄ¾»ú 15µñËÜÄ¾ + 4¹ÇÍ· + 1Êé -> 1µñËÜÄ¾¸ÖÇÙ
            CreateRecipe()
                .AddIngredient<SculptureWood>(15)
                .AddIngredient(ItemID.Bone, 4)
                .AddIngredient(ItemID.Book)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}