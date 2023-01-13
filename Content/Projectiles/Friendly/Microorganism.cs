using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Ferustria.Common.GlobalNPCs;

namespace Ferustria.Content.Projectiles.Friendly
{
	public class Microorganism : ModProjectile
	{

        public int FromNPCid { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public bool isFromNPC { get => Projectile.ai[1] == 1; }
        public NPC FromNPC { get; set; }


        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Microorganism");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.friendly = true;
            Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.knockBack = 1.8f;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}


        public override void OnSpawn(IEntitySource source)
        {
            if (!isFromNPC) FromNPCid = -1;
            if (FromNPCid > -1)
            {
                Projectile.localAI[0] = 60;
            }
        }

        public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			//Main.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void AI()
		{
            FadeInAndOut();
            DustEff();
            HomingAI();

            if (FromNPCid > 0)
            {
                Projectile.localAI[0]--;
                if (Projectile.localAI[0] <= 0)
                    FromNPCid = 0;
            }

            Projectile.SetStraightRotation();
		}


        void HomingAI()
        {
            float maxDetectRadius = 10 * 16f;
            float projSpeed = 15;
            Projectile.FindClosestNPC(maxDetectRadius, out NPC closestNPC, FromNPCid);

            if (closestNPC == null)
                return;

            // If found, change the velocity of the projectile and turn it in the direction of the target
            // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
            Projectile.HomingTowards(closestNPC.Center, projSpeed, 13f);
        }


        void DustEff()
        {
            if (Main.rand.NextFloat() < 0.75f)
            {
                int type = ModContent.DustType<Rot_Particles>();
                if (Main.rand.NextBool() && Main.rand.NextBool()) type = ModContent.DustType<Void_Particles>();
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, type, -Projectile.velocity.X * 0.45f, -Projectile.velocity.Y * 0.45f,
                    Scale: Main.rand.NextFloat(0.45f, 1.1f));
            }
        }

        void FadeInAndOut()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 26;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            float holder = damage;
            holder += holder * (target.GetGlobalNPC<GlobalNPCEffects>().Recraphor_Infestation / 100);
            damage = (int)holder;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.GetGlobalNPC<GlobalNPCEffects>().Recraphor_Infestation += 1.35f;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI != FromNPCid) return true;
            return false;
        }
    }


}