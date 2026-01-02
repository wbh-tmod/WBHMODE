using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable
{
    public class FlowingSandstoneBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FlowingSandstone>());
        }
        public override void AddRecipes()
        {
#if DEBUG
            CreateRecipe(100).
                AddIngredient(ItemID.DirtBlock, 1).
                AddTile(TileID.WorkBenches).
                Register();
#endif
            // 在叶绿提炼机1:1转化为沙岩块
            CreateRecipe().
                AddIngredient(ModContent.ItemType<FlowingSandstoneBlock>(), 1).
                AddTile(TileID.ChlorophyteExtractinator).
                Register().
                ReplaceResult(ItemID.Sandstone,1);
            // 工作台 流动沙岩墙4:1流动沙岩块
            CreateRecipe().
                AddIngredient(ModContent.ItemType<FlowingSandstoneWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
