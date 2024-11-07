using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Materials.Drop
{
    public class Barathrum_Sample : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Barathrum Sample");
            //Tooltip.SetDefault("");
            //DisplayName.AddTranslation(FSHelper.RuTrans, "Образец Пустоты");
            //Tooltip.AddTranslation(FSHelper.RuTrans, "");
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 4, 20);
            Item.rare = ItemRarityID.LightPurple;
        }

    }

}
