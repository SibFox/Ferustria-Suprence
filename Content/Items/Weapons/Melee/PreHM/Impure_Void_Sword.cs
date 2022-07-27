using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.Items.Weapons.Melee.PreHM
{
	public class Impure_Void_Sword : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Impure Void Sword");
			Tooltip.SetDefault("Exhausts your enemies");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		//DisplayName.AddTranslation(FSHelper.RuTrans, "Неочищенный Меч Пустоты");
		//Tooltip.AddTranslation(FSHelper.RuTrans, "Истощает ваших врагов");

		public override void SetDefaults() 
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 3;
			Item.width = 42;
			Item.height = 42;
			Item.scale *= 1.2f;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4.5f;
			Item.value = Item.sellPrice(0, 0, 42, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

        public override void AddRecipes()
        {
            _ = new RegisterRecipe(new CraftMaterial[]
            { new(ItemID.DemoniteBar, 10), new(ModContent.ItemType<Impure_Dust>(), 16)
            }, Type, tile: TileID.DemonAltar);
            _ = new RegisterRecipe(new CraftMaterial[]
            { new(ItemID.CrimtaneBar, 10), new(ModContent.ItemType<Impure_Dust>(), 16)
            }, Type, tile: TileID.DemonAltar);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextFloat() < .38f)
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Void_Particles>(), player.direction / 4, -0.3f, 0, default, Main.rand.NextFloat(0.45f, 0.8f));
			}
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			target.AddBuff(ModContent.BuffType<Weak_Void_Leach>(), Main.rand.Next(4, 8) * 60);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (Main.rand.NextFloat() < .07f)
            {
				for (int i = 0; i < Main.rand.Next(6); i++)
                {
					Dust.NewDust(Item.position, Item.width, Item.height, ModContent.DustType<Void_Particles>(), Main.rand.NextFloat(-.2f, .2f), Main.rand.NextFloat(-.2f, .2f), 0, default, Main.rand.NextFloat(0.45f, 0.8f));
                }
            }
        }
    }

}