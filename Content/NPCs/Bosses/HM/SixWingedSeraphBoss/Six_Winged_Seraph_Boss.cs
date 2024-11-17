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
    [AutoloadBossHead]
    public class Six_Winged_Seraph_Boss : ModNPC
	{
		/// <summary>
		/// Таймер атак
		/// </summary>
		int attackTimer = 200;
		int moveTimer = 0;
        int attackDeterminator, sequenceDeterminator, movementDeterminator;
        int choosenAttack, continueAttack, moveChoise, continueMove;
        int dashed, face, sequenceTimer;
        float fromAngle, flyDistanceToPlayer;
        List<int> attackWeights = [200, 800, 600, 240];
        List<int> sequenceWeights = [600, 325]; //1 - Тройной дэш; 2 - Кружения с обстрелом
        List<int> movementWeights = [80, 250, 135]; //Продление, движение в стороне от игрока, дэш в игрока
        int allAttackWeights, allSequenceWeights, allMovementWeights;
		float accSpeedY, accSpeedX, rotate;
		string sequence;
        int projDamage_lightBall = (int)(FSHelper.Scale(130, 165, 250) / 2.5);
        int projDamage_lightBallCircle = (int)(FSHelper.Scale(100, 145, 200) / 2.5);
        int projDamage_angelicScythe = (int)(FSHelper.Scale(220, 280, 350) / 2.3);
        float maxDashSpeed = !Main.masterMode ? 16f : 20f;
        Vector2 center;
        /// <summary>
		/// Ограничитель в единицах перед началом последовательности
		/// </summary>
		int sequenceCounter;

        enum Sequence
        {
            None,
            TrippleDash
        }
        Sequence currentSequence;

        Player player;

        int GetPhase
        {
            get
            {
                if (NPC.life <= NPC.lifeMax * (Main.masterMode ? 0.32 : 0.35))
                    return 3;
                else if (NPC.life <= NPC.lifeMax * (Main.masterMode ? 0.55 : 0.6))
                    return 2;
                return 1;
            }
        }
		

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Weak_Barathrum_Leach>()] = true;

            // Influences how the NPC looks in the Bestiary
            //NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            //{
            //    CustomTexturePath = "ExampleMod/Assets/Textures/Bestiary/MinionBoss_Preview",
            //    PortraitScale = 0.6f, // Portrait refers to the full picture when clicking on the icon in the bestiary
            //    PortraitPositionYOverride = 0f,
            //};
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

		public override void SetDefaults()
		{
			NPC.lifeMax = FSHelper.Scale(70000, 90500, 115000);
			NPC.damage = FSHelper.Scale(180, 230, 285);
			NPC.defense = 40;
			NPC.knockBackResist = 0f;
			NPC.width = 192 / 3;
			NPC.height = 120 / 2;
            DrawOffsetY = NPC.width / 3;
            NPC.noGravity = true;
			NPC.aiStyle = -1;
			NPC.boss = true;
			NPC.npcSlots = 15f;
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
            NPC.direction = 1;
            NPC.netAlways = true;
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

            Vector2 targetDefinedPosition, target;

            if (NPC.HasValidTarget)
            {
                player = Main.player[NPC.target];
                target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
                targetDefinedPosition = player.Center - NPC.Center;
            }
            else return;
			
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

                    //// ~~~~ Веса Второй фазы
                    if (GetPhase == 2 && !setSecondPhaseWeights)
                    {
                        attackWeights = [275, 750, 585, 350, 325, 260];
                        CalculateWeights(true);
                        setSecondPhaseWeights = true;
                    }

                    ////     Атаки     ////

                    int modifier = 1;
                    if (attackTimer <= 0)
                    {
                        attackDeterminator = Main.rand.Next(allAttackWeights);

                        for (int i = 0; i < attackWeights.Count; i++)
                        {
                            if (attackWeights[i] > attackDeterminator) { choosenAttack = i; break; }
                        }
                        switch (choosenAttack)
                        {
                            case 0: RotationAttackToCenter(); attackTimer = Main.rand.Next(160 - rageBuff / 2, 280 - rageBuff); break;
                            case 1: InPlayerTripleAttack(player); attackTimer = Main.rand.Next(100 - rageBuff / 2, 195 - rageBuff); break;
                            case 2: WideScytheAttack(player); attackTimer = Main.rand.Next(140 - rageBuff / 2, 220 - rageBuff); break;
                            case 3: OutOfScreenAttack(player); attackTimer = Main.rand.Next(165 - rageBuff / 2, 300 - rageBuff); break;
                            case 4: GatheringAttack(player); attackTimer = Main.rand.Next(170 - rageBuff / 2, 250 - rageBuff); break;
                            case 5:
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
                                MoveToSideOfPlayer(player, distance > 400 ? 12f : 8.5f, fromAngle);
                                NPC.damage = FSHelper.Scale(130, 150, 180);
                            }

                            while (moveTimer <= 0)
                            {
                                movementDeterminator = Main.rand.Next(allMovementWeights);

                                for (int i = 0; i < movementWeights.Count; i++)
                                {
                                    if (movementWeights[i] > movementDeterminator) { moveChoise = i; break; }
                                }
                                if (moveChoise == 0 && dashed == 1) moveChoise = 1;
                                if (GetPhase == 3 && Main.rand.NextFloat() <= 0.28f)
                                {
                                    if (++sequenceCounter >= 0)
                                    {
                                        sequence = "TrippleDash";
                                        attackDeterminator = 0;
                                        choosenAttack = 0;
                                        sequenceCounter = 0;
                                        moveTimer = !Main.masterMode ? 90 : 70;
                                    }
                                }
                                switch (moveChoise)
                                {
                                    case 0: moveTimer = Main.rand.Next(90, 180); break;
                                    case 1: continueMove = 1; moveTimer = Main.rand.Next(180, 260); dashed = 0; 
                                        fromAngle = MathHelper.ToRadians(Main.rand.NextFloat(360f)); flyDistanceToPlayer = Main.rand.NextFloat(120f, 280f) ; break;
                                    case 2:
                                        if (dashed == 0 && sequence == "None")
                                        {
                                            if (GetPhase == 2) StarAttack();
                                            center = targetDefinedPosition;
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
                            center = targetDefinedPosition;
                            face = NPC.direction;
                            continueMove = 2;
                            sequenceTimer = 40;
                            StarAttack();
                        }
                    }
                    switch (continueMove)
                    {
                        case 0: SlowDown(); dashed = 1; break;
                        case 1: MoveToSideOfPlayer(player, distance > 400 ? 12f : 8.5f, fromAngle); dashed = 0; break;
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
		public void RotationAttackToCenter()
        {
			float toPlayer = Main.rand.NextFloat(160f, 210f);
            //float held = toPlayer;
            for (int k = 1; k <= 6; k++)
            {
                //toPlayer = held;
                //if (k % 2 == 0) toPlayer += held / 4;
                if (Main.netMode != 1)
                {
                    Projectile lightBall = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position, Vector2.Zero,
                    ModContent.ProjectileType<Light_Ball_Circle_6>(), projDamage_lightBallCircle, 3f, Main.myPlayer, k, toPlayer);
                    lightBall.netUpdate = true;

                    if (GetPhase == 3)
                    {
                        Projectile lightBallPhase = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position, Vector2.Zero,
                            ModContent.ProjectileType<Light_Ball_Circle_6>(), projDamage_lightBallCircle, 3f, Main.myPlayer, k, toPlayer + 145f);
                        lightBallPhase.localAI[0] = 1;
                        lightBallPhase.netUpdate = true;
                    }
                }
			}
		}

		 ////                           Выстрел тремя снарядами								////
		//// При достижении определённого здоровья пускает две дополнительные косы побокам ////
		public void InPlayerTripleAttack(Player player)
        {
			Vector2 target = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 16f;
			float rotation = MathHelper.ToRadians(17);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, target.X, target.Y, ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall + 12, 3f, Main.myPlayer);
                for (int i = 0; i < 3; i++)
                {
                    Vector2 perturbedSpeed = target.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / 2));
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y,
                        ModContent.ProjectileType<Light_Ball_Forward>(),
                        projDamage_lightBall, 3f, Main.myPlayer);
                    if ((NPC.life <= NPC.lifeMax * 0.38 && Main.expertMode && !Main.masterMode) || (NPC.life <= NPC.lifeMax * 0.55 && Main.masterMode))
                    {
                        perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation / 1.5f, rotation / 1.5f, i / 2));
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X * 0.9f, perturbedSpeed.Y * 0.9f,
                            ModContent.ProjectileType<Angelic_Scythe>(), projDamage_angelicScythe, 4.5f, Main.myPlayer);
                    }
                }
            }
		}

		public void StarAttack()
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
				Vector2 speed = Vector2.One.GetVector_ToAngle_WithMult((float)angle, max);
				//speed.Normalize();
				//speed *= max;
                if (Main.netMode != 1)
				    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, 
                        ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall - 8, 3f, Main.myPlayer, 1);
			}
		}



		public void WideScytheAttack(Player player)
        {
			Vector2 target = player.Center - NPC.Center;
			target.Normalize();
			target *= 14f;
			float rotation = MathHelper.ToRadians(30);
            if (Main.netMode != 1)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, target.X, target.Y, ModContent.ProjectileType<Angelic_Scythe>(),
                    projDamage_angelicScythe, 4.5f, Main.myPlayer);
                for (int i = 0; i < 2; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / 1));
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y,
                        ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall, 3f, Main.myPlayer);
                    perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation / 2, rotation / 2, i / 1));
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y,
                        ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall, 3f, Main.myPlayer);
                }
                if (GetPhase == 3)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(target.X, target.Y).RotatedBy(MathHelper.Lerp(-rotation * 1.5f, rotation * 1.5f, i / 1));
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y,
                            ModContent.ProjectileType<Angelic_Scythe>(), projDamage_angelicScythe, 4.5f, Main.myPlayer);
                    }
                    
                }
            }
		}

        ////     Стрельба в цель, как из пулемёта      ////
        int await = 0;
        int rapidAttackLocalTimer = 0;
        public void RapidInPlayerAttack(Player player, bool tping = false)
        {
            if ((tping && appeared))
            {
                if (rapidAttackLocalTimer-- < 0)
                {
                    Vector2 target = player.Center - NPC.Center;
                    target.Normalize();
                    Vector2 speed = target * 13.5f;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, 
                            ModContent.ProjectileType<Light_Ball_Forward>(), (int)(projDamage_lightBall / 1.25), 3f, Main.myPlayer);
                    rapidAttackLocalTimer = 5;
                }
            }
        }

          ////    Из экрана вылетает несколько снарядов(5-7) летящие в одну точку    ////
         ////    В этой точке они собираются в один большой шар, который потом стреляет в игрока     ////
        ////    Отстреляв всё, шар потом самоноводкой летит в игрока     ////
        public void GatheringAttack(Player player)
        {
            int projectiles = Main.rand.Next(7, 13);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 topLeftCorner = player.Center + new Vector2(-250f, -250f);
                Vector2 bottomRightCorner = player.Center + new Vector2(250f, 250f);
                Vector2 centerPosition = FerustriaFunctions.RandomPointInArea(topLeftCorner, bottomRightCorner);
                //Mod.Logger.Debug($"TopLeft: {topLeftCorner}; BotmRight: {bottomRightCorner}; Center: {centerPosition}");
                Projectile centerProj = Projectile.NewProjectileDirect(NPC.GetSource_FromThis("GatheringLight:Appear"), centerPosition, Vector2.Zero,
                    ModContent.ProjectileType<Gathering_Light>(), (int)(projDamage_lightBall / 1.25), 3f, Main.myPlayer, ai0: projectiles);
                //Mod.Logger.Debug($"Gatheting proj: {debug1}");
                for (int i = 0; i < projectiles; i++)
                {
                    
                    float angle = MathHelper.TwoPi + MathHelper.ToRadians(Main.rand.NextFloat(360f));
                    int desSpeed = Main.rand.Next(23, 28);
                    Vector2 speed = (Vector2.One * -desSpeed).GetVector_ToAngle(angle);
                    Vector2 fromPoint = Extensions.GetVector_WithAngle(angle) * Main.rand.NextFloat(1500f, 1800f) + centerPosition;
                    Projectile outOS = Projectile.NewProjectileDirect(NPC.GetSource_FromThis("LightBall:OutOfScreenToGather"), fromPoint, speed,
                        ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall, 3f, Main.myPlayer, ai1: centerProj.whoAmI);
                    float distanceToCenter = Vector2.Distance(outOS.Center, centerPosition);
                    Projectile laserLine = Projectile.NewProjectileDirect(NPC.GetSource_FromThis("LaserLine:OutOfScreenToGather"), fromPoint, speed,
                        ModContent.ProjectileType<Laser_Line>(), 0, 0, player.whoAmI, desSpeed, distanceToCenter);
                    //Mod.Logger.Debug($"OutOfScreen proj: {debug2}\n" +
                    //    $"Angle: {angle}; Speed: {speed}; From point: {fromPoint}");
                }
            }
        }

        ////     Шары света, летящие почти в игрока(в рандомную область в квадрате от игрока)     ////
        public void OutOfScreenAttack(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int projectiles = 8;
                if (!Main.expertMode)
                {
                    if (GetPhase >= 2) projectiles += 2;
                    else projectiles += 1;
                }
                else if (Main.expertMode && !Main.masterMode)
                {
                    if (GetPhase == 2) projectiles += 3;
                    if (GetPhase == 3) projectiles += 4;
                    else projectiles += 2;
                }
                else if (Main.masterMode)
                {
                    projectiles += 1;
                    if (GetPhase == 2) projectiles += 4;
                    else if (GetPhase == 3) projectiles += 6;
                    else projectiles += 3;
                }
                for (int i = 0; i < Main.rand.Next(projectiles - 4, projectiles) + 1; i++)
                {
                    Vector2 topLeftCorner = player.Center + new Vector2(-300f, -300f);
                    Vector2 bottomRightCorner = player.Center + new Vector2(300f, 300f);
                    Vector2 centerPosition = FerustriaFunctions.RandomPointInArea(topLeftCorner, bottomRightCorner);
                    float angle = MathHelper.TwoPi + MathHelper.ToRadians(Main.rand.NextFloat(360f));
                    int desSpeed = Main.rand.Next(25, 30);
                    Vector2 speed = (Vector2.One * -desSpeed).GetVector_ToAngle(angle);
                    Vector2 fromPoint = Extensions.GetVector_WithAngle(angle) * Main.rand.NextFloat(2100f, 2900f) + centerPosition;
                    Projectile laserLine = Projectile.NewProjectileDirect(NPC.GetSource_FromThis("LaserLine:OutOfScreen"), fromPoint, speed,
                        ModContent.ProjectileType<Laser_Line>(), 0, 0, player.whoAmI, desSpeed, 6000f);
                    Projectile debug1 = Projectile.NewProjectileDirect(NPC.GetSource_FromThis("LightBall:OutOfScreen"), fromPoint, speed,
                        ModContent.ProjectileType<Light_Ball_Forward>(), projDamage_lightBall, 3f, Main.myPlayer);
                    laserLine.netUpdate = true;
                    debug1.netUpdate = true;
                }
            }
        }
		// --------------------------- Движение ---------------------------

		public void SlowDown()
        {
			/*if (Math.Abs(NPC.velocity.Y) > 8f) NPC.velocity.Y *= 0.97f;
			if (Math.Abs(NPC.velocity.X) > 8f) NPC.velocity.X *= 0.97f;*/
			NPC.SimpleFlyMovement(new(0, 0), 0.68f);
			//NPC.velocity *= 0.986f;
        }

        public float actualSpeed = 0f;

		public void MoveToSideOfPlayer(Player player, float maxSpeed, float angle)
		{
            if (actualSpeed < maxSpeed) actualSpeed += 0.036f;
            if (actualSpeed > maxSpeed) actualSpeed -= 0.028f;
			Vector2 toSide = Vector2.One.GetVector_ToAngle_WithMult(angle, flyDistanceToPlayer);
			NPC.MoveTowards(player.Center + toSide, maxSpeed, 16f);
			NPC.noTileCollide = true;
		}

		public void DashIntoPlayer(Vector2 targetCenter)
        {
			NPC.TargetClosest(false);
			targetCenter.Normalize();
			NPC.velocity = targetCenter * maxDashSpeed;
        }

        bool appeared = false;
        int maxTime;
        public void CircleAroundThePlayer(Player player, int modifier)
        {
            Vector2 sideToStart = new();
            float distance = 320f;
            if (maxTime < 1)
            {
                maxTime = teleportTime;
            }
            if (teleportTime > 0 && !appeared)
            {
                MoveToSideOfPlayer(player, 9f, fromAngle);
                for (int i = 0; i < 4; i++)
                {
                    Vector2 pos = Vector2.One.GetVector_ToAngle_WithMult(fromAngle, distance);
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
                    Vector2 posC = Vector2.One.GetVector_ToAngle_WithMult(fromAngle, distance);
                    NPC.position = posC + player.position;
                    for (int i = 0; i < 250; i++)
                    {
                        double angle = 2.0 * Math.PI * i / 250;
                        Vector2 speed = Vector2.One.GetVector_ToAngle_WithMult(angle, 7f);
                        Dust.NewDustPerfect(NPC.position, ModContent.DustType<Star_of_Hope_Effect>(), new(speed.X, speed.Y), 20, default, 3f);
                    }
                    for (int i = 0; i < 200; i++)
                    {
                        double angle = 2.0 * Math.PI * i / 200;
                        Vector2 speed = Vector2.One.GetVector_ToAngle_WithMult(angle, 5.5f);
                        Dust.NewDustPerfect(NPC.position, ModContent.DustType<Star_of_Hope_Effect>(), new(speed.X, speed.Y), 20, default, 2.5f);
                    }
                    for (int i = 0; i < 150; i++)
                    {
                        double angle = 2.0 * Math.PI * i / 150;
                        Vector2 speed = Vector2.One.GetVector_ToAngle_WithMult(angle, 4f);
                        Dust.NewDustPerfect(NPC.position, ModContent.DustType<Star_of_Hope_Effect>(), new(speed.X, speed.Y), 20, default, 2f);
                    }
                    appeared = true;
                }

                //// ~~~~~~~~~~~ Кружение

                if (++teleportTime <= 80 && appeared)
                {
                    double angleC = ((2.0 * Math.PI * (double)teleportTime / 80.0 * 0.45) + Math.Abs(sideToStart.ToRotation())) * modifier;
                    Vector2 posC = Vector2.One.GetVector_ToAngle_WithMult(angleC, distance);
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
        readonly List<string> text = [];
        readonly List<string> text2 = [];

        void SetTextTranslation()
        {
            if (LanguageManager.Instance.ActiveCulture == FSHelper.RuTrans)
            {
                text.AddRange(
                [
                    "Террариан...",
                    "Благодарю тебя за своё освобождение.",
                    "Наконец-то я свободен...",
                    "Пропасть заточила меня в эту шкатулку.",
                    "Но...",
                    "Преступления... Которые ты совершил против ангелов...",
                    "Не могут быть прощены.",
                    "И наказание твоё...",
                    "Смерть!"
                ]);
                text2.AddRange(
                [
                    "Ах, вот оно что...",
                    "Так ты.. не отсюда...",
                    "Да простят нас.. всевышние..."
                ]);
            }
            else
            {
                text.AddRange(
                [
                    "Terrarian...",
                    "My gratitude upon thy for my freedom.",
                    "Free at last...",
                    "The Barathrum sealed me inside this casket.",
                    "But...",
                    "Crimes... thy have commited against the angels...",
                    "Could not be forgiven.",
                    "And thy punishment...",
                    "Is Death!"
                ]);
                text2.AddRange(
                [
                    "Ah, so that's how it is...",
                    "Thy.. not from here...",
                    "May the Almighty.. forgive us..."
                ]);
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

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
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