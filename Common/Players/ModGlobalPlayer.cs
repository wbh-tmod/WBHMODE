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
using System.Collections.Specialized;
using Terraria.WorldBuilding;

namespace WBHMODE.Common.Players
{
    public class ModGlobalPlayer : ModPlayer
    {
        public bool composureRingBuff;
        public bool composureRingDebuff;
        public int silenceShield;
        public bool silenceArmorBuff;
        public bool silenceArmorTimerBuff;
        public int silenceTimer;

        public bool VirtualArmyBuff;
        //public int composureRingBuffTimer;
        public override void ResetEffects()
        {
            composureRingBuff = false;
            composureRingDebuff = false;
            silenceShield = 0;
            silenceArmorBuff = false;
            silenceArmorTimerBuff = false;
            silenceTimer = 0;
            //composureRingBuffTimer = 0;
            VirtualArmyBuff = false;
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            //if (composureRingBuff && composureRingBuffTimer == 0)
            if (composureRingBuff)
            {
#if DEBUG
                Player.AddBuff(BuffID.Regeneration, 120);
#endif
                //composureRingBuffTimer = 180;
                Player.AddBuff(ModContent.BuffType<ComposureRingDebuff>(), 180);

                IEntitySource source = Player.GetSource_FromThis();
                Vector2 unit = Vector2.UnitX; // 这是（1,0）
                int p = Projectile.NewProjectile(source, Player.Center, -unit, ModContent.ProjectileType<ComposureRingProjectile>(), 1, 10);
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (silenceArmorBuff)
            {
                if (!silenceArmorTimerBuff)
                    silenceShield = 10;
                if (silenceShield > 0)
                    modifiers.ModifyHurtInfo += ModifyHurtInfo_Mod;
            }
        }
        private void ModifyHurtInfo_Mod(ref Player.HurtInfo info)
        {
            // Boss Rush's damage floor is implemented as a dirty modifier
            // TODO -- implementing this correctly would require fully reimplementing all of DR and ADR
            if (info.Damage <= silenceShield)
            {
                silenceShield -= info.Damage;
                info.Cancelled = true;
#if DEBUG
                Player.AddBuff(BuffID.Gills, 180);
#endif
            }
            else
            {
                info.Damage -= silenceShield;
                silenceShield = 0;
#if DEBUG
                Player.AddBuff(BuffID.ObsidianSkin, 180);
#endif
            }
        }
        public override void PreUpdate()
        {
            if (silenceArmorBuff)
            {
                if (silenceShield != 10 && !silenceArmorTimerBuff)
                {
                    Player.AddBuff(ModContent.BuffType<SilenceArmorTimerBuff>(), 600);
                }
            }
            //if (composureRingBuffTimer > 0)
            //{
            //    composureRingBuffTimer--;
            //}
            //if (composureRingBuffTimer < 0)
            //{
            //    composureRingBuffTimer = 0;
            //}
        }
    }
}
