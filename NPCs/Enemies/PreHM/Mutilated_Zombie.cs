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

namespace Ferustria.NPCs.Enemies.PreHM
{
    public class Mutilated_Zombie : ModNPC
    {
        private int jumpCD, leapCD, roarCD;
        private bool leap = false, leaped = false, climb = false, enraged = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mutilated zombie");
            DisplayName.AddTranslation(FSHelper.RuTrans(), "Изуродованный зомби");
            Main.npcFrameCount[NPC.type] = 9;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = FSHelper.Scale(80, 115, 150);
            NPC.damage = FSHelper.Scale(25, 45, 60);
            NPC.defense = FSHelper.WOScale(9, 12, 14);
            NPC.knockBackResist = 0.2f;
            NPC.width = 40;
            NPC.height = 45;
            AnimationType = NPCID.BloodZombie;
            NPC.aiStyle = -1;
            AIType = NPCID.BloodZombie;
            NPC.npcSlots = 0.4f;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.value = Item.buyPrice(0, 0, 6, 0);
            NPC.rarity = 1;
            NPC.noGravity = false;
            jumpCD = jumpCD = roarCD = 0;
            /*banner = NPC.type;
            bannerItem = mod.ItemType("PetrousKnight1Banner");*/
        }

        /*public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var GlowMask = mod.GetTexture("Glow/PetrousKnight1_GlowMask");
            var Effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(GlowMask, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, Effects, 0);
        }*/

        public override void AI()
        {
            jumpCD--;
            roarCD--;
            if (!leap) leapCD--; 
             NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];
            if (!leap)
            {
                leaped = false;
                if (NPC.collideY || climb)
                {
                    if (target.position.X <= NPC.position.X) NPC.velocity.X -= 3.5f / 60;
                    if (target.position.X >= NPC.position.X) NPC.velocity.X += 3.5f / 60;
                    if (target.position.X <= NPC.position.X && NPC.velocity.X > 0) NPC.velocity.X -= 6.5f / 60;
                    if (target.position.X >= NPC.position.X && NPC.velocity.X < 0) NPC.velocity.X += 6.5f / 60;
                }
                if (NPC.collideY) climb = false;

                if (NPC.collideX && jumpCD <= 0) { NPC.velocity.Y = -8f; jumpCD = 60; climb = true; }

                //bool[] ig = new bool[TileID.AccentSlab];
                if (NPC.position.Y < target.position.Y - 20 && !Collision.SolidTiles(NPC.position, NPC.width, NPC.height)) NPC.noTileCollide = true;
                else NPC.noTileCollide = false;
                    //Collision.AdvancedTileCollision(ig, NPC.position, NPC.velocity, NPC.width, NPC.height, true, true);
                    //else NPC.stairFall = false;
                if (Math.Abs(target.position.X - NPC.position.X) < 100f && target.position.Y + 40 < NPC.position.Y && NPC.collideY && jumpCD <= 0) { NPC.velocity.Y = -9f; jumpCD = 80; }
                
                
                if (NPC.velocity.X >= 4.5f && !leap) NPC.velocity.X = 4.5f;
                if (NPC.velocity.X <= -4.5f && !leap) NPC.velocity.X = -4.5f;
            }

            if (NPC.life <= NPC.lifeMax * 0.45) enraged = true;

            if (Math.Abs(target.position.X - NPC.position.X) < 50 * 16f && leapCD <= 0 && jumpCD <= 0 && !leap && enraged)
            {
                leap = true;
                leaped = false;
                if (jumpCD <= 0)
                    NPC.velocity.Y -= Main.rand.NextFloat(5.5f, 6.8f);
                jumpCD = 60;
                leapCD = Main.rand.Next(60, 120);
            }
            if (leap && NPC.velocity.Y <= 0f && !leaped)
            {
                leaped = true;
                float leapStrenght = Main.rand.NextFloat(8.5f, 12.5f);
                if (target.position.X <= NPC.position.X) NPC.velocity.X -= leapStrenght;
                if (target.position.X >= NPC.position.X) NPC.velocity.X += leapStrenght;
                        if (NPC.velocity.X >= leapStrenght) NPC.velocity.X = leapStrenght;
                if (NPC.velocity.X <= -leapStrenght) NPC.velocity.X = -leapStrenght;
                
            }
            if (leap && (NPC.collideX || NPC.collideY || Collision.down) && NPC.velocity.Y >= 0f) { leap = false; leaped = false; }

            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
            if (NPC.velocity.Y >= 0) Collision.StepDown(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
            if (NPC.velocity.X < 0) NPC.direction = -1;
            else NPC.direction = 1;

            if (Main.rand.NextFloat() < .8f && roarCD <= 0)
            {
                roarCD = Main.rand.Next(360, 600);
                SoundStyle sound;
                switch (Main.rand.NextBool())
                {
                    case true: sound = SoundID.NPCDeath38; break;
                    case false: sound = SoundID.NPCDeath39; break;
                }
                sound.Volume = 0.5f;
                sound.Type = SoundType.Sound;
                sound.PitchRange = enraged ? (-0.5f, -0.18f) : (-0.4f, -0.08f);
                SoundEngine.PlaySound(sound, NPC.position);
            }
            
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (enraged)
            {
                int heal = (int)(damage / 1.5);
                if (heal > 0)
                {
                    NPC.life += heal;
                    NPC.HealEffect(heal);
                }
                
            }
        }


        

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 80; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 1.3f);
                }
                /*Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("Gores/PetrousKnightGore1"), 1f);
                Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("Gores/PetrousKnightGore2"), 1f);
                Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("Gores/PetrousKnightGore2"), 1f);
                Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("Gores/PetrousKnightGore3"), 1f);
                Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("Gores/PetrousKnightGore3"), 1f);*/
            }
            else
            {
                for (int k = 0; k < Main.rand.Next(10, 20); k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * (float)hitDirection, -2.5f, 0, default, 1.3f);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1 == true)
            {
                if (NPC.CountNPCS(ModContent.NPCType<Mutilated_Zombie>()) < 3) return SpawnCondition.OverworldNightMonster.Chance * 0.029f;
                else return 0f;
            }
            else return 0f;
            /*if (NPC.CountNPCS(ModContent.NPCType<Mutilated_Zombie>()) < 3)
            {
                if (NPC.downedBoss2 == true)
                {
                    if (spawnInfo.player.ZoneUnderworldHeight)
                    {
                        return SpawnCondition.Underworld.Chance * 0.1f;
                    }
                    else return SpawnCondition.OverworldNightMonster.Chance * 0.0195f;
                }
                else if (spawnInfo.player.ZoneUnderworldHeight)
                {
                    return SpawnCondition.Underworld.Chance * 0.03f;
                }
                else return 0f;
            }
            else return 0f;*/
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Wire, 100 / 11, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.Rotten_Skin>(), 1, 1, 3));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("The abominated flesh seeking everywhere for the victim to rip it apart. They become more fearsome lesser their health.")
            });
        }

    }
}