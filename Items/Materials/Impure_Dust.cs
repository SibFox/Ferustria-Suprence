using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Materials
{
	public class Impure_Dust : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Impure Dust");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Нечистая пыль");
			Tooltip.SetDefault("Little echo from afar");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Маленький отголосок из далека");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 20;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(0, 0, 0, 55);
			Item.rare = ItemRarityID.White;
		}

	}

}
