using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Friendly
{
    // This file shows an animated projectile
    // This file also shows advanced drawing to center the drawn projectile correctly
    public class Ceremonial_Proejctile_Forward_Friendly : ModProjectile
    {
        public override string Texture => Ferustria.Paths.TexturesPathPrj + "Ceremonial_Slice";

        public override void SetStaticDefaults()
        {
            // Total count animation frames
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 11;
            Projectile.timeLeft = 1200;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 0;
        }

        // Allows you to determine the color and transparency in which a projectile is drawn
        // Return null to use the default color (normally light and buff color)
        // Returns null by default.
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }

        public override void AI()
        {
            FadeInAndOut();
            

            Point coords = Projectile.Center.ToTileCoordinates();
            Tile projectileTile = Main.tile[coords.X, coords.Y];
            if (projectileTile.HasTile && Main.tileSolid[projectileTile.TileType] && projectileTile.TileType != TileID.Platforms) Projectile.velocity *= 0.955f;
            else Projectile.velocity *= 0.98f;

            // Loop through the 4 animation frames, spending 5 ticks on each
            // Projectile.frame — index of current frame
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }


            // Set both direction and spriteDirection to 1 or -1 (right and left respectively)
            // Projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
        }

        // Many projectiles fade in so that when they spawn they don't overlap the gun muzzle they appear from
        public void FadeInAndOut()
        {
            if (Projectile.penetrate <= 1) Projectile.ai[0] = 86;
            if (++Projectile.ai[0] <= 85f)
            {
                Projectile.alpha -= 25;
                if (Projectile.alpha < 20)
                    Projectile.alpha = 20;
                return;
            }
            
            Projectile.alpha += 22;
            if (Projectile.alpha > 255)
                Projectile.Kill();
        }

        // Some advanced drawing because the texture image isn't centered or symetrical
        // If you dont want to manually drawing you can use vanilla projectile rendering offsets
        // Here you can check it https://github.com/tModLoader/tModLoader/wiki/Basic-Projectile#horizontal-sprite-example
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
            Players.FSSpesialWeaponsPlayer weaponManager = Main.player[Projectile.owner].GetModPlayer<Players.FSSpesialWeaponsPlayer>();
            if (Projectile.penetrate > 7) weaponManager.CKnifeL1_Knifes_Charge += 1.25f;
            Projectile.velocity *= 0.955f;
            //target.immune[0] = 15;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.penetrate <= 1) return false;
            return null;
        }
    }
}