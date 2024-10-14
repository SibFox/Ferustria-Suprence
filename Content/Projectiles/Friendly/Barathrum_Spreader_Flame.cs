using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Ferustria.Content.Projectiles.Friendly
{
	public class Barathrum_Spreader_Flame : ModProjectile
	{
        public override string Texture => Ferustria.emptyPixel;

		public override void SetDefaults()
		{
			Projectile.width = 15;
			Projectile.height = 15;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 30;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.knockBack = 1f;
			Projectile.penetrate = 8;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 1;
		}


		public override void AI()
		{
            for (int i = 0; i < Main.rand.Next(1, 3); i++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType<Barathrum_Particles>(), Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 
                    Alpha: 70, Scale: Main.rand.NextFloat(1.8f, 2.35f));
            }
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<Buffs.Negatives.Weak_Barathrum_Leach>(), 120);
		}


    }

}