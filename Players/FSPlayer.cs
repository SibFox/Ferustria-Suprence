using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Projectiles.BGs;
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
using Ferustria.Content.Dusts;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace Ferustria.Players
{
    class FSPlayer : ModPlayer
    {
        public int Crucifixion_Halo_Existance = -1;
        public bool Crucifixion_Tier2;

          //////////////////////////////////////
         ////     Аксессуары/Артефакты     ////
        //////////////////////////////////////
        public bool Accessory_StarOfHope_Equiped = false;
        public int Accessory_StarOfHope_Countdown = 0;
        public int Endure_Effect = 0;
        public float endure = -0.15f;

          //////////////////////////////////
         ////    Эффекты де/баффов     ////
        //////////////////////////////////
        public bool DeBuff_ShatteredArmor_Applied = false;
        public bool DeBuff_SlicedDefense_Applied = false;


        public override void ResetEffects()
        {
            Crucifixion_Tier2 = false;
            Accessory_StarOfHope_Equiped = false;
            DeBuff_ShatteredArmor_Applied = false;
            DeBuff_SlicedDefense_Applied = false;
        }

        public override void PreUpdate()
        {
            if (Accessory_StarOfHope_Equiped)
            {
                Accessory_StarOfHope_Countdown--;
                /*if (Resist_Accs_Countdown % 6 == 0 && endure >= 0)
                {
                    for (float i = 0; i < endure; i += 0.15f)
                    {
                        Dust.NewDust(Player.position, Player.width / 2, Player.height / 2, ModContent.DustType<Angelic_Particles>(), Player.velocity.X / 3, Player.velocity.Y / 3, 0, default, Main.rand.NextFloat(0.7f, 1.15f));
                    }
                }*/
                if (Main.rand.NextFloat() < endure * 2)
                    Dust.NewDust(Player.position, Player.width / 2, Player.height / 2, DustType<Angelic_Particles>(), Player.velocity.X / 3, Player.velocity.Y / 3, 0, default, Main.rand.NextFloat(0.7f, 1.15f));
            }
            if (Accessory_StarOfHope_Countdown <= 0) { Accessory_StarOfHope_Countdown = 0; endure = -0.15f; Endure_Effect = 0; Player.noKnockback = false; }
        }

          //////////////////////////////////////////////////
         //// Всё что связано со статами, делать здесь ////
        //////////////////////////////////////////////////
        public override void PostUpdate()
        {
            if (DeBuff_ShatteredArmor_Applied && !DeBuff_SlicedDefense_Applied)
            {
                if (Player.HasBuff(ModContent.BuffType<Shattered_Armor>()))
                {
                    int getMinuser = Convert.ToInt32(Math.Round(Player.statDefense / 2.0));
                    if (getMinuser > 25) getMinuser = Convert.ToInt32(Player.statDefense - 25.0);
                    Player.statDefense = getMinuser;
                }
            }

            if (DeBuff_SlicedDefense_Applied)
            {
                int getMinuser = Convert.ToInt32(Math.Round(Player.statDefense / 1.5));
                if (getMinuser > 60) getMinuser = Convert.ToInt32(Player.statDefense - 60.0);
                Player.statDefense = getMinuser;
            }
        }

        public override void PreUpdateBuffs()
        {
            FSPlayer refer = Player.GetModPlayer<FSPlayer>();
            if (Player.HasBuff(ModContent.BuffType<Under_Crucifixion_Tier2>())) refer.Crucifixion_Tier2 = true;
            else refer.Crucifixion_Tier2 = false;
            if (refer.Crucifixion_Tier2)
            {
                if (refer.Crucifixion_Halo_Existance < 0)
                {
                    int proj = Projectile.NewProjectile(Player.GetSource_Buff(BuffType<Under_Crucifixion_Tier2>()), Player.position, new Vector2(0f, 0f), ProjectileType<Crucifixion_Halo_Player>(), 0, 0f, Player.whoAmI);
                    Main.projectile[proj].timeLeft = 65;
                    Main.projectile[proj].netUpdate = true;
                }
            }
        }

        public override void PostUpdateBuffs()
        {
            
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //  Star of Hope            (Акссессуар увеличивающий сопротивляемость урону и откидыванию с каждым ударом)
            if (Accessory_StarOfHope_Equiped)
            {
                Accessory_StarOfHope_Countdown = 320;
                endure += 0.1f;
                if (endure >= 0.33f) { endure = 0.33f; Player.noKnockback = true; }
                Player.endurance += endure;
                if (endure >= 0 && Endure_Effect == 0)
                {
                    Endure_Effect = 1;
                    int particles = 100;
                    SoundEngine.PlaySound(SoundID.Item4.WithVolumeScale(.75f), Player.position);
                    for (int i = 0; i < particles; i++)
                    {
                        double angle = 2.0 * Math.PI * i / particles;
                        Vector2 speed = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                        speed.SafeNormalize(new(0f, 4.5f));
                        speed *= 4.5f;
                        Dust.NewDustPerfect(Player.position, DustType<Star_of_Hope_Effect>(), new(speed.X, speed.Y), 20, default, 2f);
                        
                    }
                }
            }
            // ---------------------------
            return true;
        }
        public override void OnHitAnything(float x, float y, Entity victim)
        {
            //if (victim is NPC npc)
            //{ 
            //}
        }

        public override void UpdateDead()
        {
            ResetEffects();
        }

    }
}
