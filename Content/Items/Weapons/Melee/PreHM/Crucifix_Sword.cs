using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Buffs.Statuses;
using Ferustria.Content.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.GameContent.Creative;

namespace Ferustria.Content.Items.Weapons.Melee.PreHM
{
	public class Crucifix_Sword : ModItem
	{
		bool canSlot;
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Crucifix Sword");
			Tooltip.SetDefault("Absolution to your enemies.");
            DisplayName.AddTranslation(FSHelper.RuTrans, "Меч-распятие");
            Tooltip.AddTranslation(FSHelper.RuTrans, "Очищение вашим врагам.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		//DisplayName.AddTranslation(FSHelper.RuTrans, "Меч Распятия");
		//Tooltip.AddTranslation(FSHelper.RuTrans, "Отпущение вашим врагам");

		public override void SetDefaults() 
		{
			Item.DamageType = DamageClass.Melee;
			Item.damage = 17;
			Item.crit = 0;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4.2f;
			Item.value = Item.sellPrice(0, 0, 17, 0);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			canSlot = false;
		}

		/*public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextFloat() < 0.95f)
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Void_Particles>(), player.direction / 4, -0.3f, 0, default, Main.rand.NextFloat(0.45f, 0.8f));
			}
		}*/

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			//target.buffTime[1] = BuffID.OnFire + 70;
			/*if (!target.HasBuff(BuffID.OnFire)) target.AddBuff(BuffID.OnFire, Main.rand.Next(4, 8) * 60);
			else if (target.buffTime[0] >= 300) target.AddBuff(ModContent.BuffType<Pyrite_Overheat_Status>(), 3 * 60);*/
			if (!target.HasBuff(BuffID.OnFire))
			{
				target.AddBuff(BuffID.OnFire, 3 * 60);
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, 10, 10), Color.Red, 1, true);
				canSlot = false;
			}
			float time = target.buffTime[target.FindBuffIndex(BuffID.OnFire)];
			if (time >= 9.2 * 65 && canSlot)
			{
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, 10, 10), Color.LawnGreen, 6, true);
				target.DelBuff(target.FindBuffIndex(BuffID.OnFire));
                target.AddBuff(BuffID.OnFire3, 320);
			}
			else if (time >= 6.5 * 65 && canSlot)
			{
				target.AddBuff(BuffID.OnFire, 11 * 60);
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, 10, 10), Color.Yellow, 5, true);
			}
			else if (time >= 4.5 * 65 && canSlot)
			{
				target.AddBuff(BuffID.OnFire, 8 * 60);
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, 10, 10), Color.Yellow, 4, true);
			}
			else if (time >= 3 * 65 && canSlot)
			{
				target.AddBuff(BuffID.OnFire, 6 * 60);
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, 10, 10), Color.Yellow, 3, true);
			}
			else if (time >= 2 * 65 && canSlot) 
			{
                target.AddBuff(BuffID.OnFire, (int)(4.5 * 60));
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, 10, 10), Color.Yellow, 2, true);
            }
			canSlot = true;
		}
    }

}