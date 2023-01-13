﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Ferustria.Content.Items.Consumables
{
    public class Thick_Blood_Potion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanguine Flask");
            Tooltip.SetDefault("Health regeneration is increased while you're on the move");
            DisplayName.AddTranslation(FSHelper.RuTrans, "Кровавый флакон");
            Tooltip.AddTranslation(FSHelper.RuTrans, "Регенерация здоровья увеличена, пока вы двигаетесь");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

            // Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
            ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
                new Color(240, 240, 240),
                new Color(200, 200, 200),
                new Color(140, 140, 140)
            };
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.buffType = ModContent.BuffType<Buffs.Positives.Thick_Blood>(); // Specify an existing buff to be applied when used.
            Item.buffTime = 4 * 60 * 60; // The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
        }
    }
}
