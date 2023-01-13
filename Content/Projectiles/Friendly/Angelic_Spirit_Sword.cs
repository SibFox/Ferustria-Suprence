using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace Ferustria.Content.Projectiles.Friendly
{
	public class Angelic_Spirit_Sword : ModProjectile
	{
        Players.FSMinionsPlayer minionsManager = null;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angelic Spirit Sword");
		}

		public override void SetDefaults()
		{
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 240;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.scale = 1.25f;
			Projectile.knockBack = 2f;
			Projectile.alpha = 255;
            Projectile.hide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }


        public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
        {
            Visuals();
            Projectile.SetStraightRotation();
			Lighting.AddLight(Projectile.position, 0, 0.45f, 0.4f);
            Point point = Projectile.Center.ToTileCoordinates();
            Tile tile = Main.tile[point.X, point.Y];
            if (tile.IsTileSolid()) Projectile.timeLeft -= 8;
		}

        private void Visuals()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            Dust.NewDust(Projectile.Center, 4, 4, ModContent.DustType<Angelic_Particles>(), Projectile.velocity.X / 2.5f, Projectile.velocity.Y / 2.5f, 85, Scale: Main.rand.NextFloat(.85f, 1.25f));
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Players.FSMinionsPlayer minionsManager = Main.player[Projectile.owner].GetModPlayer<Players.FSMinionsPlayer>();
            //minionsManager.Minion_AngelicSwordsman_Charge++;
            //minionsManager.Minion_AngelicSwordsman_DischargeCooldown = 180;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			
		}

	}

}