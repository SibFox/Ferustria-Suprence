using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Ferustria.Content.Pets;
using Ferustria.Content.Buffs.Minions_And_Pets;

namespace Ferustria.Content.Items.Pets
{
    public class JiL_Pet_Item : ModItem
    {
        // Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
        public override void SetDefaults()
        {
            //Broken PDA
            Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.

            Item.shoot = ModContent.ProjectileType<JiL_Pet>(); // "Shoot" your pet projectile.
            Item.buffType = ModContent.BuffType<JiL_Pet_Buff>(); // Apply buff upon usage of the Item.
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(Item.buffType, 3600);
            }
            return true;
        }
    }
}
