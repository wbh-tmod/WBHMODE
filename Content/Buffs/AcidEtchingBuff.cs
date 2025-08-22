using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace WBHMODE.Content.Buffs
{
    public class AcidEtchingBuff : ModBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            float v = (float)Math.Sqrt(npc.velocity.X * npc.velocity.X + npc.velocity.Y * npc.velocity.Y);
            //if (npc.lifeRegen > 0)
            //{
            //    npc.lifeRegen = 0;
            //}
            if (Main.time % 30 == 0)
            {
                npc.lifeRegen -= 50 * (int)v;
            }
        }
    }
}
