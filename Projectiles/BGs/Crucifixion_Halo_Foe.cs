using Microsoft.Xna.Framework;
using Ferustria.Dusts;
using Ferustria.Buffs.Negatives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Ferustria.Projectiles.BGs
{
	public class Crucifixion_Halo_Foe : ModProjectile
	{
		Player owner = null;
		public override string Texture => "Ferustria/Projectiles/BGs/Crucifixion_Halo";

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("");
		}

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 56;
			Projectile.aiStyle = -1;
			//Projectile.scale = 1.3f;
			Projectile.timeLeft = 10000;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
		}


		public override void Kill(int timeLeft)
		{
			/*FSPlayer refer = owner.GetModPlayer<FSPlayer>();
			refer.Crucifixion_Tier2 = false;
			refer.Crucifixion_Halo_Existance = -1;*/
		}

		public override void AI()
		{
			if (Projectile.owner != -1)
			{
				owner = Main.player[Projectile.owner];
			}
			else if (Projectile.owner == 255)
			{
				owner = Main.LocalPlayer;
			}
			if (owner.HasBuff(ModContent.BuffType<Under_Crucifixion_Tier2>())) Projectile.timeLeft = 100;
			Projectile.position = new Vector2(owner.position.X, owner.position.Y);
			if (!owner.active || owner.dead) Projectile.active = false;
			Projectile.localAI[0]++;
			if (Projectile.localAI[0] > 0 && Projectile.localAI[0] <= 1200 && Projectile.localAI[1] < 1)
			{
				Projectile.scale = (Projectile.localAI[0] / 12) * .01f + 0.3f;
				Projectile.alpha = 180 - (int)(Projectile.localAI[0] / 8);
			}
			if (Projectile.localAI[0] > 1200)
            {
				Projectile.localAI[1]++;
				Projectile.localAI[0] -= 2;
				Projectile.scale = 1.5f - Projectile.localAI[1] / 12 * .01f;
				if (Projectile.localAI[0] > 0)
					Projectile.alpha = 180 - (int)(Projectile.localAI[0] / 8);
				else Projectile.alpha = 30;
			}
			if (Projectile.localAI[0] <= 0)
            {
				Projectile.localAI[1] = 0;
            }
			owner.GetModPlayer<FSPlayer>().Crucifixion_Halo_Existance++;
			

			/*if (Main.rand.NextFloat() < .25f)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Void_Particles>(), Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.52f, .95f));
			}*/

		}
	}

}