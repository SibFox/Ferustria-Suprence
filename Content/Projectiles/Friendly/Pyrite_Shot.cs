using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace Ferustria.Content.Projectiles.Friendly
{
	public class Pyrite_Shot : ModProjectile
	{
        public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.scale = 1f;
            Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 400;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.knockBack = 2.5f;
			Projectile.alpha = 255;
            Projectile.hide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }


        public override void OnKill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 30;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
            Projectile.SetStraightRotation();
			//Projectile.rotation = Projectile.GetStraightRotation();
			Lighting.AddLight(Projectile.position, 0.8f, 0.5f, 0);
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if (Main.rand.NextFloat() < .5f)
			{
				target.AddBuff(BuffID.OnFire3, Main.rand.Next(5, 8) * 60);
			}
			//else if (false)
			//{
			//	target.AddBuff(BuffID.OnFire3, Main.rand.Next(6, 8) * 70);
			//	//target.AddBuff(BuffID., Main.rand.Next(6, 10) * 60); //Увеличение получаемого урона
			//}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Main.rand.NextFloat() < .2f)
			{
				target.AddBuff(BuffID.OnFire3, Main.rand.Next(5, 10) * 60);
			}
			else if (hit.Crit)
			{
				target.AddBuff(BuffID.OnFire3, Main.rand.Next(6, 12) * 70);
				//target.AddBuff(BuffID., Main.rand.Next(6, 10) * 60); //Увеличение получаемого урона
			}
		}

	}

}