using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Ferustria.Content.Buffs.Negatives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Ferustria.Content.Projectiles.Friendly
{
	public class Void_Opposite_Bounce : ModProjectile
	{
		public override string Texture => "Ferustria/Assets/Textures/Projectiles/Void_Echo";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jumpy Void Echo");
		}

		public override void SetDefaults()
		{
			Projectile.width = 13;
			Projectile.height = 13;
			Projectile.aiStyle = 0;
			Projectile.scale = 0.9f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 400;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.knockBack = 2.2f;
			Projectile.alpha = 255;
			Projectile.ai[1] = 0.9f;
			DrawOffsetX = -2;
			DrawOriginOffsetY = -6;
		}


		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			if (Projectile.ai[0] != 1f && Projectile.ai[0] != 0.9f && Projectile.owner == Main.myPlayer)
			{
				for (int i = 0; i < 4; i++)
				{
					float kos = Main.rand.NextFloat(0.6f, 1.025f);
					float speedX = -Projectile.velocity.X * Main.rand.NextFloat(.7f, 0.9f) + Main.rand.NextFloat(-2f, 2f);
					float speedY = -Projectile.velocity.Y * Main.rand.NextFloat(.7f, 0.9f) + Main.rand.NextFloat(-2f, 2f);
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + speedX, Projectile.position.Y + speedY, speedX, speedY, ModContent.ProjectileType<Void_Opposite_Bounce>(), (int)(Projectile.damage * kos), 1.2f, Projectile.owner, 1f, kos);
					Main.projectile[proj].timeLeft = 70;
				}
				float koso = Main.rand.NextFloat(0.6f, 1.025f);
				float speedXo = Projectile.velocity.X;
				float speedYo = Projectile.velocity.Y;
				if (Projectile.velocity.X != Projectile.oldVelocity.X)
				{
					speedXo = -Projectile.oldVelocity.X * Main.rand.NextFloat(.5f, 0.75f) + Main.rand.NextFloat(-3.5f, 3.5f);
				}
				if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
				{
					speedYo = -Projectile.oldVelocity.Y * Main.rand.NextFloat(.4f, 0.6f) + Main.rand.NextFloat(-3.5f, 3.5f);
				}
				
				int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + speedXo, Projectile.position.Y + speedYo, speedXo, speedYo, ModContent.ProjectileType<Void_Opposite_Bounce>(), (int)(Projectile.damage * koso), 1.2f, Projectile.owner, 1f, koso);
				Main.projectile[proj2].timeLeft = 80;
			}
		}

        public override void AI()
		{
			Projectile.netUpdate = true;
			if (Projectile.ai[0] == 1f)
			{
				Projectile.alpha = 50;
				Projectile.scale = Projectile.ai[1];
				int hitbox = (int)(14 * Projectile.scale);
				Projectile.width = hitbox;
				Projectile.height = hitbox;
				Projectile.ai[0] = 0.9f;
			}
			if (Projectile.alpha > 0)
            {
				Projectile.alpha -= 15;
            }
			if (Projectile.alpha < 0)
            {
				Projectile.alpha = 0;
            }
			++Projectile.localAI[0];
			if (++Projectile.localAI[0] >= 36f)
            {
				if (Projectile.velocity.Y <= 12.8f)
                {
					Projectile.velocity.Y += 0.25f;
				}
				Projectile.localAI[0] = 36f;
            }
			if (Main.rand.NextFloat() < .75f)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height , ModContent.DustType<Void_Particles>(), Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.52f, .95f));
			}
            Projectile.SetStraightRotation();
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			Projectile.penetrate--;
			Projectile.velocity *= 0.9f;
			target.AddBuff(ModContent.BuffType<Weak_Void_Leach>(), Main.rand.Next(4, 8) * 60);
			Projectile.scale *= .8f;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Weak_Void_Leach>(), Main.rand.Next(2, 5) * 60);
		}

	}
	
}