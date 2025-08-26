using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace WBHMODE.Content.Buffs
{
    public class AcidEtchingDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            //npc.acidEtchingDebuff = true;
            float v = (float)Math.Sqrt(npc.velocity.X * npc.velocity.X + npc.velocity.Y * npc.velocity.Y);
            //if (npc.lifeRegen > 0)
            //{
            //    npc.lifeRegen = 0;
            //}
            if (Main.time % 12 == 0)
            {
                npc.lifeRegen -= 50 * (int)v;
            }
        }
    }
    //public class AcidEtchingDebuffNPC : ModNPC
    //{
    //    // Flag checking when life regen debuff should be activated
    //    public bool acidEtchingDebuff;

    //    public override void ResetEffects()
    //    {
    //        acidEtchingDebuff = false;
    //    }

    //    // Allows you to give the player a negative life regeneration based on its state (for example, the "On Fire!" debuff makes the player take damage-over-time)
    //    // This is typically done by setting player.lifeRegen to 0 if it is positive, setting player.lifeRegenTime to 0, and subtracting a number from player.lifeRegen
    //    // The player will take damage at a rate of half the number you subtract per second
    //    public override void UpdateBadLifeRegen()
    //    {
    //        if (acidEtchingDebuff)
    //        {
    //            // These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects
    //            if (Player.lifeRegen > 0)
    //                Player.lifeRegen = 0;
    //            // Player.lifeRegenTime used to increase the speed at which the player reaches its maximum natural life regeneration
    //            // So we set it to 0, and while this debuff is active, it never reaches it
    //            Player.lifeRegenTime = 0;
    //            // lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second
    //            Player.lifeRegen -= 16;
    //        }
    //    }
    //}
}
