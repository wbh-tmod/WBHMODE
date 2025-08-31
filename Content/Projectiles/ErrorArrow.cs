using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Projectiles
{
	// This example is similar to the Wooden Arrow projectile
	public class ErrorArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// If this arrow would have strong effects (like Holy Arrow pierce), we can make it fire fewer projectiles from Daedalus Stormbow for game balance considerations like this:
			//ProjectileID.Sets.FiresFewerFromDaedalusStormbow[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 10; // The width of projectile hitbox
			Projectile.height = 10; // The height of projectile hitbox
			Projectile.penetrate = -1;
			Projectile.arrow = true;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
		}

		public override void AI() {
			// The code below was adapted from the ProjAIStyleID.Arrow behavior. Rather than copy an existing aiStyle using Projectile.aiStyle and AIType,
			// like some examples do, this example has custom AI code that is better suited for modifying directly.
			// See https://github.com/tModLoader/tModLoader/wiki/Basic-Projectile#what-is-ai for more information on custom projectile AI.

			// Apply gravity after a quarter of a second
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 15f) {
				Projectile.ai[0] = 15f;
				Projectile.velocity.Y += 0.1f;
			}
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] == 15f)
			{
				IEntitySource source = Projectile.GetSource_FromThis();
				Vector2 NewVelocity;
				Random ran = new Random();
				for (int i = 0; i <= 2; i++)
				{
                    int YSpeedDiff = ran.Next(-10, 10);
                    NewVelocity.Y = Projectile.velocity.Y + YSpeedDiff / 3;
                    NewVelocity.X = Projectile.velocity.X;
                    int p = Projectile.NewProjectile(source, Projectile.position, NewVelocity, ModContent.ProjectileType<ErrorArrow>(), 8, 1, -1, 15f, 16f, 0f);
                }
				
            }
				// The projectile is rotated to face the direction of travel
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			// Cap downward velocity
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
		}

		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
			for (int i = 0; i < 5; i++) // Creates a splash of dust around the position the projectile dies.
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Silver);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 0.9f;
			} 
		}
	}
}