using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using Ferustria.Content.Projectiles.Friendly;
using Ferustria.Content.Buffs.Minions_And_Pets;
using rail;

namespace Ferustria.Content.Pets
{
    public class JiL_Pet : ModProjectile
	{

        private ref float LocalFrame => ref Projectile.ai[0];
        private ref float Stage => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            Main.projPet[Projectile.type] = true;

            // This code is needed to customize the vanity pet display in the player select screen. Quick explanation:
            // * It uses fluent API syntax, just like Recipe
            // * You start with ProjectileID.Sets.SimpleLoop, specifying the start and end frames as well as the speed, and optionally if it should animate from the end after reaching the end, effectively "bouncing"
            // * To stop the animation if the player is not highlighted/is standing, as done by most grounded pets, add a .WhenNotSelected(0, 0) (you can customize it just like SimpleLoop)
            // * To set offset and direction, use .WithOffset(x, y) and .WithSpriteDirection(-1)
            // * To further customize the behavior and animation of the pet (as its AI does not run), you have access to a few vanilla presets in DelegateMethods.CharacterPreview to use via .WithCode(). You can also make your own, showcased in MinionBossPetProjectile
            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 1, 9)
                .WithOffset(0, 0)
                .WithCode(DelegateMethods.CharacterPreview.Float);
        }

        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.ZephyrFish); // Copy the stats of the Zephyr Fish
            Projectile.friendly = true;
            Projectile.netImportant = true;
            Projectile.width = Projectile.height = 40;
            Projectile.aiStyle = -1;
            //AIType = ProjectileID.ZephyrFish; // Mimic as the Zephyr Fish during AI.
            DrawOffsetX = -12;
            DrawOriginOffsetY = -12;
        }

        public override bool PreAI()
        {
            //Player player = Main.player[Projectile.owner];

            //player.zephyrfish = false; // Relic from AIType

            //Projectile.gfxOffY = -12;

            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            CheckStage(player);

            FrameCalculate();

            CheckActive(player);

            Movement(player);            
        }

        void CheckActive(Player player)
        {
            // Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
            if (!player.dead && player.HasBuff(ModContent.BuffType<JiL_Pet_Buff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        int disSQ = (23 * 16) * (23 * 16);

        void CheckStage(Player player)
        {
            if (Projectile.DistanceSQ(player.Center) > ((4 * 16) * (4 * 16)) && Stage != 4) Stage = 2;
            if (Projectile.DistanceSQ(player.Center) > disSQ) Stage = 4;
            if ((player.Center.X - Projectile.Center.X > -60 && player.Center.X - Projectile.Center.X < 60)) Stage = 0;
        }

        void Movement(Player player)
        {
            //bool playerMoving = player.velocity.X != 0;
            //bool playerAir = player.velocity.Y != 0;

            //if (Stage != 4) Stage = 0; //2, 4
            

            Projectile.tileCollide = Stage != 4;

            Projectile.direction = Projectile.spriteDirection = player.position.X < Projectile.position.X ? -1 : 1;

            if (Stage != 4)
            {
                Projectile.velocity.Y += 12f / 60f;
                Projectile.velocity.Y = Utils.Clamp(Projectile.velocity.Y, -1f, 8f);
                Projectile.rotation = 0f;
            }

            if (Stage == 0)
            {
                Projectile.velocity.X *= 0.95f;
            }

            if (Stage == 2)
            {
                Projectile.velocity.X += 4.75f / 60f * Projectile.direction;
                Projectile.velocity.X = Utils.Clamp(Projectile.velocity.X, -5.5f, 5.5f);
            }

            if (Stage == 4)
            {
                Vector2 destination = player.Center + new Vector2(16 * player.direction, 0);
                Projectile.HomingTowards(destination, 20f, 2f);
                Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.direction == 1 ? 0 : MathHelper.ToRadians(180f));
                Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, new(0, 0), 60, new(0, Main.rand.Next(230, 256), 255), 1f);
            }
        }

        void FrameCalculate()
        {
            Projectile.frame = (int)(LocalFrame + Stage);
            if (Projectile.frameCounter++ >= (Stage != 0 ? 9 : 20))
            {
                Projectile.frameCounter = 0;
                if (++LocalFrame > 1) LocalFrame = 0;
            }
        }
    }

}