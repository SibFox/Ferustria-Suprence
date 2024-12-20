﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Materials.Ore
{
    public class Inactive_Pyrite : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 8, 65);
            Item.rare = ItemRarityID.Orange;
        }
    }
}
