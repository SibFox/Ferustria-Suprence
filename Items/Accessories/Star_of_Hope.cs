using Microsoft.Xna.Framework;
using Ferustria.Buffs.Negatives;
using Ferustria.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Accessories
{
	public class Star_of_Hope : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star of Hope"); //Со сетры Меллиссии будет выпадать Слеза Горя, которая будет объединяться с этим предметом, который будет также увеличивать наносимый урон
			DisplayName.AddTranslation("Russian", "Звезда Надежды");
			Tooltip.SetDefault("You lose 15% of your damage resistance\nBut it increases for short period every time you get hit.\nAfter getting max resistance, you become resistant to knockback.");
			Tooltip.AddTranslation("Russian", "Сопротивление урону уменьшается на 15%\nНо с каждым полученным ударом, оно возростает на короткий промежуток.\nПри достижении максимального сопротивления, вы получаете иммунитет к отбрасыванию.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.accessory = true;
			Item.width = 38;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
		}

        /*public override void AddRecipes() //Будет выпадать из Меллиссии как эксперт предмет
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 10);
			recipe.AddIngredient(ItemID.ShadowScale, 5);
			recipe.AddIngredient(mod, "Void_Dust", 16);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			FerustriaPlayer modPlayer = player.GetModPlayer<FerustriaPlayer>();
			modPlayer.Resist_Accs_Equiped = true;
        }
    }

}