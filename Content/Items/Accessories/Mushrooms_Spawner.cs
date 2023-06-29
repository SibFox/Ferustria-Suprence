using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Accessories
{
	public class Mushrooms_Spawner : ModItem
	{
        public override string Texture => "Ferustria/Content/Items/Accessories/Pyrite_Cooler";

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mushrooms Spawner");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Грибной Рассадник");
			Tooltip.SetDefault("Enchanses Pyrite gear.\n" +
                "Increases amount of time until machinegun overheat, and overheat damage increased.\n" +
                "Shotgun shoot speed increased, and shot spread decreased");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Усиливает Пиритовое вооружение.\n" +
                "Колличество выстрелов перед перегревом пулемёта сильно увеличено и урон от перегрева увеличен.\n" +
                "Скорость стрельбы дробовика увеличена, а разброс уменьшен.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.accessory = true;
			Item.width = 12;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().Accessory_PMachinegun_Enchanser_Equiped = true;
        }
    }

}