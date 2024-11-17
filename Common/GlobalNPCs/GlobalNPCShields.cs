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
    public class GlobalNPCShields : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        float barScale;
        public bool Any_Active_Shield { get; private set; }
        // Святой щит   |50% от стрелкового урона и 75% от магического, в общем снижает урон на 50%|
        public bool Uses_Holy_Shied { get; private set; }
        public bool HolyShield_Active { get; private set; }
        /// <summary>
        /// Переменная для определния логики у NPC во время разрушения щита
        /// </summary>
        public bool HolyShield_Destroyed_Control { get; set; }
        public int HolyShield_Durability_Max { get; private set; }
        public int HolyShield_Durability { get; private set; }
        public int HolyShield_Recharge_Timer { get; private set; }

        public void SetHolyShield(int shieldAmount = 1, int? maxShieldAmount = null, float barScale = 0.75f)
        {
            if (shieldAmount > 0)
            {
                this.barScale = barScale;
                HolyShield_Durability_Max = maxShieldAmount == null ? shieldAmount : maxShieldAmount.Value;
                HolyShield_Durability = shieldAmount;
                Uses_Holy_Shied = HolyShield_Active = true;
            }
            else
            {
                new ArgumentException("Holy Shield amount set to less or equal to zero", "shieldAmount");
            }
        }

        public void RechargeHolyShield(int amount = 0)
        {
            if (Uses_Holy_Shied && amount >= 0)
            {
                HolyShield_Active = true;
                HolyShield_Durability = amount == 0 ? HolyShield_Durability_Max : amount;
            }
            else
            {
                new ArgumentException("Tried to recharge holy shield when it's in no use or amount set to zero", "amount");
            }
        }

        public void SetHolyShieldRecharge(int time)
        {
            HolyShield_Recharge_Timer = time;
            HolyShield_Destroyed_Control = false;
        }

        private static void HolyShieldDamageText(int damage, NPC npc)
        {
            CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new(0, 220, 220), damage);
        }


        public override void AI(NPC npc)
        {
            Any_Active_Shield = HolyShield_Active;
            
            if (Any_Active_Shield)
            {
                for (int i = 0; i < NPC.maxBuffs; i++)
                    npc.buffTime[i] = 0;
            }

            if (Uses_Holy_Shied)
            {
                npc.immortal = npc.HideStrikeDamage = HolyShield_Active;
                if (HolyShield_Recharge_Timer >= 0) HolyShield_Recharge_Timer--;
                if (HolyShield_Recharge_Timer < 0 && !HolyShield_Active)
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
                    if (HolyShield_Active)
                    {
                        int damageDealt = (int)((hit.Crit ? hit.Damage /2 : hit.Damage) * (hit.DamageType == DamageClass.Magic ? 0.25 : hit.DamageType == DamageClass.Ranged ? 0.5 : 1) * 0.5);
                        HolyShield_Durability -= damageDealt;

                        //modifiers.FinalDamage *= 0;
                        HolyShieldDamageText(damageDealt, npc);

                        if (HolyShield_Durability < 0)
                        {
                            HolyShield_Durability = 0;
                            HolyShield_Active = false;
                            HolyShield_Destroyed_Control = true;
                        }
                    }

                }
            }
        }

        Texture2D texture = TextureAssets.MagicPixel.Value;
        Texture2D textureHP = TextureAssets.MagicPixel.Value;

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Вычесть screenPos из drawPos
            if (Uses_Holy_Shied && HolyShield_Active && HolyShield_Durability < HolyShield_Durability_Max)
            {
                texture = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetHPBarTexture("Holy_Shield"));
                textureHP = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetHPBarTexture("Holy_Shield_HP"));

                Color gradientA = Color.Blue;
                Color gradientB = Color.Aqua;

                spriteBatch.Draw(texture, (npc.Center + new Vector2((int)(-npc.width / 1.3 * barScale), npc.height / 2 + 8)) - screenPos, new Rectangle(0, 0, texture.Width, texture.Height),
                    new Color(255, 255, 255) * 0.5f, 0, new(), barScale, SpriteEffects.None, 1f);
                float charge = (float)HolyShield_Durability / (float)HolyShield_Durability_Max;
                charge = Utils.Clamp(charge, 0f, 1f);

                int width = 60;
                int steps = (int)(width * charge);
                for (int i = 0; i < steps; i++)
                {
                    //float percent = (float)i / steps; // Alternate Gradient Approach
                    float percent = (float)i / width;
                    spriteBatch.Draw(textureHP, npc.Center + new Vector2((int)(-npc.width / 1.3 * barScale), npc.height / 2 + 8) - screenPos, new Rectangle(0, 0, i, 16), new Color(255, 255, 255, 100) * 0.5f,
                        0, new(), barScale, SpriteEffects.None, 1f);
                }
                
                //spriteBatch.Draw(TextureAssets.MagicPixel.Value, npc.Center - screenPos + new Vector2(0, npc.height + 32), Color.White);
            }
        }

        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = Any_Active_Shield ? 0f : 1f;
            return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);
        }
    }
}