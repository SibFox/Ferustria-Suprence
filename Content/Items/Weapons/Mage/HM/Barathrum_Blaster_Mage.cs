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
	public class Barathrum_Blaster_Mage : ModItem
	{
        public override string Texture => Ferustria.Paths.TexturesPathItems + "HM/Barathrum_Blaster";

        public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.crit = 0;
			Item.width = 68;
			Item.height = 26;
			Item.useAnimation = 12;
			Item.useTime = 3;
			Item.reuseDelay = 14;
			Item.shootSpeed = 22f;
			Item.shoot = ProjectileType<Neon_Laser>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.25f;
			Item.UseSound = SoundID.Item33;
			Item.value = Item.sellPrice(0, 2, 65, 0);
			Item.rare = ItemRarityID.Purple;
			Item.mana = 9;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
            RegisterRecipe.Reg([ new(ItemID.ClockworkAssaultRifle), new(ItemID.HallowedBar, 10), new(ItemID.SoulofSight, 10), new(ItemType<Impure_Dust>(), 12), new(ItemType<Barathrum_Extract>(), 3)],
                                 Type, tile: TileID.MythrilAnvil);
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position.ApplyMuzzleOffset(velocity);
        }


		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-15, 0);
		}

	}

}