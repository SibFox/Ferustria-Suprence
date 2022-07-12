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

namespace Ferustria.Items.Weapons.Ranger.PreHM
{
	public class Cross_Bow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cross-Bow");
			Tooltip.SetDefault("Shoots angelic bolts.\nOn hit they spawn additional bolts around the target.");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Кресто-Лук");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Стреляет англельскими болтами.\nПри попадании, они вызывают дополнительные болты вокруг цели.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 56;
			Item.height = 20;
			Item.useTime = 38;
			Item.useAnimation = 38;
			Item.shootSpeed = 10.2f;
			Item.shoot = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.6f;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = AmmoID.Arrow;
			Item.autoReuse = false;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			type = ModContent.ProjectileType<Angelic_Bolt_Friendly>();
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 1f);
			return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-8f, 0);

    }
}
