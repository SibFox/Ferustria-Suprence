using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.Items.Materials.Craftable
{
    public class Void_Extract : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Extract");
            Tooltip.SetDefault("Concentrated Void matter.\n" +
                "You shouldn't drink that...");
            DisplayName.AddTranslation(FSHelper.RuTrans, "Экстракт Пустоты");
            Tooltip.AddTranslation(FSHelper.RuTrans, "Концентрированная материя Пустоты.\n" +
                "Не стоит это пить...");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 3, 0);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            _ = new RegisterRecipe(new CraftMaterial[]
            { new(ItemType<Void_Sample>()), new(ItemID.Bottle), new(ItemID.PixieDust, 10), new(ItemID.PurificationPowder, 5)
            }, Type, tile: TileID.AlchemyTable);
        }

    }

}
