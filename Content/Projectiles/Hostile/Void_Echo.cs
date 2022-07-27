using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Ferustria.Content.Buffs.Negatives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Ferustria.Content.Projectiles.Hostile
{
	public class Void_Echo : ModProjectile
	{
		public override string Texture => "Ferustria/Assets/Textures/Void_Echo";
		/*private static float scale = 0.85f;
		private int hitbox = (int)(14 * scale);*/

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Echo");
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.scale = 0.9f;
			Projectile.aiStyle = ProjectileID.Bullet;
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 3;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			DrawOffsetX = -2;
			DrawOriginOffsetY = -6;
		}

		private void Shrink()
		{
			Projectile.scale *= 0.86f;
			/*Projectile.width = (int)(14.5f * Projectile.scale);
			Projectile.height = (int)(14.5f * Projectile.scale);*/
			Projectile.damage = (int)(Projectile.damage * Projectile.scale * 1.5f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X * 1.05f;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y * 1.015f;
				}
			Projectile.velocity *= .97f;
			Shrink();
			return false;
		}

		
		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
		{
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			if (Main.rand.NextFloat() < .75f)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Void_Particles>(), Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.52f, .95f));
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			Projectile.penetrate--;
			Projectile.velocity *= 0.9f;
			target.AddBuff(ModContent.BuffType<Weak_Void_Leach>(), Main.rand.Next(4, 10) * 60);
			Shrink();
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.penetrate--;
			Projectile.velocity *= 0.9f;
			target.AddBuff(ModContent.BuffType<Weak_Void_Leach>(), Main.rand.Next(4, 10) * 60);
			Shrink();
		}

	}
	
}