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
    class FSSpecialArmorPlayer : ModPlayer
    {
        //// ~~~ Гнилая броня
        public bool RottenArmor_SetBonus = false;
        public float RottenArmor_ArmorMult = 0.0f;
        public int RottenArmor_ArmorTimer = 0;
        public const int RottenArmor_ArmorTimer_Max = 600;
        public float RottenArmor_Regeneration = 0.0f;

        public override void ResetEffects()
        {
            RottenArmor_SetBonus = false;
        }

        public override void PreUpdate()
        {
            //// ~~~ Гнилая броня
            if (RottenArmor_ArmorTimer > 0 && RottenArmor_ArmorMult > 0)
                RottenArmor_ArmorTimer--;
            if (RottenArmor_ArmorTimer <= 0)
            {
                RottenArmor_ArmorTimer = RottenArmor_ArmorTimer_Max;
                RottenArmor_ArmorMult = 0;
                RottenArmor_Regeneration = 0;
            }
        }

          //////////////////////////////////////////////////
         //// Всё что связано со статами, делать здесь ////
        //////////////////////////////////////////////////
        public override void PostUpdate()
        {
            //// ~~~ Гнилая броня
            if (RottenArmor_ArmorMult > 0)
            {
                if (RottenArmor_ArmorMult > 30) RottenArmor_ArmorMult = 30;
                Player.statDefense += (int)(Player.statDefense * (RottenArmor_ArmorMult / 100));
            }
            if (RottenArmor_Regeneration > 6)
                RottenArmor_Regeneration = 6;
        }

        public override void PreUpdateBuffs()
        {
           
        }

        public override void PostUpdateBuffs()
        {
            
        }

        public override void UpdateLifeRegen()
        {
            Player.lifeRegen += (int)RottenArmor_Regeneration;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            //// ~~~ Гнилая броня
            if (RottenArmor_SetBonus)
            {
                RottenArmor_ArmorMult += 2.5f;
                RottenArmor_Regeneration += 0.25f;
                RottenArmor_ArmorTimer = RottenArmor_ArmorTimer_Max;
            }
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            //if (victim is NPC npc)
            //{ 
            //}
        }



    }
}
