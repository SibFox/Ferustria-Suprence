using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Ferustria.Content.Buffs.Negatives;
using Terraria.DataStructures;

namespace Ferustria.Content.NPCs.Enemies.PreHM
{
    public class Mutilated_Zombie : ModNPC
    {
        int jumpCD, leapCD, roarCD;
        bool leap = false, leaped = false, climb = false, enraged = false;
        float standartKB = 0.2f;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 9;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Rapid_Blood_Loss>()] = true;

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            });
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = FSHelper.Scale(90, 135, 180);
            NPC.damage = FSHelper.Scale(30, 50, 75);
            NPC.defense = FSHelper.WOScale(9, 12, 14);
            NPC.knockBackResist = standartKB;
            NPC.width = 40;
            NPC.height = 45;
            AnimationType = NPCID.BloodZombie;
            NPC.aiStyle = -1;
            AIType = NPCID.BloodZombie;
            NPC.npcSlots = 0.65f;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.value = Item.buyPrice(0, 0, 3, 0);
            NPC.rarity = 1;
            NPC.noGravity = false;
            jumpCD = roarCD = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Main.hardMode) { NPC.lifeMax = (int)(NPC.lifeMax * 3); NPC.damage = (int)(NPC.damage * 1.5); NPC.defense = (int)(NPC.defense * 2.5); }
        }


        public override void AI()
        {
            //NPC.netUpdate = true;
            jumpCD--;
            roarCD--;
            if (!leap) leapCD--; 
            NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];

            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
            if (NPC.velocity.Y >= 0) Collision.StepDown(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
            //if (target.position.X < NPC.position.X && NPC.collideY) NPC.direction = -1;
            //else NPC.direction = 1;

            if (!leap)
            {
                leaped = false;
                if (NPC.collideY || climb)
                {
                    if (target.position.X <= NPC.position.X) NPC.velocity.X -= 3.5f / 60; //Игрок слева
                    else if (target.position.X >= NPC.position.X) NPC.velocity.X += 3.5f / 60; //Игрок справа
                    if (target.position.X <= NPC.position.X && NPC.velocity.X > 0) NPC.velocity.X -= 6.5f / 60; //Игрок слева, резкий разворот справа
                    else if (target.position.X >= NPC.position.X && NPC.velocity.X < 0) NPC.velocity.X += 6.5f / 60; //Игрок справа, резикий разворот слева
                }
                if (NPC.collideY) climb = false;

                if (NPC.collideX && jumpCD <= 0) { NPC.velocity.Y = -8f; jumpCD = 60; climb = true; }

                //if (NPC.position.Y < target.position.Y - 20 && !Collision.SolidTiles(NPC.position, NPC.width, NPC.height) && !NPC.collideX) NPC.noTileCollide = true;
                //else NPC.noTileCollide = false;
                if (Math.Abs(target.position.X - NPC.position.X) < 100f && target.position.Y + 40 < NPC.position.Y && NPC.collideY && jumpCD <= 0) 
                { NPC.velocity.Y = -9f; jumpCD = 80; }
                
                if (NPC.velocity.X >= 4.7f && !leap) NPC.velocity.X = 4.7f;
                if (NPC.velocity.X <= -4.7f && !leap) NPC.velocity.X = -4.7f;
            }


            if (NPC.life <= NPC.lifeMax * 0.45) enraged = true;

            EnragedLeap(target);


            SoundPlay();
        }

        public override bool? CanFallThroughPlatforms() =>
            NPC.position.Y < Main.player[NPC.target].position.Y - 20;

        void EnragedLeap(Player target)
        {
            if (Math.Abs(target.position.X - NPC.position.X) < 50 * 16f && leapCD <= 0 && jumpCD <= 0 && !leap && enraged)
            {
                leap = true;
                leaped = false;
                NPC.velocity.Y -= Main.rand.NextFloat(4.5f, 6.15f);
                jumpCD = 60;
                leapCD = Main.rand.Next(60, 120);
                NPC.knockBackResist = 0;
            }
            if (leap && NPC.velocity.Y <= 0f && !leaped)
            {
                leaped = true;
                float leapStrenght = Main.rand.NextFloat(8.5f, 12.5f);
                if (target.position.X <= NPC.position.X) NPC.velocity.X -= leapStrenght;
                if (target.position.X >= NPC.position.X) NPC.velocity.X += leapStrenght;
                NPC.velocity.X = Utils.Clamp(NPC.velocity.X, -leapStrenght, leapStrenght);

            }
            if (leap && (NPC.collideX || NPC.collideY || Collision.down)) { leap = false; leaped = false; NPC.knockBackResist = standartKB; }
        }


        void SoundPlay()
        {
            if (Main.rand.NextFloat() < .5f && roarCD <= 0)
            {
                roarCD = Main.rand.Next(360, 550);
                SoundStyle sound = Main.rand.NextBool() ? SoundID.NPCDeath38 : SoundID.NPCDeath39;
                sound.Volume = 0.45f;
                sound.Type = SoundType.Sound;
                sound.PitchRange = enraged ? (-0.68f, -0.25f) : (-0.5f, -0.18f);
                SoundEngine.PlaySound(sound, NPC.position);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextFloat() < .35f) target.AddBuff(ModContent.BuffType<Shattered_Armor>(), Main.rand.Next(4, 8) * 60);
            if (enraged)
            {
                target.AddBuff(ModContent.BuffType<Shattered_Armor>(), Main.rand.Next(5, 10) * 60);
                if (Main.rand.NextFloat() < .65f) target.AddBuff(ModContent.BuffType<Rapid_Blood_Loss>(), (int)(Main.rand.NextFloat(1f, 2.5f) * 60));
                int heal = (int)(info.Damage / 1.5);
                if (heal > 0)
                {
                    NPC.Heal(heal);
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 80; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, 0, default, 1.3f);
                }
            }
            else
            {
                for (int k = 0; k < Main.rand.Next(10, 20); k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, 0, default, 1.3f);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1 == true)
            {
                if (NPC.CountNPCS(ModContent.NPCType<Mutilated_Zombie>()) < 3 && !Main.dayTime) return SpawnCondition.OverworldNightMonster.Chance * 0.033f;
                else if (NPC.CountNPCS(ModContent.NPCType<Mutilated_Zombie>()) < 2 && Main.dayTime) return SpawnCondition.OverworldDay.Chance * 0.0283f; 
                return 0f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Wire, 100 / 11, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.Drop.Rotten_Skin>(), 1, 0, 2));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Mods.Ferustria.Bestiary.Mutilated_Zombie")
            });
        }
    }
}