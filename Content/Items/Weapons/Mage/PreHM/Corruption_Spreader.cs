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
	public class Corruption_Spreader : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corruption Spreader");
			Tooltip.SetDefault("Sends rot sacks, that sticks to enemies and explodes into small corrupted petals.");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Распространитель Искажения");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Выплёвывает гнилые мешочки, которые прилипают к врагам и разрываются распростроняя маленькие заражённые лепестки.");
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
            _ = new RegisterRecipe(new CraftMaterial[]
            { new(ItemID.DemoniteBar, 8), new(ItemID.ShadowScale, 7), new(ItemID.VileMushroom, 3), new(ItemID.Deathweed, 5)
            }, Type, tile: TileID.Anvils);
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