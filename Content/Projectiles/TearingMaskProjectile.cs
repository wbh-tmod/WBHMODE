using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Projectiles
{
    public class TearingMaskProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.knockBack = 5f;
        }
        public override void AI()
        {
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * Projectile.direction;
            Projectile.velocity *= 0.9f;
            int randomDust = Utils.SelectRandom(Main.rand, new int[]
            {
                15,
                107
            });
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, randomDust, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }
        public override void OnKill(int timeLeft)
        {
            float spread = 90f * 0.0174f;
            double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
            double deltaAngle = spread / 8f;
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<TearingMaskFog>(), 10, 0, Projectile.owner);
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
    }
}
