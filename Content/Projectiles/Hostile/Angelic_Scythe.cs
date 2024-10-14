using System;
using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Ferustria.Content.Buffs.Negatives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Hostile
{
	public class Angelic_Scythe : ModProjectile
	{
		public override string Texture => Ferustria.Paths.TexturesPathPrj + "Angelic_Scythe";
		private bool set = false;
		private Vector2 heldVel;
		private float acc;


		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 0;
			Projectile.scale = 1f;
		}


		public override void OnKill(int timeLeft)
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
					acc += 0.0085f;
					Projectile.velocity = acc * heldVel * 2.5f;
				}
				
            }
			float rotSpd = (((Projectile.velocity.X + Projectile.velocity.Y) / 8) + (0.1f * Projectile.direction));
			if (Math.Abs(rotSpd) >= 0.65f) rotSpd = 0.65f * Projectile.direction;
			Projectile.rotation += rotSpd;

			if (Main.rand.NextFloat() < .4f)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Angelic_Particles>(), Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.72f, 1.08f));
			}
			if (Math.Abs(Projectile.velocity.X) >= Math.Abs(heldVel.X)) Projectile.velocity.X = heldVel.X;
			if (Math.Abs(Projectile.velocity.Y) >= Math.Abs(heldVel.Y)) Projectile.velocity.Y = heldVel.Y;
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Sliced_Defense>(), Main.rand.Next(8, 15) * 60);
        }
	}

}