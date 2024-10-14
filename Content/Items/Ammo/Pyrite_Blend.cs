using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Ferustria.Content.Projectiles.Friendly;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Ammo
{
	public class Pyrite_Blend : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 12;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.knockBack = 0.5f;
			Item.value = Item.sellPrice(0, 0, 0, 7);
			Item.rare = ItemRarityID.White;
			Item.shoot = ModContent.ProjectileType<Pyrite_Shot>();
			Item.shootSpeed = 12f;
			Item.ammo = Item.type;
		}

        public override void AddRecipes()
        {
            RegisterRecipe.Reg(new CraftMaterial(ModContent.ItemType<Materials.Ore.Inactive_Pyrite>()), Type, 150);
        }
    }
}