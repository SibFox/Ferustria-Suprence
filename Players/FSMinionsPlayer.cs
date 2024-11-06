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
using Ferustria.Content.Buffs.Minions_And_Pets;

namespace Ferustria.Players
{
    class FSMinionsPlayer : ModPlayer
    {
        public bool Minion_AngelicSwordsman_Summoned;
        //public int Minion_AngelicSwordsman_Charge;
        //public int Minion_AngelicSwordsman_DischargeCooldown;


        public override void ResetEffects()
        {
            Minion_AngelicSwordsman_Summoned = Player.HasBuff<Angelic_Swordsman_Summoned_Buff>();
        }

        public override void PreUpdate()
        {
            //// ~~~ Ангельский мечник
            //if (Minion_AngelicSwordsman_Charge > 0 && Minion_AngelicSwordsman_DischargeCooldown < 0)
            //{
            //    Minion_AngelicSwordsman_Charge--;
            //    Minion_AngelicSwordsman_DischargeCooldown = 60;

            //}
            //if (Minion_AngelicSwordsman_DischargeCooldown > 0) Minion_AngelicSwordsman_DischargeCooldown--;

            //if (!Minion_AngelicSwordsman_Summoned)
            //{
            //    Minion_AngelicSwordsman_Charge = 0;
            //    Minion_AngelicSwordsman_DischargeCooldown = 0;
            //}
        }

          //////////////////////////////////////////////////
         //// Всё что связано со статами, делать здесь ////
        //////////////////////////////////////////////////
        public override void PostUpdate()
        {
            
        }

        public override void PreUpdateBuffs()
        {

        }

        public override void PostUpdateBuffs()
        {
            
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
