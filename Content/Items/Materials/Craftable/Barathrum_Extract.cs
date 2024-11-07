using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.Items.Materials.Craftable
{
    public class Barathrum_Extract : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            RegisterRecipe.Reg([ new(ItemType<Barathrum_Sample>()), new(ItemID.Bottle), new(ItemID.PixieDust, 10), new(ItemID.PurificationPowder, 5) ], Type, tile: TileID.AlchemyTable);
        }

    }

}
