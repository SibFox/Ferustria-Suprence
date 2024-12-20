﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Materials.Drop
{
    public class Impure_Dust : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 30);
            Item.rare = ItemRarityID.White;
        }
    }
}
