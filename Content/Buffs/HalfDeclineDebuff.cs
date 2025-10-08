using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using WBHMODE.Common.GlobalNPCs;
using Microsoft.Xna.Framework;

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
            //Main.NewText("Last Time: " + npc.buffTime[buffIndex]);
            //ModGlobalNPC modGlobalNPC = npc.GetGlobalNPC<ModGlobalNPC>();
            npc.GetGlobalNPC<ModGlobalNPC>().halfDeclineDebuff = true;
            //ref float AI_State = ref npc.ai[0];
            //ref float AI_Timer = ref npc.ai[1];
            npc.ai[1]+=1f;
            //Main.NewText("Timer: " + npc.ai[1] + " | MaxLife: " + npc.lifeMax + " | CurLife: " + npc.life + " | OriMax: " + npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2, new Color(0, 255, 255));
            if (npc.ai[1] >= 12f) {
                float percent = npc.life / (npc.lifeMax * 1.0f);
                if (npc.lifeMax * 2 > npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2)
                {
                    if (npc.lifeMax - Math.Max(npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2 / 1000, 1) >= 1) { 
                        npc.lifeMax -= Math.Max(npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2 / 1000, 1);
                    } else {
                        npc.lifeMax = 1;
                    }
                    npc.life = Math.Max((int)(percent * npc.lifeMax), 1);
                    npc.ai[1] = 0;
                } else {
                    npc.ai[1] = 12f;
                }
            }
            //if (npc.GetGlobalNPC<ModGlobalNPC>().halfDeclineFlag != 0) {
            //    npc.ai[0] = 1f;
            //}
            if (npc.buffTime[buffIndex] == 0)
            {
                if (npc.lifeMax != npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2)
                {
                    float percent = npc.life / (npc.lifeMax * 1.0f);
                    npc.lifeMax = npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2;
                    npc.life = (int)(percent * npc.lifeMax);
                }
            }
        }
    }
}
