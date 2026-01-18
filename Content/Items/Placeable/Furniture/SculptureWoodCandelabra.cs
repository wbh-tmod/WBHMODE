using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable.Furniture
{
	public class SculptureWoodCandelabra : ModItem
	{
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SculptureWoodCandelabra>());
            Item.width = 28;
            Item.height = 20;
            Item.value = 1500;
        }

        
        public override void AddRecipes()
        {
            // 工作台 5雕塑木 + 3火把 -> 1雕塑木烛台
            CreateRecipe()
                .AddIngredient<SculptureWood>(5)
                .AddIngredient(ItemID.Torch, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}