using Microsoft.Xna.Framework;
using Ferustria.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using System.Collections.Generic;

namespace Ferustria.Items.Weapons.Melee
{
	public class Kanabo : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kanabo");
			Tooltip.SetDefault("Crushes foes defense.");
			DisplayName.AddTranslation("Russian", "Канабо");
			Tooltip.AddTranslation("Russian", "Рушит броню врагов.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 28;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 62;
			Item.height = 62;
			Item.useTime = 37;
			Item.useAnimation = 37;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6.2f;
			Item.value = Item.buyPrice(0, 0, 36, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = false;
			Item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 8);
			recipe.AddIngredient(ItemID.Ebonwood , 25);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 8);
			recipe.AddIngredient(ItemID.Shadewood, 25);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}

		List<NPC> npcs = new List<NPC>() { new NPC() };

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
            double double1 = target.defDefense;
            int getMinuser = Convert.ToInt32(Math.Round(double1 / 2.0));
            if (getMinuser > 30) getMinuser = Convert.ToInt32(double1 - 20.0);
            target.defense = getMinuser;

        }

        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
			target.AddBuff(BuffID.BrokenArmor, 20 * 60);
        }
    }

}