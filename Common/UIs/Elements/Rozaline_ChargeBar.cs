using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Ferustria.Content.Items.Weapons.Melee.PreHM;

namespace Ferustria.Common.UIs.Elements
{
    internal class Rozaline_ChargeBar : UIElement
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)Request<Texture2D>(Ferustria.GetChargeBarTexture("Rozaline"));
            Texture2D textureBG = (Texture2D)Request<Texture2D>(Ferustria.GetChargeBarTexture("Rozaline", true));

            if (Main.LocalPlayer.HeldItem.Name == FSHelper.GetItem<Rozaline>().Name && !Main.LocalPlayer.dead)
            {
                spriteBatch.Draw(textureBG, new Vector2(Main.screenWidth - texture.Width, Main.screenHeight - 128) / 2f, Color.White);
                Color gradientB = Color.DarkOliveGreen;
                Color gradientA = Color.RosyBrown;
                Color gradientC = Color.YellowGreen;

                float charge = Main.LocalPlayer.GetModPlayer<Players.FSSpesialWeaponsPlayer>().Rozaline_Spikes_ChargeMeter * 0.01f;
                charge = Utils.Clamp(charge, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

                //Vector2 hitbox = texture.Size();
                int left = (Main.screenWidth - texture.Width + 29) / 2;
                int right = (Main.screenWidth + texture.Width - 28) / 2;
                int steps = (int)((right - left) * charge);
                for (int i = 0; i < steps; i++)
                {
                    float percent = (float)i / steps; // Alternate Gradient Approach
                                                      //float percent = (float)i / (right - left);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, (Main.screenHeight - 108) / 2, 1, 8), Color.Lerp(Color.Lerp(gradientB, gradientA, percent), Color.Lerp(gradientA, gradientC, percent), percent));
                }

                spriteBatch.Draw(texture, new Vector2(Main.screenWidth - texture.Width, Main.screenHeight - 128) / 2f, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
        }
    }

    //class Rozaline_UI_ChargeBar : UIState
    //{
    //    public Rozaline_ChargeBar rozaline_ChargeBar;

    //    public override void OnInitialize()
    //    {
    //        rozaline_ChargeBar = new Rozaline_ChargeBar();
    //        Append(rozaline_ChargeBar);
    //    }
    //}
}
