using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Ferustria.Content.Minions.PreHM;
using Ferustria.Content.Pets;

namespace Ferustria.Content.Buffs.Minions_And_Pets
{
	public class JiL_Pet_Buff : ModBuff
	{
		public override void SetStaticDefaults()
		{
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<JiL_Pet>());
        }
    }
}