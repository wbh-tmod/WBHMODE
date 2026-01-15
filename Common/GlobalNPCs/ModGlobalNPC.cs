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
    internal class ModGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public static readonly int derelictPercent = 10;

        public bool acidEtchingDebuff; // 酸蚀

        public bool halfDeclineDebuff; // 半衰
        //public int halfDeclineFlag;
        public int lifeMax2; // original lifeMax

        public bool derelictDebuff; // 破败
        public bool derelictDebuffFlag; // 破败结算
        //public bool derelictDebuffTypeMelee;
        //public bool derelictDebuffTypeRanged;
        //public bool derelictDebuffTypeMagic;
        //public bool derelictDebuffTypeSummoning;

        public int[] derelictDebuffPool = new int[4]; // 血池

        public override void ResetEffects(NPC npc) {
            acidEtchingDebuff = false;
            halfDeclineDebuff = false;
            //halfDeclineFlag = 0;
            //lifeMax2 = npc.lifeMax;
            derelictDebuff = false;
            derelictDebuffFlag = false;
            //derelictDebuffTypeMelee = false;
            //derelictDebuffTypeRanged = false;
            //derelictDebuffTypeMagic = false;
            //derelictDebuffTypeSummoning = false;
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            lifeMax2 = npc.life;
            derelictDebuffPool[0] = 0;
            //Main.NewText("CurLife: " + npc.life + " | MaxLife: " + npc.lifeMax, new Color(0, 255, 255));
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (derelictDebuffFlag)
            {
                int hurt = derelictDebuffPool.Max();
                // 先获取最大值，再找到最大值的索引，最后通过索引映射到对应的枚举
                DamageClass hurtType = new[] { DamageClass.Melee, DamageClass.Ranged, DamageClass.Magic, DamageClass.Summon }[Array.IndexOf(derelictDebuffPool, derelictDebuffPool.Max())];
                //Main.NewText("Hurt: " + hurt);
                //npc.life -= (int)(hurt * (derelictPercent / 100.0));
                npc.SimpleStrikeNPC(
                    (int)(hurt * (derelictPercent / 100.0)),          // 第1个参数：伤害值
                    1,                               // 第2个参数：打击方向（1=右，-1=左）
                    false,                           // 第3个参数：是否暴击（默认false）
                    0f,                              // 第4个参数：击退力
                    hurtType,                        // 第5个参数：伤害种类
                    false                            // 第6个参数：是否禁止玩家交互（默认false）
                );
                derelictDebuffFlag = false;
                derelictDebuffPool = [0, 0, 0, 0]; // Melee Ranged Magic Summoning
                //Main.NewText("END");
            }
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
                int newLifeMax = npc.lifeMax + npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2 / 10;
                npc.lifeMax = Math.Min(newLifeMax, npc.GetGlobalNPC<ModGlobalNPC>().lifeMax2);
                npc.life = (int)(percent * npc.lifeMax);
            }
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (derelictDebuff)
            {
                if (hit.DamageType == DamageClass.Melee)
                {
                    derelictDebuffPool[0] += damageDone;
                }
                if (hit.DamageType == DamageClass.Ranged)
                {
                    derelictDebuffPool[1] += damageDone;
                }
                if (hit.DamageType == DamageClass.Magic)
                {
                    derelictDebuffPool[2] += damageDone;
                }
                if (hit.DamageType == DamageClass.Summon)
                {
                    derelictDebuffPool[3] += damageDone;
                }
                //Main.NewText("Damage Class: " + hit.DamageType);
                //npc.ai[0] += damageDone;
                base.OnHitByItem(npc, player, item, hit, damageDone);
            }
            //Main.NewText("POOL: " + derelictDebuffPool[0]);
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            //Main.NewText("Damage Class: " + hit.DamageType);
            if (derelictDebuff)
            {
                //Main.NewText("Damage Class: " + hit.DamageType);
                if (hit.DamageType == DamageClass.Melee)
                {
                    derelictDebuffPool[0] += damageDone;
                }
                if (hit.DamageType == DamageClass.Ranged)
                {
                    derelictDebuffPool[1] += damageDone;
                }
                if (hit.DamageType == DamageClass.Magic)
                {
                    derelictDebuffPool[2] += damageDone;
                }
                if (hit.DamageType == DamageClass.Summon)
                {
                    derelictDebuffPool[3] += damageDone;
                }
                base.OnHitByProjectile(npc, projectile, hit, damageDone);
            }
        }
    }
}
