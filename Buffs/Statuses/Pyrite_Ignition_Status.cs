using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;

namespace Ferustria.Buffs.Statuses

{
	public class Pyrite_Ignition_Status : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrite Ignition");
			Description.SetDefault("Your shotgun become more hot\nShoot more to perform a powered shot");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
	}
}