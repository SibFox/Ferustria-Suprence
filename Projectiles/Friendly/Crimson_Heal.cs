using Microsoft.Xna.Framework;
using Ferustria.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Ferustria.Projectiles.Friendly
{
	public class Crimson_Heal : ModProjectile
	{
        public override string Texture => "Ferustria/Projectiles/Textures/Crimson_Heal";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("");
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Default;
			Projectile.timeLeft = 260;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.knockBack = 0f;
			Projectile.alpha = 255;
		}


		public override void Kill(int timeLeft)
		{
			//Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			//Main.PlaySound(SoundID.Item10, Projectile.position);
			
		}

		public override void AI()
		{
			Projectile.netUpdate = true;
			Player player = null;
			if (Projectile.owner != -1)
			{
				player = Main.player[Projectile.owner];
			}
			else if (Projectile.owner == 255)
			{
				player = Main.LocalPlayer;
			}
			if (Projectile.localAI[0]++ <= 0)
			{
				Projectile.velocity = (Projectile.velocity + Projectile.velocity / 3) * 1.8f;
			}
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 26;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			Lighting.AddLight(Projectile.position, 0.4f, 0.08f, 0.08f);
			Dust.NewDustPerfect(Projectile.Center, DustID.Blood, new(0, 0), 60, default, 1.2f);
			Vector2 center = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float offsetX = player.Center.X - center.X;
			float offsetY = player.Center.Y - center.Y;
			float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 17f;
			if (distance < 50f && Projectile.position.X < player.position.X + player.width && Projectile.position.X + Projectile.width > player.position.X && Projectile.position.Y < player.position.Y + player.height && Projectile.position.Y + Projectile.height > player.position.Y)
			{
				if (Projectile.owner == Main.myPlayer && !player.moonLeech)
				{
					int heal = Main.rand.Next(4) + 1;
					player.statLife += heal;
					if (player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
					player.HealEffect(heal);
					NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, Projectile.owner, heal, 0.0f, 0.0f, 0, 0, 0);
				}
				Projectile.Kill();
				/*if (Projectile.owner == Main.myPlayer && !Main.LocalPlayer.moonLeech)
				{
					
				}*/
			}
		}

	}

}