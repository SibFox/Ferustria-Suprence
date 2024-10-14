using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using Ferustria.Content.NPCs.Bosses.HM.SixWingedSeraphBoss;
using Terraria.Audio;

namespace Ferustria.Content.Items.SummonItems
{
    public class Oppened_Casket : ModItem
    {
        public override string Texture => "Ferustria/Content/Items/Materials/Drop/Barathrum_Sample";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.maxStack = 99;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.White;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // If the player using the item is the client
                // (explicitely excluded serverside here)
                //SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = ModContent.NPCType<Six_Winged_Seraph_Boss>();

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // If the player is not in multiplayer, spawn directly
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    // If the player is in multiplayer, request a spawn
                    // This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
            }

            return true;
            //if (Main.myPlayer == player.whoAmI)
            //{
            //    NPC.NewNPC(Item.GetSource_ItemUse(Item, "Summoned"), (int)player.position.X + player.width / 2, (int)player.position.Y + player.height,
            //        ModContent.NPCType<Six_Winged_Seraph_Boss>(), Target: player.whoAmI);
            //}
            //return true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Six_Winged_Seraph_Boss>());
            //return NPC.CountNPCS(ModContent.NPCType<NPCs.Bosses.HM.SixWingedSeraphBoss.Six_Winged_Seraph_Boss>()) < 1;
        }
    }
}
