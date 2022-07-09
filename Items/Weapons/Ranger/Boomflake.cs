using Microsoft.Xna.Framework;
using Ferustria.Buffs.Statuses;
using Ferustria.Projectiles.Friendly;
using Ferustria.Items.Ammo;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Weapons.Ranger
{
	public class Boomflake : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boomflake");
			Tooltip.SetDefault("Shoots a barrage of snowballs.");
			DisplayName.AddTranslation("Russian", "Снего-шот");
			Tooltip.AddTranslation("Russian", "Стреляет шквалом снежков.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 56;
			Item.height = 20;
			Item.useTime = 38;
			Item.useAnimation = 38;
			Item.shootSpeed = 8f;
			Item.shoot = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 2, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = AmmoID.Snowball;
			Item.autoReuse = false;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			type = ProjectileID.SnowBallFriendly;
			for (int i = 0; i < Main.rand.Next(1) + 3; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(6));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false;
        }

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.Boomstick)
				.AddIngredient(ItemID.SnowballCannon)
				.AddIngredient(ItemID.GoldBar, 6)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.Boomstick)
				.AddIngredient(ItemID.SnowballCannon)
				.AddIngredient(ItemID.PlatinumBar, 6)
				.AddTile(TileID.Anvils)
				.Register();
		}

        public override Vector2? HoldoutOffset() => new Vector2(-8f, 0);

    }
}
