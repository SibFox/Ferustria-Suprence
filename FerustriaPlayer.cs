using Ferustria.Buffs.Negatives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Ferustria.Dusts;
using Terraria.Audio;

namespace Ferustria
{
    class FerustriaPlayer : ModPlayer
    {
        public int Crucifixion_Halo_Existance = -1;
        public bool Crucifixion_Tier2;

        public bool Resist_Accs_Equiped = false;
        public int Resist_Accs_Countdown = 0;
        public int Endure_Effect = 0;
        public float endure = -0.15f;



        public override void ResetEffects()
        {
            Crucifixion_Tier2 = false;
            Resist_Accs_Equiped = false;
        }

        public override void PreUpdate()
        {
            if (Resist_Accs_Equiped)
            {
                Resist_Accs_Countdown--;
                /*if (Resist_Accs_Countdown % 6 == 0 && endure >= 0)
                {
                    for (float i = 0; i < endure; i += 0.15f)
                    {
                        Dust.NewDust(Player.position, Player.width / 2, Player.height / 2, ModContent.DustType<Angelic_Particles>(), Player.velocity.X / 3, Player.velocity.Y / 3, 0, default, Main.rand.NextFloat(0.7f, 1.15f));
                    }
                }*/
                if (Main.rand.NextFloat() < endure * 2)
                    Dust.NewDust(Player.position, Player.width / 2, Player.height / 2, ModContent.DustType<Angelic_Particles>(), Player.velocity.X / 3, Player.velocity.Y / 3, 0, default, Main.rand.NextFloat(0.7f, 1.15f));
            }
            if (Resist_Accs_Countdown <= 0) { Resist_Accs_Countdown = 0; endure = -0.15f; Endure_Effect = 0; Player.noKnockback = false; }
            
        }

        public override void PreUpdateBuffs()
        {
            FerustriaPlayer refer = Player.GetModPlayer<FerustriaPlayer>();
            if (Player.HasBuff(ModContent.BuffType<Under_Crucifixion_Tier2>())) refer.Crucifixion_Tier2 = true;
            else refer.Crucifixion_Tier2 = false;
            if (refer.Crucifixion_Tier2)
            {
                if (refer.Crucifixion_Halo_Existance < 0)
                {
                    int proj = Projectile.NewProjectile(Player.GetSource_Buff(ModContent.BuffType<Under_Crucifixion_Tier2>()), Player.position, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.BGs.Crucifixion_Halo_Player>(), 0, 0f, Player.whoAmI);
                    Main.projectile[proj].timeLeft = 65;
                    Main.projectile[proj].netUpdate = true;
                }
                    
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //  Star of Hope            (Акссессуар увеличивающий сопротивляемость урону и откидыванию с каждым ударом)
            if (Resist_Accs_Equiped)
            {
                Resist_Accs_Countdown = 320;
                endure += 0.1f;
                if (endure >= 0.33f) { endure = 0.33f; Player.noKnockback = true; }
                Player.endurance += endure;
                if (endure >= 0 && Endure_Effect == 0)
                {
                    Endure_Effect = 1;
                    int particles = 200;
                    //SoundEngine.PlaySound(SoundID.Item4, Player.position).Volume *= 0.75f;
                    SoundEngine.PlaySound(SoundID.Item4, Player.position);
                    for (int i = 0; i < particles; i++)
                    {
                        double angle = 2.0 * Math.PI * i / particles;
                        Vector2 speed = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        float magnitude = (float)Math.Sqrt(speed.X * speed.X + speed.Y * speed.Y);
                        if (magnitude > 0) speed *= 6f / magnitude;
                        else speed = new Vector2(0f, 6f);
                        Dust.NewDustPerfect(Player.position, ModContent.DustType<Star_of_Hope_Effect>(), new(speed.X, speed.Y), 20, default, 2.5f);
                    }
                    /*for (int i = 0; i < particles / 4; i++)
                    {
                        double angle = 2.0 * Math.PI * i / particles;
                        Vector2 speed = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        float magnitude = (float)Math.Sqrt(speed.X * speed.X + speed.Y * speed.Y);
                        if (magnitude > 0) speed *= 2f / magnitude;
                        else speed = new Vector2(0f, 2f);
                        Dust.NewDustPerfect(Player.position, ModContent.DustType<Star_of_Hope_Effect>(), new(speed.X, speed.Y), 20, default, 1.5f);
                    }*/
                }

                // ---------------------------
            }
            /*for (int k = 1; k <= 12; k++)
            {
                float max;
                if (k % 2 != 0) max = 5.8f;
                else max = 4f;
                double angle = Math.PI * 2.0 * k / 12.0;
                Vector2 speed = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                float magnitude = (float)Math.Sqrt(speed.X * speed.X + speed.Y * speed.Y);
                if (magnitude > 0)
                {
                    speed *= max / magnitude;
                }
                else
                {
                    speed = new Vector2(0f, max);
                }
                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<Light_Ball_Forward>(), npc.damage / 5, 3f, Main.myPlayer, 1);
            }*/
            return true;
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            
            
        }

    }
}
