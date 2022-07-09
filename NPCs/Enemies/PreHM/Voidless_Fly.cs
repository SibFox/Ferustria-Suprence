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
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;

namespace Ferustria.NPCs.Enemies.PreHM
{

	public class Voidless_Fly : ModNPC
	{
		private float scale;
		IEntitySource source;

		public override string Texture => !Main.hardMode ? "Ferustria/NPCs/Enemies/PreHM/Voidless_Fly_1" : "Ferustria/NPCs/Enemies/PreHM/Voidless_Fly_2";

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidless Fly");
			DisplayName.AddTranslation("Russian", "Безпустотная мушка");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			if (!Main.hardMode) scale = Main.rand.NextFloat(0.63f, 1.12f);
			else scale = Main.rand.NextFloat(0.75f, 1.24f);
			int life = (int)(FSHelper.Scale(80, 110, 145) * scale);
			NPC.lifeMax = life;
			NPC.damage = 25;
			NPC.defense = 7;
			NPC.knockBackResist = 0.3f;
			NPC.width = 26;
			NPC.height = 152/4;
			NPC.scale = scale;
			NPC.aiStyle = 14;
			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath4;
			NPC.value = Item.buyPrice(0, 0, 4, 0);
			NPC.buffImmune[ModContent.BuffType<Weak_Void_Leach>()] = true;
			AIType = NPCID.CaveBat;
			AnimationType = NPCID.GiantBat;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.CountNPCS(ModContent.NPCType<Voidless_Fly>()) < 2)
			{
				if (NPC.downedBoss1 == true)
				{
					if (spawnInfo.Player.ZoneUnderworldHeight)
					{
						return SpawnCondition.Underworld.Chance * 0.08f;
					}
					else return SpawnCondition.OverworldNightMonster.Chance * 0.0255f;
				}
				else if (spawnInfo.Player.ZoneUnderworldHeight)
				{
					return SpawnCondition.Underworld.Chance * 0.035f;
				}
				else return 0f;
			}
			else return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				double num11 = NPC.life + damage / 3;
				if (num11 > 35) num11 = 35;
				for (int i = 0; i < num11 / 10; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<Void_Particles>(), hitDirection, -2f, 0, default, Main.rand.NextFloat(0.6f, 0.8f));
					dust.noGravity = true;
				}
				return;
			}
			for (int i = 0; i < 55; i++)
			{
				Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<Void_Particles>(), 2.5f * hitDirection, -2.3f, 0, default, Main.rand.NextFloat(0.4f, 1f));
				dust.noGravity = true;
			}
		}

		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			bool triple;
			float playerXvel = player.velocity.X * 0.5f;
			float playerYvel = player.velocity.Y * 0.2f;
			if (scale > 1.05f && !Main.hardMode) triple = true;
			else if (scale > 1.12f && Main.hardMode) triple = true;
			else triple = false;
			if (Math.Abs(NPC.Center.X - player.Center.X) < 150f * 6f && Math.Abs(NPC.Center.Y - player.Center.Y) < 140f * 6f)
			{
				NPC.localAI[1] += 1f;
				float req = Main.rand.NextFloat(240f, 360f);
				if (NPC.localAI[1] >= req)
				{
					NPC.localAI[1] = 0f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{

						if (Math.Abs(NPC.Center.Y - player.Center.Y) < NPC.Center.Y && Math.Abs(NPC.Center.X - player.Center.X) < 6f * 16f)
						{
							float velY = NPC.velocity.Y * 1.5f;
							if (NPC.velocity.Y <= 0) velY = NPC.velocity.Y * -1.5f;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * -0.4f + playerXvel, velY + playerYvel, ModContent.ProjectileType<Void_Echo>(), NPC.damage / 4, 3f, Main.myPlayer, 0f, 0f);

						}
						else if (!triple) Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * 2.4f + playerXvel, NPC.velocity.Y * 2.4f + playerYvel, ModContent.ProjectileType<Void_Echo>(), NPC.damage / 4, 3f, Main.myPlayer, 0f, 0f);
						else if (triple)
						{
							Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * 2.45f + playerXvel, NPC.velocity.Y * 2.2f + playerYvel, ModContent.ProjectileType<Void_Echo>(), NPC.damage / 4, 3f, Main.myPlayer, 0f, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * 2.8f + playerXvel, NPC.velocity.Y + playerYvel, ModContent.ProjectileType<Void_Echo>(), NPC.damage / 4, 3f, Main.myPlayer, 0f, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * 2.45f + playerXvel, NPC.velocity.Y * -2.2f + playerYvel, ModContent.ProjectileType<Void_Echo>(), NPC.damage / 4, 3f, Main.myPlayer, 0f, 0f);

						}
						NPC.velocity.X = -NPC.velocity.X * 4.5f;
						NPC.velocity.Y = -NPC.velocity.Y * 4.5f;
						NPC.velocity *= .4f;

					}
					NPC.netUpdate = true;
				}

			}
			else NPC.localAI[1] = 0f;
			
			base.AI();
		}


		public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			target.AddBuff(ModContent.BuffType<Weak_Void_Leach>(), Main.rand.Next(5, 10)*60);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Impure_Dust>(), 1, 2, 5));
			npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<Void_Sample>(), 10));
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("This fly is looking for anything to fill itself.")
			});
		}
			
	}

}