using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Terraria.UI;
using System.Collections.Generic;
using Ferustria.Common.UIs.States;

namespace Ferustria.Common.Systems
{
    internal class UISystem : ModSystem
    {
        internal Weapon_ChargeBars_UIState weapon_chargeBars_State;
        private UserInterface weapon_chargeBars_Interface;


        public override void Load()
        {
            weapon_chargeBars_State = new Weapon_ChargeBars_UIState();
            weapon_chargeBars_State.Activate();
            weapon_chargeBars_Interface = new UserInterface();
            weapon_chargeBars_Interface.SetState(weapon_chargeBars_State);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            weapon_chargeBars_Interface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    Mod.Name + ": Weapons Charge Bars",
                    delegate
                    {
                        weapon_chargeBars_Interface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.Game)
                );
            }
        }
    }
}
