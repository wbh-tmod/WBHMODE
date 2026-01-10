using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Chat;
using Terraria.IO;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using System.Threading;


namespace WBHMODE.Content.Tiles
{
    public class LiquidGoldOre : ModTile
    {
        public override void SetStaticDefaults() {
            TileID.Sets.Ore[Type] = true;
            TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
            Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
            Main.tileOreFinderPriority[Type] = 305; // Metal Detector value, see https://terraria.wiki.gg/wiki/Metal_Detector
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(255, 215, 0), name);

            DustType = 84;
            HitSound = SoundID.Tink;
            //MineResist = 4f;
            MinPick = 55;
        }
        
        public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor)
        {
            sightColor = Color.Gold;
            return true;
        }
    }
    public class LiquidGoldOreSystem : ModSystem
    {
        public static LocalizedText LiquidGoldOrePassMessage { get; private set; }
        public static LocalizedText BlessedWithLiquidGoldOreMessage { get; private set;}
        public override void SetStaticDefaults() {
            LiquidGoldOrePassMessage = Mod.GetLocalization($"WorldGen.{nameof(LiquidGoldOrePassMessage)}");
            BlessedWithLiquidGoldOreMessage = Mod.GetLocalization($"WorldGen.{nameof(BlessedWithLiquidGoldOreMessage)}");
        }
        public void BlessWorldWithLiquidGoldOre() {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                return; // This should not happen, but just in case.
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText(BlessedWithLiquidGoldOreMessage.Value, 50, 255, 130);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(BlessedWithLiquidGoldOreMessage.ToNetworkText(), new Color(50, 255, 130));
                }

                int splotches = (int)(100 * (Main.maxTilesX / 4200f));
                int highestY = (int)Utils.Lerp(Main.rockLayer, Main.UnderworldLayer, 0.5);
                for (int iteration = 0; iteration < splotches; iteration++)
                {
                    // Find a point in the lower half of the rock layer but above the underworld depth.
                    int i = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                    int j = WorldGen.genRand.Next(highestY, Main.UnderworldLayer);

                    // OreRunner will spawn LiquidGoldOre in splotches. OnKill only runs on the server or single player, so it is safe to run world generation code.
                    WorldGen.OreRunner(i, j, WorldGen.genRand.Next(5, 9), WorldGen.genRand.Next(5, 9), (ushort)ModContent.TileType<LiquidGoldOre>());
                }
            });
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1) {
                tasks.Insert(ShiniesIndex + 1, new LiquidGoldOrePass("Liquid Gold Ore", 237.4298f));
            }
        }
    }

    public class LiquidGoldOrePass : GenPass
    {
        public LiquidGoldOrePass(string name, float loadWeight) : base(name, loadWeight) { }
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = LiquidGoldOreSystem.LiquidGoldOrePassMessage.Value;

            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), ModContent.TileType<LiquidGoldOre>());
            }
        }
    }
}
