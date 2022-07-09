﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Materials
{
	public class Void_Sample : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Sample");
			Tooltip.SetDefault("");
			DisplayName.AddTranslation("Russian", "Образец Пустоты");
			Tooltip.AddTranslation("Russian", "");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.maxStack = 99;
			Item.value = Item.buyPrice(0, 0, 1, 40);
			Item.rare = ItemRarityID.LightPurple;
		}

	}

}