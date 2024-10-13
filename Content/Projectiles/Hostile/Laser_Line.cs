using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace Ferustria.Content.Projectiles.Hostile
{
	public class Laser_Line : ModProjectile
	{
        Vector2 startPos;

        int Speed
        {
            get => (int)(Projectile.ai[0]);
            set => Projectile.ai[0] = value;
        }

        int MaxDistance
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("");
		}

		public override void SetDefaults()
		{
            Projectile.damage = 0;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 3600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
		}

        public override void OnSpawn(IEntitySource source)
        {
            startPos = Projectile.Center;
            //Mod.Logger.Debug($"Projectile activated: {Projectile}; StartPos: {startPos}");
        }

        private void DecreaseAlpha()
        {
            if (Projectile.alpha > 80) Projectile.alpha -= 4;
            else Projectile.alpha = 80;
        }

		public override void AI()
		{
            Projectile.netUpdate = true;
            if (dist > MaxDistance) Projectile.Kill();
            dist += Speed;
            DecreaseAlpha();
            //Mod.Logger.Debug($"Projectile working: {Projectile};");
        }

        int dist = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            //Mod.Logger.Debug("PreDraw entered");
            DrawLaser(TextureAssets.Projectile[ModContent.ProjectileType<Laser_Line>()].Value, startPos,
                    Projectile.velocity, 1, Projectile.damage, 1.5f, MaxDistance, (Color)GetAlpha(lightColor), dist);
            
            return true;
        }

        public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float step,float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50)
        {
            float r = unit.ToRotation() + rotation;

            // Draws the laser 'body'
            for (float i = transDist; i <= maxDist; i += step)
            {
                //((255 - Projectile.alpha) / 255f)
                Color c = color;
                var origin = start + i * unit.SafeNormalize(default);
                Main.spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 0, 4, 4), i < transDist ? Color.Transparent : c, r,
                    new Vector2(0, 0), scale, 0, 0);
                //Main.EntitySpriteDraw(texture, origin - Main.screenPosition,
                //    new Rectangle(0, 0, 4, 4), i < transDist ? Color.Transparent : c, r,
                //    new Vector2(0, 0), scale, 0, 0);
            }
            // Draws the laser 'tail'
            //Main.EntitySpriteDraw(texture, start + unit * (transDist - step) - Main.screenPosition,
            //    new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

            //// Draws the laser 'head'
            //Main.EntitySpriteDraw(texture, start + (2500 + step) * unit - Main.screenPosition,
            //    new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            //Mod.Logger.Debug($"Texture: {texture};\n StartPos: {start}; Velocity: {unit}");
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255);
        }

        public override bool ShouldUpdatePosition() => true;

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        
    }

}