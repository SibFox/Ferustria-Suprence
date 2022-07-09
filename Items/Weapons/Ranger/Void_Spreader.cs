using Microsoft.Xna.Framework;
using Ferustria.Projectiles.Friendly;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Weapons.Ranger
{
	public class Void_Spreader : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Spreader");
			Tooltip.SetDefault("Flames of Neon. Sets enemies onto neon fire.\n[c/FA0000:WIP]");
			DisplayName.AddTranslation("Russian", "Распростронитель Пустоты");
			Tooltip.AddTranslation("Russian", "Пламя Неона. Поджигает врагов неоновым пламенем.\n[c/FA0000:WIP]");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 52;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 56;
			Item.height = 28;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.shootSpeed = 14.5f;
			Item.shoot = ModContent.ProjectileType<Neon_Laser>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.25f;
			Item.UseSound = SoundID.Item10;
			Item.value = Item.sellPrice(0, 4, 35, 0);
			Item.rare = ItemRarityID.Purple;
			Item.mana = 13;
			Item.autoReuse = true;
			Item.scale *= 0.86f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.HallowedBar, 15)
			.AddIngredient(ItemID.IllegalGunParts, 2)
			.AddIngredient(ItemID.SoulofSight, 12)
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