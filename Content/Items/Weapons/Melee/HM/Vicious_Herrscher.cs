using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Weapons.Melee.HM
{
	public class Vicious_Herrscher : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 74;
			Item.height = 78;
			Item.useAnimation = 22;
			Item.useTime = 22;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3.3f;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(0, 9, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			//CreateRecipe()
			//.AddIngredient(ItemID.BreakerBlade)
			//.AddIngredient(ItemID.HallowedBar, 10)
			//.AddIngredient(ItemID.SoulofFright, 10)
			//.AddIngredient(Mod, "Impure_Dust", 10)
			//.AddIngredient(Mod, "Barathrum_Sample", 3)
			//.AddTile(TileID.MythrilAnvil)
			//.Register();
		}


    }

}