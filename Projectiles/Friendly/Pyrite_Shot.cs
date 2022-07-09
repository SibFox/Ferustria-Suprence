using Microsoft.Xna.Framework;
using Ferustria.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Ferustria.Projectiles.Friendly
{
	public class Pyrite_Shot : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrite Shot");
		}

		public override void SetDefaults()
		{
			Projectile.width = 70;
			Projectile.height = 2;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 400;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.knockBack = 2.5f;
			Projectile.alpha = 255;
		}


		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 30;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Lighting.AddLight(Projectile.position, 0.8f, 0.5f, 0);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextFloat() < .2f)
			{
				target.AddBuff(BuffID.OnFire, Main.rand.Next(5, 12) * 60);
			}
			else if (crit)
			{
				target.AddBuff(BuffID.OnFire, Main.rand.Next(6, 10) * 60);
				//target.AddBuff(BuffID., Main.rand.Next(6, 10) * 60); //Увеличение получаемого урона
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextFloat() < .2f)
			{
				target.AddBuff(BuffID.OnFire, Main.rand.Next(5, 12) * 60);
			}
			else if (crit)
			{
				target.AddBuff(BuffID.OnFire, Main.rand.Next(6, 14) * 60);
				//target.AddBuff(BuffID., Main.rand.Next(6, 10) * 60); //Увеличение получаемого урона
			}
		}

	}

}