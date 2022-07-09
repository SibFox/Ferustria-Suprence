﻿using Microsoft.Xna.Framework;
using Ferustria.Projectiles.Friendly;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Weapons.Mage
{
	public class Void_Wrecker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Wrecker");
			Tooltip.SetDefault("Shoots a heavy neon blast that explodes upon contact.\nExplosion stays for a time and heals you if hits enemies.");
			DisplayName.AddTranslation("Russian", "Пустотный Разрушитель");
			Tooltip.AddTranslation("Russian", "Выстреливает тяжёлый неоновый лазер, который взрывается при контакте.\nВзрыв на время остаётся и лечит вас при нанесении урона врагам.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 72;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 56;
			Item.height = 28;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 14.5f;
			Item.shoot = ModContent.ProjectileType<Neon_Blast>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.25f;
			Item.UseSound = SoundID.Item91;
			Item.value = Item.sellPrice(0, 2, 95, 0);
			Item.rare = ItemRarityID.Purple;
			Item.mana = 13;
			Item.autoReuse = true;
			Item.scale *= 0.86f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.LaserRifle)
			.AddIngredient(ItemID.HallowedBar, 10)
			.AddIngredient(ItemID.SoulofMight, 10)
			.AddIngredient(Mod, "Impure_Dust", 10)
			.AddIngredient(Mod, "Void_Sample", 3)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, -2);
		}

	}

}