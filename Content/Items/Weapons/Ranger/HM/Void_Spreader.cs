using Microsoft.Xna.Framework;
using Ferustria.Content.Projectiles.Friendly;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Drop;
using static Terraria.ModLoader.ModContent;
using Ferustria.Content.Items.Materials.Craftable;

namespace Ferustria.Content.Items.Weapons.Ranger.HM
{
	public class Void_Spreader : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Spreader");
			Tooltip.SetDefault("Flames of the Void. Sets enemies onto void fire.\n25% chanse to not consume gel.");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Распростронитель Пустоты");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Пламя Пустоты. Поджигает врагов пустотным пламенем.\n25% шанс не потратить гель.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 56;
			Item.height = 28;
			Item.useAnimation = 40;
			Item.useTime = 8;
			Item.shootSpeed = 14.5f;
			Item.shoot = ModContent.ProjectileType<Void_Spreader_Flame>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1f;
			Item.UseSound = SoundID.Item74;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Purple;
			Item.autoReuse = true;
            Item.useAmmo = AmmoID.Gel;
            Item.consumeAmmoOnFirstShotOnly = true;
		}

		public override void AddRecipes()
		{
            _ = new RegisterRecipe(new CraftMaterial[]
            { new CraftMaterial(ItemID.IllegalGunParts), new(ItemID.HallowedBar, 15), new(ItemID.SoulofSight, 10), new(ItemType<Impure_Dust>(), 15), new(ItemType<Void_Extract>(), 3)
            }, Type, tile: TileID.MythrilAnvil);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3.75f));
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() > .25f;
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 1.5f);
		}

	}

}