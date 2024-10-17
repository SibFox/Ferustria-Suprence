using Microsoft.Xna.Framework;
using Ferustria.Content.Projectiles.Friendly;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using Ferustria.Content.Items.Materials.Specials;
using Ferustria.Content.Items.Materials.Craftable;

namespace Ferustria.Content.Items.Weapons.Mage.PreHM
{
    public class Ceremonial_Knife : ModItem
	{
        public bool MakeCircle;

        public static LocalizedText ChargeText { get; private set; }
            
        public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.crit = 2;
			Item.width = 72;
			Item.height = 40;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.shootSpeed = 11.5f;
			Item.shoot = ModContent.ProjectileType<Ceremonial_Proejctile_Forward_Friendly>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.25f;
			Item.UseSound = SoundID.Item71;
			Item.value = Item.sellPrice(0, 0, 23, 50);
			Item.rare = ItemRarityID.Blue;
			Item.mana = 13;
			Item.autoReuse = true;
            Item.scale = 0.8f;
		}

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().CKnifeL1_Knifes_Charge >= 100f && player.CheckMana(25))
                {
                    MakeCircle = true;
                    Item.mana = 25;
                    player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().CKnifeL1_Knifes_Charge = 0f;
                    return true;
                }
                else return false;
            }
            else Item.mana = 13;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (MakeCircle)
            {
                for (int i = 1; i <= 3; i++)
                {
                    type = ModContent.ProjectileType<Ceremonial_Proejctile_Circle_Friendly>();
                    if (Main.myPlayer == player.whoAmI)
                    {
                        Projectile.NewProjectileDirect(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, i);
                    }
                }
                MakeCircle = false;
                Item.mana = 13;
                return false;
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            float charge = Main.LocalPlayer.GetModPlayer<Players.FSSpesialWeaponsPlayer>().CKnifeL1_Knifes_Charge;
            //string theText;
            //if (LanguageManager.Instance.ActiveCulture == FSHelper.RuTrans) theText = $"Клинки заряжены на {charge:N1}%";
            //else theText = $"Blades are charged for {charge:N1}%";
            foreach (var line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Text == "<CHARGE>")
                {
                    line.Text = this.GetLocalization("ChargeTip").Format(charge);
                    line.OverrideColor = charge >= 100f ? Color.Aqua : Color.Blue;
                }
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new(-17.5f, 16);
        }

        public override void AddRecipes()
        {
            RegisterRecipe.Reg([ new(ModContent.ItemType<Ceremonial_Knife_Piece>(), 5), new(ItemID.Bone, 30), new(ModContent.ItemType<Reinforced_Living_Fiber_Tube>(), 5),
                new(ItemID.IllegalGunParts), new(ItemID.GoldBar, 8) ], Type, tile: TileID.Anvils);
            RegisterRecipe.Reg([ new(ModContent.ItemType<Ceremonial_Knife_Piece>(), 5), new(ItemID.Bone, 30), new(ModContent.ItemType<Reinforced_Living_Fiber_Tube>(), 5),
                new(ItemID.IllegalGunParts), new(ItemID.PlatinumBar, 8) ], Type, tile: TileID.Anvils);
        }
    }
}