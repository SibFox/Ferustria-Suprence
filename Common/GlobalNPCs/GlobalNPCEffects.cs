using Ferustria.Content.Dusts;
using Ferustria.Content.Items.Materials.Drop;
using Ferustria.Content.Items.Materials.Specials;
using Ferustria.Content.Projectiles.Friendly;
using IL.Terraria.GameContent.ObjectInteractions;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Ferustria.Common.GlobalNPCs
{
    // This file shows numerous examples of what you can do with the extensive NPC Loot lootable system.
    // You can find more info on the wiki: https://github.com/tModLoader/tModLoader/wiki/Basic-NPC-Drops-and-Loot-1.4
    // Despite this file being GlobalNPC, everything here can be used with a ModNPC as well! See examples of this in the Content/NPCs folder.
    public class GlobalNPCEffects : GlobalNPC
    {
        //// ~~~ Рекрафор
        public float Recraphor_Infestation = 0.0f;
        public const int Recraphor_InfestTimer_Max = 240;
        public int Recraphor_InfestTimer = 240;

        public override bool InstancePerEntity => true;

        public override void AI(NPC npc)
        {
            //// ~~~ Рекрафор
            if (Main.rand.NextFloat() < (Recraphor_Infestation / 100f) * 2)
            {
                Dust.NewDustDirect(npc.TopLeft, npc.width, npc.height, DustType<Rot_Particles>(), Main.rand.NextFloat(-1.75f, 1.75f), Main.rand.NextFloat(-1.75f, 1.75f),
                    Scale: Main.rand.NextFloat(0.85f, 1.45f));
            }

            if (Recraphor_Infestation / 20 > 1)
            {
                Recraphor_InfestTimer--;
            }
            else Recraphor_InfestTimer = Recraphor_InfestTimer_Max;
            if (Recraphor_InfestTimer <= 0)
            {
                Recraphor_Infestation -= 20.0f;
                Recraphor_InfestTimer = Recraphor_InfestTimer_Max;
                int projs = Main.rand.Next(3, 8);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    for (int i = 0; i < projs; i++)
                    {
                        Vector2 velocity = new(Main.rand.NextFloat() * 10f * Main.rand.NextFloatDirection()
                           , Main.rand.NextFloat() * 10f * Main.rand.NextFloatDirection());

                        if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 5f) velocity *= 5f;
                        velocity = Vector2.Clamp(velocity, new Vector2(-12f, -12f), new Vector2(12f, 12f));
                        Projectile.NewProjectileDirect(npc.GetSource_FromThis(), npc.Center, velocity, ProjectileType<Microorganism>(), 25, 2f, 0, npc.whoAmI, 1);
                    }
            }

            base.AI(npc);
        }

        public override bool PreKill(NPC npc)
        {
            //// ~~~ Рекрафор
            int Recraphor_OnDeathOut = 0;
            int Recraphor_OnDeathPrepare = (int)(Recraphor_Infestation / 4f);
            for (int i = 0; i < Recraphor_OnDeathPrepare; i++)
            {
                Recraphor_OnDeathOut += Main.rand.Next(2) + 1;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
                for (int i = 0; i < Recraphor_OnDeathOut; i++)
                {
                    Vector2 velocity = new(Main.rand.NextFloat() * 10f * Main.rand.NextFloatDirection()
                       , Main.rand.NextFloat() * 10f * Main.rand.NextFloatDirection());

                    if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 5f) velocity *= 5f;
                    velocity = Vector2.Clamp(velocity, new Vector2(-12f, -12f), new Vector2(12f, 12f));
                    Projectile.NewProjectileDirect(npc.GetSource_FromThis(), npc.Center, velocity, ProjectileType<Microorganism>(), 25, 2f, 0, npc.whoAmI, 1);
                }


            return base.PreKill(npc);
        }
    }
}