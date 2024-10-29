using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Ferustria.Content.Items.Weapons.Mage.PreHM;

namespace Ferustria.Common.UIs.Elements.ChargeBars
{
    internal class CKnife1_Charge_Bar : UIElement
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetChargeBarTexture("CKnife1"));
            Texture2D textureBG = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetChargeBarTexture("CKnife1", true));


            if (Main.LocalPlayer.HeldItem.Name == FSHelper.GetItemName<Ceremonial_Knife>() && !Main.LocalPlayer.dead)
            {
                spriteBatch.Draw(textureBG, new Vector2(Main.screenWidth - texture.Width, Main.screenHeight - 120) / 2f, Color.White);
                Color gradientA = Color.Blue;
                Color gradientB = Color.Aqua;
                Color gradientC = Color.AliceBlue;

                float charge = Main.LocalPlayer.GetModPlayer<Players.FSSpesialWeaponsPlayer>().CKnifeL1_Knifes_Charge * 0.01f;
                charge = Utils.Clamp(charge, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

                //Vector2 hitbox = texture.Size();
                int left = (Main.screenWidth - texture.Width + 17) / 2;
                int right = (Main.screenWidth + texture.Width - 16) / 2;
                int steps = (int)((right - left) * charge);
                for (int i = 0; i < steps; i++)
                {
                    float percent = (float)i / steps; // Alternate Gradient Approach
                                                      //float percent = (float)i / (right - left);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, (Main.screenHeight - 107) / 2, 1, 9),
                        Color.Lerp(Color.Lerp(gradientB, gradientA, percent), Color.Lerp(gradientA, gradientB, percent), percent));
                }

                spriteBatch.Draw(texture, new Vector2(Main.screenWidth - texture.Width, Main.screenHeight - 120) / 2f, Color.White);
            }
        }
    }

    //class CKnife1_UI_ChargeBar : UIState
    //{
    //    public CKnife1_Charge_Bar cKnife1_ChargeBar;

    //    public override void OnInitialize()
    //    {
    //        cKnife1_ChargeBar = new CKnife1_Charge_Bar();
    //        Append(cKnife1_ChargeBar);
    //    }    
    //}
}
