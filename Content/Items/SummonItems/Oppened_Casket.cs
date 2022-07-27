using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using System.Collections.Generic;

namespace Ferustria.Content.Items.Materials
{
	public class Oppened_Casket : ModItem
	{
        public override string Texture => "Ferustria/Content/Items/Materials/Drop/Void_Sample";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oppend Casket");
			//DisplayName.AddTranslation(FSHelper.RuTrans, "Нечистая пыль");
			//Tooltip.SetDefault("Little echo from afar");
			//Tooltip.AddTranslation(FSHelper.RuTrans, "Маленький отголосок из далека");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 20;
			Item.maxStack = 99;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.rare = ItemRarityID.White;
		}

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                NPC.NewNPC(Item.GetSource_ItemUse(Item, "Summoned"), (int)player.position.X + player.width / 2, (int)player.position.Y + player.height,
                    ModContent.NPCType<Content.NPCs.Bosses.HM.SixWingedSeraphBoss.Six_Winged_Seraph_Boss>(), Target: player.whoAmI);
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return NPC.CountNPCS(ModContent.NPCType<NPCs.Bosses.HM.SixWingedSeraphBoss.Six_Winged_Seraph_Boss>()) < 1;
        }
    }
}
