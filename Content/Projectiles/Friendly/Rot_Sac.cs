using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using Ferustria.Assets.ClassTemplates;

namespace Ferustria.Content.Projectiles.Friendly
{
	public class Rot_Sac : StickyProjectile
	{
        public override void SetValues()
        {
            hitbox = 22;
            damgeType = DamageClass.Magic;
            timeLeft = 360;
            stickyTimeLeft = 120;
            hitEffectTimer = 120;
            MAX_STICKY_PROJECTILE = 3;
            SetTexture = "Assets/Textures/Projectiles/Rot_Sac";
        }

        public override void CreateTrail()
        {
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustType<Rot_Particles>(),
                    Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 80, Scale: 1.6f);
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.3f;
            }
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustType<Rot_Particles>(),
                    0, 0, 60, Scale: 1.2f);
                dust.velocity += Projectile.velocity * 0.5f;
                dust.velocity *= 0.5f;
            }
        }
        public override void Behaviour()
        {
            if (TargetWhoAmI++ >= 10)
            {
                TargetWhoAmI = 10;
                Projectile.velocity *= 0.975f;
            }
            Projectile.rotation = (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) / 2 * -Projectile.direction;
        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < Main.rand.Next(3, 7); i++)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
                    new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-2f, -6f)),
                    ProjectileType<Rot_Petal>(), 20, 0.02f, Projectile.owner);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            return false;
        }
    }
}
