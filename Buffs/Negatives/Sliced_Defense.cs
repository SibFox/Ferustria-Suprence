using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Ferustria.Dusts;
using Terraria.ID;
using System;

namespace Ferustria.Buffs.Negatives
{
	public class Sliced_Defense : ModBuff
	{
        bool set;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Armor sliced");
			Description.SetDefault("Your defence is incinerated");
			DisplayName.AddTranslation(FSHelper.RuTrans, "Разрез брони");
			Description.AddTranslation(FSHelper.RuTrans, "Ваша броня испепелена");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			set = false;
			//BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.GetModPlayer<FSPlayer>().DeBuff_SlicedDefense_Applied = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			int getMinuser = Convert.ToInt32(Math.Round(npc.defDefense / 1.5));
			if (getMinuser > 50) getMinuser = Convert.ToInt32(npc.defDefense - 50.0);
			npc.defense = getMinuser;
		}
    }
}
