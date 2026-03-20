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

/*
 * 虚像兵最远可以锁定120格的敌怪，距离玩家60格以外2s后爆炸，并重新生成在玩家身侧；
 * 距离玩家60格以内时，虚像兵倾向于靠近敌怪造成碰撞伤害（做一个攻击动画），击中敌怪时会造成15嘀嗒的局部无敌帧；
 * 虚像兵会在下列情况中发起高速冲刺：1.距离玩家30格以外超过2s且锁定敌怪时；
 * 2.目标敌怪与自身距离超过30格时，虚像兵的冲刺会造成更高的伤害且对路径上的所有敌怪造成碰撞伤害和12嘀嗒静态无敌帧
 * 每两次冲刺之间至少存在2s间隔，且总会在对目标敌怪造成伤害后沿原方向继续移动2s格时停下，并尝试继续造成碰撞伤害。
*/

namespace WBHMODE.Content.Projectiles
{
    public class VirtualSoldier : ModProjectile
    {
        public enum ProjectileState
        {
            MoveAroundPlayer,
            Attack, // 重命名为Attack更贴合近战逻辑，包含贴近+远离循环
            Dash,
            Explode,
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

        private const int ExplodeDistance = 60;      // 自爆距离
        private const int ExplodeTimer = 2;          // 自爆计时

        private const float MaxView = 100 * 16f;       // 最大索敌视野
                                                       // 攻击相关常量（可根据需求调整）
        private const float CollisionDistance = 50 * 16f; // 近距离/远距离分界（单位：格）
        private int CloseAttackDamage = 20;    // 近距离冲撞伤害
        private int LongAttackDamage = 40;     // 远距离冲刺伤害
        private int CloseHitInvincibility = 15; // 近距离击中无敌帧（嘀嗒）
        private int LongHitInvincibility = 12;  // 远距离击中无敌帧（嘀嗒）
        private float DashCooldown = 2f;       // 冲刺冷却（秒）
        private float DashMoveDistance = 2f;   // 冲刺后额外移动距离（格）

        private int pos = 0; // 击退方向
        private const int knockBack = 2; // 击退力
        // 状态控制变量
        private float _dashCooldownTimer = 0f; // 冲刺冷却计时器（帧，1秒=60帧）
        private bool _isDashing = false;       // 是否正在冲刺
        private Vector2 _dashDirection = Vector2.Zero; // 冲刺方向
        private float _dashMoveTimer = 0f;     // 冲刺后移动计时器
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
            Projectile.localNPCHitCooldown = 15;
            Projectile.idStaticNPCHitCooldown = 12;
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
            //if (State == ProjectileState.Explode)
            //{
            //    Explode(player);
            //    State = ProjectileState.MoveAroundPlayer;
            //}
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
            pos = 0; // 击退方向
            NPC npc = FindCloestEnemy(Projectile.Center, MaxView, (n) =>
            {
                return n.CanBeChasedBy() && !n.dontTakeDamage;
                //return n.CanBeChasedBy() && !n.dontTakeDamage && Collision.CanHitLine(Projectile.Center, 1, 1, n.Center, 1, 1);
            });
            tar = npc;
            // 如果能找到NPC进入攻击状态，否则返回玩家身边
            if (tar != null)
            {
                State = ProjectileState.Attack;
                if (tar.Center.X < Projectile.Center.X)
                {
                    // tar在Projectile左侧
                    pos = 1;
                }
                else if (tar.Center.X > Projectile.Center.X)
                {
                    // tar在Projectile右侧
                    pos = -1;
                }
            }
            else
            {
                State = ProjectileState.MoveAroundPlayer;
                // 重置攻击阶段，回到初始状态
                CurrentAttackPhase = AttackPhase.Approach;
                AttackPhaseTimer = 0;
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
                        //var targetPosition = (TargetLocation == Vector2.Zero) ? tar.Center : TargetLocation;
                        //// 攻击循环逻辑（贴近-远离-冷却）
                        //AttackCycle(targetPosition - Projectile.Center);
                        //// 近战攻击判定
                        //MeleeAttackAround(targetPosition - Projectile.Center);
                        // 先更新冷却计时器（每帧递减）
                        if (_dashCooldownTimer > 0)
                        {
                            _dashCooldownTimer--;
                        }

                        // 如果没有锁定目标，切回绕玩家移动状态
                        if (tar == null || !tar.active || tar.friendly)
                        {
                            State = ProjectileState.MoveAroundPlayer;
                            _isDashing = false; // 重置冲刺状态
                            break;
                        }

                        // 计算敌怪与玩家的距离（转换为格：像素/16）
                        float distanceToPlayer = Vector2.Distance(tar.Center, Main.player[Projectile.owner].Center);

                        // 1. 近距离冲撞攻击（敌怪在CollisionDistance格内）
                        if (distanceToPlayer <= CollisionDistance && !_isDashing)
                        {
                            //Main.NewText("Close:" + distanceToPlayer / 16f);
                            CloseRangeRamAttack(tar, pos);
                        }
                        // 2. 远距离冲刺攻击（敌怪超出范围，且冲刺冷却完成）
                        else if (distanceToPlayer > CollisionDistance && _dashCooldownTimer <= 0)
                        {
                            //Main.NewText("Far:" + distanceToPlayer / 16f);
                            LongRangeDashAttack(tar, pos);
                        }
                        // 3. 处理冲刺后的持续移动
                        else if (_isDashing)
                        {
                            HandleDashAfterMove(pos);
                        }

                        break;
                    }
                case ProjectileState.Explode:
                    {
                        Main.NewText("Explode!");
                        Explode(player);
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
        // 保留原有配置常量（需确保类中已定义）
        private const float ReturnToPlayerSpeed = 7f; // 新增：返回玩家速度

        private void AttackCycle(Vector2 diff)
        {
            float distance = diff.Length();
            diff.Normalize();

            // 攻击阶段计时器递增
            AttackPhaseTimer++;

            // 存储侧方撤退的方向（用localAI2临时存储，避免每帧随机变化）
            float retreatAngle = Projectile.localAI[2];

            // 获取玩家实例（核心新增：冷却阶段需要向玩家移动）
            Player player = Main.player[Projectile.owner];
            // 计算到玩家的向量（新增）
            Vector2 toPlayerDiff = player.Center - Projectile.Center;
            toPlayerDiff.Normalize();

            switch (CurrentAttackPhase)
            {
                // 阶段1：贴近敌人（逻辑不变）
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

                // 阶段2：从敌人侧方远离（逻辑不变）
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

                // 阶段3：冷却阶段 → 修改为：移动到玩家身边
                case AttackPhase.Cooldown:
                    {
                        // 核心修改：向玩家中心移动（平滑插值保证移动流畅）
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, toPlayerDiff * ReturnToPlayerSpeed, 0.15f);

                        // 可选优化：到达玩家附近（如30像素内）则提前结束冷却
                        float distanceToPlayer = Vector2.Distance(Projectile.Center, player.Center);
                        bool reachPlayer = distanceToPlayer <= 30f;

                        // 1秒（60帧）冷却结束，或已到达玩家身边 → 回到贴近阶段
                        if (AttackPhaseTimer >= RetreatDuration || reachPlayer)
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
                            npc.AddBuff(ModContent.BuffType<DerelictDebuff>(), 300);

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
        /// 自爆逻辑（修复核心：瞬移到玩家身边）
        /// </summary>
        /// <param name="player"></param>
        private void Explode(Player player)
        {
            // 1. 播放爆炸特效（可选）
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                             DustID.Ash, Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4),
                             100, Color.OrangeRed, 1.5f);
            }

            // 2. 核心修复：瞬移到玩家身侧（偏移随机位置避免重叠）
            Vector2 playerSidePos = player.Center + new Vector2(
                Main.rand.Next(-30, 30), // X轴偏移
                Main.rand.Next(-30, 30)  // Y轴偏移
            );

            // 确保瞬移位置合法（无墙体阻挡）
            if (Collision.CanHit(player.Center, 1, 1, playerSidePos, 1, 1))
            {
                Projectile.Center = playerSidePos;
            }
            else
            {
                // 如果身侧有墙体，直接瞬移到玩家中心
                Projectile.Center = player.Center;
            }

            // 3. 重置速度和状态
            Projectile.velocity = Vector2.Zero;
            State = ProjectileState.MoveAroundPlayer;
            CurrentAttackPhase = AttackPhase.Approach;
            AttackPhaseTimer = 0;

            // 4. 网络同步：确保多人游戏下位置更新
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
            }

            //Main.NewText("虚像兵自爆后重新出现在玩家身边！");
        }


        /// <summary>
        /// 绕玩家移动逻辑
        /// </summary>
        /// <param name="player"></param>
        //private void MoveAroundPlayer(Player player)
        //{
        //    Vector2 diff = Projectile.Center - player.Center;
        //    //Main.NewText("Dis: " +(int) diff.Length()/16);
        //    if (diff.Length() > ExplodeDistance * 16f && Timer < ExplodeTimer * 60) // 距离过大但不够计时
        //    {
        //        Timer++;
        //        //Main.NewText("Timer:" + Timer);
        //    }
        //    else if (diff.Length() <= ExplodeDistance * 16f) 
        //    {
        //        Timer = 0;
        //    }
        //    else
        //    {
        //        //Main.NewText("Explode");
        //        Timer = 0;
        //        State = ProjectileState.Explode;
        //        return;
        //    }
        //    diff.Normalize();
        //    Projectile.velocity -= diff * 0.2f;

        //    if (Projectile.Center.X < player.Center.X)
        //    {
        //        Projectile.velocity.X += _nearPlayerSpeed;
        //    }
        //    if (Projectile.Center.X > player.Center.X)
        //    {
        //        Projectile.velocity.X -= _nearPlayerSpeed;
        //    }
        //    if (Projectile.Center.Y < player.Center.Y)
        //    {
        //        Projectile.velocity.Y += _nearPlayerSpeed;
        //    }
        //    if (Projectile.Center.Y > player.Center.Y)
        //    {
        //        Projectile.velocity.Y -= _nearPlayerSpeed;
        //    }
        //    // 定义速度上限常量（便于后续调整）
        //    float maxSpeed = 4f;

        //    // 计算当前速度向量的长度（勾股定理：√(x² + y²)）
        //    float currentSpeed = Projectile.velocity.Length();

        //    // 如果当前速度超过上限，等比缩放到maxSpeed
        //    if (currentSpeed > maxSpeed && currentSpeed > 0) // 避免除以0
        //    {
        //        // 等比缩放公式：新速度 = 原速度方向 * 最大速度
        //        // 原速度方向 = 原速度 / 原速度长度
        //        Projectile.velocity = Projectile.velocity / currentSpeed * maxSpeed;
        //    }
        //}
        // 新增角度变量（类中定义）
        private float _orbitAngle = 0f;
        // 公转基础速度（正数=逆时针，负数=顺时针）
        private float _orbitSpeed = 0.05f;
        // 公转半径（5格 = 5 * 16像素）
        private float _orbitRadius = 5 * 16f;
        // 移动平滑系数
        private float _moveSmoothness = 0.1f;
        // 记录当前最优旋转方向（避免频繁切换）
        private int _optimalDirection = 1; // 1=逆时针，-1=顺时针
                                           // 随机数生成器（类中定义，确保随机性稳定）
        private Random _random = new Random();

        private void MoveAroundPlayer(Player player)
        {
            // ========== 新增：随机切换旋转方向逻辑 ==========
            // 每个滴答（帧）有1/1000的概率切换方向
            int randomValue = _random.Next(1000); // 生成0-999的随机数
            if (randomValue == 0) // 只有随机数为0时触发（概率1/1000）
            {
                //Main.NewText("CHANGE!");
                _optimalDirection *= -1; // 切换方向：1变-1，-1变1
            }

            // 1. 计算召唤物当前位置对应的角度
            Vector2 diffFromPlayer = Projectile.Center - player.Center;
            float currentAngle = (float)Math.Atan2(diffFromPlayer.Y, diffFromPlayer.X);

            // 2. 计算目标角度（理想公转位置）
            float targetAngle = _orbitAngle;

            // 3. 计算两个方向的角度差（取最小差值，避免绕整圆）
            float clockwiseDiff = MathHelper.WrapAngle(targetAngle - currentAngle); // 顺时针差值
            float counterClockwiseDiff = MathHelper.WrapAngle(currentAngle - targetAngle); // 逆时针差值

            // 4. 判断更平滑的旋转方向（仅在未随机切换时生效）
            // 注：随机切换后会优先使用新方向，直到下一次随机或平滑逻辑重新主导
            //if (Math.Abs(clockwiseDiff) < Math.Abs(counterClockwiseDiff) && randomValue != 0)
            //{
            //    _optimalDirection = -1; // 顺时针更平滑
            //}
            //else if (randomValue != 0)
            //{
            //    _optimalDirection = 1; // 逆时针更平滑
            //}

            // 5. 更新公转角度（使用当前方向，包含随机切换后的方向）
            _orbitAngle += _orbitSpeed * _optimalDirection;
            // 重置角度避免数值过大（简化写法，等价于原逻辑）
            _orbitAngle = MathHelper.WrapAngle(_orbitAngle);

            // 6. 计算公转的目标位置
            float targetX = player.Center.X + (float)Math.Cos(_orbitAngle) * _orbitRadius;
            float targetY = player.Center.Y + (float)Math.Sin(_orbitAngle) * _orbitRadius;
            Vector2 targetPos = new Vector2(targetX, targetY);

            // 7. 平滑移动到目标位置
            Vector2 moveDiff = targetPos - Projectile.Center;
            Projectile.velocity = moveDiff * _moveSmoothness;

            // 8. 速度上限控制
            float maxSpeed = 4f;
            float currentSpeed = Projectile.velocity.Length();
            if (currentSpeed > maxSpeed && currentSpeed > 0)
            {
                Projectile.velocity = Projectile.velocity / currentSpeed * maxSpeed;
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

        // ========== 近距离冲撞攻击函数 ==========
        /// <summary>
        /// 近距离冲撞攻击：向目标移动并造成碰撞伤害，击中后目标获得15嘀嗒无敌帧
        /// </summary>
        /// <param name="target">锁定的敌怪</param>
        private void CloseRangeRamAttack(NPC target, int hitDirection)
        {
            // 计算向目标移动的方向（归一化，避免速度过快）
            Vector2 moveDir = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);

            // 设置冲撞速度（可调整，建议3-5）
            Projectile.velocity = moveDir * 4f;

            // 检测碰撞并造成伤害
            if (Projectile.Hitbox.Intersects(target.Hitbox) && !target.immortal && !target.dontTakeDamage)
            {
                // 造成伤害（参数：伤害值、击退、无敌帧）

                //target.SimpleStrikeNPC(
                //            Projectile.damage + 10,          // 第1个参数：伤害值（近战伤害+10）
                //                                             //hitDirection,                    // 第3个参数：打击方向（1=右，-1=左）
                //            hitDirection,
                //            false,                           // 第4个参数：是否暴击（默认false）
                //            knockBack,
                //            //Projectile.knockBack,            // 第2个参数：击退力
                //            DamageClass.Summon,                           // 第5个参数：伤害种类
                //            false                            // 第6个参数：是否禁止玩家交互（默认false）
                //        );
                //target.StrikeNPC(CloseAttackDamage, 0f, 0, false, false, false);
                //// 设置15嘀嗒局部无敌帧（只对当前召唤物生效）
                //target.immune[Projectile.owner] = CloseHitInvincibility;
                //target.immuneTime = CloseHitInvincibility;

                // 冲撞后短暂停住（可选，增加打击感）
                Projectile.velocity = Vector2.Zero;
            }
        }

        // ========== 远距离冲刺攻击函数 ==========
        /// <summary>
        /// 远距离冲刺攻击：高速冲刺，对路径上所有敌怪造成伤害，击中后目标获得12嘀嗒静态无敌帧
        /// </summary>
        /// <param name="target">锁定的敌怪</param>
        private void LongRangeDashAttack(NPC target, int hitDirection)
        {
            // 标记开始冲刺
            _isDashing = true;
            // 记录冲刺方向
            _dashDirection = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
            // 设置冲刺速度（比近距离更快，建议8-10）
            Projectile.velocity = _dashDirection * 40f;
            // 重置冲刺后移动计时器（2格距离对应的移动时长，根据速度计算）
            _dashMoveTimer = (DashMoveDistance * 16f) / Projectile.velocity.Length();
            // 启动冲刺冷却（2秒 = 120帧）
            _dashCooldownTimer = DashCooldown * 60f;

            // 检测冲刺路径上的所有敌怪并造成伤害
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.immortal || npc.dontTakeDamage)
                    continue;

                // 检测碰撞（冲刺路径上的敌怪）
                if (Projectile.Hitbox.Intersects(npc.Hitbox))
                {
                    // 造成更高的远距离伤害
                    ////npc.StrikeNPC(LongAttackDamage, 0f, 0, false, false, false);
                    //npc.SimpleStrikeNPC(
                    //            Projectile.damage + 10,          // 第1个参数：伤害值（近战伤害+10）
                    //            //hitDirection,                    // 第3个参数：打击方向（1=右，-1=左）
                    //            hitDirection,
                    //            false,                           // 第4个参数：是否暴击（默认false）
                    //            knockBack,
                    //            //Projectile.knockBack,            // 第2个参数：击退力
                    //            DamageClass.Summon,                           // 第5个参数：伤害种类
                    //            false                            // 第6个参数：是否禁止玩家交互（默认false）
                    //        );
                    // 设置12嘀嗒静态无敌帧
                    //npc.immune[Projectile.owner] = LongHitInvincibility;
                    //npc.immuneTime = LongHitInvincibility;

                    // 如果击中的是锁定目标，标记冲刺伤害完成（可选）
                    if (npc.whoAmI == target.whoAmI)
                    {
                        // 可添加击中目标后的特效/逻辑
                    }
                }
            }
        }

        // ========== 处理冲刺后持续移动逻辑 ==========
        /// <summary>
        /// 冲刺击中目标后，沿原方向继续移动2格后停下，并尝试继续造成碰撞伤害
        /// </summary>
        private void HandleDashAfterMove(int hitDirection)
        {
            // 沿原冲刺方向继续移动
            Projectile.velocity = _dashDirection * 9f;
            // 递减移动计时器
            _dashMoveTimer--;

            // 继续检测路径上的敌怪伤害
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.immortal || npc.dontTakeDamage)
                    continue;

                if (Projectile.Hitbox.Intersects(npc.Hitbox))
                {
                    npc.SimpleStrikeNPC(
                                Projectile.damage + 10,          // 第1个参数：伤害值（近战伤害+10）
                                                                 //hitDirection,                    // 第3个参数：打击方向（1=右，-1=左）
                                1,
                                false,                           // 第4个参数：是否暴击（默认false）
                                knockBack,
                                //Projectile.knockBack,            // 第2个参数：击退力
                                DamageClass.Summon,                           // 第5个参数：伤害种类
                                false                            // 第6个参数：是否禁止玩家交互（默认false）
                            );
                    //npc.StrikeNPC(LongAttackDamage, 0f, 0, false, false, false);
                    //npc.immune[Projectile.owner] = LongHitInvincibility;
                    //npc.immuneTime = LongHitInvincibility;
                }
            }

            // 移动时长耗尽，停止冲刺
            if (_dashMoveTimer <= 0)
            {
                _isDashing = false;
                Projectile.velocity = Vector2.Zero; // 停下
                _dashDirection = Vector2.Zero;     // 重置冲刺方向
            }
        }
    }
}
