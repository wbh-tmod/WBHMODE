using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Common.GlobalNPCs
{
    public class ModGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool acidEtchingDebuff;
        public override void ResetEffects(NPC npc) {
            acidEtchingDebuff = false;
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (acidEtchingDebuff)
            {
                //if (npc.lifeRegen > 0)
                //{
                //    npc.lifeRegen = 0;
                //}
                float v = (float)Math.Sqrt(npc.velocity.X * npc.velocity.X + npc.velocity.Y * npc.velocity.Y);
                npc.lifeRegen = -(int)v;
            }
        }
    }
}
