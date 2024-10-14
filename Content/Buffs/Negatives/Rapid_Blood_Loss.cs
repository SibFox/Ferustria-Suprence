using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;

namespace Ferustria.Content.Buffs.Negatives

{
	public class Rapid_Blood_Loss : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.lifeRegen > 0) { player.lifeRegen = 0; player.lifeRegenTime = 0; }
			player.lifeRegen -= 40;
			if (Main.rand.NextFloat() < .65f)
				Dust.NewDustDirect(player.position, player.width, player.height, DustID.Blood, 0f, 0f, 0, default, Main.rand.NextFloat(1f, 1.5f));
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.lifeRegen > 0) npc.lifeRegen = 0;
			npc.lifeRegen -= 40;
			if (Main.rand.NextFloat() < .65f)
				Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Blood, 0f, 0f, 0, default, Main.rand.NextFloat(1f, 1.5f));
		}

    }
}
