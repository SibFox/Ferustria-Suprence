using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Accessories
{
	public class Star_of_Hope : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star of Hope"); //Со сетры Меллиссии будет выпадать Слеза Горя, которая будет объединяться с этим предметом, который будет также увеличивать наносимый урон
			DisplayName.AddTranslation(FSHelper.RuTrans, "Звезда Надежды");
			Tooltip.SetDefault("You lose 15% of your damage resistance\n" +
                "But it increases for short period every time you get hit.\n" +
                "After getting max resistance, you become resistant to knockback.");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Сопротивление урону уменьшается на 15%\n" +
                "Но с каждым полученным ударом, оно возростает на короткий промежуток.\n" +
                "При достижении максимального сопротивления, вы получаете иммунитет к отбрасыванию.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.accessory = true;
			Item.width = 38;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Expert; //Будет выпадать из Меллиссии как эксперт предмет
			Item.expert = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.GetModPlayer<Players.FSPlayer>().Acc_StarOfHope_Equiped = true;
        }
    }

}