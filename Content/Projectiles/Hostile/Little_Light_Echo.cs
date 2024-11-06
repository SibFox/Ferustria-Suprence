using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Ferustria.Content.Buffs.Negatives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Ferustria.Content.Projectiles.Hostile
{
	public class Little_Light_Echo : ModProjectile
	{
        public override string Texture => Ferustria.Paths.TexturesPathPrj + "Burning_Light_Ball";

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30; 
			Projectile.aiStyle = 0;
			Projectile.hostile = true;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 3;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			DrawOffsetX = -5;
			DrawOriginOffsetY = -6;
            Projectile.scale = 0.5f;
		}

		public override void AI()
		{
			if (Main.rand.NextFloat() < .55f)
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Angelic_Particles>(), 
                    Projectile.velocity.X * .1f, Projectile.velocity.Y * .1f, 0, default, Main.rand.NextFloat(.42f, .75f));

            if (Projectile.velocity.Y < 16f)
                Projectile.velocity.Y += 0.17f;
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, Main.rand.Next(2, 5) * 60);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, Main.rand.Next(2, 5) * 60);
        }

	}
	
}