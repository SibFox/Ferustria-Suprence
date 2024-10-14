using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Dusts;
using Ferustria.Content.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.NPCs
{

    public class Balistic_Firing_TestNPC : ModNPC
    {

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 100;
            NPC.damage = 15;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.width = 26;
            NPC.height = 26;
            NPC.aiStyle = -1;
            NPC.npcSlots = 0f;
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath4;
            NPC.value = Item.buyPrice(0, 0, 1, 35);
        }

        bool setTimer = false;
        float req = 40f;
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            bool triple = false;
            float playerXvel = player.velocity.X * 0.5f;
            float playerYvel = player.velocity.Y * 0.2f;
            NPC.localAI[1] += 1f;
            if (!setTimer)
            {
                req = 40f;
                setTimer = true;
            }
            if (NPC.localAI[1] >= req)
            {
                NPC.localAI[1] = 0f;
                setTimer = false;

                if (Main.netMode != 1)
                {
                    FSHelper.SolveBalisticArc(NPC.Center, 16.5f, player.Center, 0.17f, out Vector2 s0, out Vector2 s1);

                    //if (s1 != Vector2.Zero && (player.Center.Y + 60f < NPC.Center.Y || player.Center.Y - 60f > NPC.Center.Y))
                    //{
                    //    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, s1, ModContent.ProjectileType<Barathrum_Echo>(), NPC.damage / 5, 0f, Main.myPlayer);
                    //}
                    //else
                    if (s0 != Vector2.Zero)
                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, s0/*.RotatedBy((NPC.Center - player.Center).SafeNormalize(Vector2.Zero).ToRotation())*/,
                            ModContent.ProjectileType<Barathrum_Echo>(), NPC.damage / 5, 0f, Main.myPlayer);

                    //if (Math.Abs(NPC.Center.Y - player.Center.Y) < NPC.Center.Y && Math.Abs(NPC.Center.X - player.Center.X) < 6f * 16f)
                    //{
                    //    float velY = NPC.velocity.Y * 1.5f;
                    //    if (NPC.velocity.Y <= 0) velY *= -1;
                    //Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * -0.4f + playerXvel, velY + playerYvel, ModContent.ProjectileType<Barathrum_Echo>(), NPC.damage / 5, 3f, Main.myPlayer, 0f, 0f);

                    //}
                    //else if (!triple) Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * 2.4f + playerXvel, NPC.velocity.Y * 2.4f + playerYvel, ModContent.ProjectileType<Barathrum_Echo>(), NPC.damage / 5, 3f, Main.myPlayer, 0f, 0f);
                    //else if (triple)
                    //{
                    //    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * 2.45f + playerXvel, NPC.velocity.Y * 2.2f + playerYvel, ModContent.ProjectileType<Barathrum_Echo>(), NPC.damage / 5, 3f, Main.myPlayer, 0f, 0f);
                    //    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * 2.8f + playerXvel, NPC.velocity.Y + playerYvel, ModContent.ProjectileType<Barathrum_Echo>(), NPC.damage / 5, 3f, Main.myPlayer, 0f, 0f);
                    //    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, NPC.velocity.X * 2.45f + playerXvel, NPC.velocity.Y * -2.2f + playerYvel, ModContent.ProjectileType<Barathrum_Echo>(), NPC.damage / 5, 3f, Main.myPlayer, 0f, 0f);

                    //}
                }
                NPC.velocity.X = -NPC.velocity.X * 4.5f;
                NPC.velocity.Y = -NPC.velocity.Y * 4.5f;
                NPC.velocity *= .4f;
            }

            base.AI();
        }
        
    }
}