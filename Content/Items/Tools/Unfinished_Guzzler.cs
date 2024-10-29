using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Dusts;
using Ferustria.Content.Projectiles.Friendly;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Drop;

namespace Ferustria.Content.Items.Tools
{
	public class Unfinished_Guzzler : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
			Item.ResearchUnlockCount = 1;
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
			Item.hammer = 65;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 8.3f;
			Item.value = Item.sellPrice(0, 0, 46, 0);
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
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.damage = 24;
				Item.noMelee = true;
				Item.DamageType = DamageClass.Magic;
				Item.useTime = 36;
				Item.useAnimation = 36;
				Item.knockBack = 3f;
				Item.UseSound = SoundID.Item20;
				Item.autoReuse = false;
				Item.shoot = ModContent.ProjectileType<Barathrum_Opposite_Bounce>();
				Item.shootSpeed = 11.8f;
				Item.mana = 12;
			}
            else
            {
				Item.useStyle = ItemUseStyleID.Swing;
				Item.damage = 42;
				Item.DamageType = DamageClass.Melee;
				Item.noMelee = false;
				Item.useTime = 16;
				Item.useAnimation = 58;
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
            RegisterRecipe.Reg([ new(ItemID.WarAxeoftheNight), new(ItemID.TheBreaker), new(ModContent.ItemType<Impure_Dust>(), 10) ], Type, tile: TileID.DemonAltar);
            RegisterRecipe.Reg([ new(ItemID.BloodLustCluster), new(ItemID.FleshGrinder), new(ModContent.ItemType<Impure_Dust>(), 10)], Type, tile: TileID.DemonAltar);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextFloat() < 0.65f)
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height / 2, ModContent.DustType<Barathrum_Particles>(), player.direction / 4, -0.34f, 0, default, Main.rand.NextFloat(0.52f, 0.84f));
			}
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(ModContent.BuffType<Weak_Barathrum_Leach>(), Main.rand.Next(2, 7) * 60);
		}
		
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (Main.rand.NextFloat() < .1f)
			{
				for (int i = 0; i < Main.rand.Next(6); i++)
				{
					Dust.NewDust(Item.position, Item.width, Item.height, ModContent.DustType<Barathrum_Particles>(), Main.rand.NextFloat(-.2f, .2f), Main.rand.NextFloat(-.2f, .2f), 0, default, Main.rand.NextFloat(0.45f, 0.8f));
				}
			}
		}
	}

}