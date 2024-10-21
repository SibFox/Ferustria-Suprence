using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Friendly.Rozaline
{
    public class Rozaline_SpearProjectile : ModProjectile
    {
        // Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
        protected virtual float HoldoutRangeMin => 24f;
        protected virtual float HoldoutRangeMax => 140f;
        int duration;
        bool set = false;
        Player player = null;
        float angle = 0;
        Vector2 savedVel;

        float Sequence => Projectile.ai[0];
        float MaxAngle => Projectile.ai[1];
        bool secondHitbox = false;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.
            Projectile.width = 35;
            Projectile.height = 35;
            if (Sequence > 2) { Projectile.localNPCHitCooldown = -1; Projectile.usesLocalNPCImmunity = true; }
            Projectile.ownerHitCheck = true;
            Projectile.hide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            secondHitbox = source.Context == "Hitbox";
            if (secondHitbox) Projectile.alpha = 255;
        }

        public override bool PreAI()
        {
            player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
                                                    //int duration = player.itemAnimationMax; // Define the duration the projectile will exist in frames

            Projectile.netUpdate = true;

            if (!set)
            {
                duration = Projectile.timeLeft;
                savedVel = Projectile.velocity;
                Projectile.direction = Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
                switch (Sequence)
                {
                    case 1:
                        Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90f));
                        if (!secondHitbox)
                        {
                            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis("Hitbox"), Projectile.position, savedVel, ModContent.ProjectileType<Rozaline_SpearProjectile>(),
                                Projectile.damage, Projectile.knockBack, Projectile.owner, Sequence, MaxAngle);
                            proj.timeLeft = duration;
                        }
                        break;
                    case 2:
                        Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-90f));
                        if (!secondHitbox)
                        {
                            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis("Hitbox"), Projectile.position, savedVel, ModContent.ProjectileType<Rozaline_SpearProjectile>(),
                                Projectile.damage, Projectile.knockBack, Projectile.owner, Sequence, MaxAngle);
                            proj.timeLeft = duration;
                        }
                        break;
                    case 3: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(12f)); break;
                    case 5: Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-12f)); break;
                }
                SoundEngine.PlaySound(SoundID.Item71.WithPitchOffset(Sequence != 6 ? -0.07f : -0.45f), player.Center);
                set = true;
            }

            player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

            if (Sequence == 1f || Sequence == 2f)
            {
                int modifier = Sequence % 2 == 0 ? 1 : -1;

                //float progress = modifier == -1 ? (float)Projectile.timeLeft / (float)duration : ((float)duration - (float)Projectile.timeLeft) / (float)duration;
                float progress = Projectile.timeLeft / (float)duration * modifier;
                angle = (float)(MathHelper.TwoPi * progress) * (MathHelper.ToRadians(MaxAngle) / MathHelper.TwoPi / MathHelper.TwoPi);
                Projectile.velocity = Vector2.Normalize(Projectile.velocity);
                Projectile.velocity = Projectile.velocity.RotatedBy(angle);
                Projectile.Center = player.MountedCenter + Projectile.velocity * (HoldoutRangeMax / (!secondHitbox ? 1.2f : 2.5f));
            }

            if (++Projectile.localAI[0] % 7 == 0 && !secondHitbox)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * (Sequence != 6f ? 2f : 0.2f),
                        ModContent.ProjectileType<Rozaline_RoseProjectile>(), 5, 0, Projectile.owner);
                }
            }

            ////// Три посследовательных тычка под разными углами
            if (Sequence >= 3f && Sequence <= 5f)
            {
                float progress = (duration - (float)Projectile.timeLeft) / duration;
                Projectile.velocity = Vector2.Normalize(Projectile.velocity);
                Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax * 1.25f, progress);
            }

            if (Sequence == 6f)
            {
                float progress = (duration - (float)Projectile.timeLeft) / duration;
                Projectile.velocity *= 0.945f;
                Projectile.alpha = (int)(235 * progress);
            }

            // Avoid spawning dusts on dedicated servers
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(3))
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, Alpha: 128, Scale: 1.2f);

                if (Main.rand.NextBool(4))
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Alpha: 128, Scale: 0.3f);
            }

            if (Main.player[Projectile.owner].direction == -1)
            {
                // If sprite is facing left, rotate 45 degrees
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                // If sprite is facing right, rotate 135 degrees
                Projectile.rotation += MathHelper.ToRadians(135f);
            }
            player.direction = Projectile.direction;
            return false; // Don't execute vanilla AI.
        }

        public override bool PreKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                if (Sequence == 3f || Sequence == 4f)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.MountedCenter, savedVel, Projectile.type, Projectile.damage,
                        Projectile.knockBack, Projectile.owner, Projectile.ai[0] + 1f);
                    Main.projectile[proj].timeLeft = 8;
                }
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Players.FSSpesialWeaponsPlayer chargeManager = player.GetModPlayer<Players.FSSpesialWeaponsPlayer>();
            if (target.life <= 0 && target.lifeMax > 10)
            {
                chargeManager.Rozaline_Spikes_ChargeMeter += 2.5f;
                chargeManager.Rozaline_Spikes_UnchargeCooldown = 600;
            }
        }
    }
}
