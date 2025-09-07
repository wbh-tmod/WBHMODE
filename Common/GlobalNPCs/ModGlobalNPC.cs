#define DEBUG

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
        public bool halfDeclineDebuff;
        public int halfDeclineFlag;
        public int lifeMax2; // original lifeMax
        public override void ResetEffects(NPC npc) {
            acidEtchingDebuff = false;
            halfDeclineDebuff = false;
            halfDeclineFlag = 0;
            lifeMax2 = npc.lifeMax;
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (acidEtchingDebuff)
            {
                float v = (float)Math.Sqrt(npc.velocity.X * npc.velocity.X + npc.velocity.Y * npc.velocity.Y);
                npc.lifeRegen -= (int)v;
            }
        }
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            npc.ai[0] = 1f;
        }
    }
}
