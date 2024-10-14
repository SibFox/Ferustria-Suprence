using Microsoft.Xna.Framework;
using Ferustria.Content.Buffs.Negatives;
using Ferustria.Content.Dusts;
using Ferustria.Content.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using Ferustria.Content.Items.Materials.Drop;
using Microsoft.CodeAnalysis.Operations;

namespace Ferustria.Content.NPCs.Friendlies.PlayerBehalf
{

    public class Shroom_Budy_Melee : ModNPC
	{
        public override string Texture => "Ferustria/Content/NPCs/Balistic_Firing_TestNPC";

        public override void SetStaticDefaults()
		{
			//Main.npcFrameCount[NPC.type] = 4;

        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 30;
            NPC.damage = 20;
			NPC.defense = 0;
            NPC.knockBackResist = 1.2f;
			NPC.width = 26;
			NPC.height = 30;//152/4
			NPC.aiStyle = -1;
            NPC.noGravity = false;
			//NPC.HitSound = SoundID.NPCHit1;
			//NPC.DeathSound = SoundID.NPCDeath4;
            NPC.value = 0;
            NPC.friendly = true;
            NPC.dontTakeDamageFromHostiles = false;
            NPC.npcSlots = 0;
            NPC.dontCountMe = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0f;
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
			if (NPC.life > 0)
			{
				double particles = hit.Damage;
                if (particles > NPC.lifeMax) particles = NPC.lifeMax;
				if (particles > 35) particles = 35;
				for (int i = 0; i < particles; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.GlowingMushroom, hit.HitDirection, -2f, 0, default, Main.rand.NextFloat(0.6f, 0.8f));
				}
				return;
			}
			for (int i = 0; i < 30; i++)
			{
				Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.GlowingMushroom, 2.5f * hit.HitDirection, -2.3f, 0, default, Main.rand.NextFloat(0.4f, 1f));
			}
		}


        
		public override void AI()
		{
            NPC.netUpdate = true;
            NPC targetNPC = NPC.TargetClosestNPC();
		}
	}
}