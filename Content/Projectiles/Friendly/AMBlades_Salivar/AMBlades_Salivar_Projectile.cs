using Ferustria.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Friendly.AMBlades_Salivar
{
    public class AMBlades_Salivar_Projectile : ModProjectile
    {
        bool CanAttack { get; set; }
        bool CanBeAnimated { get; set; }
        bool IsEnhansed {  get; set; }
        bool setFrames = false;

        float distanceFromPlayer = 70f;

        float[] frames =    new float[50];
        int[] frameCount =  new   int[50];
        float[] distances = new float[50];
        float[] angles =    new float[50];


        Player player = null;
        FSSpesialWeaponsPlayer weaponManager = null;
        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 28;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.timeLeft = FSSpesialWeaponsPlayer.AMBlades_Salivar_FrameCountFor1;
            Projectile.ownerHitCheck = true;
            Projectile.hide = true;
            Projectile.scale = 1.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
        }

        public override void OnSpawn(IEntitySource source)
        {
            player = Main.player[Projectile.owner];
            weaponManager = player.GetModPlayer<FSSpesialWeaponsPlayer>();
            IsEnhansed = weaponManager.AMBlades_Salivar_Enhansed;
            if (IsEnhansed && !weaponManager.AMBlades_Salivar_Enhansed_Active)
                PreKill(0);
            if (weaponManager != null && weaponManager.AMBlades_Salivar_Enhansed && !setFrames && !weaponManager.AMBlades_Salivar_Enhansed_Active)
            {
                for (int i = 0; i < frames.Length; i++)
                {
                    frames[i] = -5 * (int)(i / 5);
                    frameCount[i] = Main.rand.Next(4);
                    distances[i] = Main.rand.NextFloat() * 30f + i * 1.5f + 20f;
                    angles[i] = Main.rand.NextFloat() * 360f;
                }
                Projectile.width = Projectile.height = 300;
                //Projectile.localNPCHitCooldown = 5;
                Projectile.timeLeft = 60;
                Projectile.penetrate = -1;
                setFrames = true;
            }
            base.OnSpawn(source);
        }

        public override bool PreAI()
        {
            player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

            //if (SoundEngine.TryGetActiveSound(new SlotId((uint)Projectile.localAI[0]), out ActiveSound sound1))
            //    Projectile.localAI[0] = SoundEngine.PlaySound(SoundID.Item71.WithPitchOffset(weaponManager.AMBlades_Salivar_Combo_Count % 8 == 0 ? -1.8f : -0.5f), player.Center).Value;
            //else
            //    Projectile.localAI[1] = SoundEngine.PlaySound(SoundID.Item71.WithPitchOffset(weaponManager.AMBlades_Salivar_Combo_Count % 8 == 0 ? -1.8f : -0.5f), player.Center).Value;

            Projectile.Center = !setFrames ? player.MountedCenter + (Projectile.velocity.SafeNormalize(default) * distanceFromPlayer) : player.MountedCenter;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.scale = weaponManager.AMBlades_Salivar_Combo_Count % 8 == 0 && !IsEnhansed ? 3f : 2f;
            if (!setFrames) Projectile.width = Projectile.height = (int)(50 * Projectile.scale);

            // Avoid spawning dusts on dedicated servers
            if (!Main.dedServ)
            {
                //if (Main.rand.NextBool(3))
                //{
                //    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, Alpha: 128, Scale: 1.2f);
                //}

                //if (Main.rand.NextBool(4))
                //{
                //    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Alpha: 128, Scale: 0.3f);
                //}
            }

            if (!setFrames)
                FrameLogic();

            return false; // Don't execute vanilla AI.
        }
        void FrameLogic()
        {
            if (Projectile.frame > 5) { Projectile.Kill(); return; }
            if (++Projectile.frameCounter >= FSSpesialWeaponsPlayer.AMBlades_Salivar_FrameTime)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(255, weaponManager.AMBlades_Salivar_Combo_Count % 8 == 0 && !weaponManager.AMBlades_Salivar_Enhansed ? 255 : 100, 0, 0) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (setFrames) EnhansedAttackDraw(lightColor);
            else NormalAttackDraw(lightColor);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }

        ////Анимация обычной атаки
        void NormalAttackDraw(Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (player.direction == 1)
                spriteEffects = SpriteEffects.FlipVertically;

            // Getting texture of projectile
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * (Projectile.frame + (7 * weaponManager.AMBlades_Salivar_Frame_Count));

            // Get this frame on texture
            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;

            // If image isn't centered or symmetrical you can specify origin of the sprite
            // (0,0) for the upper-left corner
            float offsetX = 30f;
            origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

            // If sprite is vertical
            // float offsetY = 20f;
            // origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);

            
            // Applying lighting and draw current frame
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
        }

        ////Анимация усиленной атаки
        void EnhansedAttackDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] >= 0 && frames[i] <= 6)
                {
                    SpriteEffects spriteEffects = angles[i] > 90 && angles[i] < 180 ? SpriteEffects.None : SpriteEffects.FlipVertically;
                    int frameHeight = texture.Height / Main.projFrames[Projectile.type];
                    int startY = frameHeight * ((int)frames[i] + (7 * frameCount[i]));

                    Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight);

                    Vector2 origin = sourceRectangle.Size() / 2f;

                    float offsetX = distances[i];
                    origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

                    Color drawColor = Projectile.GetAlpha(lightColor);
                    Main.EntitySpriteDraw(texture,
                        Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                        sourceRectangle, drawColor, MathHelper.ToRadians(angles[i]), origin, Projectile.scale, spriteEffects, 0);
                }
                frames[i] += 1f;
            }

            weaponManager.AMBlades_Salivar_Enhansed_Active = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!setFrames)
            {
                int manaGain = Main.rand.Next(5) + 1;
                player.statMana += manaGain;
                if (player.statMana > player.statManaMax2) player.statMana = player.statManaMax2;
                player.ManaEffect(manaGain);
                NetMessage.SendData(MessageID.ManaEffect, -1, -1, null, Projectile.owner, manaGain, 0.0f, 0.0f, 0, 0, 0);
            }

            if (IsEnhansed)
                target.AddBuff(BuffID.OnFire, new Random().Next(2, 4) * 60);
            else
            {
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 15, 10, 5), Color.YellowGreen, $"{weaponManager.AMBlades_Salivar_Charge:N2}", true, true);
                weaponManager.AMBlades_Salivar_Charge += weaponManager.AMBlades_Salivar_Combo_Count > 12 ? 8f : 3f;
                weaponManager.AMBlades_Salivar_Charge_Deplete_Timer = 420;
            }
        }

        public override bool PreKill(int timeLeft)
        {
            if (Projectile.timeLeft > 0)
                return false;
            return base.PreKill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.penetrate <= 1 && Projectile.penetrate != -1) return false;
            return null;
        }
    }
}
