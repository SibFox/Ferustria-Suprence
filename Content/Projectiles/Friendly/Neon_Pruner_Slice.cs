using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Friendly
{
    // This file shows an animated projectile
    // This file also shows advanced drawing to center the drawn projectile correctly
    public class Neon_Pruner_Slice : ModProjectile
    {
        public override string Texture => Ferustria.Paths.TexturesPathPrj + "Neon_Pruner_Slice";

        public float Pruner_Charge { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }

        public int Cleave_Level { get
            {
                int r = 0;
                if (Projectile.ai[0] >= 40f)
                    r = 1;
                if (Projectile.ai[0] >= 66f)
                    r = 2;
                if (Projectile.ai[0] >= 99f)
                    r = 3;
                return r;
            } }

        public int critDenum = 15;
        public float penDenum = 1.4f;

        public float LIFE_TIME = 25;


        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 11;
            Projectile.timeLeft = 1200;
            Projectile.alpha = 255;
            Projectile.CritChance = 100;
        }

        // Allows you to determine the color and transparency in which a projectile is drawn
        // Return null to use the default color (normally light and buff color)
        // Returns null by default.
        public override Color? GetAlpha(Color lightColor)
        {
            int r = 255;
            int g = 255;
            if (Cleave_Level == 2)
            {
                r -= 105;
                g -= 105;
            }
            if (Cleave_Level == 3)
            {
                r -= 155;
                g -= 155;
            }

            return new Color(r, g, 255, 0) * Projectile.Opacity;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Cleave_Level == 0)
                Projectile.Kill();
            if (Cleave_Level == 1)
            {
                Projectile.scale = 2f;
                Projectile.damage += 50;
                Projectile.CritChance = 80;
                critDenum = 10;
                penDenum = 1.4f;
                Projectile.penetrate = 11;
                SoundEngine.PlaySound(SoundID.Item96.WithPitchOffset(.25f).WithVolumeScale(1.55f), Main.player[Projectile.owner].position);
            }
            if (Cleave_Level == 2)
            {
                Projectile.scale = 2.8f;
                Projectile.damage += 150;
                critDenum = 15;
                Projectile.velocity *= 1.1f;
                penDenum = 1.25f;
                Projectile.penetrate = 16;
                SoundEngine.PlaySound(SoundID.Item96.WithPitchOffset(0f).WithVolumeScale(1.55f), Main.player[Projectile.owner].position);
            }
            if (Cleave_Level == 3)
            {
                Projectile.scale = 3.8f;
                Projectile.damage += 250;
                critDenum = 8;
                LIFE_TIME = 35;
                Projectile.velocity *= 1.22f;
                penDenum = 1.12f;
                Projectile.penetrate = 21;
                SoundEngine.PlaySound(SoundID.Item122.WithPitchOffset(.2f).WithVolumeScale(1.55f), Main.player[Projectile.owner].position);
                SoundEngine.PlaySound(SoundID.Item96.WithPitchOffset(-.3f).WithVolumeScale(1.9f), Main.player[Projectile.owner].position);
            }


            Projectile.height = Projectile.width = (int)(70 * Projectile.scale);
            Projectile.Center = Main.player[Projectile.owner].MountedCenter;
            base.OnSpawn(source);
        }

        public override void AI()
        {
            FadeInAndOut();
            

            Point coords = Projectile.Center.ToTileCoordinates();
            Tile projectileTile = Main.tile[coords.X, coords.Y];
            //if (projectileTile.HasTile && Main.tileSolid[projectileTile.TileType] && projectileTile.TileType != TileID.Platforms) Projectile.velocity *= 0.955f;
            Projectile.velocity *= 0.995f;


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
            if (Projectile.penetrate <= 1) Projectile.ai[1] = LIFE_TIME;
            if (++Projectile.ai[1] <= LIFE_TIME)
            {
                Projectile.alpha -= 27;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
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
            float offsetX = 40f;
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
            target.AddBuff(ModContent.BuffType<Buffs.Negatives.Sliced_Defense>(), (Cleave_Level * 2 + 1) * 60);
            Projectile.damage = (int)(Projectile.damage / penDenum);
            Projectile.CritChance -= critDenum;
            if (Cleave_Level == 3)
                target.AddBuff(BuffID.Frostburn2, 5 * 60);

            Player player = Main.player[Projectile.owner];

            if (Projectile.owner == Main.myPlayer && !player.moonLeech && Projectile.ai[2]++ <= 5)
            {
                int heal = Main.rand.Next(1, Cleave_Level * 2 + 1) * 10;
                if (heal > 0)
                {
                    if (target.life <= 0) heal = (int)(heal * 1.5f);
                    player.Heal(heal);
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.penetrate <= 1) return false;
            return null;
        }
    }
}