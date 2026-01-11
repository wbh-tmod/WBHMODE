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
    public class DerelictDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            BuffID.Sets.GrantImmunityWith[Type].Add(ModContent.BuffType<DerelictDebuff>());
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ModGlobalNPC>().derelictDebuff = true;
            if (npc.buffTime[buffIndex] == 0) // 结算血池
            {
                npc.GetGlobalNPC<ModGlobalNPC>().derelictDebuffFlag = true;
                //Main.NewText("Melee: " + npc.GetGlobalNPC<ModGlobalNPC>().derelictDebuffPool[0]); // 近战
                //Main.NewText("Ranged: " + npc.GetGlobalNPC<ModGlobalNPC>().derelictDebuffPool[1]); // 远程
                //Main.NewText("Magic: " + npc.GetGlobalNPC<ModGlobalNPC>().derelictDebuffPool[2]); // 法术
                //Main.NewText("Summoning: " + npc.GetGlobalNPC<ModGlobalNPC>().derelictDebuffPool[3]); // 召唤
                //npc.GetGlobalNPC<ModGlobalNPC>().derelictDebuffPool = [0, 0, 0, 0];
            }
            //Main.NewText("TIME: " + );
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            //Main.NewText("Time: " + npc.buffTime[buffIndex], new Color(0, 255, 255));
            return true; // 禁用原版
        }
    }
}
