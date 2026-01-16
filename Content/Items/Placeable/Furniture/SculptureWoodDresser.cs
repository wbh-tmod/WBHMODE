using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodDresser : ModItem
	{
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodDresser>());

            Item.width = 26;
            Item.height = 22;
            Item.value = 300;
        }

        public override void AddRecipes()
        {
            // ¾âÄ¾»ú 16µñËÜÄ¾ -> 1µñËÜÄ¾Êá×±Ì¨
            CreateRecipe()
                .AddIngredient<SculptureWood>(16)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}