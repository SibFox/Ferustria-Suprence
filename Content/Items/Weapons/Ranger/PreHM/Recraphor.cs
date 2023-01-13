using Microsoft.Xna.Framework;
using Ferustria.Content.Projectiles.Friendly;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Ammo;
using Ferustria.Content.Items.Materials.Craftable;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.Items.Weapons.Ranger.PreHM
{
	public class Recraphor : ModItem
	{		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Recraphor");
			Tooltip.SetDefault("Shoots homing void micro-organisms.\n" +
                 "Microorganisms, upon hitting an enemy, slowly infect them.\n" +
                 "Infection increases microbial damage against the target.\n" +
                 "Sufficient infestation will produce more micro-organisms from the victim.\n" +
                 "Shoots 1 to 4 microorganisms.\n" +
                 "Requires [c/a0af6a:Living Compound Tank] to shoot");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Рекрафор");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Стреляет самонаводящимеся пустотными микроорганизмами.\n" +
                "Микрооргаизмы, при попадании по врагу, медленно заражают его.\n" +
                "Заражение увеличивает урон микрооганизмов по цели.\n" +
                "При достаточном заражении, из жертвы появятся ещё микроорганизмы.\n" +
                "Выстреливает от 1 до 4 микроорганизмов.\n" +
                "Для стрельбы необходим [c/a0af6a:Баллон с Живыми Соединениями]");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 32;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.crit = 4;
			Item.width = 66;
			Item.height = 26;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.shootSpeed = 10.5f;
			Item.shoot = ModContent.ProjectileType<Microorganism>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.6f;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item11;
            Item.useAmmo = ModContent.ItemType<Live_Compound_Tank>();
			Item.autoReuse = true;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;
            int projsToShoot = Main.rand.Next(4) + 1;
            float angle = MathHelper.ToRadians(15);
            for (int i = 0; i < projsToShoot; i++)
            {
                Projectile.NewProjectileDirect(source, position, velocity.RotatedBy(MathHelper.Lerp(-angle, angle, Main.rand.NextFloat())),
                    type, damage, knockback, player.whoAmI);
            }
			return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-15f, 0);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Impure_Dust>(14)
                .AddIngredient(ItemID.Bone, 40)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddIngredient<Reinforced_Living_Fiber_Tube>(5)
                .AddIngredient(ItemID.GoldBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
}
