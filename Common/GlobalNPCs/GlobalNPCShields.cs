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
    public class GlobalNPCShields : GlobalNPC
    {
        public override bool InstancePerEntity => true;


        public bool Any_Active_Shield { get; private set; }
        float barScale;
        // Святой щит   |50% от стрелкового урона и 75% от магического, в общем снижает урон на 50%|
        public bool Uses_Holy_Shied { get; private set; }
        public bool Holy_Shield_Active { get; private set; }
        public bool Holy_Shield_Destroyed_Control { get; set; }
        public int Holy_Shield_Durability_Max { get; private set; }
        public int Holy_Shield_Durability { get; private set; }
        public int Holy_Shield_Recharge_Timer { get; private set; }

        public void SetHolyShield(int shieldAmount = 1, int? maxShieldAmount = null, float barScale = 0.75f)
        {
            if (shieldAmount > 0)
            {
                this.barScale = barScale;
                Holy_Shield_Durability_Max = maxShieldAmount == null ? shieldAmount : maxShieldAmount.Value;
                Holy_Shield_Durability = shieldAmount;
                Uses_Holy_Shied = Holy_Shield_Active = true;
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
                Holy_Shield_Active = true;
                Holy_Shield_Durability = amount == 0 ? Holy_Shield_Durability_Max : amount;
            }
            else
            {
                Ferustria.InnerDebug.Print("Tried to recharge holy shield when it's in no use or amount set to zero");
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

        Texture2D texture = TextureAssets.MagicPixel.Value;
        Texture2D textureHP = TextureAssets.MagicPixel.Value;

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Вычесть screenPos из drawPos
            if (Uses_Holy_Shied && Holy_Shield_Active && Holy_Shield_Durability < Holy_Shield_Durability_Max)
            {
                texture = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetHPBarTexture("Holy_Shield"));
                textureHP = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetHPBarTexture("Holy_Shield_HP"));

                Color gradientA = Color.Blue;
                Color gradientB = Color.Aqua;

                spriteBatch.Draw(texture, (npc.Center + new Vector2((int)(-npc.width / 1.3 * barScale), npc.height / 2 + 8)) - screenPos, new Rectangle(0, 0, texture.Width, texture.Height),
                    new Color(255, 255, 255) * 0.5f, 0, new(), barScale, SpriteEffects.None, 1f);
                float charge = (float)Holy_Shield_Durability / (float)Holy_Shield_Durability_Max;
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