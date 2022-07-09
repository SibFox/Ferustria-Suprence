using Microsoft.Xna.Framework;
using Ferustria.Buffs.Negatives;
using Ferustria.Dusts;
using Ferustria.Projectiles.Friendly;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;

namespace Ferustria.Items.Tools
{
	public class Unfinished_Guzzler : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unfinished Guzzler");
			Tooltip.SetDefault("'This thing weights over 50 killogramms'" +
				"\nRight Click to shoot void projectile that splits into smaller projectiles.");
			DisplayName.AddTranslation("Russian", "Незавершённый Пожиратель");
			Tooltip.AddTranslation("Russian", "'Эта штука весит около 50 киллограмм'\nНажмите ПКМ, чтобы выстрелить пустотный снаряд, который разделяется на кучку маленьких.");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 42;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 18;
			Item.useAnimation = 58;
			Item.axe = 21;
			Item.hammer = 74;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 8.3f;
			Item.value = Item.buyPrice(0, 0, 46, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = false;
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
				Item.damage = 24;
				Item.noMelee = true;
				Item.DamageType = DamageClass.Magic;
				Item.useTime = 36;
				Item.useAnimation = 36;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.knockBack = 3f;
				Item.UseSound = SoundID.Item20;
				Item.autoReuse = false;
				Item.shoot = ModContent.ProjectileType<Void_Opposite_Bounce>();
				Item.shootSpeed = 11.8f;
				Item.mana = 12;
			}
            else
            {
				Item.damage = 42;
				Item.DamageType = DamageClass.Melee;
				Item.noMelee = false;
				Item.useTime = 16;
				Item.useAnimation = 58;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.knockBack = 8.3f;
				Item.UseSound = SoundID.Item1;
				Item.autoReuse = true;
				Item.shootSpeed = 0f;
				Item.shoot = 0;
				Item.mana = 0;
			}
			return base.CanUseItem(player);
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.WarAxeoftheNight);
			recipe.AddIngredient(ItemID.TheBreaker);
			recipe.AddIngredient(Mod, "Impure_Dust", 19);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BloodLustCluster);
			recipe.AddIngredient(ItemID.FleshGrinder);
			recipe.AddIngredient(Mod, "Impure_Dust", 19);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextFloat() < 0.65f)
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height / 2, ModContent.DustType<Void_Particles>(), player.direction / 4, -0.34f, 0, default, Main.rand.NextFloat(0.52f, 0.84f));
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