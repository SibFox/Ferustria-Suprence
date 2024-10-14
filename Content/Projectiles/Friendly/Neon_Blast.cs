using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;
using Ferustria.Content.Projectiles.Friendly.HealProj;

namespace Ferustria.Content.Projectiles.Friendly
{
    public class Neon_Blast : ModProjectile
	{
        public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 160;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.knockBack = 1.8f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}


        public override void AI()
		{
			if (Projectile.ai[0]++ <= 1)
            {
				Projectile.velocity = (Projectile.velocity + Projectile.velocity / 3) * 1.3f;
			}
			if (Projectile.ai[1] <= 0)
            {
				if (Projectile.alpha > 0)
				{
					Projectile.alpha -= 26;
				}
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

            Projectile.SetStraightRotation();
			//Projectile.rotation = Projectile.GetStraightRotation();

			Lighting.AddLight(Projectile.position, 0, 0.8f, 0.8f);

			if (Projectile.ai[1] > 0)
            {
				int particles = Main.rand.Next(1, 15);
				for (int i = 0; i < particles; i++)
				{
					double angle = 2.0 * Math.PI * i / particles;
					Vector2 speed = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    speed.Normalize();
                    speed *= 3.5f;
					Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Smoke, speed.X, speed.Y, 80, new(0, Main.rand.Next(180, 256), 255), Main.rand.NextFloat(1f, 1.6f));
					Projectile.rotation = 0;
				}
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Main.rand.NextFloat() <= .3f && Projectile.localAI[0] < 3 && Projectile.ai[1] > 0)
			{
				Projectile.localAI[0]++;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (0, 0), ModContent.ProjectileType<Neon_Heal>(), 0, 0, Projectile.owner);
			}
			if (Projectile.ai[1]++ <= 1 && Projectile.timeLeft > 0)
			{
				Projectile.alpha = 255;
				Projectile.velocity = new Vector2(0, 0);
				Projectile.tileCollide = false;
				Projectile.damage /= 2;
				Projectile.position = Projectile.Center;
				Projectile.width = 150;
				Projectile.height = 150;
				Projectile.Center = Projectile.position;
				Projectile.timeLeft = 45;
				Projectile.ai[1]++;
			}
			
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (Projectile.ai[1]++ <= 1 && Projectile.timeLeft > 0)
			{
				Projectile.alpha = 255;
				Projectile.velocity = new Vector2(0, 0);
				Projectile.tileCollide = false;
				Projectile.damage /= 2;
				Projectile.position = Projectile.Center;
				Projectile.width = 150;
				Projectile.height = 150;
				Projectile.Center = Projectile.position;
				Projectile.timeLeft = 45;
				Projectile.ai[1]++;
			}
			return false;
        }

    }

}