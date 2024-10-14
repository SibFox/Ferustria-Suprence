using Microsoft.Xna.Framework;
using Ferustria.Content.Projectiles.Friendly;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Weapons.Mage.PreHM
{
	public class Crimtane_Needler : ModItem
	{
		public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 44;
			Item.height = 42;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.shootSpeed = 11.5f;
			Item.shoot = ModContent.ProjectileType<Blood_Needle>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.25f;
			Item.UseSound = SoundID.Item17;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Blue;
			Item.mana = 6;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
            RegisterRecipe.Reg([ new(ItemID.CrimtaneBar, 8), new(ItemID.TissueSample, 7), new(ItemID.ViciousMushroom, 3), new(ItemID.Deathweed, 5) ], Type, tile: TileID.Anvils);
        }

	}

}