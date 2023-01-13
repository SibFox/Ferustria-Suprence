using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace Ferustria.Content.Buffs.Positives

{
	public class Thick_Blood : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thick Blood");
			Description.SetDefault("Health regeneration is increased while you're on the move");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Сгущенная кровь");
			Description.AddTranslation(FSHelper.RuTrans, "Регенерация здоровья увеличена, пока вы двигаетесь");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            if (player.velocity != Vector2.Zero)
            {
                //player.lifeRegenTime = 2;
                player.lifeRegen += (int)(8 * (Math.Abs(player.velocity.X + player.velocity.Y)) / 9.5f);
            }
                

            if (Main.rand.NextFloat() < Math.Abs((player.velocity.X + player.velocity.Y)) / 12.0f)
                Dust.NewDustDirect(player.position, player.width, player.height, DustID.Blood, 0f, 0f, 0, default, Main.rand.NextFloat(1f, 1.5f));
		}

    }
}
