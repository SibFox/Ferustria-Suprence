using Ferustria.Content.Projectiles.Friendly.Rozaline;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Ferustria.Content.Items.Weapons.Melee.PreHM
{
    public class Rozaline : ModItem
    {
        float charge;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rozaline");
            //Tooltip.SetDefault("Attacks with unique pattern.\n" +
            //    "Leaves a trail of [c/E45C5F:Blood Roses] at the tip of the spear.\n" +
            //    "[c/E45C5F:Roses] stick to the enemy and make him [c/e43a20:Rapidly Bleed].\n" +
            //    "While [c/E45C5F:Rose] sticks to the enemy, it slowly [c/a6cc34:charges] [c/ff9ba8:the Rozaline]\n" +
            //    "After a while, [c/E45C5F:the Rose] explodes into the [c/885041:Thorns].\n" +
            //    "On hitting the enemy, they make the victim [c/e43a20:Rapidly Bleed] and [c/a6cc34:charge] [c/ff9ba8:the Rozaline]\n" +
            //    "Killing the enemy [c/a6cc34:charges] [c/ff9ba8:the Rozaline]\n" +
            //    "When [c/ff9ba8:Rozaline] is fully [c/a6cc34:charged], press RMB to create a circle of [c/885041:Thorns] around you.\n" +
            //    "They have a chanse to heal you a little.\n" +
            //    "<CHARGE>");
            //DisplayName.AddTranslation(FSHelper.RuTrans, "Розалина");
            //Tooltip.AddTranslation(FSHelper.RuTrans, "Бьёт уникальной комбо атакой.\n" +
            //    "Оставляет след из [c/E45C5F:Кровавых Роз] на кончике копья.\n" +
            //    "[c/E45C5F:Розы] присасываются к врагу и заставляют его [c/e43a20:Быстро Кровоточить].\n" +
            //    "Пока [c/E45C5F:Розы] вистя на враге, они медленно [c/a6cc34:заряжают] [c/ff9ba8:Розалину].\n" +
            //    "Через время, [c/E45C5F:Роза] распадается на [c/885041:Шипы].\n" +
            //    "При поадании по врагу, они вызывают у него  [c/e43a20:Быстро Кровопотерю] и [c/a6cc34:заряжают] [c/ff9ba8:Розалину].\n" +
            //    "Убийство врага [c/a6cc34:заряжают] [c/ff9ba8:Розалину].\n" +
            //    "Когда [c/ff9ba8:Розалина] полностью [c/a6cc34:заряжена], нажмите ПКМ, чтобы испустить вокруг себя кучу [c/885041:Шипов].\n" +
            //    "Они имеют шанс слабо вас излечить.\n" +
            //    "<CHARGE>");

            ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // This skips use animation-tied sound playback, so that we're able to make it be tied to use time instead in the UseItem() hook.
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            // Common Properties
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 1, 55, 0);

            // Use Properties
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 3;
            Item.useTime = 3;
            Item.UseSound = SoundID.Item71; // The sound that this item plays when used.
            Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            // Weapon Properties
            Item.damage = 36;
            Item.knockBack = 6.5f;
            Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.

            // Projectile Properties
            Item.shootSpeed = 22f; // The speed of the projectile measured in pixels per frame.
            Item.shoot = ModContent.ProjectileType<Rozaline_SpearProjectile>(); // The projectile that is fired from this weapon
            charge = 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Players.FSSpesialWeaponsPlayer comboManager = player.GetModPlayer<Players.FSSpesialWeaponsPlayer>();

            if (type == ModContent.ProjectileType<Rozaline_SpearProjectile>())
            {
                float maxAngle = 0f;
                comboManager.Rozaline_Combo_Count++;
                if (comboManager.Rozaline_Combo_Count > 4) comboManager.Rozaline_Combo_Count = 1;
                switch (comboManager.Rozaline_Combo_Count)
                {
                    case 0: case 1: case 2: comboManager.Rozaline_UseCooldown = 25; comboManager.Rozaline_SpearTime = 17; maxAngle = 90f; break;
                    case 3: comboManager.Rozaline_UseCooldown = 40; comboManager.Rozaline_SpearTime = 8; break;
                    case 4: comboManager.Rozaline_UseCooldown = 48; comboManager.Rozaline_SpearTime = 25; break;
                }

                comboManager.Rozaline_Combo_Timer = 25 + comboManager.Rozaline_UseCooldown;
                if (Main.myPlayer == player.whoAmI)
                {
                    //float putAi = 0f;
                    //switch (comboManager.Rozaline_Combo_Count)
                    //{
                    //    case 1: putAi = 1f; break;
                    //    case 2: putAi = 2f; break;
                    //    case 3: putAi = 3f; break;
                    //    case 4: putAi = 6f; break;
                    //}
                    int id = Projectile.NewProjectile(Item.GetSource_FromThis(), position, velocity, type, (int)(damage * 1.5), knockback, player.whoAmI, 
                        comboManager.Rozaline_Combo_Count == 4 ? 6f : comboManager.Rozaline_Combo_Count, maxAngle);
                    Projectile proj = Main.projectile[id];
                    proj.timeLeft = comboManager.Rozaline_SpearTime;
                }
            }
            else if (type == ModContent.ProjectileType<Rozaline_ThornProjectile>())
            {
                int spikes = 25;
                SoundEngine.PlaySound(SoundID.Item151.WithPitchOffset(.2f), player.position);
                if (Main.myPlayer == player.whoAmI)
                {
                    for (int i = 0; i < spikes; i++)
                    {
                        double angle = 2.0 * Math.PI * i / spikes;
                        Vector2 speed = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                        speed.Normalize();
                        speed *= 7.5f;
                        Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, speed, type, 20, 2f, player.whoAmI, 1f);
                    }
                }
            }
            return false;
        }

        public override bool AltFunctionUse(Player player) => player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().Rozaline_ChargedUp_Notification;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ModContent.ProjectileType<Rozaline_ThornProjectile>();
                player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().Rozaline_Spikes_ChargeMeter = 0f;
                return true;
            }
            else
            {
                Item.shoot = ModContent.ProjectileType<Rozaline_SpearProjectile>();
                //if (player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().Rozaline_UseCooldown > 0) return false;
                return player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().Rozaline_UseCooldown <= 0;
                //return player.ownedProjectileCounts[Item.shoot] < 1;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            float charge = Main.LocalPlayer.GetModPlayer<Players.FSSpesialWeaponsPlayer>().Rozaline_Spikes_ChargeMeter;
            //string theText;
            //if (LanguageManager.Instance.ActiveCulture == FSHelper.RuTrans) theText = $"Шипы заряжены на {charge:N1}%";
            //else theText = $"Thorns are charged for {charge:N1}%";
            foreach (var line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Text == "<CHARGE>")
                {
                    line.Text = this.GetLocalization("ChargeTip").Format(charge);
                    line.OverrideColor = charge >= 100f ? Color.GreenYellow : Color.DarkGreen;
                }
            }
        }

        public override void AddRecipes()
        {
            RegisterRecipe.Reg([ new(ItemID.JungleSpores, 12), new(ItemID.Vine, 8), new(ItemID.Stinger, 6), new(ItemID.JungleRose) ], Type, tile: TileID.Anvils);
        }
    }
}
