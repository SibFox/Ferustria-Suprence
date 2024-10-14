using Microsoft.Xna.Framework;
using Ferustria.Content.Projectiles.Friendly;
using Ferustria.Content.Buffs.Negatives;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Ferustria.Content.Items.Materials.Drop;
using static Terraria.ModLoader.ModContent;
using Ferustria.Content.Items.Materials.Craftable;
using System.Threading;

namespace Ferustria.Content.Items.Weapons.Melee.HM
{
	public class Barathrum_Pruner : ModItem
	{
        bool canShoot = false;

		public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 104;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.width = 74;
			Item.height = 78;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 7.28f;
			Item.UseSound = SoundID.Item15;
			Item.value = Item.sellPrice(0, 4, 35, 0);
			Item.rare = ItemRarityID.Purple;
			Item.autoReuse = true;
            Item.shoot = ProjectileType<Neon_Pruner_Slice>();
            Item.shootSpeed = 22.5f;
		}

        public override bool AltFunctionUse(Player player)
        {
            if (player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().BarathrumPruner_Charge >= 40f)
            {
                canShoot = true;
                return true;
            }
            return false;
        }
        

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                canShoot = true;
            }

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (canShoot)
            {
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, (int)(damage * 1.5), knockback, Main.myPlayer, 
                    player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().BarathrumPruner_Charge);
                canShoot = false;
                player.GetModPlayer<Players.FSSpesialWeaponsPlayer>().BarathrumPruner_Charge = 0f;
            }
            return false;
        }

        public override void AddRecipes()
		{
            RegisterRecipe.Reg(
            [ new(ItemID.BreakerBlade), new(ItemID.HallowedBar, 10), new(ItemID.SoulofFright, 10), new(ItemType<Impure_Dust>(), 12), new(ItemType<Barathrum_Extract>(), 3)
            ], Type, tile: TileID.MythrilAnvil);
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.life >= target.lifeMax) modifiers.SourceDamage *= 2.2f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<Sliced_Defense>(), 4 * 60);

            Players.FSSpesialWeaponsPlayer weaponCharge = player.GetModPlayer<Players.FSSpesialWeaponsPlayer>();
            if (target.boss)
                weaponCharge.BarathrumPruner_Charge += 7.15f;
            else
                weaponCharge.BarathrumPruner_Charge += 3.85f;
            weaponCharge.BarathrumPruner_Charge_DepleteTimer = 420;
        }

    }

}