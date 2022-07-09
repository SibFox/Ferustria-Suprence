using Microsoft.Xna.Framework;
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
	public class Crimtane_Needler : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimtane Needler");
			Tooltip.SetDefault("Sends thin needles that attaches to enemies and sucks their life.");
			DisplayName.AddTranslation("Russian", "Кримтановый Игольщик");
			Tooltip.AddTranslation("Russian", "Выплёвывает тонкие иглы, которые прикрепляются к врагам и высасывают их жизнь.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
			CreateRecipe()
			.AddIngredient(ItemID.CrimtaneBar, 8)
			.AddIngredient(ItemID.TissueSample, 7)
			.AddIngredient(ItemID.ViciousMushroom, 3)
			.AddIngredient(ItemID.Deathweed, 5)
			.AddTile(TileID.Anvils)
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

	}

}