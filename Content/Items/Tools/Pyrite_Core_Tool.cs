﻿using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Tools
{
	public class Pyrite_Core_Tool : ModItem
	{
        public override string Texture => "Ferustria/Content/Items/Tools/Scavenger";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miltitool with Pyrite Core");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Мультитул с Пиритовым сердечником");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 42;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 34;
			Item.height = 32;
			Item.useTime = 6;
			Item.useAnimation = 22;
            Item.axe = 23;
			Item.pick = 205;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3.2f;
			Item.value = Item.sellPrice(0, 4, 60, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = true;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
            _ = new RegisterRecipe(new CraftMaterial[] { new CraftMaterial(ModContent.ItemType<Materials.Ore.Inactive_Pyrite>(), 16), new CraftMaterial(ItemID.HallowedBar, 6) },
                Type, tile: TileID.MythrilAnvil);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextFloat() < 0.75f)
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height / 3, ModContent.DustType<Void_Particles>(), player.direction / 4, -0.34f, 0, default, Main.rand.NextFloat(0.4f, 0.78f));
			}
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			target.AddBuff(BuffID.OnFire3, Main.rand.Next(2, 7) * 60);
		}


	}

}