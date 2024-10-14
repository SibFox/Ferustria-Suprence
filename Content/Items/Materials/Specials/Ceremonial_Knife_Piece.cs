using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Materials.Specials
{
    public class Ceremonial_Knife_Piece : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 26;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.rare = ItemRarityID.Blue;
        }

    }

}
