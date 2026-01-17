using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodChair : ModItem
	{
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodChair>());
            Item.width = 12;
            Item.height = 30;
            Item.value = 0;
        }

        
        public override void AddRecipes()
        {
            // 工作台 4雕塑木 -> 1雕塑木椅子
            CreateRecipe()
                .AddIngredient<SculptureWood>(4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}