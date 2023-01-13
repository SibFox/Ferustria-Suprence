using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Friendly
{
    // This file shows an animated projectile
    // This file also shows advanced drawing to center the drawn projectile correctly
    public class Ceremonial_Proejctile_Circle_Friendly : ModProjectile
    {
        public override string Texture => "Ferustria/Assets/Textures/Projectiles/Ceremonial_Slice";

        public float maxDistance = 220f;
        public float currentDistance { get => Projectile.localAI[0]; set => Projectile.localAI[0] = value; }
        public float rotation { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public int projectileNumber { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public Dictionary<int, int> cooldownSlots = new();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ceremonial Slice");
            // Total count animation frames
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 75;
            Projectile.height = 75;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 0;

            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCImmunity[0] = 600;
            //Projectile.localNPCHitCooldown = 60;
        }

        // Allows you to determine the color and transparency in which a projectile is drawn
        // Return null to use the default color (normally light and buff color)
        // Returns null by default.
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.direction = Projectile.spriteDirection = owner.direction;
        }

        public override void AI()
        {
            Projectile.netUpdate = true;
            FadeInAndOut();
            Rotate();
            CalculateCooldowns();
            if (currentDistance < maxDistance) currentDistance += 2;

            // Loop through the 4 animation frames, spending 5 ticks on each
            // Projectile.frame — index of current frame
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            //Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
        }
        
        public void Rotate()
        {
            float angle = MathHelper.TwoPi * projectileNumber / 3 + MathHelper.ToRadians(rotation += 2f) * Projectile.spriteDirection;
            Vector2 pos = Vector2.One.GetVectorToAngleWithMult(angle, currentDistance);
            Projectile.Center = Main.player[Projectile.owner].Center + pos;
        }

        // Many projectiles fade in so that when they spawn they don't overlap the gun muzzle they appear from
        public void FadeInAndOut()
        {
            if (++Projectile.ai[0] <= 420f)
            {
                Projectile.alpha -= 12;
                if (Projectile.alpha < 20)
                    Projectile.alpha = 20;
                return;
            }

            Projectile.alpha += 18;
            if (Projectile.alpha > 255)
                Projectile.Kill();
        }

        public void CalculateCooldowns()
        {
            if (cooldownSlots.Count > 0)
            {
                foreach (int npcId in cooldownSlots.Keys)
                    if (--cooldownSlots[npcId] <= 0) cooldownSlots.Remove(npcId);
            }
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
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[0] = 10;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (currentDistance <= 30) return false;
            return null;
        }

        public override bool CanHitPvp(Player target)
        {
            return currentDistance > 30;
        }
    }
}