using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using WBHMODE.Content.Items.Placeable.Furniture;

namespace WBHMODE.Content.Tiles.Furniture
{
    public class SculptureWoodSofa : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.addTile(Type);

            // All sofas count as chairs.
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            AddMapEntry(new Color(191, 142, 111), Language.GetText("ItemName.Sofa"));
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Player player = Main.LocalPlayer;
            int nextStyleHeight = 40;

            info.DirectionOffset = 0;
            float offset = 0f;
            if (tile.TileFrameX < 17 && player.direction == 1)
                offset = 8f;
            if (tile.TileFrameX < 17 && player.direction == -1)
                offset = -8f;
            if (tile.TileFrameX > 34 && player.direction == 1)
                offset = -8f;
            if (tile.TileFrameX > 34 && player.direction == -1)
                offset = 8f;
            info.VisualOffset = new Vector2(offset, 0f);
            info.TargetDirection = player.direction;

            info.AnchorTilePosition.X = i;
            info.AnchorTilePosition.Y = j;

            if (tile.TileFrameY % nextStyleHeight == 0)
            {
                info.AnchorTilePosition.Y++;
            }
        }


        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            {
                player.GamepadEnableGrappleCooldown();
                player.sitting.SitDown(player, i, j);
            }
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            {
                return;
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.SculptureWoodSofa>();
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
        }
    }
}