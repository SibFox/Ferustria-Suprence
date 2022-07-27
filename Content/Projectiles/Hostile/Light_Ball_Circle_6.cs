using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Hostile
{
	public class Light_Ball_Circle_6 : ModProjectile
	{
		private int projectileNumber
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		private float distanceToPlayer
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		private bool secondLayer
        {
			get => Projectile.localAI[0] == 1 ? true : false;
		}

		private bool set = false;
		private int toCenterCooldown = 135;
		private bool appeared = false;
		private float stopDistance;
        private Vector2 holdPlayerPos;

        public override string Texture => "Ferustria/Assets/Textures/Burning_Light_Ball";

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Burning Light Ball");
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.timeLeft = 2200;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		
		public override void AI()
		{
			Projectile.netUpdate = true;
			Player target = null;
			if (Projectile.owner != -1)
			{
				target = Main.player[Projectile.owner];
			}
			else if (Projectile.owner == 255)
			{
				target = Main.LocalPlayer;
			}
			if (Main.rand.NextFloat() < .15f)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Angelic_Particles>(), Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, 0, default, Main.rand.NextFloat(.52f, .95f));
			}

			Vector2 playerCenter = target.Center;
			
			if (!set)
            {
				stopDistance = distanceToPlayer;
				distanceToPlayer = 0;
				//rotSpd = Main.rand.NextFloat(0.03f, 0.042f);
				//if (Main.rand.NextBool()) rotSpd *= -1;
				set = true;
			}
			
			

			if (appeared)
            {
				--toCenterCooldown;
                
				Projectile.alpha = 0;
				if (Projectile.localAI[0] != 1f)
				{
					Projectile.localAI[0] = 1f;
                    holdPlayerPos = playerCenter;
				}
				float rotSpd1, rotSpd2;
				if (!Main.expertMode && !Main.masterMode) { rotSpd1 = 0.016f; rotSpd2 = 0.0242f; }
				else if (Main.expertMode && !Main.masterMode) { rotSpd1 = 0.018f; rotSpd2 = 0.026f; }
				else if (Main.masterMode) { rotSpd1 = 0.02f; rotSpd2 = 0.0285f; }
				else { rotSpd1 = 0f; rotSpd2 = 0f; }
				Rotate(Projectile, holdPlayerPos, distanceToPlayer, secondLayer ? rotSpd2 : rotSpd1);
			}
            else
            {
				Appereance(Projectile, stopDistance);
				for (int i = 0; i < Main.rand.Next(5); i++)
                {
					Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Angelic_Particles>(), Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 80, Scale: Main.rand.NextFloat(.87f, 1.2f));
                }
				Projectile.alpha = 255;
				Rotate(Projectile, playerCenter, distanceToPlayer, secondLayer ? -0.65f : 0.4f);
			}
			
			if (toCenterCooldown <= 0f)
            {
				distanceToPlayer -= 3.8f;
            }
			
			Projectile.rotation += 0.15f;
			if ((distanceToPlayer <= 0f && !Main.expertMode) || (distanceToPlayer <= -stopDistance / 1.7 && Main.expertMode && !Main.masterMode) || (distanceToPlayer <= -stopDistance && Main.masterMode)) Projectile.Kill();
		}

		public static void Rotate(Projectile Projectile, Vector2 targetCenter, float distance, float rotateSpeed)
        {
			Projectile.localAI[1] += rotateSpeed;
			Light_Ball_Circle_6 modProj = Projectile.ModProjectile as Light_Ball_Circle_6;
			double angle = 2.0 * Math.PI * modProj.projectileNumber / 6.0 + Projectile.localAI[1];
			Projectile.position = targetCenter + distance * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) - Projectile.Size / 2f;
		}

		public static void Appereance(Projectile Projectile, float stopDistance)
        {
			Light_Ball_Circle_6 modProj = Projectile.ModProjectile as Light_Ball_Circle_6;
			if (modProj.distanceToPlayer <= stopDistance) modProj.distanceToPlayer += stopDistance / 2.5f / 60;
			else modProj.appeared = true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			//target.AddBuff(ModContent.BuffType<Under_Crucifixion_Tier2>(), Main.rand.Next(4, 10) * 60);
			Projectile.Kill();
		}

        public override bool CanHitPlayer(Player target)
        {
			if (appeared) return true;
			return false;
        }

        public override bool? CanHitNPC(NPC target) => false;

	}
	
}