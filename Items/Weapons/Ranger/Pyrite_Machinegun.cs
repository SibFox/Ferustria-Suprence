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
using Terraria.Audio;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Weapons.Ranger
{
	public class Pyrite_Machinegun : ModItem
	{
		int shotsDone, angle, averageShots, maximumShots, desicion;
		bool canShoot;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrite Machinegun");
			Tooltip.SetDefault("Accuracy improves over time.\nWarms up for more damage. Beware of overheat!\n67% chance not to consume ammo");
			DisplayName.AddTranslation("Russian", "Пиритовый Пулемёт");
			Tooltip.AddTranslation("Russian", "Точность повышается со временем\nРазогревается для нанесения большего урона. Остерегайтесь перегрева!\n67% шанс не потратить боеприпас");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 76;
			Item.height = 32;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.reuseDelay = 50;
			Item.shootSpeed = 26f;
			Item.shoot = ModContent.ProjectileType<Pyrite_Shot>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.2f;
			Item.value = Item.sellPrice(0, 4, 10, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item11;
			Item.useAmmo = ModContent.ItemType<Pyrite_Blend>();
			Item.autoReuse = true;
			shotsDone = 0;
			canShoot = true;
		}

		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 10);
			recipe.AddIngredient(mod, "Void_Dust", 17);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			desicion = 1;
			int alert = maximumShots - (maximumShots - averageShots) / 2;
			/*(type ==  ModContent.ProjectileType)
			{
				type = ModContent.ProjectileType<Pyrite_Shot>();
			}*/
			if (!player.HasBuff(ModContent.BuffType<Pyrite_Overheating_Status>()) && !player.HasBuff(ModContent.BuffType<Pyrite_Overheat_Status>()))
            {
				shotsDone = 0;
				Item.reuseDelay = 50;
				angle = 22;
				canShoot = true;
				Item.UseSound = SoundID.Item11;
				SoundEngine.PlaySound(SoundID.Item11, player.position);
				if (desicion == 1)
                {
					averageShots = 130;
					maximumShots = 160;
				}
				else
                {
					averageShots = 160;
					maximumShots = 220;
				}
				
			}
			if (canShoot) player.AddBuff(ModContent.BuffType<Pyrite_Overheating_Status>(), 2 * 70);
			//
			if (shotsDone > 10) damage = damage + (shotsDone / 9);
			if (damage >= 55) damage = 55 + (shotsDone / 16);
			if (damage >= 78) damage = 78;
			if (shotsDone <= averageShots && player.HasBuff(ModContent.BuffType<Pyrite_Overheating_Status>()) && canShoot)
			{
				shotsDone++;
				if (shotsDone >= 5 && shotsDone / 5 != 0) angle = 22 / (int)(shotsDone / 4.5);
				else angle = 22;
				if (angle < 2) angle = 2;
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(angle));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				if (Item.reuseDelay > 18) Item.reuseDelay = (int)(Item.reuseDelay / 1.4);
				else Item.reuseDelay -= 2;
				if (Item.reuseDelay < 2) Item.reuseDelay = 2;
			}
			if (shotsDone > averageShots && player.HasBuff(ModContent.BuffType<Pyrite_Overheating_Status>()) && canShoot)
            {
				shotsDone++;
				if (shotsDone % 4 == 0) CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 5, 20, 20), Color.DarkRed, "!!!", shotsDone >= alert ? true : false, false);
				if (angle < 26 && desicion == 1) angle += 2;
				else if (angle < 28 && desicion != 1) angle += 1;
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(angle));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)(damage * 1.2), knockBack, player.whoAmI);
				player.AddBuff(ModContent.BuffType<Pyrite_Overheating_Status>(), 3 * 70);
				Item.UseSound = SoundID.Item108;
			}
			if (shotsDone >= maximumShots && player.HasBuff(ModContent.BuffType<Pyrite_Overheating_Status>()) && canShoot) 
			{ 
				canShoot = false; 
				Item.UseSound = null;
				for (int i = 0; i < 6 + Main.rand.Next(5); i++)
                {
					Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(15));
					perturbedSpeed *= 1.6f - Main.rand.NextFloat() * .25f;
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)(damage * 1.8), knockBack, player.whoAmI);
					Dust.NewDust(player.position, Item.width * player.direction, Item.height, DustID.Torch, player.direction * Main.rand.NextFloat(-8.6f, -5f), player.direction * .8f, 0, default, Main.rand.NextFloat(.8f, 1.6f)); ;
				}
				if (Main.rand.NextBool()) SoundEngine.PlaySound(SoundID.Item74, player.position);
				else SoundEngine.PlaySound(SoundID.Item88, player.position);
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 5, 20, 20), Color.Red, "Overheat!", true, true);
				player.ClearBuff(ModContent.BuffType<Pyrite_Overheating_Status>());
				player.AddBuff(ModContent.BuffType<Pyrite_Overheat_Status>(), (3 + desicion) * 70);
			}
			Dust.NewDust(player.position, Item.width * player.direction, Item.height, DustID.Torch, player.direction * Main.rand.NextFloat(-8.6f, -5f), player.direction * .8f, 0, default, Main.rand.NextFloat(.8f, 1.6f));
			if (!canShoot) Item.reuseDelay = 54;
			return false;
		}



        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
			if (canShoot) return Main.rand.NextFloat() >= .67f;
			else return false;
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -1);
		}

	}

}