using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Hostile
{
	public class Light_Ball_Forward : ModProjectile
	{
		private int upwards;
		public override string Texture => "Ferustria/Assets/Textures/Burning_Light_Ball";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Burning Light Ball");
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.timeLeft = 350;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 0;
			//Projectile.scale = 1.2f;
			upwards = 0;
		}



		public override void AI()
		{
            Projectile.netUpdate = true;
			if (Projectile.ai[0] == 1)
            {
				if (Projectile.scale > 0.5f && upwards == 0)
                {
					Projectile.scale -= 1f / 60;
					if (Projectile.scale <= 0.5f) upwards = 1;
				}
				if (upwards == 1)
                {
					Projectile.scale += 1.2f / 60;
				}
				if (Projectile.scale >= 1.3f) upwards = 0;
				Projectile.alpha = 255 - (int)(200 * Projectile.scale);
				Projectile.tileCollide = true;
				Projectile.velocity *= 0.992f;
			}

			if (Main.rand.NextFloat() < .45f)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Angelic_Particles>(), 
                    Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.82f, 1.15f));
			}

			Projectile.rotation += 0.15f * Projectile.direction;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			//target.AddBuff(ModContent.BuffType<Under_Crucifixion_Tier2>(), Main.rand.Next(4, 10) * 60);
		}
	}

}