using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Materials
{
	public class Rotten_Skin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rotten Skin");
			Tooltip.SetDefault("Disgusting thing");
			DisplayName.AddTranslation("Russian", "Гнилая кожа");
			Tooltip.AddTranslation("Russian", "Мерзкая штука");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.maxStack = 999;
			Item.value = Item.buyPrice(0, 0, 0, 40);
			Item.rare = ItemRarityID.White;
		}

	}

}
