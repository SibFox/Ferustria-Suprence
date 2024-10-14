using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using static Terraria.ModLoader.ModContent;
using Ferustria.Assets.ClassTemplates;

namespace Ferustria.Content.Projectiles.Friendly.Rozaline
{
    public class Rozaline_RoseProjectile : StickyProjectile
    {
        Players.FSSpesialWeaponsPlayer chargeManager = null;

        public override void SetValues()
        {
            hitbox = 23;
            scale = 0.8f;
            damgeType = DamageClass.MeleeNoSpeed;
            timeLeft = 420;
            stickyTimeLeft = 240;
            hitEffectTimer = 30;
            MAX_STICKY_PROJECTILE = 6;
            SetTexture = "Content/Projectiles/Friendly/Rozaline/Rozaline_RoseProjectile";
        }

        float rotate = 0;
        public override void Behaviour()
        {
            chargeManager = Main.player[Projectile.owner].GetModPlayer<Players.FSSpesialWeaponsPlayer>();
            Projectile.velocity.X *= 0.987f;
            Projectile.velocity.Y += 0.02f;
            if (Projectile.velocity.Y > 20f) Projectile.velocity.Y = 20f;

            float maxRotate = MathHelper.ToRadians(Projectile.velocity.X * 45);
            rotate += 0.06f * Projectile.direction;
            if (rotate > maxRotate && Projectile.direction == 1) rotate = maxRotate;
            if (rotate < maxRotate && Projectile.direction == -1) rotate = maxRotate;
            Projectile.rotation = rotate + MathHelper.ToRadians(45f);
        }

        public override void CreateTrail()
        {
            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood,
                    Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 128, Scale: 1.2f);
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.3f;
            }
            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood,
                    0, 0, 128, Scale: 0.8f);
                dust.velocity += Projectile.velocity * 0.5f;
                dust.velocity *= 0.5f;
            }
        }

        public override void HitEffect()
        {
            chargeManager.Rozaline_Spikes_ChargeMeter += 0.5f;
            chargeManager.Rozaline_Spikes_UnchargeCooldown = 600;
            Main.npc[TargetWhoAmI].AddBuff(BuffType<Buffs.Negatives.Rapid_Blood_Loss>(), 20);
            base.HitEffect();
        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            for (int i = 0; i < Main.rand.Next(1, 3); i++)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
                    new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-2f, -6f)),
                    ProjectileType<Rozaline_ThornProjectile>(), 20, 0.02f, Projectile.owner);
            }
        }
    }
}
