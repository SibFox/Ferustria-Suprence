using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Friendly
{
	public class Rot_Petal : ModProjectile
	{
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
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 7;
		}


		public override void OnKill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			//Main.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
		{
			Projectile.velocity.X *= 0.992f;
			Projectile.velocity.Y += 0.14f;
			if (Projectile.velocity.Y > 20f) Projectile.velocity.Y = 20f;
			Projectile.rotation += 0.15f;
			if (Main.rand.NextBool())
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Rot_Particles>(), Projectile.velocity.X * 1.2f,
				Projectile.velocity.Y / 1.2f, 60, default, 1f);
		}

	}

}