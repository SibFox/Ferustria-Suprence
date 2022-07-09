using Microsoft.Xna.Framework;
using Ferustria.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace Ferustria.Projectiles.Friendly
{
	public class Rot_Sac : ModProjectile
	{
		// Are we sticking to a target?
		public bool IsStickingToTarget
		{
			get => Projectile.ai[0] == 1f;
			set => Projectile.ai[0] = value ? 1f : 0f;
		}

		// Index of the current target
		public int TargetWhoAmI
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rot Sac");
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = -1;
			Projectile.scale = 1f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 400;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 0;
		}

		public override void AI()
		{
			UpdateAlpha();
			if (IsStickingToTarget) StickyAI();
			else NormalAI();
		}

		private void UpdateAlpha()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 8;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
		}

		private void NormalAI()
		{
			TargetWhoAmI++;

			// For a little while, the javelin will travel with the same speed, but after this, the javelin drops velocity very quickly.
			if (TargetWhoAmI >= 15)
			{
				// Change these multiplication factors to alter the javelin's movement change after reaching maxTicks
											  //const float velYmult = 0.35f; // y velocity factor, every AI update the y velocity will be be 0.35f bigger of the original speed, causing the javelin to drop to the ground
				TargetWhoAmI = 15; // set ai1 to maxTicks continuously
				Projectile.velocity *= 0.98f;
				//Projectile.velocity.Y += velYmult;
			}

			// Make sure to set the rotation accordingly to the velocity, and add some to work around the sprite's rotation
			// Please notice the MathHelper usage, offset the rotation by 90 degrees (to radians because rotation uses radians) because the sprite's rotation is not aligned!
			Projectile.rotation = (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) / 2 * -Projectile.direction;

			// Spawn some random dusts as the javelin travels
			if (Main.rand.NextBool(2))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustType<Rot_Particles>(),
					Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 80, Scale: 1.6f);
				dust.velocity += Projectile.velocity * 0.3f;
				dust.velocity *= 0.3f;
			}
			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustType<Rot_Particles>(),
					0, 0, 60, Scale: 1.2f);
				dust.velocity += Projectile.velocity * 0.5f;
				dust.velocity *= 0.5f;
			}
		}

		private void StickyAI()
		{
			Projectile.ignoreWater = true; // Make sure the Projectile ignores water
			Projectile.tileCollide = false; // Make sure the Projectile doesn't collide with tiles anymore
			const double aiFactor = 2; // Change this factor to change the 'lifetime' of this sticking javelin

			// Every 30 ticks, the javelin will perform a hit effect
			bool hitEffect = ++Projectile.localAI[0] % 30f == 0f;
			int projTargetIndex = TargetWhoAmI;
			if (Projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
			{ // If the index is past its limits, kill it
				Projectile.Kill();
			}
			else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
			{ // If the target is active and can take damage
			  // Set the Projectile's position relative to the target's center
				Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
				Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
				if (hitEffect)
				{ // Perform a hit effect here
					Main.npc[projTargetIndex].HitEffect(0, 1.0);
				}
			}
			else
			{ // Otherwise, kill the Projectile
				Projectile.Kill();
			}
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			// If attached to an NPC, draw behind tiles (and the npc) if that NPC is behind tiles, otherwise just behind the NPC.
			if (Projectile.ai[0] == 1f) // or if(isStickingToTarget) since we made that helper method.
			{
				int npcIndex = (int)Projectile.ai[1];
				if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active)
				{
					if (Main.npc[npcIndex].behindTiles)
					{
						behindNPCsAndTiles.Add(index);
					}
					else
					{
						behindNPCs.Add(index);
					}

					return;
				}
			}
			// Since we aren't attached, add to this list
			behindProjectiles.Add(index);
		}


		private const int MAX_STICKY_SACS = 3; // This is the max. amount of javelins being able to attach
		private readonly Point[] _stickingSacs = new Point[MAX_STICKY_SACS]; // The point array holding for sticking javelins

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			IsStickingToTarget = true; // we are sticking to a target
			TargetWhoAmI = target.whoAmI; // Set the target whoAmI
			Projectile.velocity =
				(target.Center - Projectile.Center) *
				0.75f; // Change velocity based on delta center of targets (difference between entity centers)
			Projectile.netUpdate = true; // netUpdate this javelin

			Projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore

			// It is recommended to split your code into separate methods to keep code clean and clear
			UpdateStickySacs(target);
		}

		/*
		* The following code handles the javelin sticking to the enemy hit.
		*/
		private void UpdateStickySacs(NPC target)
		{
			int currentSacIndex = 0; // The javelin index

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all Projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (i != Projectile.whoAmI // Make sure the looped Projectile is not the current javelin
					&& currentProjectile.active // Make sure the Projectile is active
					&& currentProjectile.owner == Main.myPlayer // Make sure the Projectile's owner is the client's player
					&& currentProjectile.type == Projectile.type // Make sure the Projectile is of the same type as this javelin
					&& currentProjectile.ModProjectile is Rot_Sac rot_sac // Use a pattern match cast so we can access the Projectile like an ExampleJavelinProjectile
					&& rot_sac.IsStickingToTarget // the previous pattern match allows us to use our properties
					&& rot_sac.TargetWhoAmI == target.whoAmI)
				{

					_stickingSacs[currentSacIndex++] = new Point(i, currentProjectile.timeLeft); // Add the current Projectile's index and timeleft to the point array
					if (currentSacIndex >= _stickingSacs.Length)  // If the javelin's index is bigger than or equal to the point array's length, break
						break;
				}
			}

			// Remove the oldest sticky javelin if we exceeded the maximum
			if (currentSacIndex >= MAX_STICKY_SACS)
			{
				int oldSacIndex = 0;
				// Loop our point array
				for (int i = 1; i < MAX_STICKY_SACS; i++)
				{
					// Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
					if (_stickingSacs[i].Y < _stickingSacs[oldSacIndex].Y)
					{
						oldSacIndex = i; // Remember the index of the removed javelin
					}
				}
				// Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
				Main.projectile[_stickingSacs[oldSacIndex].X].Kill();
			}
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			for (int i = 0; i < Main.rand.Next(3, 7); i++)
			{
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, 
					new Vector2(Main.rand.NextFloat(-6f, 6f) , Main.rand.NextFloat(-2f, -6f)),
					ProjectileType<Rot_Petal>(), 20, 0.02f, Projectile.owner);
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			// For going through platforms and such, javelins use a tad smaller size
			width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
			return true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// Inflate some target hitboxes if they are beyond 8,8 size
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			// Return if the hitboxes intersects, which means the javelin collides or not
			return projHitbox.Intersects(targetHitbox);
		}
	}
}
