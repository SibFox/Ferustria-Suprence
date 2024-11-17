using Ferustria.Content.Projectiles.Friendly.AMBlades_Salivar;
using Ferustria.Players;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Ferustria.Content.Items.Weapons.Melee.PreHM
{
    public class AMBlades_Salivar : ModItem
    {
        public override string Texture => "Ferustria/Content/Items/Weapons/Melee/PreHM/Impure_Barathrum_Sword";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // This skips use animation-tied sound playback, so that we're able to make it be tied to use time instead in the UseItem() hook.
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            // Common Properties
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 64, 0);

            // Use Properties
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = FSSpesialWeaponsPlayer.AMBlades_Salivar_UseCooldown_Max;
            Item.UseSound = SoundID.Item71; // The sound that this item plays when used.
            Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            // Weapon Properties
            Item.damage = 48;
            Item.knockBack = 4.5f;
            Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.

            // Projectile Properties
            Item.shootSpeed = 1f; // The speed of the projectile measured in pixels per frame.
            Item.shoot = ModContent.ProjectileType<AMBlades_Salivar_Projectile>(); // The projectile that is fired from this weapon
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            FSSpesialWeaponsPlayer weaponManager = player.GetModPlayer<FSSpesialWeaponsPlayer>();

            weaponManager.AMBlades_Salivar_Combo_Count++;
            weaponManager.AMBlades_Salivar_Combo_Timer = (int)(FSSpesialWeaponsPlayer.AMBlades_Salivar_UseCooldown_Max * 2);
            weaponManager.AMBlades_Salivar_UseCooldown = Item.useTime * (int)(weaponManager.AMBlades_Salivar_Enhansed && !weaponManager.AMBlades_Salivar_Enhansed_Active ? 3 : 1);
            weaponManager.AMBlades_Salivar_Frame_Count++;
            Projectile.NewProjectileDirect(source, position, velocity, type, damage / (int)(weaponManager.AMBlades_Salivar_Enhansed ? 2.5 : 1), knockback, player.whoAmI);

            return false;
        }

        public override void UpdateInventory(Player player)
        {

        }

        public override bool AltFunctionUse(Player player) => player.GetModPlayer<FSSpesialWeaponsPlayer>().AMBlades_Salivar_Charge >= 30 &&
                                                              player.GetModPlayer<FSSpesialWeaponsPlayer>().AMBlades_Salivar_Combo_Count >= 0;

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<FSSpesialWeaponsPlayer>().AMBlades_Salivar_UseCooldown > 0)
                return false;

            if (player.altFunctionUse == 2)
            {
                player.GetModPlayer<FSSpesialWeaponsPlayer>().AMBlades_Salivar_Enhansed = true;
                return true;
            }

            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            float charge = Main.LocalPlayer.GetModPlayer<FSSpesialWeaponsPlayer>().AMBlades_Salivar_Charge;
            foreach (var line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Text == "<CHARGE>")
                {
                    line.Text = this.GetLocalization("ChargeTip").Format(charge);
                    line.OverrideColor = charge >= 100f ? Color.Orange : Color.Yellow;
                }
            }
        }

        public override void AddRecipes()
        {
            //RegisterRecipe.Reg([ new(ItemID.JungleSpores, 12), new(ItemID.Vine, 8), new(ItemID.Stinger, 6), new(ItemID.JungleRose) ], Type, tile: TileID.Anvils);
        }
    }
}
