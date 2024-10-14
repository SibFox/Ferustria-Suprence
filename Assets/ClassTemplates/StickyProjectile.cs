using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using Ferustria.Content.Projectiles.Friendly.HealProj;

namespace Ferustria.Assets.ClassTemplates
{
    public abstract class StickyProjectile : ModProjectile
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
            DisplayName.SetDefault("");
        }

        public int hitbox = 10;
        public float scale = 1f;
        public DamageClass damgeType = DamageClass.Default;
        public int timeLeft = 60;
        public bool ignoreWater = true;
        public bool tileCollide = true;
        public string displayName = "";

        
        private string _texture = Ferustria.emptyPixel;
        public string SetTexture { get => _texture; set => _texture = value; }
        public override string Texture => SetTexture;


        public abstract void SetValues();

		public override void SetDefaults()
		{
            SetValues();
            Projectile.width = hitbox;
			Projectile.height = hitbox;
			Projectile.aiStyle = -1;
			Projectile.scale = scale;
			Projectile.friendly = true;
			Projectile.DamageType = damgeType;
			Projectile.timeLeft = timeLeft;
			Projectile.ignoreWater = ignoreWater;
			Projectile.tileCollide = tileCollide;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			UpdateAlpha();
			if (IsStickingToTarget) StickyAI();
			else NormalAI();
		}

		private void UpdateAlpha()
		{
			// Slowly remove alpha as it is present
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 10;
			}

			// If alpha gets lower than 0, set it to 0
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
		}

        public virtual void CreateTrail()
        {
            Dust.NewDust(Projectile.Center, 1, 1, DustID.Smoke, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f);
        }

        public virtual void Behaviour() { }
        public virtual void StickyBehaviour() { }

		private void NormalAI()
		{
            Projectile.SetStraightRotation();
			//Projectile.rotation = Projectile.GetStraightRotation();
            Behaviour();
            CreateTrail();
        }

        public int stickyTimeLeft = 60;
        public int hitEffectTimer = 30;

        private int StickyLifeTime { get => (int)Projectile.localAI[0]; set => Projectile.localAI[0] = value; }

        public virtual void HitEffect()
        {
            Main.npc[TargetWhoAmI].HitEffect(0, 1.0);
        }

		private void StickyAI()
		{
            if (Projectile.timeLeft < 10) Projectile.timeLeft = 10;
			Projectile.ignoreWater = true; // Make sure the Projectile ignores water
			Projectile.tileCollide = false; // Make sure the Projectile doesn't collide with tiles anymore
			bool hitEffect = StickyLifeTime % hitEffectTimer == 0;
			int projTargetIndex = TargetWhoAmI;
			if (StickyLifeTime++ >= stickyTimeLeft || projTargetIndex < 0 || projTargetIndex >= 200)
			{ // If the index is past its limits, kill it
				Projectile.Kill();
			}
			else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
			{ // If the target is active and can take damage
			  // Set the Projectile's position relative to the target's center
				Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
				Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
				if (hitEffect) HitEffect();
			}
            else
            {
                Projectile.Kill();
            }
            Projectile.alpha = 80;
            StickyBehaviour();
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            // If attached to an NPC, draw behind tiles (and the npc) if that NPC is behind tiles, otherwise just behind the NPC.
			if (IsStickingToTarget) // or if(isStickingToTarget) since we made that helper method.
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

        public int MAX_STICKY_PROJECTILE = 3;

        private Point[] _stickingNeedles; // The point array holding for sticking javelins

        private void SetPointArray() { _stickingNeedles = new Point[MAX_STICKY_PROJECTILE]; }

        public virtual void OnFirstNPCHit() { }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            SetPointArray();
            IsStickingToTarget = true; // we are sticking to a target
            TargetWhoAmI = target.whoAmI; // Set the target whoAmI
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f; // Change velocity based on delta center of targets (difference between entity centers)
            Projectile.netUpdate = true; // netUpdate this javelin
            Projectile.damage = 0;
            OnFirstNPCHit();
            UpdateStickyProjectiles(target);
        }

		/*
		* The following code handles the javelin sticking to the enemy hit.
		*/
		private void UpdateStickyProjectiles(NPC target)
		{
			int currentProjectileIndex = 0; // The javelin index
			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all Projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (i != Projectile.whoAmI // Make sure the looped Projectile is not the current javelin
					&& currentProjectile.active // Make sure the Projectile is active
					&& currentProjectile.owner == Main.myPlayer // Make sure the Projectile's owner is the client's player
					&& currentProjectile.type == Projectile.type // Make sure the Projectile is of the same type as this javelin
					&& currentProjectile.ModProjectile is StickyProjectile stickyProjectile // Use a pattern match cast so we can access the Projectile like an ExampleJavelinProjectile
					&& stickyProjectile.IsStickingToTarget // the previous pattern match allows us to use our properties
					&& stickyProjectile.TargetWhoAmI == target.whoAmI)
				{
					_stickingNeedles[currentProjectileIndex++] = new Point(i, currentProjectile.timeLeft); // Add the current Projectile's index and timeleft to the point array
					if (currentProjectileIndex >= _stickingNeedles.Length)  // If the javelin's index is bigger than or equal to the point array's length, break
						break;
				}
			}

			// Remove the oldest sticky javelin if we exceeded the maximum
			if (currentProjectileIndex >= MAX_STICKY_PROJECTILE)
			{
				int oldNeedleIndex = 0;
				// Loop our point array
				for (int i = 1; i < MAX_STICKY_PROJECTILE; i++)
				{
					// Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
					if (_stickingNeedles[i].Y < _stickingNeedles[oldNeedleIndex].Y)
					{
						oldNeedleIndex = i; // Remember the index of the removed javelin
					}
				}
				// Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
				Main.projectile[_stickingNeedles[oldNeedleIndex].X].Kill();
			}
		}

        /// <summary>
        /// Use it, if you're not overrading Kill hook.<para>Default sound is - Item10</para>
        /// </summary>
        public SoundStyle killSound = SoundID.Item10;

		public override void OnKill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(killSound, Projectile.position);
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
