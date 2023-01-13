using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;

namespace Ferustria.Content.Projectiles.Hostile
{
	public class Gathering_Light : ModProjectile
	{
		public override string Texture => "Ferustria/Assets/Textures/Projectiles/Burning_Light_Ball";

        int gather = 0;
        int NeedToGrow { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        bool doneShooting { get => toShoot == 0; }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gathering Light");
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.timeLeft = 800;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 0;
			Projectile.scale = 1.2f;
		}

		public override void AI()
		{
            Projectile.netUpdate = true;
			
			if (Main.rand.NextFloat() < .45f)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Angelic_Particles>(), 
                    Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.82f, 1.15f));
			}

			Projectile.rotation += 0.15f * Projectile.direction;

            if (gather < NeedToGrow)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile currentProjectile = Main.projectile[i];
                    if (i != Projectile.whoAmI
                        && currentProjectile.active
                        && currentProjectile.type == ModContent.ProjectileType<Light_Ball_Forward>()
                        && currentProjectile.ai[1] == Projectile.whoAmI
                        && Vector2.Distance(currentProjectile.Center, Projectile.Center) <= 48f)
                    {
                        currentProjectile.Kill();
                        gather++;
                    }
                }
                Projectile.alpha = 255 - (200 * (gather / NeedToGrow));
            }
            else if (!doneShooting)
                WhenGrown();
            else
                FlyAfterShooting();
        }


        int rapidTimer = 60;
        int toShoot = 10;
        private void WhenGrown()
        {
            Projectile.alpha = 0;
            if (--rapidTimer < 0)
            {
                rapidTimer = Main.rand.Next(6, 10);
                toShoot--;

                List<Player> activePlayers = new();
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player currentPlayer = Main.player[p];
                    if (currentPlayer.active) activePlayers.Add(currentPlayer);
                }
                //Mod.Logger.DebugFormat($"APlayers", activePlayers);
                float minimumDistance = 8000f;
                Player closePlayer = Main.LocalPlayer;
                for (int a = 0; a < activePlayers.Count; a++)
                {
                    float previousDistance = Vector2.Distance(activePlayers[a].Center, Projectile.Center);
                    float localDist = Math.Min(previousDistance, minimumDistance);
                    if (localDist != minimumDistance) { closePlayer = activePlayers[a]; minimumDistance = previousDistance;
                        //Mod.Logger.Debug($"prevDist: {previousDistance}; minDist: {minimumDistance}; Close Player: {closePlayer}");
                    }
                }
                Vector2 vel = (closePlayer.Center - Projectile.Center).SafeNormalize(default) * 14.5f;
                
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis("LightBall:GrownShot"), Projectile.Center, vel,
                        ModContent.ProjectileType<Light_Ball_Forward>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                    //Mod.Logger.Debug($"Projectile: {proj}");
                }
            }
        }

        private void FlyAfterShooting()
        {
            Projectile.HomingTowards(Main.LocalPlayer.Center, 14.5f, 30f);
        }

        public override bool CanHitPlayer(Player target)
        {
            return gather >= NeedToGrow;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
            Projectile.Kill();
			//target.AddBuff(ModContent.BuffType<Under_Crucifixion_Tier2>(), Main.rand.Next(4, 10) * 60);
		}
	}

}