using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Ferustria.Dusts;
using Terraria.ID;

namespace Ferustria.Buffs.Negatives

{
	public class Weak_Void_Leach : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weak Void Leach");
			DisplayName.AddTranslation(FSHelper.RuTrans(), "Слабое пустотное истощение");
			Description.SetDefault("Your body slowly exhausting");
			Description.AddTranslation(FSHelper.RuTrans(), "Ваше тело медленно истощается");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; //Не даёт Нюрсе очистить бафф. Так как это связано с Пустотой, его очистить нельзя.
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.manaRegen > 0) { player.manaRegen = 0; player.manaRegenBonus = 0; }
			player.manaRegen -= 8;
			if (player.lifeRegen > 0) { player.lifeRegen = 0; player.lifeRegenTime = 0; }
			player.lifeRegen -= 14;
			player.maxRunSpeed *= 0.9f;
			player.accRunSpeed *= 0f;
			player.moveSpeed *= 0.45f;
			if (Main.rand.NextFloat() < .65f)
				Dust.NewDustDirect(player.position, player.width, player.height, ModContent.DustType<Void_Particles>(), 0f, 0f, 0, default, Main.rand.NextFloat(0.6f, 1f));
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.velocity.X *= 0.992f;
			if (npc.lifeRegen > 0) npc.lifeRegen = 0;
			npc.lifeRegen -= 14;
			if (Main.rand.NextFloat() < .65f)
				Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<Void_Particles>(), 0f, 0f, 0, default, Main.rand.NextFloat(0.6f, 1f));
		}

    }
}
