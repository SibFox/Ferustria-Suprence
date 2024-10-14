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
            //Со сетры Меллиссии будет выпадать Слеза Горя, которая будет объединяться с этим предметом, который будет также увеличивать наносимый урон
            Item.ResearchUnlockCount = 1;
        }

		public override void SetDefaults()
		{
			Item.accessory = true;
			Item.width = 38;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.GetModPlayer<Players.FSPlayer>().Acc_StarOfHope_Equiped = true;
        }
    }

}