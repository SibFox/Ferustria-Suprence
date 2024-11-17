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

namespace Ferustria.Content.Items.Weapons.Mage.HM
{
	public class Barathrum_Wrecker_Mage : ModItem
	{
        public override string Texture => Ferustria.Paths.TexturesPathItems + "HM/Barathrum_Wrecker";

        public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
        }

		public override void SetDefaults()
		{
			Item.damage = 72;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 56;
			Item.height = 28;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 18f;
			Item.shoot = ModContent.ProjectileType<Neon_Blast>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.25f;
			Item.UseSound = SoundID.Item91;
			Item.value = Item.sellPrice(0, 2, 95, 0);
			Item.rare = ItemRarityID.Purple;
			Item.mana = 20;
			Item.autoReuse = true;
			Item.scale *= 0.86f;
		}

		public override void AddRecipes()
		{
            RegisterRecipe.Reg([ new(ItemID.LaserRifle), new(ItemID.HallowedBar, 10), new(ItemID.SoulofMight, 10), new(ItemType<Impure_Dust>(), 12), new(ItemType<Barathrum_Extract>(), 3) ], 
                Type, tile: TileID.MythrilAnvil);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return base.Shoot(player, source, position.ReturnMuzzleOffset(velocity), velocity, type, damage, knockback);
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, -2);
		}

	}

}