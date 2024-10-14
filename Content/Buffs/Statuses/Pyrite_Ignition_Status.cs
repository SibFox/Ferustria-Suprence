using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;

namespace Ferustria.Content.Buffs.Statuses

{
	public class Pyrite_Ignition_Status : ModBuff
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