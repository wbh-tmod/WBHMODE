using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Cil;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace WBHMODE.Common.Systems
{
    public class ErodedLandHookingSystem : ModSystem
    {
        public override void Load()
        {
            WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Corruption"], ErodedBiome);
            // IL editing the pyramids pass
            WorldGen.ModifyPass((PassLegacy)WorldGen.VanillaGenPasses["Pyramids"], Modify_Pyramids);

            // Detouring the shinies pass (generates ore)
            WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Shinies"], Detour_Shinies);
        }

        void ErodedBiome(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
        {
            ModContent.GetInstance<WBHMODE>().Logger.Debug("(On Hook) Start Evil");
            orig(self, progress, configuration);
            ModContent.GetInstance<WBHMODE>().Logger.Debug("(On Hook) End Evil");
        }

        void Modify_Pyramids(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);
                c.EmitDelegate(() => ModContent.GetInstance<WBHMODE>().Logger.Debug("(In ILHook) Generating Pyramids"));
            }
            catch (Exception)
            {
                MonoModHooks.DumpIL(ModContent.GetInstance<WBHMODE>(), il);
            }
        }

        // Detouring should be the same (except for one thing mentioned below), this is just an example so you can check this is actually working
        // One thing to note is that for technical reasons, the self parameter is an object type
        // You will never need to actually cast it to type WorldGen though, since it contains no instance fields or methods
        void Detour_Shinies(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
        {
            ModContent.GetInstance<WBHMODE>().Logger.Debug("(On Hook) Before Shinies");
            orig(self, progress, configuration);
            ModContent.GetInstance<WBHMODE>().Logger.Debug("(On Hook) After Shinies");
        }
    }
}
