using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using Ferustria.Content.Buffs.Negatives;

namespace Ferustria.Content.Items.Weapons.Melee.PreHM
{
	public class Kanabo : ModItem
	{
		public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 28;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 62;
			Item.height = 62;
			Item.useTime = 33;
			Item.useAnimation = 33;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6.2f;
			Item.value = Item.sellPrice(0, 0, 33, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = false;
			Item.autoReuse = false;
		}

		public override void AddRecipes()
		{
            RegisterRecipe.Reg([new(ItemID.DemoniteBar, 8), new(ItemID.Ebonwood, 25) ], Type, tile: TileID.DemonAltar);
            RegisterRecipe.Reg([ new(ItemID.CrimtaneBar, 8), new(ItemID.Shadewood, 25) ], Type, tile: TileID.DemonAltar);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(ModContent.BuffType<Shattered_Armor>(), Main.rand.Next(12, 16) * 60);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
			target.AddBuff(ModContent.BuffType<Shattered_Armor>(), Main.rand.Next(12, 16) * 60);
		}
    }

}