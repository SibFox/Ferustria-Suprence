using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Statuses;
using Ferustria.Content.Projectiles.Friendly;
using Ferustria.Content.Items.Ammo;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Ore;

namespace Ferustria.Content.Items.Weapons.Ranger.HM
{
	public class Pyrite_Shotgun : ModItem
	{
		int shotsDone;
		bool powered;
        int definedDamage;
		public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = definedDamage = 105;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.crit = 7;
			Item.width = 66;
			Item.height = 28;
			Item.useTime = 58;
			Item.useAnimation = 58;
			Item.shootSpeed = 14.5f;
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


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
		{
			int numberProjectiles = 3 + Main.rand.Next(4);
			if (!player.HasBuff(ModContent.BuffType<Pyrite_Ignition_Status>())) shotsDone = 0;
			player.AddBuff(ModContent.BuffType<Pyrite_Ignition_Status>(), 5*60);

            Players.FSSpesialWeaponsPlayer weaponManager = player.GetModPlayer<Players.FSSpesialWeaponsPlayer>(); 
			
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 10f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			if (shotsDone < 5)
            {
				shotsDone++;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(weaponManager.Accessory_PMachinegun_Enchanser_Equiped ? 12.5f : 16.4f));
					float scale = 1.35f - (Main.rand.NextFloat() * .35f);
					perturbedSpeed *= scale;
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
				if (shotsDone >= 4 && player.HasBuff(ModContent.BuffType<Pyrite_Ignition_Status>())) 
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 10, 10, 2), Color.Yellow, (6 - shotsDone).ToString() + "!", true, true);
				SoundEngine.PlaySound(SoundID.Item38, player.position);
			}
            else if (shotsDone >= 5 && player.HasBuff(ModContent.BuffType<Pyrite_Ignition_Status>()))
            {
				SoundEngine.PlaySound(SoundID.Item88.WithPitchOffset(-0.2f).WithVolumeScale(1.2f), player.position);
				powered = true;
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 15, 10, 5), Color.IndianRed, "Ka-Boom!", true, false);
				player.ClearBuff(ModContent.BuffType<Pyrite_Ignition_Status>());
				for (int i = 0; i < (int)(numberProjectiles * 1.5); i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(3.5f));
					float scale = 1.65f - (Main.rand.NextFloat() * .35f);
					perturbedSpeed *= scale;
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
				shotsDone = 0;
			}
            int widthPos = Item.width;
            if (player.direction == -1) widthPos -= widthPos * 2;
			for (int i = 0; i < numberProjectiles * 3; i++)
				Dust.NewDust(player.position, widthPos, Item.height, DustID.Torch, player.direction * Main.rand.NextFloat(-8.6f, - 5f), player.direction * .8f, 0, default, Main.rand.NextFloat(.8f, 1.6f));
			
			return false;
		}

        public override void UpdateInventory(Player player)
        {
            if (!player.HasBuff<Pyrite_Ignition_Status>()) shotsDone = 0;
            if (shotsDone >= 5)
            {
                Item.crit = 24;
            }
            else Item.crit = 6;
            Item.damage = shotsDone != 0 ? 105 + shotsDone * 8 : 105;
            
        }

        public override void AddRecipes()
        {
            RegisterRecipe.Reg([ new(ItemID.TitaniumBar, 8), new(ItemID.HallowedBar, 14), new(ModContent.ItemType<Inactive_Pyrite>(), 16), new(ItemID.IllegalGunParts) ], Type, tile: TileID.MythrilAnvil);
            RegisterRecipe.Reg([ new(ItemID.AdamantiteBar, 8), new(ItemID.HallowedBar, 14), new(ModContent.ItemType<Inactive_Pyrite>(), 16), new(ItemID.IllegalGunParts) ], Type, tile: TileID.MythrilAnvil);
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

	}

}