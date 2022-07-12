using Microsoft.Xna.Framework;
using Ferustria.Projectiles.Friendly;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Weapons.Melee.HM
{
	public class Void_Pruner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Pruner");
			Tooltip.SetDefault("Giant sword that cuts enemies defence with its neon blade.\nDeals more damage to unhurt enemies.");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Секатор Пустоты");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Гигантсикй меч, который разрезает броню врагу своим неоновым клинком.\nНаносит больше урона неповреждённым врагам");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 104;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 74;
			Item.height = 78;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 7.28f;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(0, 4, 35, 0);
			Item.rare = ItemRarityID.Purple;
			Item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.BreakerBlade)
			.AddIngredient(ItemID.HallowedBar, 10)
			.AddIngredient(ItemID.SoulofFright, 10)
			.AddIngredient(Mod, "Impure_Dust", 10)
			.AddIngredient(Mod, "Void_Sample", 3)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		}

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
			if (target.life >= target.lifeMax) damage = (int)(damage * 2.5);
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			
        }

    }

}