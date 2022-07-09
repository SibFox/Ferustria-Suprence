using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Ferustria.Dusts;

namespace Ferustria.Buffs.Negatives

{
	public class Under_Crucifixion_Tier1 : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Under Crusifixion");
			Description.SetDefault("You feel blessed!\nEven if you don't think, you are...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (Main.rand.NextFloat() < .5f)
			{
				Dust dust;
				dust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height - 30, 158, 0f, 4f, 0, default, 1.05f)];
				dust.scale *= 0.95f;
				if (dust.scale <= 0.25f) dust.active = false;
			}
		}
		//6(жёлтые частицы), 87(тоже жёлтые частицы, но хорошо летят вниз, 158) 
		public override void Update(Player player, ref int buffIndex)
        {
			if (Main.rand.NextFloat() < .5f)
			{
				Dust dust;
				dust = Main.dust[Dust.NewDust(player.position, player.width, player.height - 30, 158, 0f, 4f, 0, default, 1.05f)];
				dust.scale *= 0.95f;
				if (dust.scale <= 0.25f) dust.active = false;
			}
			base.Update(player, ref buffIndex);
        }

    }
}
