using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.Items.Materials.Craftable
{
    public class Reinforced_Living_Fiber_Tube : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reinforced living fiber Tube");
            Tooltip.SetDefault("Surprisingly, incredibly solid thing.");
            DisplayName.AddTranslation(FSHelper.RuTrans, "Укреплённая Трубка из живого волокна");
            Tooltip.AddTranslation(FSHelper.RuTrans, "На удивление, невероятно плотная штука.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 1, 65);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            _ = new RegisterRecipe(new CraftMaterial[]
            { new(ModContent.ItemType<Rotten_Skin>(), 3), new(ItemID.Cobweb, 10), new(ItemID.Wire, 2), new(ItemID.Bone, 6)
            }, Type, tile: TileID.Loom);
        }

    }

}
