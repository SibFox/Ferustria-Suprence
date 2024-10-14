using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Ferustria.Content.Minions.PreHM;

namespace Ferustria.Content.Buffs.Statuses
{
	public class Angelic_Swordsman_Summoned_Buff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            // If the minions exist reset the buff time, otherwise remove the buff from the player
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Angelic_Spirit_Swordsman>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}