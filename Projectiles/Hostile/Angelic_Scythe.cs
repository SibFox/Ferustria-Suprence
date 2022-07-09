using System;
using Microsoft.Xna.Framework;
using Ferustria.Dusts;
using Ferustria.Buffs.Negatives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Projectiles.Hostile
{
	public class Angelic_Scythe : ModProjectile
	{
		public override string Texture => "Ferustria/Projectiles/Textures/Angelic_Scythe";
		private bool set = false;
		private Vector2 heldVel;
		private float acc;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Burning Light Ball");
		}

		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.timeLeft = 650;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 0;
			Projectile.scale = 1f;
		}


		public override void Kill(int timeLeft)
		{
			//Main.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
		{
			if (!set)
            {
				heldVel = Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
				set = true;
				heldVel *= 3;
            }
            else
            {
				if (Projectile.ai[0]++ >= 25)
                {
					acc += 0.007f;
					Projectile.velocity = acc * heldVel * 3;
				}
				
            }
			float rotSpd = (((Projectile.velocity.X + Projectile.velocity.Y) / 15) + (0.1f * Projectile.direction)) * Projectile.direction;
			if (Math.Abs(rotSpd) >= 1f) rotSpd = 1f * Projectile.direction;
			Projectile.rotation += rotSpd;

			if (Main.rand.NextFloat() < .4f)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Angelic_Particles>(), Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.72f, 1.08f));
			}
			if (Math.Abs(Projectile.velocity.X) >= Math.Abs(heldVel.X)) Projectile.velocity.X = heldVel.X;
			if (Math.Abs(Projectile.velocity.Y) >= Math.Abs(heldVel.Y)) Projectile.velocity.Y = heldVel.Y;
			//Projectile.rotation += 0.15f * Projectile.direction;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			//target.AddBuff(ModContent.BuffType<Under_Crucifixion_Tier2>(), Main.rand.Next(4, 10) * 60);
		}
	}

}