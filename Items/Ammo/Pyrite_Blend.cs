using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Ferustria.Projectiles.Friendly;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Ammo
{
	public class Pyrite_Blend : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrite Blend");
			Tooltip.SetDefault("Pyrite compound that is used in pyrite weapon");
			DisplayName.AddTranslation("Russian", "Пиритовая смесь");
			Tooltip.AddTranslation("Russian", "Пиритовое соединение использующееся в пиритовом оружие");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 12;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 0.7f;
			Item.value = Item.buyPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.White;
			Item.shoot = ModContent.ProjectileType<Pyrite_Shot>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 12f;                  //The speed of the projectile
			Item.ammo = Item.type;              //The ammo class this ammo belongs to.
		}


		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MusketBall, 50);
			recipe.AddIngredient(ModContent.ItemType<ExampleItem>(), 1);
			recipe.AddTile(ModContent.TileType<ExampleWorkbench>());
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}*/
	}
}