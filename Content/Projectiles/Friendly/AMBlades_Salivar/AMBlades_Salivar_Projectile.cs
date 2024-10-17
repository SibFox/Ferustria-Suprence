using Ferustria.Content.Projectiles.Friendly.Rozaline;
using Ferustria.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        bool CanDamage { get; set; }
        bool IsEnhansed => Projectile.ai[0] == 1;
        float ComboCount {  get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
        Player player = null;
        FSSpesialWeaponsPlayer weaponManager = null;
        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 28;
            Projectile.width = 35;
            Projectile.height = 35;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
            Projectile.ownerHitCheck = true;
            Projectile.hide = true;
        }


        public override bool PreAI()
        {
            player = Main.player[Projectile.owner];
            //Projectile.netUpdate = true;
            //SoundEngine.PlaySound(SoundID.Item71.WithPitchOffset(-0.07 * ComboCount - 0.07), player.Center);

            player.heldProj = Projectile.whoAmI; // Update the player's held projectile id
            weaponManager = player.GetModPlayer<FSSpesialWeaponsPlayer>();




            if (false)
            {
                Projectile.Center = player.MountedCenter; //+ range;
            }

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

            //CanAttack = weaponManager.AMBlades_Salivar_UseCooldown <= 0;
            CanDamage = weaponManager.AMBlades_Salivar_UseTime > 0;
            if (CanDamage)
                FrameLogic();

            return false; // Don't execute vanilla AI.
        }
        void FrameLogic()
        {
            if (++Projectile.frameCounter >= FSSpesialWeaponsPlayer.AMBlades_Salivar_FrameTime)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            ComboCount = Main.projFrames[Projectile.type] % 7;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(215, 160, 0, 0) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // Getting texture of projectile
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;

            // If image isn't centered or symmetrical you can specify origin of the sprite
            // (0,0) for the upper-left corner
            float offsetX = 50f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

            // If sprite is vertical
            // float offsetY = 20f;
            // origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);


            // Applying lighting and draw current frame
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }

   

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsEnhansed)
                target.AddBuff(BuffID.OnFire, new Random().Next(2, 4) * 60);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!CanDamage) return false;
            return null;
        }
    }
}
