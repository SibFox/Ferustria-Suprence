using Microsoft.Xna.Framework;
using Ferustria.Projectiles.Friendly;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Weapons.Mage.HM
{
	public class Void_Blaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Blaster");
			Tooltip.SetDefault("Shoots 4 neon lasers in a row, that pierce through enemies.");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Бластер Пустоты");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Выстреливает 4 неоновых лазера очередью, пробивающих врагов насквозь.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 68;
			Item.height = 26;
			Item.useAnimation = 12;
			Item.useTime = 3;
			Item.reuseDelay = 14;
			Item.shootSpeed = 11.5f;
			Item.shoot = ModContent.ProjectileType<Neon_Laser>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.25f;
			Item.UseSound = SoundID.Item33;
			Item.value = Item.sellPrice(0, 2, 65, 0);
			Item.rare = ItemRarityID.Purple;
			Item.mana = 9;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.ClockworkAssaultRifle)
			.AddIngredient(ItemID.HallowedBar, 10)
			.AddIngredient(ItemID.SoulofSight, 10)
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
			return new Vector2(-15, 0);
		}

	}

}