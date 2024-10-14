using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Ferustria.Content.Projectiles.Friendly;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.Items.Ammo
{
	public class Live_Compound_Tank : ModItem
	{
		public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 100;
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
			Item.value = Item.sellPrice(0, 0, 0, 18);
			Item.rare = ItemRarityID.White;
			Item.shoot = ModContent.ProjectileType<Microorganism>();
			Item.ammo = Item.type;
		}

        public override void AddRecipes()
        {
            RegisterRecipe.Reg([ new(ModContent.ItemType<Rotten_Skin>()), new(ModContent.ItemType<Impure_Dust>(), 3), new(ItemID.Bottle) ], Type, 250, TileID.AlchemyTable);
        }
    }
}