using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Ferustria.Dusts;

namespace Ferustria.Buffs.Negatives

{
	public class Under_Crucifixion_Tier2 : ModBuff
	{
		bool Crucifixion_Applied;
		int Crucifixion_Timer;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Under Crusifixion");
			Description.SetDefault("You feel your sins crawl on your back.\nSomething not good is going to happen..."); //3: Punishment awaits you.
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
			Crucifixion_Applied = false;
			Crucifixion_Timer = -1;
		}
		
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Crucifixion_Applied)
            {
				if (Crucifixion_Timer < 0)
                {
					int proj = Projectile.NewProjectile(npc.GetSource_FromThis(), npc.position, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.BGs.Crucifixion_Halo_Foe>(), 0, 0);
					Main.projectile[proj].netUpdate = true;
                }
            }
		}

	}
}
