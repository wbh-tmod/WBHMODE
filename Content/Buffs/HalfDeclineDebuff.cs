using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using WBHMODE.Common.GlobalNPCs;

namespace WBHMODE.Content.Buffs
{
    public class HalfDeclineDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            BuffID.Sets.GrantImmunityWith[Type].Add(ModContent.BuffType<HalfDeclineDebuff>());
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            //ModGlobalNPC modGlobalNPC = npc.GetGlobalNPC<ModGlobalNPC>();
            npc.GetGlobalNPC<ModGlobalNPC>().halfDeclineDebuff = true;
            //ref float AI_State = ref npc.ai[0];
            //ref float AI_Timer = ref npc.ai[1];
            npc.ai[1]++;
            if (npc.ai[1] == 60f) {
                float percent = npc.life / (npc.lifeMax * 1.0f);
                if (npc.lifeMax * 2 > npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2)
                {
                    npc.lifeMax -= Math.Max(npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2 / 100, 1);
                    npc.life = (int)(percent * npc.lifeMax);
                    npc.ai[1] = 0;
                } else {
                    npc.ai[1] = 60;
                }
            }
            //if (npc.GetGlobalNPC<ModGlobalNPC>().halfDeclineFlag != 0) {
            //    npc.ai[0] = 1f;
            //}
            if (npc.ai[0] == 1f) {
                float percent = npc.life / (npc.lifeMax * 1.0f);
                npc.lifeMax = npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2;
                npc.life = (int)(percent * npc.lifeMax);
                //npc.life = npc.lifeMax;
                npc.ai[1] = 0;
                npc.ai[0] = 0;
            }
        }
    }
}
