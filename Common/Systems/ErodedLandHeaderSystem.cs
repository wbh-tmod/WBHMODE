using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Localization;
using WBHMODE.Content;

namespace WBHMODE.Common.Systems
{
    public class ErodedLandHeaderSystem : ModSystem
    {
        public override void SaveWorldHeader(TagCompound tag)
        {
            tag["ErodedLandModExists"] = true;
        }
        public override void Load()
        {
            On_UIWorldListItem.DrawSelf += (orig, self, spriteBatch) => {
                orig(self, spriteBatch);
                DrawWorldSelectItemOverlay(self, spriteBatch);
            };
        }
        private void DrawWorldSelectItemOverlay(UIWorldListItem uiItem, SpriteBatch spriteBatch)
        {
            if (MenuLoader.CurrentMenu is not ErodedLandModMenu)
                return;

            if (!uiItem.Data.TryGetHeaderData(this, out var data) || !data.GetBool("ExampleModExists"))
                return;

            var dims = uiItem.GetInnerDimensions();
            var pos = new Vector2(dims.X + 400, dims.Y);
            Utils.DrawBorderString(spriteBatch, "EM played before", pos, Color.BlueViolet);
        }
    }

    public class ErodedLandHeaderPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            // This data can only be checked in Single Player
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                return;
            }

            // Check if this world was generated with at least a specific version of a mod.
            // Tracking mods used to generate a world was added in v2023.8, so if WorldGenModsRecorded is false we don't know for sure if ExampleMod was enabled when this world was generated.
            if (!Main.ActiveWorldFileData.WorldGenModsRecorded)
            {
                return;
            }

            if (Main.ActiveWorldFileData.TryGetModVersionGeneratedWith("ExampleMod", out Version modVersion))
            {
                if (modVersion < new Version(1, 0))
                {
                    // Here we could have a message about the world missing a new biome added in v1.0 of this mod that won't be present in the world because it was generated before then.
                }
            }
            else
            {
                Main.NewText(Language.GetTextValue(Mod.GetLocalizationKey("NotPresentDuringWorldGenMessage")), Color.Orange);
            }
        }
    }
}
