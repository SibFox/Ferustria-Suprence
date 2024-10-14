using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Ferustria.Content.Projectiles.Friendly.Rozaline
{
    public class Rozaline_ThornProjectile : ModProjectile
    {
        private float chargeAmount = 0.85f;
        private bool ChargedAttack
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        private Player player = null;
        public float gravityStrength = 0.18f;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (ChargedAttack) { chargeAmount = 0.045f; gravityStrength = 0.13f; }
        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            //Main.PlaySound(SoundID.Item10, Projectile.position);
        }

        public override void AI()
        {
            player = Main.player[Projectile.owner];
            Projectile.velocity.X *= 0.992f;
            Projectile.velocity.Y += gravityStrength;
            if (Projectile.velocity.Y > 20f) Projectile.velocity.Y = 30f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (Main.rand.NextBool(5))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BorealWood, Projectile.velocity.X * .2f,
                Projectile.velocity.Y * .2f, 150, default, 1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Players.FSSpesialWeaponsPlayer chargeManager = Main.player[Projectile.owner].GetModPlayer<Players.FSSpesialWeaponsPlayer>();
            chargeManager.Rozaline_Spikes_ChargeMeter += chargeAmount;
            chargeManager.Rozaline_Spikes_UnchargeCooldown = 600;
            if (ChargedAttack && Main.rand.NextBool(2))
            {
                if (Projectile.owner == Main.myPlayer && !player.moonLeech)
                {
                    int heal = Main.rand.Next(3) + 2;
                    player.statLife += heal;
                    if (player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
                    player.HealEffect(heal);
                    NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, Projectile.owner, heal, 0.0f, 0.0f, 0, 0, 0);
                }
            }
        }

    }

}