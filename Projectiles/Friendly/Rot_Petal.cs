using Microsoft.Xna.Framework;
using Ferustria.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Projectiles.Friendly
{
	public class Rot_Petal : ModProjectile
	{
		private bool rotate = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rot Petal");
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 10;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1200;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.alpha = 0;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
		}


		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			//Main.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
		{
			Projectile.velocity.X *= 0.992f;
			Projectile.velocity.Y += 0.14f;
			//else Projectile.velocity.Y *= 1.04f;
			if (Projectile.velocity.Y > 20f) Projectile.velocity.Y = 20f;
			Projectile.rotation += 0.15f;
			if (Main.rand.NextBool())
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.Rot_Particles>(), Projectile.velocity.X * 1.2f,
				Projectile.velocity.Y / 1.2f, 60, default, 1f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 5;
		}

	}

}