using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace WBHMODE.Content.Tiles.Furniture
{
    public class SculptureWoodCandle : ModTile
    {
        public const int NextStyleHeight = 40; // Calculated by adding all CoordinateHeights + CoordinatePaddingFix.Y applied to all of them + 2

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 20 };
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.DrawYOffset = -4;
            TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.addTile(Type);

            // All candles count as light sources.
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            if (true)
                AddMapEntry(new Color(253, 221, 3), Language.GetText("ItemName.Candle"));

            AdjTiles = new int[] { TileID.Candles };
        }
        
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = 1f;
                g = 1f;
                b = 1f;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }
        }

        public override void HitWire(int i, int j)
        {
            int tileX = 1;
            int tileY = 1;
            int x = i - Main.tile[i, j].TileFrameX / 18 % tileX;
            int y = j - Main.tile[i, j].TileFrameY / 18 % tileY;
            int tileXX18 = 18 * tileX;
            for (int l = x; l < x + tileX; l++)
            {
                for (int m = y; m < y + tileY; m++)
                {
                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
                    {
                        if (Main.tile[l, m].TileFrameX < tileXX18)
                            Main.tile[l, m].TileFrameX += (short)(tileXX18);
                        else
                            Main.tile[l, m].TileFrameX -= (short)(tileXX18);
                    }
                }
            }

            if (Wiring.running)
            {
                for (int k = 0; k < tileX; k++)
                {
                    for (int l = 0; l < tileY; l++)
                        Wiring.SkipWire(x + k, y + l);
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, x, y, tileX, tileY);
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.SculptureWoodCandle>();
        }

        public override bool RightClick(int i, int j)
        {
            if (Main.tile[i, j] != null && Main.tile[i, j].HasTile)
            {
                WorldGen.KillTile(i, j, false, false, false);
                if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                }
            }
            return true;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

            Texture2D flameTexture = ModContent.Request<Texture2D>("WBHMODE/Content/Tiles/Furniture/SculptureWoodCandle_Flame").Value;
            int offsetX = 0;
            int offsetY = 0;
            int width = 16;
            int height = 16;
            int yOffset = TileObjectData.GetTileData(tile).DrawYOffset;

            ulong randShakeEffect = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
            float drawPositionX = i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f;
            float drawPositionY = j * 16 - (int)Main.screenPosition.Y;
            for (int c = 0; c < 7; c++)
            {
                float shakeX = Utils.RandomInt(ref randShakeEffect, -10, 11) * 0.15f;
                float shakeY = Utils.RandomInt(ref randShakeEffect, -10, 1) * 0.35f;
                Main.spriteBatch.Draw(flameTexture, new Vector2(drawPositionX + shakeX, drawPositionY + shakeY + yOffset) + zero, new Rectangle(tile.TileFrameX + offsetX, tile.TileFrameY + offsetY, width, height), new Color(100, 100, 100, 0), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}