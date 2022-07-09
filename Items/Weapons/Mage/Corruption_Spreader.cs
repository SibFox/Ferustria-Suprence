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
	public class Corruption_Spreader : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corruption Spreader");
			Tooltip.SetDefault("Sends rot sacks, that sticks to enemies and explodes into small corrupted petals.");
			DisplayName.AddTranslation("Russian", "Распространитель Искажения");
			Tooltip.AddTranslation("Russian", "Выплёвывает гнилые мешочки, которые прилипают к врагам и разрываются распростроняя маленькие заражённые лепестки.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 40;
			Item.height = 42;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.shootSpeed = 11.5f;
			Item.shoot = ModContent.ProjectileType<Rot_Sac>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.25f;
			Item.UseSound = SoundID.Item17;
			Item.value = Item.sellPrice(0, 0, 23, 50);
			Item.rare = ItemRarityID.Blue;
			Item.mana = 7;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.DemoniteBar, 8)
			.AddIngredient(ItemID.ShadowScale, 7)
			.AddIngredient(ItemID.VileMushroom, 3)
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