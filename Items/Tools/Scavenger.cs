using Microsoft.Xna.Framework;
using Ferustria.Buffs.Negatives;
using Ferustria.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Tools
{
	public class Scavenger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scavenger");
			DisplayName.AddTranslation(FSHelper.RuTrans(), "Мусорщик");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 34;
			Item.height = 32;
			Item.useTime = 12;
			Item.useAnimation = 21;
			Item.pick = 85;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3.2f;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = true;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 10);
			recipe.AddIngredient(ItemID.ShadowScale, 5);
			recipe.AddIngredient(Mod, "Impure_Dust", 16);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 10);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.AddIngredient(Mod, "Impure_Dust", 16);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
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
			target.AddBuff(ModContent.BuffType<Weak_Void_Leach>(), Main.rand.Next(2, 7) * 60);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (Main.rand.NextFloat() < .1f)
			{
				for (int i = 0; i < Main.rand.Next(6); i++)
				{
					Dust.NewDust(Item.position, Item.width, Item.height, ModContent.DustType<Void_Particles>(), Main.rand.NextFloat(-.2f, .2f), Main.rand.NextFloat(-.2f, .2f), 0, default, Main.rand.NextFloat(0.45f, 0.8f));
				}
			}
		}
	}

}