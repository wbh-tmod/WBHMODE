using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using WBHMODE.Common.Players;
using Terraria.GameContent;
using System.IO;
using WBHMODE.Content.Buffs;

namespace WBHMODE.Content.Projectiles
{
    public class VirtualSoldier : ModProjectile
    {
        public enum ProjectileState
        {
            MoveAroundPlayer,
            Attack, // 重命名为Attack更贴合近战逻辑，包含贴近+远离循环
        };

        // 新增攻击阶段枚举，控制贴近/远离
        private enum AttackPhase
        {
            Approach,   // 贴近敌人
            Retreat,    // 远离敌人
            Cooldown    // 后撤冷却（1秒）
        }

        private float Timer
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }

        public ProjectileState State
        {
            get { return (ProjectileState)(int)Projectile.ai[1]; }
            set { Projectile.ai[1] = (int)value; }
        }

        // 新增：攻击阶段（存储在localAI，避免同步问题）
        private AttackPhase CurrentAttackPhase
        {
            get { return (AttackPhase)(int)Projectile.localAI[0]; }
            set { Projectile.localAI[0] = (int)value; }
        }

        // 新增：攻击阶段计时器（控制1秒后撤）
        private float AttackPhaseTimer
        {
            get { return Projectile.localAI[1]; }
            set { Projectile.localAI[1] = value; }
        }

        public Vector2 TargetLocation = new Vector2();
        private static float _nearPlayerSpeed = 0.1f;
        // 配置参数：攻击距离、后撤距离、后撤时长（60帧=1秒）
        private const float AttackDistance = 50f;    // 近战攻击距离
        private const float RetreatDistance = 150f;  // 后撤目标距离
        private const int RetreatDuration = 60;      // 后撤冷却时长（1秒）
        private const float ApproachSpeed = 8f;      // 贴近速度
        private const float RetreatSpeed = 6f;       // 后撤速度

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 3;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1.1f;
            // 召唤物必备的属性
            Projectile.netImportant = true;
            Projectile.minionSlots = 1;
            Projectile.minion = true;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        /// <summary>
        /// 没有接触伤害
        /// </summary>
        /// <returns></returns>
        public override bool MinionContactDamage()
        {
            return false;
        }

        /// <summary>
        /// 寻找最近的敌方单位
        /// </summary>
        /// <param name="position"></param>
        /// <param name="maxDistance"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static NPC FindCloestEnemy(Vector2 position, float maxDistance, Func<NPC, bool> predicate)
        {
            float maxDis = maxDistance;
            NPC res = null;
            foreach (var npc in Main.npc.Where(n => n.active && !n.friendly && predicate(n)))
            {
                float dis = Vector2.Distance(position, npc.Center);
                if (dis < maxDis)
                {
                    maxDis = dis;
                    res = npc;
                }
            }
            return res;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            var modPlayer = player.GetModPlayer<ModGlobalPlayer>();

            if (player.dead)
            {
                modPlayer.VirtualArmyBuff = false;
            }
            if (modPlayer.VirtualArmyBuff)
            {
                Projectile.timeLeft = 2;
            }
            player.AddBuff(ModContent.BuffType<VirtualSoldierBuff>(), 2);

            ProjectileState prevState = State;
            NPC tar = null;

            // 如果有强制瞄准位置，就进入攻击状态
            if (TargetLocation != Vector2.Zero)
            {
                State = ProjectileState.Attack;
            }
            else
            {
                NPC npc = FindCloestEnemy(Projectile.Center, 1200f, (n) =>
                {
                    return n.CanBeChasedBy() && !n.dontTakeDamage && Collision.CanHitLine(Projectile.Center, 1, 1, n.Center, 1, 1);
                });

                if (Vector2.Distance(Projectile.Center, player.Center) < 700)
                {
                    tar = npc;
                }

                // 如果能找到NPC进入攻击状态，否则返回玩家身边
                if (tar != null)
                {
                    State = ProjectileState.Attack;
                }
                else
                {
                    State = ProjectileState.MoveAroundPlayer;
                    // 重置攻击阶段，回到初始状态
                    CurrentAttackPhase = AttackPhase.Approach;
                    AttackPhaseTimer = 0;
                }
            }

            // 状态切换时重置计时器
            if (prevState != State)
            {
                Timer = 0;
                CurrentAttackPhase = AttackPhase.Approach; // 切换到攻击状态时默认开始贴近
                AttackPhaseTimer = 0;
                Projectile.netUpdate = true;
            }

            switch (State)
            {
                case ProjectileState.MoveAroundPlayer:
                    {
                        MoveAroundPlayer(player);
                        break;
                    }
                case ProjectileState.Attack:
                    {
                        var targetPosition = (TargetLocation == Vector2.Zero) ? tar.Center : TargetLocation;
                        // 攻击循环逻辑（贴近-远离-冷却）
                        AttackCycle(targetPosition - Projectile.Center);
                        // 近战攻击判定
                        MeleeAttackAround(targetPosition - Projectile.Center);
                        break;
                    }
            }

            // 速度限制和基础轨迹处理
            if (Projectile.velocity.Length() > 16)
            {
                Projectile.velocity *= 0.98f;
            }
            if (Math.Abs(Projectile.velocity.X) < 0.01f || Math.Abs(Projectile.velocity.Y) < 0.01f)
            {
                Projectile.velocity = Main.rand.NextVector2Circular(1, 1) * 2f;
                Projectile.netUpdate = true;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            // 移动特效
            if (Projectile.velocity.Length() > 6)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Torch, -Projectile.velocity.X, -Projectile.velocity.Y, 100, Color.Red, 1.0f);
                dust.noGravity = true;
                dust.position = Projectile.Center - Projectile.velocity;
            }
        }

        /// <summary>
        /// 攻击循环逻辑：贴近→远离→冷却→再贴近
        /// </summary>
        /// <param name="diff">召唤物到目标的向量</param>
        private void AttackCycle(Vector2 diff)
        {
            float distance = diff.Length();
            diff.Normalize();

            // 攻击阶段计时器递增
            AttackPhaseTimer++;

            // 存储侧方撤退的方向（用localAI2临时存储，避免每帧随机变化）
            float retreatAngle = Projectile.localAI[2];

            switch (CurrentAttackPhase)
            {
                // 阶段1：贴近敌人（直到进入攻击距离）
                case AttackPhase.Approach:
                    {
                        // 向敌人移动，速度更快
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, diff * ApproachSpeed, 0.1f);

                        // 进入攻击距离后，切换到后撤阶段，同时确定侧方撤退方向
                        if (distance <= AttackDistance)
                        {
                            CurrentAttackPhase = AttackPhase.Retreat;
                            AttackPhaseTimer = 0;
                            // 随机选择左侧或右侧撤退（旋转90° 或 -90°）
                            retreatAngle = Main.rand.NextBool() ? MathHelper.PiOver2 : -MathHelper.PiOver2;
                            Projectile.localAI[2] = retreatAngle; // 保存方向，避免中途改变
                        }
                        break;
                    }

                // 阶段2：从敌人侧方远离（后撤到指定距离）
                case AttackPhase.Retreat:
                    {
                        // 将朝向敌人的向量，旋转到侧方 → 再反向就是侧方撤退方向
                        Vector2 sideDir = diff.RotatedBy(retreatAngle);
                        Vector2 retreatDir = -sideDir; // 侧方撤退的最终方向

                        // 沿着侧方方向撤退
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, retreatDir * RetreatSpeed, 0.1f);

                        // 后撤到目标距离，或移动超时（防止卡墙），切换到冷却阶段
                        if (distance >= RetreatDistance || AttackPhaseTimer > 30)
                        {
                            CurrentAttackPhase = AttackPhase.Cooldown;
                            AttackPhaseTimer = 0; // 重置冷却计时器
                        }
                        break;
                    }

                // 阶段3：冷却1秒后，重新贴近
                case AttackPhase.Cooldown:
                    {
                        // 冷却期间缓慢移动，保持位置
                        Projectile.velocity *= 0.95f;

                        // 1秒（60帧）冷却结束，回到贴近阶段
                        if (AttackPhaseTimer >= RetreatDuration)
                        {
                            CurrentAttackPhase = AttackPhase.Approach;
                            AttackPhaseTimer = 0; // 重置阶段计时器
                            Projectile.localAI[2] = 0; // 清空侧方方向缓存
                        }
                        break;
                    }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var tex = TextureAssets.Projectile[Type].Value;
            var rot = Projectile.rotation + (float)Math.PI / 2f;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, rot,
                tex.Size() / 2f, Projectile.scale, 0, 0);
            return false;
        }

        // 保留原有远程射击方法（备用）
        public void ShootAround(Vector2 diff)
        {
            Timer++;
            float distance = diff.Length();
            diff.Normalize();
            Projectile.rotation = diff.ToRotation();
            if (Timer % 30 < 1)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(),
                    Projectile.Center + Projectile.velocity + diff * 30, diff * 13f,
                    ProjectileID.GreenLaser,
                    Projectile.damage + 5, Projectile.knockBack, Projectile.owner);
            }
            if (distance > 500)
            {
                Projectile.velocity = (Projectile.velocity * 20f + diff * 5) / 21f;
            }
            else
            {
                Projectile.velocity *= 0.97f;
            }
            if (distance > 200)
            {
                Projectile.velocity = (Projectile.velocity * 40f + diff * 5) / 41f;
            }
            else if (distance < 180)
            {
                Projectile.velocity = (Projectile.velocity * 20f + diff * -4) / 21f;
            }
        }

        // 攻击冷却计时器
        private int attackCooldown = 0;

        /// <summary>
        /// 近战攻击逻辑（保留原有，仅移除重复的移动控制）
        /// </summary>
        /// <param name="diff"></param>
        public void MeleeAttackAround(Vector2 diff)
        {
            Timer++;
            attackCooldown = Math.Max(0, attackCooldown - 1);

            float distance = diff.Length();
            diff.Normalize();
            Projectile.rotation = diff.ToRotation();

            // 仅在贴近阶段且冷却结束时触发攻击
            if (CurrentAttackPhase == AttackPhase.Approach && attackCooldown <= 0 && distance < AttackDistance)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.CanBeChasedBy())
                    {
                        float hitboxDistance = Vector2.Distance(Projectile.Center, npc.Center);
                        if (hitboxDistance < Projectile.width / 2 + npc.width / 2)
                        {
                            // 适配官方StrikeNPC接口
                            int hitDirection = diff.X > 0 ? 1 : -1; // 根据朝向确定打击方向（左右）
                            npc.SimpleStrikeNPC(
                                Projectile.damage + 10,          // 第1个参数：伤害值（近战伤害+10）
                                hitDirection,                    // 第3个参数：打击方向（1=右，-1=左）
                                false,                           // 第4个参数：是否暴击（默认false）
                                0f,
                                //Projectile.knockBack,            // 第2个参数：击退力
                                DamageClass.Summon,                           // 第5个参数：伤害种类
                                false                            // 第6个参数：是否禁止玩家交互（默认false）
                            );

                            // 攻击特效
                            for (int i = 0; i < 5; i++)
                            {
                                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                                             DustID.Firefly, diff.X * 2, diff.Y * 2, 0, default, 1f);
                            }

                            attackCooldown = 15; // 攻击冷却0.25秒
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绕玩家移动逻辑
        /// </summary>
        /// <param name="player"></param>
        private void MoveAroundPlayer(Player player)
        {
            Vector2 diff = Projectile.Center - player.Center;
            diff.Normalize();
            Projectile.velocity -= diff * 0.2f;

            if (Projectile.Center.X < player.Center.X)
            {
                Projectile.velocity.X += _nearPlayerSpeed;
            }
            if (Projectile.Center.X > player.Center.X)
            {
                Projectile.velocity.X -= _nearPlayerSpeed;
            }
            if (Projectile.Center.Y < player.Center.Y)
            {
                Projectile.velocity.Y += _nearPlayerSpeed;
            }
            if (Projectile.Center.Y > player.Center.Y)
            {
                Projectile.velocity.Y -= _nearPlayerSpeed;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(TargetLocation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            TargetLocation = reader.ReadVector2();
        }
    }
}