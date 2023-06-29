using Ferustria.Content.Items.Materials.Craftable;
using Ferustria.Content.Items.Materials.Drop;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class Rotten_Chest_Cover : ModItem
	{
		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Condensed Rotten Chest Cover");
            Tooltip.SetDefault("Decreases taken damage by 5%");
            DisplayName.AddTranslation(FSHelper.RuTrans, "Уплотнённый Гнилой Грудной Каркас");
            Tooltip.AddTranslation(FSHelper.RuTrans, "Уменьшает получаемый урон на 5%");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = ItemRarityID.Green;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player) {
            player.endurance += 0.05f;
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Reinforced_Living_Fiber_Tube>(2)
                .AddIngredient<Rotten_Skin>(3)
                .AddIngredient(ItemID.Bone, 65)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
