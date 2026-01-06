using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using WBHMODE.Content.Dusts;

namespace WBHMODE.Content.Biomes
{
    public class ErodedLandWaterStyle : ModWaterStyle
    {
        private Asset<Texture2D> rainTexture;
        public override void Load() {
            rainTexture = Mod.Assets.Request<Texture2D>("Content/Biomes/ErodedLandRain");
        }
        public override int ChooseWaterfallStyle() {
            return ModContent.GetInstance<ErodedLandWaterfallStyle>().Slot;
        }

        public override int GetSplashDust() {
            return ModContent.DustType<ErodedLandSolutionDust>();
        }

        public override int GetDropletGore() {
            return ModContent.GoreType<ErodedLandDroplet>();
        }

        public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
            r = 1f;
            g = 1f;
            b = 1f;
        }
        public override Color BiomeHairColor() {
            return Color.White;
        }
        public override byte GetRainVariant() {
            return (byte)Main.rand.Next(3);
        }

        public override Asset<Texture2D> GetRainTexture() => rainTexture;
    }
}
