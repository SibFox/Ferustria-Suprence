using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Ferustria.Content.Projectiles.BGs
{
	public class Crucifixion_Halo_Player : ModProjectile
	{
		Player owner = null;
		public override string Texture => Ferustria.TexturesPath + "BGs/Crucifixion_Halo";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("");
		}

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 55;
			Projectile.aiStyle = -1;
			//Projectile.scale = 1.3f;
			Projectile.timeLeft = 10000;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
			DrawOffsetX = 30;
		}


		public override void Kill(int timeLeft)
		{
            Players.FSPlayer refer = owner.GetModPlayer<Players.FSPlayer>();
			refer.Crucifixion_Tier2 = false;
			refer.Crucifixion_Halo_Existance = -1;

		}

		public override void AI()
		{
			/*if (projectile.owner != -1)
			{
				owner = Main.player[projectile.owner];
			}
			else if (projectile.owner == 255)
			{
				owner = Main.LocalPlayer;
			}
			if (owner.HasBuff(ModContent.BuffType<Under_Crucifixion_Tier2>())) projectile.timeLeft = 100;
			projectile.position = new Vector2(owner.position.X, owner.position.Y);
			if (!owner.active || owner.dead) projectile.active = false;
			projectile.localAI[0]++;
			if (projectile.localAI[0] > 0 && projectile.localAI[0] <= 1200 && projectile.localAI[1] < 1)
			{
				projectile.scale = (projectile.localAI[0] / 12) * .01f + 0.3f;
				projectile.alpha = 180 - (int)(projectile.localAI[0] / 8);
			}
			if (projectile.localAI[0] > 1200)
            {
				projectile.localAI[1]++;
				projectile.localAI[0] -= 2;
				projectile.scale = 1.5f - projectile.localAI[1] / 12 * .01f;
				if (projectile.localAI[0] > 0)
					projectile.alpha = 180 - (int)(projectile.localAI[0] / 8);
				else projectile.alpha = 30;
			}
			if (projectile.localAI[0] <= 0)
            {
				projectile.localAI[1] = 0;
            }
			owner.GetModPlayer<FSPlayer>().Crucifixion_Halo_Existance++;
			

			/*if (Main.rand.NextFloat() < .25f)
			{
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, ModContent.DustType<Void_Particles>(), projectile.velocity.X * .8f, projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.52f, .95f));
			}*/

		}
	}

}