using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using Ferustria.Content.Buffs.Statuses;
using Ferustria.Content.Projectiles.Friendly;

namespace Ferustria.Content.Minions.PreHM
{
	public class Angelic_Spirit_Swordsman : ModProjectile
	{
        private int AttackTimer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        private bool PerformCharged
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void SetStaticDefaults()
		{
            Main.projFrames[Projectile.type] = 4;
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
        }

		public override void SetDefaults()
		{
            Projectile.width = 50;
            Projectile.height = 280/4;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.knockBack = 2f;

            Projectile.tileCollide = false; // Makes the minion go through tiles freely

            // These below are needed for a minion weapon
            Projectile.minion = true; // Declares this as a minion (has many effects)
            Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
            Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
        }


        //public override void OnKill(int timeLeft)
		//{
		//	Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		//	SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		//}

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X < 0 ? -1 : 1;

            if (!CheckActive(owner))
            {
                return;
            }

            GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out NPC target);
            Movement(foundTarget, distanceFromTarget, distanceToIdlePosition, vectorToIdlePosition, target);
            Attack(foundTarget, target);
            Visuals();
        }

        // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<Angelic_Swordsman_Summoned_Buff>());

                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<Angelic_Swordsman_Summoned_Buff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            Vector2 idlePosition = owner.Center;
            idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is projectile.minionPos
            float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
            idlePosition.X += minionPositionOffsetX; // Go behind the player

            // All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

            // Teleport to player if distance is too big
            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // If your minion is flying, you want to do this independently of any conditions
            float overlapVelocity = 0.04f;

            // Fix overlap with other minions
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];

                if (i != Projectile.whoAmI
                    && other.active
                    && other.owner == Projectile.owner
                    && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
                {
                    if (Projectile.position.X < other.position.X)
                    {
                        Projectile.velocity.X -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.X += overlapVelocity;
                    }

                    if (Projectile.position.Y < other.position.Y)
                    {
                        Projectile.velocity.Y -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.Y += overlapVelocity;
                    }
                }
            }
        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out NPC target)
        {
            // Starting search distance
            distanceFromTarget = 700f;
            Vector2 targetCenter = Projectile.position;
            foundTarget = false;
            target = new NPC();

            // This code is required if your minion weapon has the targeting feature
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 1000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                // This code is required either way, used for finding a target
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 160f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                            target = npc;
                        }
                    }
                }
            }

            // friendly needs to be set to true so the minion can deal contact damage
            // friendly needs to be set to false so it doesn't damage things like target dummies while idling
            // Both things depend on if it has a target or not, so it's just one assignment here
            // You don't need this assignment if your minion is shooting things instead of dealing contact damage
            //Projectile.friendly = foundTarget;
        }

        private void Movement(bool foundTarget, float distanceFromTarget, float distanceToIdlePosition, Vector2 vectorToIdlePosition, NPC target)
        {
            // Default movement parameters (here for attacking)
            float speed = 16f;
            float inertia = 20f;
            Vector2 targetCenter = target.Center;

            if (foundTarget)
            {
                // Minion has a target: attack (here, fly towards the enemy)
                if (distanceFromTarget > 160f)
                {
                    Projectile.HomingTowards(targetCenter, speed, inertia);
                    // The immediate range around the target (so it doesn't latch onto it when close)
                    //Vector2 direction = (targetCenter - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;

                    //Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }
                else
                {
                    Projectile.HomingTowards(targetCenter + new Vector2(64 * -target.direction, -110f) + new Vector2(target.width, target.height), speed / 2, inertia * 1.5f);
                }
            }
            else
            {
                // Minion doesn't have a target: return to player and idle
                if (distanceToIdlePosition > 600f)
                {
                    // Speed up the minion if it's away from the player
                    speed = 20f;
                    inertia = 60f;
                }
                else
                {
                    // Slow down the minion if closer to the player
                    speed = 4.5f;
                    inertia = 80f;
                }

                if (distanceToIdlePosition > 20f)
                {
                    // The immediate range around the player (when it passively floats about)

                    // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
                else if (Projectile.velocity == Vector2.Zero)
                {
                    // If there is a case where it's not moving at all, give it a little "poke"
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }
        }

        bool triple = false;
        private void Attack(bool foundTarget, NPC target)
        {
            float projSpeed = 14.5f;
            Vector2 targetCenter = target.Center;
            if (foundTarget && --AttackTimer <= 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 sword1Pos = Projectile.Center.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90f)) * 80f + Projectile.Center;
                    Vector2 sword2Pos = Projectile.Center.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(-90f)) * 80f + Projectile.Center;
                    Vector2 flyTo1 = (targetCenter - sword1Pos + target.velocity).SafeNormalize(Vector2.Zero) * projSpeed;
                    Vector2 flyTo2 = (targetCenter - sword2Pos + target.velocity).SafeNormalize(Vector2.Zero) * projSpeed;
                    Projectile sword1 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis("Minion: Angelic Swordsman: Sword 1"), sword1Pos, flyTo1,
                        ModContent.ProjectileType<Angelic_Spirit_Sword>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile sword2 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis("Minion: Angelic Swordsman: Sword 2"), sword2Pos, flyTo2,
                        ModContent.ProjectileType<Angelic_Spirit_Sword>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    if (!triple) triple = true;
                    else triple = false;
                }
                if (triple)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Vector2 swordPos = Projectile.Center.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(-180f)) * 80f + Projectile.Center;
                        Vector2 flyTo = (targetCenter - swordPos + target.velocity).SafeNormalize(Vector2.Zero) * projSpeed;
                        Projectile sword = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis("Minion: Angelic Swordsman: Sword 3"), swordPos, flyTo,
                            ModContent.ProjectileType<Angelic_Spirit_Sword>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
                AttackTimer = 65;
            }
        }

        private void Visuals()
        {
            // So it will lean slightly towards the direction it's moving
            Projectile.rotation = Projectile.velocity.X * 0.08f;

            // This is a simple "loop through all frames from top to bottom" animation

            FramesHandler();

            // Some visuals here
            //Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.78f);
        }

        private void FramesHandler()
        {
            int frameSpeed = 5;

            Projectile.frameCounter++;

            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return false;
        }

    }

}