using Ferustria.Content.Dusts;
using Ferustria.Content.Items.Materials.Drop;
using Ferustria.Content.Items.Materials.Specials;
using Ferustria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace Ferustria.Common.GlobalNPCs
{
    // This file shows numerous examples of what you can do with the extensive NPC Loot lootable system.
    // You can find more info on the wiki: https://github.com/tModLoader/tModLoader/wiki/Basic-NPC-Drops-and-Loot-1.4
    // Despite this file being GlobalNPC, everything here can be used with a ModNPC as well! See examples of this in the Content/NPCs folder.
    public class GlobalNPCSpecials : GlobalNPC
    {
        public override bool InstancePerEntity => true;


        public bool Any_Active_Shield { get; private set; }
        // Святой щит   |50% от стрелкового урона и 75% от магического, в общем снижает урон на 50%|
        public bool Uses_Holy_Shied { get; private set; }
        public bool Holy_Shield_Active { get; private set; }
        public bool Holy_Shield_Destroyed_Control = false;
        public int Holy_Shield_Durability_Max { get; private set; }
        public int Holy_Shield_Durability { get; private set; }
        public int Holy_Shield_Recharge_Timer { get; private set; }

        public void SetHolyShield(int shieldAmount = 1)
        {
            if (shieldAmount > 0)
            {
                Holy_Shield_Durability_Max = Holy_Shield_Durability = shieldAmount;
                Holy_Shield_Active = Uses_Holy_Shied = true;
            }
            else
            {
                new ArgumentException("Holy Shield amount set to less or equal to zero", "shieldAmount");
            }
        }

        public void RechargeHolyShield()
        {
            if (Uses_Holy_Shied)
            {
                Holy_Shield_Active = true;
                Holy_Shield_Durability = Holy_Shield_Durability_Max;
            }
            else
            {
                Ferustria.InnerDebug.Print("Tried to recharge holy shield when it's in no use");
            }
        }

        public void SetHolyShieldRecharge(int time)
        {
            Holy_Shield_Recharge_Timer = time;
            Holy_Shield_Destroyed_Control = false;
        }

        private static void HolyShieldDamageText(int damage, NPC npc)
        {
            CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new(0, 220, 220), damage);
        }




        public override void AI(NPC npc)
        {
            Any_Active_Shield = Holy_Shield_Active;
            
            if (Uses_Holy_Shied)
            {
                npc.immortal = npc.HideStrikeDamage = Holy_Shield_Active;
                if (Holy_Shield_Recharge_Timer >= 0) Holy_Shield_Recharge_Timer--;
                if (Holy_Shield_Recharge_Timer < 0 && !Holy_Shield_Active)
                {
                    RechargeHolyShield();
                }
            }
        }

        public override bool PreKill(NPC npc)
        {
            return base.PreKill(npc);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            ShieldLogic(npc, hit, damageDone);
            base.OnHitByProjectile(npc, projectile, hit, damageDone);
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            ShieldLogic(npc, hit, damageDone);
            base.OnHitByItem(npc, player, item, hit, damageDone);
        }

        void ShieldLogic(NPC npc, NPC.HitInfo hit, int damageDone)
        {
            if (npc != null)
            {
                // Святой щит   |50% от стрелкового урона и 75% от магического, в общем снижает урон на 50%|
                if (Uses_Holy_Shied)
                {
                    if (Holy_Shield_Active)
                    {
                        int damageDealt = (int)(hit.Crit ? hit.Damage /2 : hit.Damage * (hit.DamageType == DamageClass.Magic ? 0.25 : hit.DamageType == DamageClass.Ranged ? 0.5 : 1) * 0.5);
                        Holy_Shield_Durability -= damageDealt;

                        //modifiers.FinalDamage *= 0;
                        HolyShieldDamageText(damageDealt, npc);

                        if (Holy_Shield_Durability < 0)
                        {
                            Holy_Shield_Durability = 0;
                            Holy_Shield_Active = false;
                            Holy_Shield_Destroyed_Control = true;
                        }
                    }

                }
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Вычесть screenPos из drawPos
            if (false)
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, (npc.Center + new Vector2(0, npc.height / 2 + 8)) - screenPos, new Color(220, 220, 220, 200));
                Color gradientA = Color.Blue;
                Color gradientB = Color.Aqua;

                float charge = Holy_Shield_Durability / Holy_Shield_Durability_Max;
                charge = Utils.Clamp(charge, 0f, 1f);

                int left = (npc.width / 2 - 30) / 2; //npc.width / 2 +- 30
                int right = (npc.width / 2 + 30) / 2;
                int steps = (int)((right - left) * charge);
                for (int i = 0; i < steps; i++)
                {
                    //float percent = (float)i / steps; // Alternate Gradient Approach
                    float percent = (float)i / (right - left);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, (Main.screenHeight - 107) / 2, 1, 9), Color.Lerp(gradientA, gradientB, percent));
                }

                //spriteBatch.Draw(TextureAssets.MagicPixel.Value, npc.Center - screenPos + new Vector2(0, npc.height + 32), Color.White);
            }

        }
    }
}