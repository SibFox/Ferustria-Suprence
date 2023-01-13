using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using Ferustria.Content.Projectiles.Friendly.HealProj;
using Ferustria.Assets.ClassTemplates;

namespace Ferustria.Content.Projectiles.Friendly
{
    public class Blood_Needle : StickyProjectile
	{
        public override void SetValues()
        {
            hitbox = 10;
            scale = 0.8f;
            damgeType = DamageClass.Magic;
            timeLeft = 220;
            stickyTimeLeft = 180;
            hitEffectTimer = 25;
            MAX_STICKY_PROJECTILE = 4;
            SetTexture = "Assets/Textures/Projectiles/Blood_Needle";
        }

        public override void HitEffect()
        {
            Main.npc[TargetWhoAmI].HitEffect(0, 1.0);
            if (Main.rand.NextBool(6))
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<Crimson_Heal>(), 0, 0, Projectile.owner);
                Main.npc[TargetWhoAmI].AddBuff(BuffType<Rapid_Blood_Loss>(), 45);
            }
        }

        public override void OnFirstNPCHit()
        {
            Main.npc[TargetWhoAmI].AddBuff(BuffType<Rapid_Blood_Loss>(), 45);
        }

        public override void CreateTrail()
        {
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Blood,
                    Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 80, Scale: 1.2f);
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Blood,
                    0, 0, 60, Scale: 0.3f);
                dust.velocity += Projectile.velocity * 0.5f;
                dust.velocity *= 0.5f;
            }
        }
    }
}
