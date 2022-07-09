using Microsoft.Xna.Framework;
using Ferustria.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Projectiles.Friendly
{
	public class Neon_Laser : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Neon Laser");
		}

		public override void SetDefaults()
		{
			Projectile.width = 56;
			Projectile.height = 6;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 120;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.knockBack = 1.8f;
			Projectile.penetrate = 2;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}


		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			//Main.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
		{
			if (Projectile.localAI[0]++ <= 1)
            {
				Projectile.velocity = (Projectile.velocity + Projectile.velocity / 3) * 1.2f;
			}
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 26;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Lighting.AddLight(Projectile.position, 0, 0.7f, 0.7f);
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

		}

    }

}