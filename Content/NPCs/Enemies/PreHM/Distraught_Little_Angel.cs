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
using Ferustria.Common.GlobalNPCs;
using Microsoft.Xna.Framework.Graphics;

namespace Ferustria.Content.NPCs.Enemies.PreHM
{

    public class Distraught_Little_Angel : ModNPC
	{
        //public override string Texture => Ferustria.Paths.TexturesPathNPCs + "Enemies/PreHM/" + (!Main.hardMode ? "Fathomless_Fly_1" : "Fathomless_Fly_2");
        public override string Texture => NPC.GetEnemyTexture(1, "Distraught_Little_Angel/Distraught_Little_Angel_Bestiary");
        GlobalNPCShields npcSpecials = null;
        Player player = null;

        bool enraged = false;
        bool fall = false;

        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 4;

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            });
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = FSHelper.Scale(45, 60, 70);
            NPC.damage = FSHelper.Scale(20, 30, 45);
			NPC.defense = 6;
            NPC.knockBackResist = 0.5f;
			NPC.width = 40;
			NPC.height = 40;
			NPC.aiStyle = -1;
            NPC.npcSlots = 0.4f;
            NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath4;
            NPC.value = Item.buyPrice(0, 0, 0, 75);
            //NPC.noGravity = true;

			//AIType = NPCID.CaveBat;
			//AnimationType = NPCID.GiantBat;
        }

        public override void OnSpawn(IEntitySource source)
        {
            npcSpecials = NPC.GetGlobalNPC<GlobalNPCShields>();
            npcSpecials.SetHolyShield(FSHelper.Scale(20, 30, 40));
            if (Main.hardMode) { NPC.lifeMax *= 3; NPC.damage = (int)(NPC.damage * 2); NPC.defense *= 3;
                npcSpecials = NPC.GetGlobalNPC<GlobalNPCShields>();
                npcSpecials.SetHolyShield(FSHelper.Scale(50, 70, 100));
            }
            
            NPC.TargetClosest(true);
            player = Main.player[NPC.target];
            NPC.direction = player.position.X < NPC.position.X ? -1 : 1;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.CountNPCS(ModContent.NPCType<Distraught_Little_Angel>()) < 5)
			{
				if (spawnInfo.Player.ZoneHallow)
				{
					return SpawnCondition.OverworldHallow.Chance * 0.0655f;
				}
				else if (NPC.downedBoss1 == true)
				{
					return SpawnCondition.OverworldDay.Chance * 0.035f;
				}
                else
                {
                    return SpawnCondition.OverworldDay.Chance * 0.025f;
                }
			}
			return 0f;
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            //if (!Main.dedServ)
            //{

            //}
        }
                
		public override void AI()
		{
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            fall = !npcSpecials.Holy_Shield_Active;

            //NPC.netUpdate = true;
			//NPC.TargetClosest(true);
			player = Main.player[NPC.target];
            if (enraged && !fall) NPC.direction = player.position.X < NPC.position.X ? -1 : 1;

            if (NPC.localAI[0]++ > 5 && !fall)
            {
                NPC.localAI[0] = 0;
                if (++NPC.frameCounter > 3) NPC.frameCounter = 0;
            }
            if (fall) { NPC.frameCounter = 0; }
            //NPC.localAI[0]++ % 3 == 0 ? NPC.frameCounter++ : ;

            MoveBehaviour();
            ProjectileControl();

            if (npcSpecials.Holy_Shield_Destroyed_Control) { npcSpecials.SetHolyShieldRecharge(160); enraged = true; }
		}

        Vector2 storedVelocity = Vector2.Zero;
        void MoveBehaviour()
        {
            bool groundClose = Collision.SolidCollision(NPC.Center + new Vector2(0, 1 * 16), NPC.width/2, NPC.height) || NPC.collideX;

            NPC.velocity.Y += fall ? 4f / 60 : 1.5f / 60;
            if (!fall)
            {
                if (groundClose) NPC.velocity.Y = -0.75f;
                NPC.knockBackResist = enraged ? 0.05f : 0.2f;
                NPC.velocity.X += (enraged ? 2.5f : 1f) / 60 * NPC.direction;
            }
            else { NPC.velocity.X = 0; NPC.knockBackResist = 0f; }

            if (Main.tile[NPC.Center.ToTileCoordinates() + new Point(NPC.direction * 2, 0)].HasTile && !enraged) { NPC.direction *= -1; NPC.velocity.X *= -.5f; }

            if (shootPrepare) 
            {
                if (storedVelocity != Vector2.Zero)
                    storedVelocity = NPC.velocity;
                NPC.velocity = Vector2.Zero; 
            }
            else if (!shootPrepare && storedVelocity !=  Vector2.Zero)
            {
                NPC.velocity = storedVelocity;
                storedVelocity = Vector2.Zero;
            }

            float maxVel = enraged ? 4.25f : 2.5f;
            if (NPC.velocity.X > maxVel) NPC.velocity.X = maxVel;
            if (NPC.velocity.X < -maxVel) NPC.velocity.X = -maxVel;
            if (NPC.velocity.Y < -2f) NPC.velocity.Y = -2f;
            if (NPC.velocity.Y > 8f) NPC.velocity.Y = 8f;
        }

        bool shootPrepare = false;
        float countDown { get => NPC.localAI[2]; set => NPC.localAI[2] = value; }
        void ProjectileControl()
        {
            float distance = NPC.Distance(player.position);
            if (distance < 120f * 16f && !fall && enraged)
            {
                if (NPC.localAI[1]-- < 0)
                {
                    shootPrepare = true;
                    DustAround();
                    if (countDown-- < 0)
                    {
                        NPC.localAI[1] = Main.rand.NextFloat(160f, 240f);
                        countDown = 120;
                        shootPrepare = false;
                        handsUpFrames = 80;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (float i = 1; i <= 3; i++)
                            {
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, new((1.5f * i * NPC.direction) + (NPC.direction * 1f), -5f),
                                    ModContent.ProjectileType<Little_Light_Echo>(), NPC.GetAttackDamage_ForProjectiles_DividedFromDamage(), 3f);
                            }
                        }
                    }
                }
                else { countDown = 120; shootPrepare = false; }
            }
            else { NPC.localAI[1] = 200f; shootPrepare = false; countDown = 120; }

            NPC.spriteDirection = NPC.direction;
        }

        void DustAround()
        {
            if (!Main.dedServ && NPC.ai[0]++ % 6 == 0)
                for (float i = 0; i < 160; i++)
                    Dust.NewDustDirect(NPC.Center + Vector2.One.GetVector_ToAngle_WithMult(360f * (i / 50f), countDown / 3 + 40), 1, 1, ModContent.DustType<Angelic_Particles_Stable>(), 0, 0, 0, default, .5f);
        }

        int handsUpFrames = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D wings = (Texture2D)ModContent.Request<Texture2D>(NPC.GetEnemyTexture(1, "Distraught_Little_Angel/Distraught_Little_Angel_Wings"));
            Texture2D body = (Texture2D)ModContent.Request<Texture2D>(NPC.GetEnemyTexture(1, "Distraught_Little_Angel/Distraught_Little_Angel_Body"));

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeightWings = wings.Height / Main.npcFrameCount[NPC.type];
            int frameHeightBody = body.Height / Main.npcFrameCount[NPC.type];

            int bodyFrame = 0;
            if (shootPrepare) bodyFrame = 1;
            if (fall || handsUpFrames-- > 0) bodyFrame = 2;
            if (fall && NPC.collideY/*NPC.GetTile_FromPosition(new Point(0, (int)NPC.height / 16 + 1)).IsTileSolid()*/) { bodyFrame = 3; NPC.frameCounter = 2; }

            int startYWings = frameHeightWings * (int)NPC.frameCounter;
            int startYBody = frameHeightBody * bodyFrame;

            // Get this frame on texture
            Rectangle sourceRectangleWings = new(0, startYWings, wings.Width, frameHeightWings);
            Rectangle sourceRectangleBody = new(0, startYBody, body.Width, frameHeightWings);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangleWings.Size() / 2f;

            // If image isn't centered or symmetrical you can specify origin of the sprite
            // (0,0) for the upper-left corner
            float offsetX = 30f;
            origin.X = NPC.spriteDirection == 1 ? sourceRectangleWings.Width - offsetX : offsetX;

            // If sprite is vertical
            // float offsetY = 20f;
            // origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);


            // Applying lighting and draw current frame
            //Color drawColor = Projectile.GetAlpha(lightColor);Data
            spriteBatch.Draw(wings,
                NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY),
                sourceRectangleWings, drawColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0);

            spriteBatch.Draw(body,
                NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY),
                sourceRectangleBody, drawColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0);

            return false;
        }

        //public override void ModifyNPCLoot(NPCLoot npcLoot)
        //{
        //    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Impure_Dust>(), 1, 1, 3));
        //    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Barathrum_Sample>(), 9, 6));
        //    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<Barathrum_Sample>(), 11));
        //}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return enraged && !fall;
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            NPC.target = player.whoAmI;
            if (npcSpecials.Holy_Shield_Durability < npcSpecials.Holy_Shield_Durability_Max / 2) enraged = true;
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (projectile != null) 
            {
                if (projectile.whoAmI != -1)
                {
                    Player owner = Main.player[projectile.owner];
                    if (owner.active)
                        NPC.target = owner.whoAmI;
                }                
            }
            if (npcSpecials.Holy_Shield_Durability < npcSpecials.Holy_Shield_Durability_Max / 2) enraged = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
                // Sets the description of this NPC that is listed in the bestiary.
                new FlavorTextBestiaryInfoElement("Mods.Ferustria.Bestiary.Distraught_Little_Angel")
            });
        }
	}
}