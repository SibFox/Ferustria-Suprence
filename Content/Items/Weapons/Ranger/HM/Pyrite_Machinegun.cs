using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Statuses;
using Ferustria.Content.Projectiles.Friendly;
using Ferustria.Content.Items.Ammo;
using Ferustria.Content.Items.Materials.Ore;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Weapons.Ranger.HM
{
	public class Pyrite_Machinegun : ModItem
	{
		float angle;
        Players.FSSpesialWeaponsPlayer weaponManager = null;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrite Machinegun");
			Tooltip.SetDefault("Accuracy improves over time.\nWarms up for more damage. Beware of overheat!\n50% chance not to consume ammo");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Пиритовый Пулемёт");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Точность повышается со временем\nРазогревается для нанесения большего урона. Остерегайтесь перегрева!\n50% шанс не потратить боеприпас");

            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
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
			Item.useTime = 3;
			Item.useAnimation = 3;
			Item.shootSpeed = 20f;
			Item.shoot = ModContent.ProjectileType<Pyrite_Shot>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0.8f;
			Item.value = Item.sellPrice(0, 4, 10, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item11;
			Item.useAmmo = ModContent.ItemType<Pyrite_Blend>();
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 10f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
            weaponManager = player.GetModPlayer<Players.FSSpesialWeaponsPlayer>();
            int alert = weaponManager.PMachinegun_MaximumShots - (weaponManager.PMachinegun_MaximumShots - weaponManager.PMachinegun_WarningShots) / 2;

            if (weaponManager.PMachinegun_MaxShotDelay > 20) weaponManager.PMachinegun_MaxShotDelay = (int)(weaponManager.PMachinegun_MaxShotDelay / 1.25);
            else weaponManager.PMachinegun_MaxShotDelay -= 1;
            if (weaponManager.PMachinegun_MaxShotDelay < (weaponManager.Accessory_PMachinegun_Enchanser_Equiped ? 3 : 4)) weaponManager.PMachinegun_MaxShotDelay = weaponManager.Accessory_PMachinegun_Enchanser_Equiped ? 3 : 4;
            weaponManager.PMachinegun_ShotDelay = weaponManager.PMachinegun_MaxShotDelay;

            if (weaponManager.PMachinegun_ShotsDone <= weaponManager.PMachinegun_WarningShots && player.HasBuff(ModContent.BuffType<Pyrite_Overheating_Status>()))
			{
                weaponManager.PMachinegun_ShotsDone++;

                if (weaponManager.PMachinegun_ShotsDone >= 5) angle = 22 / weaponManager.PMachinegun_ShotsDone / 4.5f;
				else angle = 22;
				if (angle < 2) angle = 2;
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(angle));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			if (weaponManager.PMachinegun_ShotsDone > weaponManager.PMachinegun_WarningShots && player.HasBuff(ModContent.BuffType<Pyrite_Overheating_Status>()))
            {
				weaponManager.PMachinegun_ShotsDone++;
				if (weaponManager.PMachinegun_ShotsDone % 4 == 0) CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 5, 20, 20), Color.DarkRed, "!!!", weaponManager.PMachinegun_ShotsDone >= alert ? true : false, false);
				if (angle < 26 && weaponManager.Accessory_PMachinegun_Enchanser_Equiped) angle += 2;
				else if (angle < 28 && !weaponManager.Accessory_PMachinegun_Enchanser_Equiped) angle += 1;
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(angle));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)(damage * 1.2), knockBack, player.whoAmI);
				player.AddBuff(ModContent.BuffType<Pyrite_Overheating_Status>(), 3 * 70);
			}
			if (weaponManager.PMachinegun_ShotsDone >= weaponManager.PMachinegun_MaximumShots && player.HasBuff(ModContent.BuffType<Pyrite_Overheating_Status>())) 
			{
				for (int i = 0; i < 6 + Main.rand.Next(5); i++)
                {
					Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(15));
					perturbedSpeed *= 1.5f - Main.rand.NextFloat() * .25f;
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)(damage * 1.8), knockBack, player.whoAmI);
					Dust.NewDust(player.position, Item.width * player.direction, Item.height, DustID.Torch, player.direction * Main.rand.NextFloat(-8.6f, -5f), player.direction * .8f, 0, default, Main.rand.NextFloat(.8f, 1.6f)); ;
				}
				if (Main.rand.NextBool()) SoundEngine.PlaySound(SoundID.Item74, player.position);
				else SoundEngine.PlaySound(SoundID.Item88, player.position);
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 5, 20, 20), Color.Red, "Overheat!", true, true);
				player.ClearBuff(ModContent.BuffType<Pyrite_Overheating_Status>());
				player.AddBuff(ModContent.BuffType<Pyrite_Overheat_Status>(), 3 * 70);
			}
            int widthPos = Item.width;
            if (player.direction == -1) widthPos -= widthPos * 2;
            Dust.NewDust(player.position, widthPos, Item.height, DustID.Torch, player.direction * Main.rand.NextFloat(-8.6f, -5f), player.direction * .8f, 0, default, Main.rand.NextFloat(.8f, 1.6f));
			return false;
		}

        public override void UpdateInventory(Player player)
        {
            weaponManager = player.GetModPlayer<Players.FSSpesialWeaponsPlayer>();
            if (weaponManager.PMachinegun_ShotsDone >= weaponManager.PMachinegun_WarningShots) Item.UseSound = SoundID.Item108;
            else Item.UseSound = SoundID.Item11;

            if (weaponManager.PMachinegun_ShotsDone == 0) angle = 22;

            if (weaponManager.PMachinegun_ShotsDone <= 10) Item.damage = 40;
            if (weaponManager.PMachinegun_ShotsDone > 10) Item.damage = 40 + (weaponManager.PMachinegun_ShotsDone / 8);
            if (Item.damage >= 40 * 1.4) Item.damage = 40 + (int)(40 * 0.4) + (weaponManager.PMachinegun_ShotsDone / 14);
            if (Item.damage >= 40 * 1.85) Item.damage = (int)(40 * 0.85);
        }

        public override bool CanUseItem(Player player)
        {
            weaponManager = player.GetModPlayer<Players.FSSpesialWeaponsPlayer>();
            return weaponManager.PMachinegun_ShotDelay <= 0;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (weaponManager.PMachinegun_ShotsDone < weaponManager.PMachinegun_MaximumShots) SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
                player.AddBuff(ModContent.BuffType<Pyrite_Overheating_Status>(), 2 * 70);
            }
            return null;
        }

        public override void AddRecipes()
        {
            FSHelper.CreateRecipe(new CraftMaterial[]
            { new CraftMaterial(ItemID.TitaniumBar, 8), new CraftMaterial(ItemID.HallowedBar, 14), new CraftMaterial(ModContent.ItemType<Inactive_Pyrite>(), 16)
            }, Type, tile: TileID.MythrilAnvil);
            FSHelper.CreateRecipe(new CraftMaterial[]
            { new CraftMaterial(ItemID.AdamantiteBar, 8), new CraftMaterial(ItemID.HallowedBar, 14), new CraftMaterial(ModContent.ItemType<Inactive_Pyrite>(), 16)
            }, Type, tile: TileID.MythrilAnvil);

        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
			return Main.rand.NextFloat() <= .5f;
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -1);
		}
	}

}