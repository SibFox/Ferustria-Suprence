using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Weapons.Melee.HM
{
	public class Vicious_Herrscher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vicious Herrscher"); //Gnadenlos
			Tooltip.SetDefault("[c/FA0000:WIP]");
            //DisplayName.AddTranslation(FSHelper.RuTrans, "Беспощадный Правитель");
            //Tooltip.AddTranslation(FSHelper.RuTrans, "\"Двуручный меч элитных королевских войск, избранных самим Королём,
            //способный разрубать, казалось бы, не разрубаемое, своим тонким лезвием заточённым в усиливающий конструкцию каркас\"");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 74;
			Item.height = 78;
			Item.useAnimation = 22;
			Item.useTime = 22;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3.3f;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(0, 9, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			//CreateRecipe()
			//.AddIngredient(ItemID.BreakerBlade)
			//.AddIngredient(ItemID.HallowedBar, 10)
			//.AddIngredient(ItemID.SoulofFright, 10)
			//.AddIngredient(Mod, "Impure_Dust", 10)
			//.AddIngredient(Mod, "Void_Sample", 3)
			//.AddTile(TileID.MythrilAnvil)
			//.Register();
		}

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
			
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			
        }

    }

}