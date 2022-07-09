using Microsoft.Xna.Framework;
using Ferustria.Buffs.Negatives;
using Ferustria.Dusts;
using Ferustria.Items.Materials;
using Ferustria.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;

namespace Ferustria.NPCs.Bosses.SixthWingedSeraphBoss
{
	public class Sixth_Winged_Seraph_Boss : ModNPC
	{
		/// <summary>
		/// Таймер атак
		/// </summary>
		private int timer1 = 200;
		private int timer2 = 0;
		private int choose, choose1, continueAttack, continueMove, dashed, face, sequenceTimer, side;
		private float accSpeedY, accSpeedX, rotate;
		private string sequence;
		Vector2 center;
		/// <summary>
		/// Ограничитель в единицах перед началом сиквенции
		/// </summary>
		private int sequenceCounter;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morghos, Sixth Winged Seraph");
			DisplayName.AddTranslation(FSHelper.RuTrans(), "Моргос, Шести-крылый Серафим");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.BossBestiaryPriority.Add(Type);
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = FSHelper.Scale(46000, 58000, 65000);
			NPC.damage = FSHelper.Scale(98, 98, 103);
			NPC.defense = FSHelper.WOScale(20, 24, 28);
			NPC.knockBackResist = 0f;
			NPC.width = 192 / 3;
			NPC.height = 120 / 2;
			NPC.noGravity = true;
			NPC.aiStyle = -1;
			NPC.boss = true;
			NPC.npcSlots = 5f;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath4;
			NPC.value = Item.buyPrice(0, 0, 1, 0);
			NPC.noTileCollide = true;
			Music = MusicID.OtherworldlyBoss1; //MusicID.OtherworldlyBoss1; Очень хорошо подходит
			accSpeedX = accSpeedY = 0;
			dashed = 1;
			sequenceTimer = 0;
			sequence = "None";
			sequenceCounter = 0;
			DrawOffsetY = NPC.width / 3;
		}

		int GetPhase()
        {
			if ((NPC.life <= NPC.lifeMax * 0.35 && !Main.masterMode) || (NPC.life <= NPC.lifeMax * 0.32 && Main.masterMode))
				return 3;
			else if ((NPC.life <= NPC.lifeMax * 0.6 && Main.expertMode && !Main.masterMode) || (NPC.life <= NPC.lifeMax * 0.55 && Main.masterMode))
				return 2;
			else return 1;
		}

        public override void AI()
		{
			NPC.netUpdate = true;
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			Player player = Main.player[NPC.target];
			Vector2 targetCenter = player.Center - NPC.Center;
			Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
			if (player.dead)
			{
				// If the targeted player is dead, flee
				NPC.velocity.Y -= 0.04f;
				// This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
				NPC.EncourageDespawn(10);
				NPC.lifeRegenCount += 4000;
				return;
			}
			--timer1;
			--timer2;
			--sequenceTimer;
			int rageBuff = 0;
            try
            {
				rageBuff = 60 * (NPC.lifeMax * 2 / NPC.life / 10);
			}
            catch
            {
				NPC.active = false;
            }
			float distance = Vector2.Distance(target, NPC.Center);
			for (int i = 0; i < NPC.buffImmune.Length; i++)
			{
				NPC.buffImmune[i] = true;
			}
			if (rageBuff >= 60) rageBuff = 60;
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				//Атаки
				if (timer1 <= 0)
				{
					choose = Main.rand.Next(10);
					switch (choose)
                    {
						case 0: case 9: 
							RotationAttackToCenter(); timer1 = Main.rand.Next(160 - rageBuff / 2, 280 - rageBuff); break;
						case 1: case 2: case 3: case 7:
							InPlayerAttack(player); timer1 = Main.rand.Next(100 - rageBuff / 2, 195 - rageBuff); break;
						case 4: case 5: case 6: case 8:
							WideScytheAttack(player); timer1 = Main.rand.Next(140 - rageBuff / 2, 220 - rageBuff); break;
					}
				}				

				if (timer2 <= 0)
                {
					if (sequence == "None")
                    {
						//if (dashed == 1)
						//{
						//	MoveToSideOfPlayer(player, distance > 400 ? 10f : 7f);
						//}
						//continueMove = 0;
					choosing:
						choose1 = Main.rand.Next(2) + 1;
						if (GetPhase() == 3 && Main.rand.NextFloat() <= 0.28f)
						{
							if (++sequenceCounter >= 0)
                            {
								sequence = "TrippleDash";
								choose1 = 0;
								sequenceCounter = 0;
								timer2 = 90;
							}
						}
						switch (choose1)
						{
							case 0: timer2 = 90; break;
							case 1: continueMove = 1; timer2 = 180; dashed = 0; side = Main.rand.Next(8)+1; break;
							case 2: if (dashed == 0 && sequence == "None")
								{
									if (GetPhase() == 2) StarAttack();
									center = targetCenter;
									face = NPC.direction;
									timer2 = 50; dashed = 1; continueMove = 2;
								};
								break; 
						}
						if (timer2 <= 0 ) goto choosing;
					}

                }
				if (sequence == "TrippleDash" && sequenceTimer <= 0 && sequenceCounter >= 0)
				{
					if (++sequenceCounter > 3) { sequence = "None"; sequenceCounter = -3; sequenceTimer = 0; timer1 = 60; timer2 = 60; dashed = 1; continueMove = 0; }
                    else
                    {
						timer1 = 160;
						timer2 = 160;
						center = targetCenter;
						face = NPC.direction;
						continueMove = 2;
						sequenceTimer = 40;
						StarAttack();
					}
				}
				switch (continueMove)
                {
					case 0: SlowDown(); dashed = 1; break;
					case 1: MoveToSideOfPlayer(player, distance > 400 ? 12f : 8.5f, side); dashed = 0; break;
					case 2: DashIntoPlayer(center); dashed = 1; break;
				}
			}
			if (dashed == 0 || continueMove == 0)
            {
				float maxRotate = MathHelper.ToRadians(NPC.velocity.X * 3);
				rotate += 0.06f * NPC.direction;
				if (rotate > maxRotate && NPC.direction == 1) rotate = maxRotate;
				if (rotate < maxRotate && NPC.direction == -1) rotate = maxRotate;
				NPC.rotation = rotate;
			}
			else NPC.rotation += .2f * face;
		}

		//// --------------------------- Атаки --------------------------- ////

		private void RotationAttackToCenter()
        {
			float toPlayer = Main.rand.NextFloat(160f, 210f);
			//float held = toPlayer;
			for (int k = 1; k <= 6; k++)
			{
				//toPlayer = held;
				//if (k % 2 == 0) toPlayer += held / 4;
				int lightBall = Projectile.NewProjectile(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, 0f, 0f, ModContent.ProjectileType<Light_Ball_Circle_6>(), (int)(NPC.damage / (Main.masterMode ? 5 : 3)), 3f, Main.myPlayer, k, toPlayer);
				Main.projectile[lightBall].netUpdate = true;

				if (GetPhase() == 3)
                {
					int lightBallPhase = Projectile.NewProjectile(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, 0f, 0f, ModContent.ProjectileType<Light_Ball_Circle_6>(), (int)(NPC.damage / (Main.masterMode ? 5 : 3)), 3f, Main.myPlayer, k, toPlayer + 145f);
					Main.projectile[lightBallPhase].localAI[0] = 1;
					Main.projectile[lightBallPhase].netUpdate = true;
				}
			}
		}

		 ////                           Выстрел тремя снарядами								////
		//// При достижении определённого здоровья пускает две дополнительные косы побокам ////
		private void InPlayerAttack(Player player)
        {
			Vector2 target = player.Center - NPC.Center;
			float magnitude = (float)Math.Sqrt(target.X * target.X + target.Y * target.Y);
			target.Normalize();
			target *= 16f;
			float rotation = MathHelper.ToRadians(17);
			Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, target.X, target.Y, ModContent.ProjectileType<Light_Ball_Forward>(), (int)(NPC.damage / 4), 3f, Main.myPlayer);
			for (int i = 0; i < 3; i++)
			{
				Vector2 perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / 2));
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Light_Ball_Forward>(),
					NPC.damage / 5, 3f, Main.myPlayer);
				if ((NPC.life <= NPC.lifeMax * 0.38 && Main.expertMode && !Main.masterMode) || (NPC.life <= NPC.lifeMax * 0.55 && Main.masterMode))
                {
					perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation / 1.5f, rotation / 1.5f, i / 2));
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X * 0.9f, perturbedSpeed.Y * 0.9f, 
						ModContent.ProjectileType<Angelic_Scythe>(), NPC.damage / 5, 3f, Main.myPlayer);
				}
			}
		}

		private void StarAttack()
        {
			timer1 = 60;
			for (int k = 1; k <= 12; k++)
            {
				float max;
				if (sequence == "TrippleDash")
                {
					if (k % 2 != 0) max = 3.6f;
					else max = 2.5f;
				}
                else
                {
					if (k % 2 != 0) max = 5.8f;
					else max = 4f;
				}
				
				double angle = Math.PI * 2.0 * k / 12.0;
				Vector2 speed = new((float)Math.Cos(angle), (float)Math.Sin(angle));
				speed.Normalize();
				speed *= max;
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<Light_Ball_Forward>(), NPC.damage / 6, 3f, Main.myPlayer, 1);
			}
		}



		private void WideScytheAttack(Player player)
        {
			Vector2 target = player.Center - NPC.Center;
			float magnitude = (float)Math.Sqrt(target.X * target.X + target.Y * target.Y);
			target.Normalize();
			target *= 14f;
			float rotation = MathHelper.ToRadians(30);
			Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, target.X, target.Y, ModContent.ProjectileType<Angelic_Scythe>(), (int)(NPC.damage / 2.5), 3f, Main.myPlayer);
			for (int i = 0; i < 2; i++)
			{
				Vector2 perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / 1));
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Light_Ball_Forward>(), NPC.damage / 4, 3f, Main.myPlayer);
				perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation /2, rotation / 2, i / 1));
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Light_Ball_Forward>(), NPC.damage / 5, 3f, Main.myPlayer);
			}
		}

		// --------------------------- Движение ---------------------------

		private void SlowDown()
        {
			/*if (Math.Abs(NPC.velocity.Y) > 8f) NPC.velocity.Y *= 0.97f;
			if (Math.Abs(NPC.velocity.X) > 8f) NPC.velocity.X *= 0.97f;*/
			NPC.SimpleFlyMovement(new(0, 0), 0.68f);
			//NPC.velocity *= 0.986f;
        }

		private void MoveToSideOfPlayer(Player player, float maxSpeed, int side)
		{
			Vector2 toSide = new Vector2();
			switch (side)
            {
				case 1: toSide = new(-160, 0); break;	//Лево
				case 2: toSide = new(-160, -160); break;	//Вниз-лево
				case 3: toSide = new(0, -160); break;	//Вниз
				case 4: toSide = new(160, -160); break;	//Вниз-право
				case 5: toSide = new(160, 0); break;	//Право
				case 6: toSide = new(160, 160); break;	//Верх-право
				case 7: toSide = new(0, 160); break;	//Верх
				case 8: toSide = new(-160, 160); break;	//Лево-верх
			}	
			NPC.MoveTowards(player.Center + (toSide * new Vector2(1, -1)), maxSpeed, 4f);
			NPC.noTileCollide = true;
		}

		private void DashIntoPlayer(Vector2 targetCenter)
        {
			NPC.TargetClosest(false);
			targetCenter.Normalize();
			float magnitude = (float)Math.Sqrt(targetCenter.X * targetCenter.X + targetCenter.Y * targetCenter.Y);
			Vector2 speed = targetCenter * 15f;
			NPC.velocity = speed;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			//target.AddBuff(ModContent.BuffType<Under_Crucifixion_Tier2>(), Main.rand.Next(6, 13)*60);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			Item.NewItem(NPC.GetSource_Loot() ,NPC.getRect(), ModContent.ItemType<Impure_Dust>(), Main.rand.Next(1, 5));
		}

    }

}