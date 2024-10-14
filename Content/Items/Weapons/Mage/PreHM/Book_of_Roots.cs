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
	public class Book_of_Roots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Book of Roots");
            Item.ResearchUnlockCount = 1;
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 28;
			Item.height = 30;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.shootSpeed = 11.5f;
			Item.shoot = ModContent.ProjectileType<Testing_Projectile_FromGround>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.25f;
			Item.UseSound = SoundID.Item17;
			Item.value = Item.sellPrice(0, 0, 23, 50);
			Item.rare = ItemRarityID.Blue;
			Item.mana = 5;
			Item.autoReuse = true;
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position.ApplyMuzzleOffset(velocity);
        }

        //public override void AddRecipes()
        //{
        //	CreateRecipe()
        //	.AddIngredient(ItemID.DemoniteBar, 8)
        //	.AddIngredient(ItemID.ShadowScale, 7)
        //	.AddIngredient(ItemID.VileMushroom, 3)
        //	.AddIngredient(ItemID.Deathweed, 5)
        //	.AddTile(TileID.Anvils)
        //	.Register();
        //}

    }

}