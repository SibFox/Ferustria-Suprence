using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using System;

namespace Ferustria.Content.Buffs.Negatives
{
	public class Sliced_Defense : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			//BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.GetModPlayer<Players.FSPlayer>().DeBuff_SlicedDefense_Applied = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			int getMinuser = Convert.ToInt32(Math.Round(npc.defDefense / 1.5));
			if (getMinuser > 50) getMinuser = Convert.ToInt32(npc.defDefense - 50.0);
			npc.defense = getMinuser;
		}
    }
}
