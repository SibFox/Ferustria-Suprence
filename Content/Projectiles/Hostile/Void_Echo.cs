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
        public override string Texture => "Ferustria/Assets/Textures/Projectiles/Void_Echo";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Echo");
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.aiStyle = 0;
			Projectile.hostile = true;
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
			Projectile.damage = (int)(Projectile.originalDamage * Projectile.scale);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10.WithVolumeScale(0.5f), Projectile.position);
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * .35f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * .35f;
            }
            Projectile.velocity *= .97f;
			Shrink();
			return false;
		}

		
		public override void OnKill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
		{
			if (Projectile.penetrate <= 0)
				Projectile.Kill();
			if (Main.rand.NextFloat() < .75f)
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Void_Particles>(), 
                    Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.52f, .95f));

            if (Projectile.velocity.Y < 16f && Projectile.penetrate < 3)
                Projectile.velocity.Y += 0.17f;

			Projectile.SetStraightRotation();
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.penetrate--;
            Projectile.velocity *= 0.9f;
            target.AddBuff(ModContent.BuffType<Weak_Void_Leach>(), Main.rand.Next(4, 10) * 60);
            Shrink();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.penetrate--;
            Projectile.velocity *= 0.9f;
            target.AddBuff(ModContent.BuffType<Weak_Void_Leach>(), Main.rand.Next(4, 10) * 60);
            Shrink();
        }

	}
	
}