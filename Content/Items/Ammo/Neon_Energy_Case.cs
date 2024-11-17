using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Ferustria.Content.Projectiles.Friendly;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.Items.Ammo
{
	public class Neon_Energy_Case : ModItem
	{
		public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 100;
        }

		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 12;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.knockBack = 0.2f;
			Item.value = Item.sellPrice(0, 0, 0, 24);
			Item.rare = ItemRarityID.White;
			Item.shoot = ModContent.ProjectileType<Neon_Laser>();
			Item.ammo = Item.type;
		}

        public override void AddRecipes()
        {
            CreateRecipe(250).
                AddIngredient<Impure_Dust>(3).
                AddIngredient(ItemID.SoulofNight).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}