using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using WBHMODE.Content.Buffs;
using Terraria.ID;
using Terraria.DataStructures;
using WBHMODE.Content.Projectiles;

namespace WBHMODE.Common.Players
{
    public class ModGlobalPlayer : ModPlayer
    {
        public bool composureRingBuff;
        public int composureRingBuffTimer;
        public override void ResetEffects()
        {
            composureRingBuff = false;
            composureRingBuffTimer = 0;
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            if (composureRingBuff && composureRingBuffTimer == 0)
            {
#if DEBUG
                Player.AddBuff(BuffID.Regeneration, 120);
#endif
                composureRingBuffTimer = 180;

                IEntitySource source = Player.GetSource_FromThis();
                Vector2 unit = Vector2.UnitX; // 这是（1,0）
                int p = Projectile.NewProjectile(source, Player.Center, -unit, ModContent.ProjectileType<ComposureRingProjectile>(), 1, 10);
            }
        }
        public override void PreUpdate()
        {
            if (composureRingBuffTimer > 0)
            {
                composureRingBuffTimer--;
            }
            if (composureRingBuffTimer < 0)
            {
                composureRingBuffTimer = 0;
            }
        }
    }
}
