using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Dusts;
using Ferustria.Content.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using Ferustria.Content.Items.Materials.Drop;
using Microsoft.CodeAnalysis.Operations;

namespace Ferustria.Content.NPCs.Enemies.PreHM
{

    public class Fathomless_Fly : ModNPC
	{
		public float scale;

		public override string Texture => Ferustria.Paths.TexturesPathNPCs + "Enemies/PreHM/Fathomless_Fly/" + (!Main.hardMode ? "Fathomless_Fly_1" : "Fathomless_Fly_2");

        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 4;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Weak_Barathrum_Leach>()] = true;

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = -1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
            });
        }

        public override void SetDefaults()
        {
            if (!Main.hardMode) scale = Main.rand.NextFloat(0.63f, 1.12f);
            else scale = Main.rand.NextFloat(0.75f, 1.24f);
            NPC.lifeMax = (int)(FSHelper.Scale(110, 145, 185) * scale);
            NPC.damage = FSHelper.Scale(30, 50, 70);
			NPC.defense = 9;
            NPC.knockBackResist = 0.25f;
			NPC.width = 26;
			NPC.height = 38;//152/4
			NPC.scale = scale;
			NPC.aiStyle = 14;
            NPC.npcSlots = 0.4f;
            NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath4;
            NPC.value = Item.buyPrice(0, 0, 1, 35);
			AIType = NPCID.CaveBat;
			AnimationType = NPCID.GiantBat;            
        }

        public override void OnSpawn(IEntitySource source)
        {
            //NPC.lifeMax = (int)(FSHelper.Scale(110, 145, 185) * scale);
            if (Main.hardMode) { NPC.lifeMax *= 3; NPC.damage = (int)(NPC.damage * 2); NPC.defense *= 3; }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.CountNPCS(ModContent.NPCType<Fathomless_Fly>()) < 3)
			{
				if (NPC.downedBoss1 == true)
				{
					if (spawnInfo.Player.ZoneUnderworldHeight)
					{
						return SpawnCondition.Underworld.Chance * 0.075f;
					}
					else return SpawnCondition.OverworldNightMonster.Chance * 0.0415f;
				}
				else if (spawnInfo.Player.ZoneUnderworldHeight)
				{
					return SpawnCondition.Underworld.Chance * 0.0435f;
				}
				return 0f;
			}
			return 0f;
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                double num11 = NPC.life + hit.Damage / 3;
                if (num11 > 35) num11 = 35;
                for (int i = 0; i < num11 / 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<Barathrum_Particles>(), hit.HitDirection, -2f, 0, default, Main.rand.NextFloat(0.6f, 0.8f));
                    dust.noGravity = true;
                }
                return;
            }
            for (int i = 0; i < 55; i++)
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<Barathrum_Particles>(), 2.5f * hit.HitDirection, -2.3f, 0, default, Main.rand.NextFloat(0.4f, 1f));
                dust.noGravity = true;
            }
        }


        bool setTimer = false;
        float req = 600f;
		public override void AI()
		{
            NPC.netUpdate = true;
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			bool triple;
            float distance = NPC.Distance(player.position);
            Vector2 playerPrediction = ((player.Center - NPC.Center) + player.velocity);
            playerPrediction *= distance;
            playerPrediction = playerPrediction.SafeNormalize(default) * 13.5f;
			if (scale > 1.05f && !Main.hardMode) triple = true;
			else if (scale > 1.12f && Main.hardMode) triple = true;
			else triple = false;
            if (distance < 130f * 16f)
            {
                NPC.localAI[1] += 1f;
                if (!setTimer)
                {
                    req = Main.rand.NextFloat(240f, 360f);
                    setTimer = true;
                }
                if (NPC.localAI[1] >= req)
                {
                    NPC.localAI[1] = 0f;
                    setTimer = false;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int projs = triple ? 3 : 1;
                        float rotation = MathHelper.ToRadians(14);
                        for (float i = 1; i <= projs; i++)
                        {
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, playerPrediction.RotatedBy(MathHelper.Lerp(rotation, -rotation, i / projs)), ModContent.ProjectileType<Barathrum_Echo>(), NPC.damage / 5, 3f, Main.myPlayer, 0f, 0f);
                        }
                    }
                    NPC.velocity.X = -NPC.velocity.X * 4.5f;
                    NPC.velocity.Y = -NPC.velocity.Y * 4.5f;
                    NPC.velocity *= .4f;
                }

            }
            else { NPC.localAI[1] = 0f; setTimer = false; }
			
			base.AI();
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Weak_Barathrum_Leach>(), Main.rand.Next(5, 10) * 60);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Impure_Dust>(), 1, 1, 3));
            //npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Barathrum_Sample>(), 9, 6));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<Barathrum_Sample>(), 11));
		}

        public static LocalizedText BestiaryEntry { get; private set; }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            //string text;
            //if (LanguageManager.Instance.ActiveCulture == FSHelper.RuTrans) 
            //    text = "Эта мушка ищет хоть что, чем бы себя заполнить.";
            //else text = "This fly is looking for anything to fill itself.";
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                // Sets the description of this NPC that is listed in the bestiary.
                new FlavorTextBestiaryInfoElement("Mods.Ferustria.Bestiary.Fathomless_Fly")
            });
        }
	}
}