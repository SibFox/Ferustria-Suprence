using Microsoft.Xna.Framework;
using Ferustria.Content.Projectiles.Friendly;
using Ferustria.Content.Buffs.Negatives;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Drop;
using static Terraria.ModLoader.ModContent;

namespace Ferustria.Content.Items.Weapons.Melee.HM
{
	public class Void_Pruner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Pruner");
			Tooltip.SetDefault("Giant sword that cuts enemies defence with its neon blade.\nDeals more damage to unhurt enemies.");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Секатор Пустоты");
			Tooltip.AddTranslation(FSHelper.RuTrans, "Гигантсикй меч, который разрезает броню врагу своим неоновым клинком.\nНаносит больше урона неповреждённым врагам");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 104;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 74;
			Item.height = 78;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 7.28f;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(0, 4, 35, 0);
			Item.rare = ItemRarityID.Purple;
			Item.autoReuse = false;
		}

		public override void AddRecipes()
		{
            _ = new RegisterRecipe(new CraftMaterial[]
            { new(ItemID.BreakerBlade), new(ItemID.HallowedBar, 10), new(ItemID.SoulofFright, 10), new(ItemType<Impure_Dust>(), 12), new(ItemType<Void_Sample>(), 3)
            }, Type, tile: TileID.MythrilAnvil);
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
			if (target.life >= target.lifeMax) damage = (int)(damage * 2.5);
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Sliced_Defense>(), 180);
        }

    }

}