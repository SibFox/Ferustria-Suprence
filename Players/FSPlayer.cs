using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Buffs.Positives;
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
        //// ~~~ Звезда Надежды
        public bool Acc_StarOfHope_Equiped = false;
        public int Acc_StarOfHope_Countdown = 0;
        public int Endure_Effect = 0;
        public float endure = -0.15f;
        //// ~~~ Грибной Рассадник
        public bool Acc_MushroomsSpawner_Equiped = false;
        public int Acc_MushroomsSpawner_DamageAccumulated = 0;
        public int Acc_MushroomsSpawner_DeaccumulationTimer = 180;
        public const int Acc_MushroomsSpawner_DeaccumulationTimer_Max = 180;

          //////////////////////////////////
         ////    Эффекты де/баффов     ////
        //////////////////////////////////
        public bool DeBuff_ShatteredArmor_Applied = false;
        public bool DeBuff_SlicedDefense_Applied = false;
        public bool Buff_Vitality_Rage_Applied = false;


        public override void ResetEffects()
        {
            Crucifixion_Tier2 = false;
            Acc_StarOfHope_Equiped = false;
            DeBuff_ShatteredArmor_Applied = Player.HasBuff<Shattered_Armor>();
            DeBuff_SlicedDefense_Applied = Player.HasBuff<Sliced_Defense>();
            Buff_Vitality_Rage_Applied = Player.HasBuff<Vitality_Rage>();
        }

        public override void PreUpdate()
        {
            //// ~~~ Star of Hope
            if (Acc_StarOfHope_Equiped)
            {
                Acc_StarOfHope_Countdown--;
                /*if (Resist_Accs_Countdown % 6 == 0 && endure >= 0)
                {
                    for (float i = 0; i < endure; i += 0.15f)
                    {
                        Dust.NewDust(Player.position, Player.width / 2, Player.height / 2, ModContent.DustType<Angelic_Particles>(), Player.velocity.X / 3, Player.velocity.Y / 3, 0, default, Main.rand.NextFloat(0.7f, 1.15f));
                    }
                }*/
                if (Main.rand.NextFloat() < endure * 2)
                    Dust.NewDust(Player.position, Player.width / 2, Player.height / 3, DustType<Angelic_Particles>(), Player.velocity.X / 3, Player.velocity.Y / 3, 0, default, Main.rand.NextFloat(0.7f, 1.15f));
            }
            if (Acc_StarOfHope_Countdown <= 0) { Acc_StarOfHope_Countdown = 0; endure = -0.15f; Endure_Effect = 0; Player.noKnockback = false; }
        }

          //////////////////////////////////////////////////
         //// Всё что связано со статами, делать здесь ////
        //////////////////////////////////////////////////
        public override void PostUpdate()
        {
            if (DeBuff_ShatteredArmor_Applied && !DeBuff_SlicedDefense_Applied)
            {
                if (Player.HasBuff(BuffType<Shattered_Armor>()))
                {
                    int getMinuser = Player.statDefense / 2;
                    if (getMinuser > 25) Player.statDefense -= 25;
                    else Player.statDefense /= 2;
                }
            }

            if (DeBuff_SlicedDefense_Applied)
            {
                int getMinuser = Convert.ToInt32(Math.Round(Player.statDefense / 1.5));
                if (getMinuser > 60) Player.statDefense -= 60;
                else Player.statDefense -= Player.statDefense - getMinuser;
            }
        }

        public override void PreUpdateBuffs()
        {

        }

        public override void PostUpdateBuffs()
        {
            
        }

        public override void UpdateLifeRegen()
        {
            base.UpdateLifeRegen();
            if (Buff_Vitality_Rage_Applied)
                Player.GetDamage(DamageClass.Generic) += Player.lifeRegen * 0.008f;
        }

        //Альтернатива PreHurt?
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            //// ~~~ Star of Hope          (Акссессуар увеличивающий сопротивляемость урону и откидыванию с каждым ударом)
            if (Acc_StarOfHope_Equiped)
            {
                Acc_StarOfHope_Countdown = 320;
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
                        Vector2 speed = Vector2.One.GetVector_ToAngle_WithMult(angle, 8.5f);
                        int type = DustType<Star_of_Hope_Effect>();
                        if (i % 4 == 0) type = DustType<Star_of_Hope_Effect_Gold>();
                        Dust.NewDustPerfect(Player.position, type, new(speed.X, speed.Y), 20, default, 2f);
                    }
                }
            }
            // ---------------------------
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Acc_MushroomsSpawner_Equiped)
            {
                Acc_MushroomsSpawner_DamageAccumulated += hit.Damage;
                Acc_MushroomsSpawner_DeaccumulationTimer = Acc_MushroomsSpawner_DeaccumulationTimer_Max;

                while (Acc_MushroomsSpawner_DamageAccumulated >= 100)
                {
                    Acc_MushroomsSpawner_DamageAccumulated -= 100;

                }
            }
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
