using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Buffs.Statuses;
using Ferustria.Content.Items.Weapons.Melee.PreHM;
using Ferustria.Content.Items.Weapons.Melee.HM;
using Ferustria.Content.Items.Weapons.Ranger.HM;
using Ferustria.Content.Items.Weapons.Mage.PreHM;
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
using static Terraria.ModLoader.ModLoader;

namespace Ferustria.Players
{
    [CloneByReference]
    class FSSpesialWeaponsPlayer : ModPlayer
    {
        enum Weapons
        {
            Unused,
            Rozaline,
            ViciousHerrscher,
            PyriteMachinegun,
            PyriteShotgun,
            CKnife1,
            BarathrumPruner,
            AMBladesSalivar
        }

        Weapons wieldedWeapon;

        //// ~~~ Копьё Розалина
        [CloneByReference] public int Rozaline_Combo_Count = 1;
        [CloneByReference] public int Rozaline_Combo_Timer = 0;
        [CloneByReference] public int Rozaline_UseCooldown = 0;
        [CloneByReference] public int Rozaline_SpearTime = 0;


        [CloneByReference] public float Rozaline_Spikes_ChargeMeter = 0;
        [CloneByReference] public int Rozaline_Spikes_UnchargeCooldown = 0;
        public bool Rozaline_ChargedUp_Notification;

        //// ~~~ Пиритовый пулемёт
        public int PMachinegun_MaximumShots = 160;
        public int PMachinegun_WarningShots = 130;
        public bool Accessory_PMachinegun_Enchanser_Equiped;
        [CloneByReference] public int PMachinegun_ShotsDone = 0;
        [CloneByReference] public int PMachinegun_ShotDelay = 0;
        [CloneByReference] public int PMachinegun_MaxShotDelay = 45;

        //// ~~~~ Церемониальный нож - Lvl 1
        [CloneByReference] public float CKnifeL1_Knifes_Charge = 0f;
        [CloneByReference] public bool CKnifeL1_ChargedUp_Notification;

        //// ~~~~ Пустотный секатор
        [CloneByReference] public float BarathrumPruner_Charge = 0f;
        [CloneByReference] public int BarathrumPruner_Charge_DepleteTimer = 420;

        //// ~~~~ АМ-Мечи ордена Саливар
        public const int AMBlades_Salivar_FrameTime = 2;
        public const int AMBlades_Salivar_FrameCountFor1 = 7 * AMBlades_Salivar_FrameTime;
        public const int AMBlades_Salivar_UseCooldown_Max = (int)(AMBlades_Salivar_FrameCountFor1 * 1.5);
        public int AMBlades_Salivar_Combo_Count = 0;
        public int AMBlades_Salivar_Combo_Timer = 0;
        public int AMBlades_Salivar_UseCooldown = 0;
        public int AMBlades_Salivar_UseTime = 0;
        


        public override void ResetEffects()
        {
            //Сделано не через switch, потому что, походу, выдаёт ошибку по причине GetItemName является статическим, или из-за самого класса являющегося статическим
            //Он пытается создать экземлпяр или что?
            //CS0426 Имя типа "GetItemName<>" не существует в типе "FSHelper"
            /*switch (heldItem)
            {
                case FSHelper.GetItemName<Rozaline>(): wieldedWeapon = Weapons.Rozaline; break;
            }*/

            wieldedWeapon = Weapons.Unused;
            string heldItem = Player.HeldItem.Name;
            if (heldItem == FSHelper.GetItemName<Rozaline>()) wieldedWeapon = Weapons.Rozaline;
            if (heldItem == FSHelper.GetItemName<Vicious_Herrscher>()) wieldedWeapon = Weapons.ViciousHerrscher;
            if (heldItem == FSHelper.GetItemName<Pyrite_Machinegun>()) wieldedWeapon = Weapons.PyriteMachinegun;
            if (heldItem == FSHelper.GetItemName<Pyrite_Shotgun>()) wieldedWeapon = Weapons.PyriteShotgun;
            if (heldItem == FSHelper.GetItemName<Ceremonial_Knife>()) wieldedWeapon = Weapons.CKnife1;
            if (heldItem == FSHelper.GetItemName<Barathrum_Pruner>()) wieldedWeapon = Weapons.BarathrumPruner;
            //if (heldItem == FSHelper.GetItemName<AMBlades_Salivar>()) wieldedWeapon = Weapons.AMBladesSalivar;


            //// ~~~ Копьё Розалина
            if (Rozaline_Combo_Timer < 1 || wieldedWeapon != Weapons.Rozaline) { Rozaline_Combo_Count = 0; Rozaline_Combo_Timer = 0; }

            if (wieldedWeapon == Weapons.Rozaline)
            {
                if (Rozaline_Combo_Timer > 0) Rozaline_Combo_Timer--;
                if (Rozaline_UseCooldown > 0) Rozaline_UseCooldown--;
                if (Rozaline_Combo_Timer < 1) { Rozaline_Combo_Count = 0; Rozaline_Combo_Timer = 0; Rozaline_UseCooldown = 0; Rozaline_SpearTime = 0; }
                if (Rozaline_Combo_Count > 4) { Rozaline_Combo_Count = 0; }
            }

            //// ~~~ Пиритовый пулемёт
            Accessory_PMachinegun_Enchanser_Equiped = false;

            if (!Player.HasBuff<Pyrite_Overheating_Status>() && !Player.HasBuff<Pyrite_Overheat_Status>())
            {
                PMachinegun_ShotsDone = 0;
                PMachinegun_ShotDelay = 0;
                PMachinegun_MaxShotDelay = 45;
            }
            if (Player.HasBuff<Pyrite_Overheat_Status>())
            {
                PMachinegun_ShotDelay = 1;
            }

            //// ~~~ АМ-Мечи ордена Саливар
            if (AMBlades_Salivar_Combo_Timer < 1 || wieldedWeapon != Weapons.AMBladesSalivar) { AMBlades_Salivar_Combo_Count = 0; AMBlades_Salivar_Combo_Timer = 0; }

            if (wieldedWeapon == Weapons.AMBladesSalivar)
            {
                if (AMBlades_Salivar_Combo_Timer > 0) AMBlades_Salivar_Combo_Timer--;
                if (AMBlades_Salivar_UseCooldown > 0) AMBlades_Salivar_UseCooldown--;
            }
        }

        public override void PreUpdate()
        {
            //// ~~~ Копьё Розалина
            if (Rozaline_Spikes_ChargeMeter >= 100f)
            {
                if (!Rozaline_ChargedUp_Notification)
                {
                    CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y - 15, 10, 5), Color.YellowGreen, "Mods.Ferustria.Common.ChargeNotifications.Rozaline", true, true);
                    Rozaline_ChargedUp_Notification = true;
                }
                Rozaline_Spikes_ChargeMeter = 100f;
            }
            else Rozaline_ChargedUp_Notification = false;
            if (Rozaline_Spikes_UnchargeCooldown > 0) Rozaline_Spikes_UnchargeCooldown--;
            if (Rozaline_Spikes_UnchargeCooldown < 1) Rozaline_Spikes_ChargeMeter -= 0.25f;
            if (Rozaline_Spikes_ChargeMeter < 0) Rozaline_Spikes_ChargeMeter = 0;


            //// ~~~ Пиритовый пулемёт
            if (Accessory_PMachinegun_Enchanser_Equiped)
            {
                PMachinegun_MaximumShots = 220;
                PMachinegun_WarningShots = 185;
            }
            else
            {
                PMachinegun_MaximumShots = 160;
                PMachinegun_WarningShots = 130;
            }
            PMachinegun_ShotDelay--;

            //// ~~~ Пиритовый дробовик
            if (wieldedWeapon == Weapons.PyriteShotgun)
            {
                if (Accessory_PMachinegun_Enchanser_Equiped)
                {
                    Player.HeldItem.useTime = 45;
                    Player.HeldItem.useAnimation = 45;
                }
                else
                {
                    Player.HeldItem.useTime = 58;
                    Player.HeldItem.useAnimation = 58;
                }
            }

            //// ~~~ Церемониальный нож - Lvl 1
            if (CKnifeL1_Knifes_Charge < 100f)
            {
                if (wieldedWeapon == Weapons.CKnife1) CKnifeL1_Knifes_Charge += 2.3f / 60f;
                else if (CKnifeL1_Knifes_Charge > 0f) CKnifeL1_Knifes_Charge -= 8f / 60;
                if (CKnifeL1_Knifes_Charge < 0f) CKnifeL1_Knifes_Charge = 0f;
                CKnifeL1_ChargedUp_Notification = false;
            }
            if (CKnifeL1_Knifes_Charge > 100f)
            {
                CKnifeL1_Knifes_Charge = 100f;
                if (!CKnifeL1_ChargedUp_Notification)
                {
                    CKnifeL1_ChargedUp_Notification = true;
                    CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y - 15, 10, 5), Color.BlueViolet, "Blades ready!", true, true);
                }
            }
            if (wieldedWeapon != Weapons.CKnife1 && CKnifeL1_Knifes_Charge >= 100f) { CKnifeL1_Knifes_Charge = 99f; }

            //// ~~~ Секатор Пропасти
            if (BarathrumPruner_Charge > 100f)
                BarathrumPruner_Charge = 100f;
            if (BarathrumPruner_Charge < 0f)
                BarathrumPruner_Charge = 0f;
            if (BarathrumPruner_Charge_DepleteTimer > 0)
                BarathrumPruner_Charge_DepleteTimer--;
            if (BarathrumPruner_Charge_DepleteTimer <= 0)
            {
                BarathrumPruner_Charge_DepleteTimer = 0;
                BarathrumPruner_Charge -= 3.5f / 60f;
            }

        }

          ////////////////////////////////////////////////////////////
         //// Всё что связано со статами персонажа, делать здесь ////
        ////////////////////////////////////////////////////////////
        public override void PostUpdate()
        {

        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            //if (victim is NPC npc)
            //{
                
            //}
        }

        public void SetEverythingToZero()
        {
            CKnifeL1_Knifes_Charge = 0;
            Rozaline_Combo_Count = 0;
            Rozaline_Combo_Timer = 0;
            Rozaline_SpearTime = 0;
            Rozaline_UseCooldown = 0;
            Rozaline_ChargedUp_Notification = false;
            BarathrumPruner_Charge = 0;
            BarathrumPruner_Charge_DepleteTimer = 420;
        }

        public override void UpdateDead()
        {
            SetEverythingToZero();
        }

    }
}
