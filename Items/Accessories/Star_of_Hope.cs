using Microsoft.Xna.Framework;
using Ferustria.Buffs.Negatives;
using Ferustria.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Accessories
{
	public class Star_of_Hope : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star of Hope"); //Со сетры Меллиссии будет выпадать Слеза Горя, которая будет объединяться с этим предметом, который будет также увеличивать наносимый урон
			DisplayName.AddTranslation(FSHelper.RuTrans, "Звезда Надежды");
			Tooltip.SetDefault("You lose 15% of your damage resistance\nBut it increases for short period every time you get hit.\nAfter getting max resistance, you become resistant to knockback.");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Сопротивление урону уменьшается на 15%\nНо с каждым полученным ударом, оно возростает на короткий промежуток.\nПри достижении максимального сопротивления, вы получаете иммунитет к отбрасыванию.");
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
			player.GetModPlayer<FSPlayer>().Accessory_StarOfHope_Equiped = true;
        }
    }

}