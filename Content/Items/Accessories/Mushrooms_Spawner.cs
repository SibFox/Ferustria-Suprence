using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Accessories
{
	public class Mushrooms_Spawner : ModItem
	{
        public override string Texture => "Ferustria/Content/Items/Accessories/Pyrite_Cooler";

        public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.accessory = true;
			Item.width = 12;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().Accessory_PMachinegun_Enchanser_Equiped = true;
        }
    }

}