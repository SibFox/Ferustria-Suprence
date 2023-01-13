using IL.Terraria;
using Terraria.ModLoader;

namespace Ferustria
{
	public class Ferustria : Mod
	{
        internal const string AssetPath = "Ferustria/Assets/";
        internal const string TexturesPath = AssetPath + "Textures/";
        internal const string TexturesPathUIs = TexturesPath + "UIs/";
        internal const string TexturesPathNPCs = TexturesPath + "NPCs/";
        internal const string TexturesPathPrj = TexturesPath + "Projectiles/";

        internal static string GetChargeBarTexture(string item, bool bg = false)
        {
            string returnPath = TexturesPathUIs + item + "_ChargeBar";
            if (bg) returnPath += "_BG";
            return returnPath;
        }

        internal static string GetCahrgeBarElement(string item, string addition) => TexturesPathUIs + item + "_ChargeBar_" + addition;
    }
}