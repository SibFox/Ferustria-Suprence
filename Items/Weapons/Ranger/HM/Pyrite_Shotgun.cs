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

namespace Ferustria.Items.Weapons.Ranger.HM
{
	public class Pyrite_Shotgun : ModItem
	{
		int shotsDone;
		bool powered;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrite Shotgun");
			Tooltip.SetDefault("It's hard to control this beast.\nWarms up for more damage");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Пиритовый Дробовик");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Этого зверя сложно контрлировать.\nРазогревается для нанесения большего урона.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 110;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.crit = 7;
			Item.width = 66;
			Item.height = 28;
			Item.useTime = 74;//74
			Item.useAnimation = 74;
			Item.shootSpeed = 16f;
			Item.shoot = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 9f;
			Item.value = Item.sellPrice(0, 4, 20, 0);
			Item.rare = ItemRarityID.Orange;
			Item.useAmmo = ModContent.ItemType<Pyrite_Blend>();
			Item.autoReuse = false;
			shotsDone = 0;
			powered = false;
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
			int numberProjectiles = 3 + Main.rand.Next(4);
			if (!player.HasBuff(ModContent.BuffType<Pyrite_Ignition_Status>())) shotsDone = 0;
			player.AddBuff(ModContent.BuffType<Pyrite_Ignition_Status>(), 5*60);
			damage = damage + shotsDone * 12;
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			/*if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<Pyrite_Shot>();
			}*/
			//ModifyWeaponCrit(player, ref Item.crit);
			if (shotsDone < 5)
            {
				shotsDone++;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(19)); //19
					float scale = 1.65f - (Main.rand.NextFloat() * .35f); //1.5
					perturbedSpeed *= scale;
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
				if (shotsDone >= 4 && player.HasBuff(ModContent.BuffType<Pyrite_Ignition_Status>())) 
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 10, 10, 2), Color.Yellow, (6 - shotsDone).ToString() + "!", true, false);
				SoundEngine.PlaySound(SoundID.Item38, player.position);
			}
            else if (shotsDone >= 5 && player.HasBuff(ModContent.BuffType<Pyrite_Ignition_Status>()))
            {
				SoundEngine.PlaySound(SoundID.Item14, player.position);
				powered = true;
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 15, 10, 5), Color.IndianRed, "Ka-Boom!", true, true);
				player.ClearBuff(ModContent.BuffType<Pyrite_Ignition_Status>());
				for (int i = 0; i < (int)(numberProjectiles * 1.5); i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(4));//3
					float scale = 2.15f - (Main.rand.NextFloat() * .1f);
					perturbedSpeed *= scale;
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
				shotsDone = 0;
			}
			for (int i = 0; i < numberProjectiles * 3; i++)
				Dust.NewDust(player.position, Item.width * player.direction, Item.height * player.direction, DustID.Torch, player.direction * Main.rand.NextFloat(-8.6f, - 5f), player.direction * .8f, 0, default, Main.rand.NextFloat(.8f, 1.6f));
			
			return false; // return false because we don't want tmodloader to shoot projectile
		}

		//public override void ModifyWeaponCrit(Player player, ref int crit)
		//	{
		//	if (powered)
		//	{
		//		crit = 26;
		//		powered = false;
		//	}
		//	else crit = 7;
		//}
        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

	}

}