using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Ferustria.Content.Items.Weapons.Mage.PreHM;
using Ferustria.Content.Items.Weapons.Melee.HM;
using System.Drawing.Text;
using System.Runtime.CompilerServices;

namespace Ferustria.Common.UIs.Elements
{
    internal class BarathrumPruner_ChargeBar : UIElement
    {
        private float pixelOff;

        Texture2D texture;
        Texture2D textureBG;
        Texture2D textureSuperMeterAct;
        Texture2D textureSuperMeterDeact;
        Texture2D textureMeterAct;
        Texture2D textureMeterDeact;

        public override void Draw(SpriteBatch spriteBatch)
        {
            pixelOff = 130;


            if (Main.LocalPlayer.HeldItem.Name == FSHelper.GetItem<Barathrum_Pruner>().Name && !Main.LocalPlayer.dead)
            {
                texture = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetChargeBarTexture("BarathrumPruner"));
                textureBG = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetChargeBarTexture("BarathrumPruner", true));
                textureSuperMeterAct = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetCahrgeBarElement("BarathrumPruner", "SuperMeter_Activated"));
                textureSuperMeterDeact = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetCahrgeBarElement("BarathrumPruner", "SuperMeter_Deactivated"));
                textureMeterAct = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetCahrgeBarElement("BarathrumPruner", "Meter_Activated"));
                textureMeterDeact = (Texture2D)Request<Texture2D>(Ferustria.Paths.GetCahrgeBarElement("BarathrumPruner", "Meter_Deactivated"));



                Texture2D smt;
                Texture2D mt1;
                Texture2D mt2;

                spriteBatch.Draw(textureBG, new Vector2(Main.screenWidth - texture.Width, Main.screenHeight + pixelOff) / 2f, null, Color.White, 0f,
                    new Vector2(texture.Width / 2 - texture.Width / 2 + texture.Width / 6, 0), 1.5f, 0, 0);
                Color gradientA = (0, (float)Main.time % 255, 255);
                Color gradientB = new(11, 18, 136);
                Color gradientC = Color.AliceBlue;
                float charge = Main.LocalPlayer.GetModPlayer<Players.FSSpesialWeaponsPlayer>().BarathrumPruner_Charge * 0.01f;
                charge = Utils.Clamp(charge, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

                //CheckForTextures(ref smt, ref mt1, ref mt2, charge);

                /////////////////////////
                if (charge > 0.4f)
                    mt1 = textureMeterAct;
                else
                    mt1 = textureMeterDeact;

                if (charge > 0.66f)
                    mt2 = textureMeterAct;
                else
                    mt2 = textureMeterDeact;

                if (charge > 0.99f)
                    smt = textureSuperMeterAct;
                else
                    smt = textureSuperMeterDeact;
                ////////////////////////


                int left = (Main.screenWidth - (texture.Width + 23)) / 2;
                int right = (Main.screenWidth + (texture.Width + 24)) / 2;
                int steps = (int)((right - left) * charge);
                for (int i = 0; i < steps; i++)
                {
                    float percent = (float)i / steps; // Alternate Gradient Approach
                                                      //float percent = (float)i / (right - left);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, (Main.screenHeight + (int)pixelOff + (int)(texture.Height)) / 2, 1, 14),
                        Color.Lerp(Color.Lerp(gradientB, gradientA, percent), Color.Lerp(gradientA, gradientB, percent), percent));
                }

                spriteBatch.Draw(texture, new Vector2(Main.screenWidth - texture.Width, Main.screenHeight + pixelOff) / 2f, null, Color.White, 0f, 
                    new Vector2(texture.Width / 2 - texture.Width / 2 + texture.Width / 6, 0), 1.5f, 0, 0.99f);

                //Super Meter
                spriteBatch.Draw(smt, new Vector2(Main.screenWidth + texture.Width / 2 - smt.Width * 1.5f, Main.screenHeight + pixelOff + texture.Height * 2 + smt.Height * 1.7f) / 2f, null,
                    Color.White, 0f, new Vector2(texture.Width / 2 - texture.Width / 2 + texture.Width / 6, 0), 1.5f, 0, 0.98f);

                //Meter First
                spriteBatch.Draw(mt2, new Vector2(Main.screenWidth - texture.Width / 2 + mt1.Width * 2.15f, Main.screenHeight + pixelOff + texture.Height * 2 + mt1.Height * 1.9f) / 2f, null,
                    Color.White, 0f, new Vector2(texture.Width / 2 - texture.Width / 2 + texture.Width / 6, 0), 1.5f, 0, 0.98f);

                spriteBatch.Draw(mt2, new Vector2(Main.screenWidth + texture.Width / 2 + mt1.Width * 2.58f, Main.screenHeight + pixelOff + texture.Height * 2 + mt1.Height * 1.9f) / 2f, null,
                    Color.White, 0f, new Vector2(texture.Width / 2 - texture.Width / 2 + texture.Width / 6, 0), 1.5f, 0, 0.98f);

                //Meter Second
                spriteBatch.Draw(mt1, new Vector2(Main.screenWidth - texture.Width / 2 - mt2.Width * 1.28f, Main.screenHeight + pixelOff + texture.Height * 2 + mt1.Height * 1.9f) / 2f, null,
                    Color.White, 0f, new Vector2(texture.Width / 2 - texture.Width / 2 + texture.Width / 6, 0), 1.5f, 0, 0.98f);

                spriteBatch.Draw(mt1, new Vector2(Main.screenWidth + texture.Width + mt2.Width * 2.15f, Main.screenHeight + pixelOff + texture.Height * 2 + mt1.Height * 1.9f) / 2f, null,
                    Color.White, 0f, new Vector2(texture.Width / 2 - texture.Width / 2 + texture.Width / 6, 0), 1.5f, 0, 0.98f);

            }
        }

        private void CheckForTextures(ref Texture2D smt, ref Texture2D mt1, ref Texture2D mt2, float charge)
        {
            if (charge > 32)
                mt1 = textureMeterAct;
            else
                mt1 = textureMeterDeact;

            if (charge > 64)
                mt2 = textureMeterAct;
            else
                mt2 = textureMeterDeact;

            if (charge > 95)
                smt = textureSuperMeterAct;
            else
                smt = textureSuperMeterDeact;
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
