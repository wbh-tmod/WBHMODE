#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Common.GlobalNPCs
{
    public class ModGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool acidEtchingDebuff;
        public bool halfDeclineDebuff;
        //public int halfDeclineFlag;
        public int lifeMax2; // original lifeMax
        public override void ResetEffects(NPC npc) {
            acidEtchingDebuff = false;
            halfDeclineDebuff = false;
            //halfDeclineFlag = 0;
            //lifeMax2 = npc.lifeMax;
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            lifeMax2 = npc.life;
            //Main.NewText("CurLife: " + npc.life + " | MaxLife: " + npc.lifeMax, new Color(0, 255, 255));
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
            if (npc.lifeMax != npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2)
            {
                float percent = npc.life / (npc.lifeMax * 1.0f);
                npc.lifeMax = npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2;
                npc.life = (int)(percent * npc.lifeMax);
            }
        }
    }
}
