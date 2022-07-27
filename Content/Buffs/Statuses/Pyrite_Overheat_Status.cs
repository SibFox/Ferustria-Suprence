using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Ferustria.Content.Buffs.Statuses

{
	public class Pyrite_Overheat_Status : ModBuff
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrite Overheat");
			Description.SetDefault("You've shot too much.\nPyrite rod is overheated");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

        public override void Update(Player player, ref int buffIndex)
        {
			if (Main.rand.NextFloat() >= .35f)
				Dust.NewDust(player.position, player.width, player.height, DustID.Smoke, player.velocity.X + Main.rand.NextFloat(-1f, 1f), player.velocity.Y + Main.rand.NextFloat(-.5f, -1.5f), 65, default, Main.rand.NextFloat(0.725f, 1.2f));
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (Main.rand.NextFloat() >= .35f)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Smoke, npc.velocity.X + Main.rand.NextFloat(-1f, 1f), npc.velocity.Y + Main.rand.NextFloat(-.5f, -1.5f), 65, default, Main.rand.NextFloat(0.725f, 1.2f));
        }
	}
}