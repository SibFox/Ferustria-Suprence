using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Ferustria.Content.Dusts;
using Terraria.ID;

namespace Ferustria.Content.Buffs.Negatives

{
	public class Weak_Barathrum_Leach : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; //Не даёт Нюрсе очистить бафф. Так как это связано с Пропастью, его очистить нельзя.
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.manaRegen > 0) { player.manaRegen = 0; player.manaRegenBonus = 0; }
			player.manaRegen -= 8;
			if (player.lifeRegen > 0) { player.lifeRegen = 0; player.lifeRegenTime = 0; }
			player.lifeRegen -= 20;
			//player.maxRunSpeed *= 0.9f;
			//player.accRunSpeed *= 0f;
			//player.moveSpeed *= 0.45f;
			if (Main.rand.NextFloat() < .65f)
				Dust.NewDustDirect(player.position, player.width, player.height, ModContent.DustType<Barathrum_Particles>(), 0f, 0f, 0, default, Main.rand.NextFloat(0.6f, 1f));
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.velocity.X *= 0.992f;
			if (npc.lifeRegen > 0) npc.lifeRegen = 0;
			npc.lifeRegen -= 20;
			if (Main.rand.NextFloat() < .65f)
				Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<Barathrum_Particles>(), 0f, 0f, 0, default, Main.rand.NextFloat(0.6f, 1f));
		}

    }
}
