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
            float v = (float)Math.Sqrt(npc.velocity.X * npc.velocity.X + npc.velocity.Y * npc.velocity.Y);
            npc.lifeRegen = -(int)v;
        }
    }
}
