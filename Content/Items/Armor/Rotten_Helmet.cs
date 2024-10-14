using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Ferustria.Players;
using Terraria.Localization;
using Ferustria.Content.Items.Materials.Craftable;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class Rotten_Helmet : ModItem
	{
        public LocalizedText RottenHelmet => this.GetLocalization(nameof(RottenHelmet));

        public static LocalizedText RottenArmorSetBonus { get; private set; }

        public override void SetStaticDefaults()
        {
            _ = RottenHelmet;
            Item.ResearchUnlockCount = 1;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;

            // If your head equipment should draw hair while drawn, use one of the following:
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			// ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			// ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			ArmorIDs.Head.Sets.DrawBackHair[Item.headSlot] = true;
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true; 
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 90);
			Item.rare = ItemRarityID.Green;
			Item.defense = 7;
		}

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 7;
        }

        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
        public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<Rotten_Chest_Cover>() && legs.type == ModContent.ItemType<Rotten_Leggings>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
            player.endurance += 0.1f;
            player.GetModPlayer<FSSpecialArmorPlayer>().RottenArmor_SetBonus = true;
            string tip = "Every time you get damage you gain additional defence and life regen.\n" +
                "Cannot exceed 30% of your initial defence.\n" +
                "Additionaly reduces taken damage by 10%.";
            if (LanguageManager.Instance.ActiveCulture == FSHelper.RuTrans)
                tip = "Каждый раз, как вы полчаете урон, ваша защита и регенерация возрастает.\n" +
                    "Максимум до дополнительных 30% от изначальной защиты.\n" +
                    "Дополнительно уменьшает получаемый урон на 10%.";
                player.setBonus = tip; // This is the setbonus tooltip
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Reinforced_Living_Fiber_Tube>()
                .AddIngredient<Rotten_Skin>(3)
                .AddIngredient(ItemID.Bone, 40)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
