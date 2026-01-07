using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Drawing;

namespace WBHMODE.Content.Tiles
{
    public class ErodingCampfire : ModTile
    {
        private Asset<Texture2D> flameTexture;
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;
            TileID.Sets.Campfire[Type] = true;

            DustType = -1; // No dust when mined.
            AdjTiles = [TileID.Campfire];

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Campfire, 0));
            /*  This is what is copied from the Campfire tile
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleWrapLimit = 16;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = 2;
			*/
            TileObjectData.newTile.StyleLineSkip = 9; // This needs to be added to work for modded tiles.
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(254, 121, 2), Language.GetText("ItemName.Campfire"));

            // Assets
            flameTexture = ModContent.Request<Texture2D>(Texture + "_Flame");
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            // HasCampfire is a gameplay effect, so we don't run the code if closer is true.
            if (closer)
            {
                return;
            }

            if (Main.tile[i, j].TileFrameY < 36)
            {
                Main.SceneMetrics.HasCampfire = true;
            }
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];

            if (!TileDrawing.IsVisible(tile))
            {
                return;
            }

            if (tile.TileFrameY < 36)
            {
                Color color = new Color(255, 255, 255, 0);

                Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

                int width = 16;
                int offsetY = 0;
                int height = 16;
                short frameX = tile.TileFrameX;
                short frameY = tile.TileFrameY;
                int addFrX = 0;
                int addFrY = 0;

                TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY); // calculates the draw offsets
                TileLoader.SetAnimationFrame(Type, i, j, ref addFrX, ref addFrY); // calculates the animation offsets

                Rectangle drawRectangle = new Rectangle(tile.TileFrameX, tile.TileFrameY + addFrY, 16, 16);

                // The flame is manually drawn separate from the tile texture so that it can be drawn at full brightness.
                spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + offsetY) + zero, drawRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
