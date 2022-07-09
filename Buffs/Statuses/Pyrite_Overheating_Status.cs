using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;

namespace Ferustria.Buffs.Statuses

{
	public class Pyrite_Overheating_Status : ModBuff
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrite Overheating");
			Description.SetDefault("Your machinegun become more hot.\nContinue shooting for more effectiveness");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
    }
}