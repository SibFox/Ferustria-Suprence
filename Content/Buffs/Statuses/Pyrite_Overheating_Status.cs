using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;

namespace Ferustria.Content.Buffs.Statuses

{
	public class Pyrite_Overheating_Status : ModBuff
	{

		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
    }
}