﻿using Terraria;
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
            Item.ResearchUnlockCount = 25;
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
            RegisterRecipe.Reg([ new(ModContent.ItemType<Rotten_Skin>(), 3), new(ItemID.Cobweb, 10), new(ItemID.Wire, 2), new(ItemID.Bone, 6) ], Type, 1, TileID.Loom);
        }

    }

}
