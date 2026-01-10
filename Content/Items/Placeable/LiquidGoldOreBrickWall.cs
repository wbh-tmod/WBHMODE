//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WBHMODE.Content.Items.Placeable.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Items.Placeable
{
    public class LiquidGoldOreBrickWall: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            
            // Mods can be translated to any of the languages tModLoader supports. See https://github.com/tModLoader/tModLoader/wiki/Localization
            // Translations go in localization files (.hjson files), but these are listed here as an example to help modders become aware of the possibility that users might want to use your mod in other lauguages:
            // English: "Example Block", "This is a modded tile."
            // German: "Beispielblock", "Dies ist ein modded Block"
            // Italian: "Blocco di esempio", "Questo è un blocco moddato"
            // French: "Bloc d'exemple", "C'est un bloc modgé"
            // Spanish: "Bloque de ejemplo", "Este es un bloque modded"
            // Russian: "Блок примера", "Это модифицированный блок"
            // Chinese: "例子块", "这是一个修改块"
            // Portuguese: "Bloco de exemplo", "Este é um bloco modded"
            // Polish: "Przykładowy blok", "Jest to modded blok"
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<Walls.LiquidGoldOreBrickWall>());
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            // 工作台 1液金矿砖 -> 4液金矿砖墙
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<LiquidGoldOreBrick>(), 1).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
