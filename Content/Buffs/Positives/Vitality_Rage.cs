using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;

namespace Ferustria.Content.Buffs.Positives

{
	public class Vitality_Rage : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Vitality Rage");
			//Description.SetDefault("The more you have regeneration - the more you deal damage");
			//DisplayName.AddTranslation(FSHelper.RuTrans, "Ярость Жизни");
			//Description.AddTranslation(FSHelper.RuTrans, "Чем больше ваша регенерация - тем больше вы наносите урона");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
		}

    }
}
