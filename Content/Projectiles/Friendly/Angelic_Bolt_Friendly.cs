﻿using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;

namespace Ferustria.Content.Projectiles.Friendly
{
	public class Angelic_Bolt_Friendly : ModProjectile
	{
        public override string Texture => Ferustria.Paths.TexturesPathPrj + "Angelic_Bolt";

		
		public override void SetDefaults()
		{
			Projectile.width = 44;
			Projectile.height = 12;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.knockBack = .8f;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.arrow = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}


		public override void OnKill(int timeLeft)
		{
			if (Projectile.ai[0] == -5f)
            {
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			}
			
		}

		public override void AI()
		{
			if (Projectile.ai[0] == -5f)
			{
				if (Projectile.localAI[0]++ >= 20) Projectile.velocity.Y += 0.095f;
				if (Projectile.velocity.Y >= 6f) Projectile.velocity.Y += 0.125f;

                if (Projectile.alpha > 0)
                    Projectile.alpha -= 26;
                else if (Projectile.alpha > 0)
                    Projectile.alpha -= 86;

				if (Projectile.alpha < 0)
					Projectile.alpha = 0;
			}
			else Projectile.alpha = 145;

            Projectile.SetStraightRotation();
			Lighting.AddLight(Projectile.position, 0, 0.2f, 0.4f);
			Projectile.localAI[1]++;
			if (Projectile.localAI[1] % 2 == 0 && Projectile.ai[0] == -5f)
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Angelic_Particles>(), Projectile.velocity.X * .25f, Projectile.velocity.Y * .25f, 0, default, Main.rand.NextFloat(.77f, 1.35f));
			else if (Projectile.localAI[1] % 6 == 0)
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Angelic_Particles>(), Projectile.velocity.X * .25f, Projectile.velocity.Y * .25f, 0, default, Main.rand.NextFloat(.77f, 1.35f));
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.timeLeft = 13;
			if (Projectile.ai[0] == -5)
            {
				for (int i = 0; i < Main.rand.Next(2) + 2; i++)
                {
					int max;
					if (target.width > target.height) max = target.width;
					else max = target.height;
					float distance = max + 120f;
					double angle = Math.PI * 2.0 * Main.rand.NextFloat();
					Vector2 setPos = target.Center + distance * Extensions.GetVector_WithAngle(angle) - Projectile.Size / 2f;
					Vector2 pos = target.Center - setPos;
                    float magnitude = (float)Math.Sqrt(pos.X * pos.X + pos.Y * pos.Y);
                    if (magnitude > 0)
                    {
                        pos *= 13f / magnitude;
                    }
                    else
                    {
                        pos = new Vector2(0f, 13f);
                    }
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), setPos, pos, ModContent.ProjectileType<Angelic_Bolt_Friendly>(), hit.Damage, 0, Projectile.owner, target.whoAmI);
					Main.projectile[proj].timeLeft = 30;
					Main.projectile[proj].tileCollide = false;
					Main.projectile[proj].arrow = false;
                    Main.projectile[proj].penetrate = 20;
                }
            }
		}

        public override bool? CanHitNPC(NPC target) => target.whoAmI == Projectile.ai[0] || Projectile.ai[0] == -5;

    }

}