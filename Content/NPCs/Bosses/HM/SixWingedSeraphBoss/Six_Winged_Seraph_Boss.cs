using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Dusts;
using Ferustria.Content.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using System.Collections.Generic;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.NPCs.Bosses.HM.SixWingedSeraphBoss
{
    public class Six_Winged_Seraph_Boss : ModNPC
	{
		/// <summary>
		/// Таймер атак
		/// </summary>
		private int attackTimer = 200;
		private int moveTimer = 0;
        private int attackDenominator, sequenceDenominator, movementDenominator;
        private int choosenAttack, continueAttack, moveChoise, continueMove;
        private int dashed, face, sequenceTimer, side;
        private List<int> attackWeights = new() { 200, 800, 600 }; //Круговая атака, три шара в игрока, с косой по середине
        private List<int> sequenceWeights = new() { 600, 325 }; //1 - Тройной дэш; 2 - Кружения с обстрелом
        private List<int> movementWeights = new() { 80, 250, 135 }; //Продление, движение в стороне от игрока, дэш в игрока
        private int allAttackWeights, allSequenceWeights, allMovementWeights;
		private float accSpeedY, accSpeedX, rotate;
		private string sequence;
        private int projDamage_lightBall = (int)(FSHelper.Scale(130, 165, 250) / 2.5);
        private int projDamage_lightBallCircle = (int)(FSHelper.Scale(100, 145, 200) / 2.5);
        private int projDamage_angelicScythe = (int)(FSHelper.Scale(220, 280, 350) / 2.3);
        private float maxDashSpeed = !Main.masterMode ? 16f : 20f;
        Vector2 center;
		/// <summary>
		/// Ограничитель в единицах перед началом сиквенции
		/// </summary>
		private int sequenceCounter;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morghos, Six-Winged Seraph");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Моргос, Шести-крылый Серафим");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.BossBestiaryPriority.Add(Type);
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = FSHelper.Scale(70000, 85500, 115000);
			NPC.damage = FSHelper.Scale(180, 230, 285);
			NPC.defense = FSHelper.WOScale(30, 35, 40);
			NPC.knockBackResist = 0f;
			NPC.width = 192 / 3;
			NPC.height = 120 / 2;
            DrawOffsetY = NPC.width / 3;
            NPC.noGravity = true;
			NPC.aiStyle = -1;
			NPC.boss = true;
			NPC.npcSlots = 5f;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath4;
			NPC.value = Item.sellPrice(0, 0, 1, 0);
			NPC.noTileCollide = true;
			Music = MusicID.OtherworldlyBoss1;
			accSpeedX = accSpeedY = 0;
			dashed = 1;
			sequenceTimer = 0;
			sequence = "None";
			sequenceCounter = 0;
            CalculateWeights(true, true, true);
            NPC.buffImmune[ModContent.BuffType<Weak_Void_Leach>()] = true;
            NPC.buffImmune[BuffID.Confused] = true;
        }

        bool summoned = false;
        int appearenceTime = 0;
        bool allowAi = false;

        public override void OnSpawn(IEntitySource source)
        {
            if (source.Context == "Summoned")
            {
                summoned = true;
                NPC.alpha = 255;
                NPC.direction = 1;
            }
            else allowAi = true;
        }

        int GetPhase()
        {
			if ((NPC.life <= NPC.lifeMax * 0.35 && !Main.masterMode) || (NPC.life <= NPC.lifeMax * 0.32 && Main.masterMode))
				return 3;
			else if ((NPC.life <= NPC.lifeMax * 0.6 && Main.expertMode && !Main.masterMode) || (NPC.life <= NPC.lifeMax * 0.55 && Main.masterMode))
				return 2;
			else return 1;
		}

        void CalculateWeights(bool calcAttacks = false, bool calcSequence = false, bool calcMovement = false)
        {
            if (calcAttacks)
            {
                allAttackWeights = 0;
                foreach (int weight in attackWeights)
                {
                    allAttackWeights += weight;
                }
                for (int i = 1; i < attackWeights.Count; i++)
                {
                    attackWeights[i] += attackWeights[i - 1];
                }
            }
            if (calcSequence)
            {
                allSequenceWeights = 0;
                foreach (int weight in sequenceWeights)
                {
                    allSequenceWeights += weight;
                }
                for (int i = 1; i < sequenceWeights.Count; i++)
                {
                    sequenceWeights[i] += sequenceWeights[i - 1];
                }
            }
            if (calcMovement)
            {
                allMovementWeights = 0;
                foreach (int weight in movementWeights)
                {
                    allMovementWeights += weight;
                }
                for (int i = 1; i < movementWeights.Count; i++)
                {
                    movementWeights[i] += movementWeights[i - 1];
                }
            }
        }

        bool setSecondPhaseWeights = false;
        bool setThirdPhaseWeights = false;
        int teleportTime;

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
				NPC.velocity.Y -= 0.065f;
                NPC.velocity.X *= 0.98f;
				NPC.EncourageDespawn(10);
				NPC.lifeRegenCount += 16000;
				return;
			}

            if (summoned && !allowAi)
            {
                BossAppearence();
            }

            if (allowAi)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    --attackTimer;
                    --moveTimer;
                    --sequenceTimer;
                    int rageBuff = 0;
                    try
                    {
                        rageBuff = 80 * (NPC.lifeMax * 2 / NPC.life / 10);
                    }
                    catch
                    {
                        NPC.active = false;
                    }
                    if (rageBuff > 80) rageBuff = 80;
                    float distance = Vector2.Distance(target, NPC.Center);
                    if (GetPhase() == 2 && !setSecondPhaseWeights)
                    {
                        attackWeights = new List<int> { 275, 750, 585, 350 };
                        CalculateWeights(true);
                        setSecondPhaseWeights = true;
                    }

                    ////     Атаки     ////

                    int modifier = 1;
                    if (attackTimer <= 0)
                    {
                        attackDenominator = Convert.ToInt32(Main.rand.NextFloat() * allAttackWeights);

                        for (int i = 0; i < attackWeights.Count; i++)
                        {
                            if (attackWeights[i] > attackDenominator) { choosenAttack = i; break; }
                        }

                        switch (choosenAttack)
                        {
                            case 0: RotationAttackToCenter(); attackTimer = Main.rand.Next(160 - rageBuff / 2, 280 - rageBuff); break;
                            case 1: InPlayerTripleAttack(player); attackTimer = Main.rand.Next(100 - rageBuff / 2, 195 - rageBuff); break;
                            case 2: WideScytheAttack(player); attackTimer = Main.rand.Next(140 - rageBuff / 2, 220 - rageBuff); break;
                            case 3:
                                continueAttack = 1;
                                attackTimer = 999; moveTimer = 999; moveChoise = 10;
                                continueMove = 10; teleportTime = Main.rand.Next(80, 105);
                                modifier = Main.rand.NextBool() ? 1 : -1; break;
                        }
                    }

                    if (continueAttack > 0)
                    {
                        switch (continueAttack)
                        {
                            case 1: RapidInPlayerAttack(player, true); break;
                        }
                    }
                    ////     Движения    ////

                    if (Math.Abs(NPC.velocity.X) < .35f && Math.Abs(NPC.velocity.Y) < .35f && continueMove == 0) moveTimer = 0;

                    if (moveTimer <= 0)
                    {
                        if (sequence == "None")
                        {
                            if (dashed == 1)
                            {
                                MoveToSideOfPlayer(player, distance > 400 ? 12f : 8.5f, 7);
                                NPC.damage = FSHelper.Scale(130, 150, 180);
                            }

                            while (moveTimer <= 0)
                            {
                                movementDenominator = Convert.ToInt32(Main.rand.NextFloat() * allMovementWeights);

                                for (int i = 0; i < movementWeights.Count; i++)
                                {
                                    if (movementWeights[i] > movementDenominator) { moveChoise = i; break; }
                                }
                                if (moveChoise == 0 && dashed == 1) moveChoise = 1;
                                if (GetPhase() == 3 && Main.rand.NextFloat() <= 0.28f)
                                {
                                    if (++sequenceCounter >= 0)
                                    {
                                        sequence = "TrippleDash";
                                        attackDenominator = 0;
                                        choosenAttack = 0;
                                        sequenceCounter = 0;
                                        moveTimer = !Main.masterMode ? 90 : 70;
                                    }
                                }
                                switch (moveChoise)
                                {
                                    case 0: moveTimer = Main.rand.Next(90, 180); break;
                                    case 1: continueMove = 1; moveTimer = Main.rand.Next(180, 260); dashed = 0; side = Main.rand.Next(8) + 1; break;
                                    case 2:
                                        if (dashed == 0 && sequence == "None")
                                        {
                                            if (GetPhase() == 2) StarAttack();
                                            center = targetCenter;
                                            face = NPC.direction;
                                            moveTimer = !Main.masterMode ? 50 : 35; dashed = 1; continueMove = 2; attackTimer = moveTimer + 20;
                                            NPC.damage = FSHelper.Scale(220, 265, 280);
                                        };
                                        break;
                                    case 10: continueMove = 10; moveTimer = 999; break;
                                }
                            }
                        }
                    }
                    if (sequence == "TrippleDash" && sequenceTimer <= 0 && sequenceCounter >= 0)
                    {
                        if (++sequenceCounter > 3) { sequence = "None"; sequenceCounter = -3; sequenceTimer = 0; attackTimer = 60; moveTimer = 60; dashed = 1; continueMove = 0; }
                        else
                        {
                            attackTimer = 160;
                            moveTimer = 160;
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
                        case 10: CircleAroundThePlayer(player, modifier); dashed = 0; break;
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
            }
		}

          ///////////////////////////////////////////////////////////////////////
		 //// --------------------------- Атаки --------------------------- ////
        ///////////////////////////////////////////////////////////////////////
		private void RotationAttackToCenter()
        {
			float toPlayer = Main.rand.NextFloat(160f, 210f);
			//float held = toPlayer;
			for (int k = 1; k <= 6; k++)
			{
				//toPlayer = held;
				//if (k % 2 == 0) toPlayer += held / 4;
				int lightBall = Projectile.NewProjectile(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, 0f, 0f, ModContent.ProjectileType<Light_Ball_Circle_6>(), projDamage_lightBallCircle, 3f, Main.myPlayer, k, toPlayer);
				Main.projectile[lightBall].netUpdate = true;

				if (GetPhase() == 3)
                {
					int lightBallPhase = Projectile.NewProjectile(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, 0f, 0f, ModContent.ProjectileType<Light_Ball_Circle_6>(), projDamage_lightBallCircle, 3f, Main.myPlayer, k, toPlayer + 145f);
					Main.projectile[lightBallPhase].localAI[0] = 1;
					Main.projectile[lightBallPhase].netUpdate = true;
				}
			}
		}

		 ////                           Выстрел тремя снарядами								////
		//// При достижении определённого здоровья пускает две дополнительные косы побокам ////
		private void InPlayerTripleAttack(Player player)
        {
			Vector2 target = player.Center - NPC.Center;
			target.Normalize();
			target *= 16f;
			float rotation = MathHelper.ToRadians(17);
			Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, target.X, target.Y, ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall + 12, 3f, Main.myPlayer);
			for (int i = 0; i < 3; i++)
			{
				Vector2 perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / 2));
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Light_Ball_Forward>(),
                    projDamage_lightBall, 3f, Main.myPlayer);
				if ((NPC.life <= NPC.lifeMax * 0.38 && Main.expertMode && !Main.masterMode) || (NPC.life <= NPC.lifeMax * 0.55 && Main.masterMode))
                {
					perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation / 1.5f, rotation / 1.5f, i / 2));
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X * 0.9f, perturbedSpeed.Y * 0.9f, 
						ModContent.ProjectileType<Angelic_Scythe>(), projDamage_angelicScythe, 3f, Main.myPlayer);
				}
			}
		}

		private void StarAttack()
        {
			attackTimer = 80;
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
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall - 8, 3f, Main.myPlayer, 1);
			}
		}



		private void WideScytheAttack(Player player)
        {
			Vector2 target = player.Center - NPC.Center;
			target.Normalize();
			target *= 14f;
			float rotation = MathHelper.ToRadians(30);
			Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, target.X, target.Y, ModContent.ProjectileType<Angelic_Scythe>(), projDamage_angelicScythe, 3f, Main.myPlayer);
			for (int i = 0; i < 2; i++)
			{
				Vector2 perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / 1));
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall, 3f, Main.myPlayer);
				perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation /2, rotation / 2, i / 1));
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall, 3f, Main.myPlayer);
			}
		}

        ////     Стрельба в цель, как из пулемёта      ////
        int await = 0;
        int rapidAttackLocalTimer = 0;
        private void RapidInPlayerAttack(Player player, bool tping = false)
        {
            if ((tping && appeared))
            {
                if (rapidAttackLocalTimer-- < 0)
                {
                    Vector2 target = player.Center - NPC.Center;
                    target.Normalize();
                    Vector2 speed = target * 13.5f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<Light_Ball_Forward>(), (int)(projDamage_lightBall / 1.5), 3f, Main.myPlayer);
                    rapidAttackLocalTimer = 5;
                }
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

        private float actualSpeed = 0f;

		private void MoveToSideOfPlayer(Player player, float maxSpeed, int side)
		{
            if (actualSpeed < maxSpeed) actualSpeed += 0.036f;
            if (actualSpeed > maxSpeed) actualSpeed -= 0.028f;
			Vector2 toSide = new();
			switch (side)
            {
				case 1: toSide = new(-160, 0); break;	    //Лево
				case 2: toSide = new(-160, -160); break;	//Вниз-лево
				case 3: toSide = new(0, -160); break;	    //Вниз
				case 4: toSide = new(160, -160); break;	    //Вниз-право
				case 5: toSide = new(160, 0); break;	    //Право
				case 6: toSide = new(160, 160); break;	    //Верх-право
				case 7: toSide = new(0, 160); break;	    //Верх
				case 8: toSide = new(-160, 160); break;	    //Лево-верх
			}	
			NPC.MoveTowards(player.Center + (toSide * new Vector2(1, -1)), maxSpeed, 4f);
			NPC.noTileCollide = true;
		}

		private void DashIntoPlayer(Vector2 targetCenter)
        {
			NPC.TargetClosest(false);
			targetCenter.Normalize();
			NPC.velocity = targetCenter * maxDashSpeed;
        }

        bool appeared = false;
        int maxTime;
        private void CircleAroundThePlayer(Player player, int modifier)
        {
            Vector2 sideToStart = new();
            float distance = 260f;
            switch ((side + 5) % 8)
            {
                case 1: sideToStart = new(-160, 0); break;       //Лево
                case 2: sideToStart = new(-160, -160); break;    //Вниз-лево
                case 3: sideToStart = new(0, -160); break;       //Вниз
                case 4: sideToStart = new(160, -160); break;     //Вниз-право
                case 5: sideToStart = new(160, 0); break;        //Право
                case 6: sideToStart = new(160, 160); break;      //Верх-право
                case 7: sideToStart = new(0, 160); break;        //Верх
                case 0: sideToStart = new(-160, 160); break;     //Лево-верх
            }
            if (maxTime < 1)
            {
                maxTime = teleportTime;
            }
            if (teleportTime > 0 && !appeared)
            {
                MoveToSideOfPlayer(player, 9f, side);
                for (int i = 0; i < 4; i++)
                {
                    double angle = Math.Abs(sideToStart.ToRotation());
                    Vector2 pos = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                    pos.SafeNormalize(Vector2.Zero);
                    pos *= distance;
                    Dust.NewDust(pos + player.Center, 60 - (teleportTime / maxTime * 60), 60 - (teleportTime / maxTime * 60), ModContent.DustType<Angelic_Particles>(),
                        Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1.5f, 1.5f), 0, default, Main.rand.NextFloat(.8f, 1.25f));
                    NPC.alpha = (int)((double)maxTime / (double)teleportTime * 60);
                }
                teleportTime--;
            }
            else
            {
                //// ~~~~~~~~~~~ Появление
                if (!appeared)
                {
                    NPC.alpha = 0;
                    double angleC = Math.Abs(sideToStart.ToRotation());
                    Vector2 posC = new((float)Math.Cos(angleC), (float)Math.Sin(angleC));
                    posC.SafeNormalize(Vector2.Zero);
                    posC *= distance;
                    NPC.position = posC + player.position;
                    for (int i = 0; i < 250; i++)
                    {
                        double angle = 2.0 * Math.PI * i / 250;
                        Vector2 speed = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                        speed.SafeNormalize(Vector2.Zero);
                        speed *= 7f;
                        Dust.NewDustPerfect(NPC.position, ModContent.DustType<Star_of_Hope_Effect>(), new(speed.X, speed.Y), 20, default, 3f);
                    }
                    for (int i = 0; i < 200; i++)
                    {
                        double angle = 2.0 * Math.PI * i / 200;
                        Vector2 speed = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                        speed.SafeNormalize(Vector2.Zero);
                        speed *= 5.5f;
                        Dust.NewDustPerfect(NPC.position, ModContent.DustType<Star_of_Hope_Effect>(), new(speed.X, speed.Y), 20, default, 2.5f);
                    }
                    for (int i = 0; i < 150; i++)
                    {
                        double angle = 2.0 * Math.PI * i / 150;
                        Vector2 speed = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                        speed.SafeNormalize(Vector2.Zero);
                        speed *= 4f;
                        Dust.NewDustPerfect(NPC.position, ModContent.DustType<Star_of_Hope_Effect>(), new(speed.X, speed.Y), 20, default, 2f);
                    }
                    appeared = true;
                }

                //// ~~~~~~~~~~~ Кружение

                if (++teleportTime <= 80 && appeared)
                {
                    double angleC = ((2.0 * Math.PI * (double)teleportTime / 80.0 * 0.45) + Math.Abs(sideToStart.ToRotation())) * modifier;
                    Vector2 posC = new((float)Math.Cos(angleC), (float)Math.Sin(angleC));
                    posC.SafeNormalize(Vector2.Zero);
                    posC *= distance;
                    NPC.position = posC + player.position;
                    NPC.rotation = 0;
                }
                else
                {
                    moveTimer = 20;
                    attackTimer = Main.rand.Next(45, 90);
                    appeared = false;
                    maxTime = 0;
                    continueAttack = 0;
                    continueMove = 0;
                    moveChoise = 0;
                    NPC.alpha = 0;
                }
            }
        }

        bool[] line = new bool[9];
        bool[] line2 = new bool[3]; 
        readonly List<string> text = new();
        readonly List<string> text2 = new();

        void SetTextTranslation()
        {
            if (LanguageManager.Instance.ActiveCulture == FSHelper.RuTrans)
            {
                text.AddRange(new string[]
                {
                    "Террариан...",
                    "Благодарю тебя за своё освобождение.",
                    "Наконец-то я свободен...",
                    "Пустота заточили меня в эту шкатулку.",
                    "Но...",
                    "Преступления... Которые ты совершил против ангелов...",
                    "Не могут быть прощены.",
                    "И наказание за это...",
                    "Смерть!"
                });
                text2.AddRange(new string[]
                {
                    "Ах, вот оно что...",
                    "Так ты.. не отсюда...",
                    "Да простят нас.. всевышние..."
                });
            }
            else
            {
                text.AddRange(new string[]
                {
                    "Terrarian...",
                    "My gratitude upon my freedom.",
                    "Free at last...",
                    "The Void sealed me inside this box.",
                    "But...",
                    "Crimes... thy have commited against the angels...",
                    "Could not be forgiven.",
                    "And thy punishment for it...",
                    "Is Death!"
                });
                text2.AddRange(new string[]
                {
                    "Ah, so that's it...",
                    "Thy.. not from here...",
                    "May the Almighty.. forgive us..."
                });
            }
                
        }

        void BossAppearence()
        {
            if (text.Count < 1) SetTextTranslation();
            
            appearenceTime++;
            NPC.velocity.Y = 0;
            if (appearenceTime < 3 * 60)
            {
                NPC.alpha -= 2;
                NPC.velocity.Y = -0.5f;
            }
            else if (appearenceTime < (3 + 3) * 60)
            {
                if (!line[0])
                { Main.NewText(text[0], 0, 200, 255); line[0] = true; }
            }
            else if (appearenceTime < (6 + 4) * 60)
            {
                if (!line[1])
                { Main.NewText(text[1], 0, 200, 255); line[1] = true; }
            }
            else if (appearenceTime < (10 + 3) * 60)
            {
                if (!line[2])
                { Main.NewText(text[2], 0, 200, 255); line[2] = true; }
            }
            else if (appearenceTime < (13 + 4) * 60)
            {
                if (!line[3])
                { Main.NewText(text[3], 0, 200, 255); line[3] = true; }
            }
            else if (appearenceTime < (17 + 2) * 60)
            {
                if (!line[4])
                { Main.NewText(text[4], 0, 200, 255); line[4] = true; }
            }
            else if (appearenceTime < (19 + 4.5) * 60)
            {
                if (!line[5])
                { Main.NewText(text[5], 0, 200, 255); line[5] = true; }
            }
            else if (appearenceTime < (24.5 + 2.5) * 60)
            {
                if (!line[6])
                { Main.NewText(text[6], 0, 200, 255); line[6] = true; }
            }
            else if (appearenceTime < (27 + 3) * 60)
            {
                if (!line[7])
                { Main.NewText(text[7], 0, 200, 255); line[7] = true; }
            }
            else if (appearenceTime < (30 + 2) * 60)
            {
                if (!line[8])
                { Main.NewText(text[8], 0, 255, 255); line[8] = true; }
            }
            else if (appearenceTime < (32 + 2) * 60) allowAi = true;
        }


        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (NPC.alpha > 160) return false;
            if (!allowAi) return false;
            return true;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (NPC.alpha > 160) return false;
            if (!allowAi) return false;
            return null;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (NPC.alpha > 160) return false;
            if (!allowAi) return false;
            return null;
        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Sliced_Defense>(), Main.rand.Next(4, 8) * 60);
			//target.AddBuff(ModContent.BuffType<Under_Crucifixion_Tier2>(), Main.rand.Next(6, 13)*60);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			Item.NewItem(NPC.GetSource_Loot() ,NPC.getRect(), ModContent.ItemType<Impure_Dust>(), Main.rand.Next(1, 5));
		}

    }

}