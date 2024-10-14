using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Weapons.Ranger.PreHM
{
	public class Boomflake : ModItem
	{
		public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
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
			Item.value = Item.sellPrice(0, 5, 35, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = AmmoID.Snowball;
			Item.autoReuse = false;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			position.ApplyMuzzleOffset(velocity);
			type = ProjectileID.SnowBallFriendly;
			for (int i = 0; i < Main.rand.Next(1) + 3; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(13.5f));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false;
        }

        public override void AddRecipes()
        {

            RegisterRecipe.Reg([ (ItemID.Boomstick), (ItemID.SnowballCannon), (ItemID.GoldBar, 6) ], Type, tile: TileID.Anvils);
            RegisterRecipe.Reg([ (ItemID.Boomstick), (ItemID.SnowballCannon), (ItemID.PlatinumBar, 6) ], Type, tile: TileID.Anvils);
        }

        public override Vector2? HoldoutOffset() => new Vector2(-8f, 0);

    }
}
