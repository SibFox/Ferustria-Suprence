using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Drop;
using Terraria.Localization;

namespace Ferustria.Content.Items.Armor
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class Rotten_Leggings : ModItem
	{
        public LocalizedText RottenLeggings => this.GetLocalization(nameof(RottenLeggings));

        public override void SetStaticDefaults()
        {
            _ = RottenLeggings;
            Item.ResearchUnlockCount = 1;
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
        {
			Item.width = 26;
			Item.height = 20;
            Item.value = Item.sellPrice(silver: 55);
			Item.rare = ItemRarityID.Green;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
        {
			player.moveSpeed -= 0.05f; // Increase the movement speed of the player
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Rotten_Skin>(4)
                .AddIngredient(ItemID.Bone, 55)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
}
