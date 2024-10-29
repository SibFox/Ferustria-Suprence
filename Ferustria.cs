using log4net.Repository.Hierarchy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ModLoader;

namespace Ferustria
{
	public class Ferustria : Mod
	{
        internal const string emptyPixel = "Ferustria/emptyPixel";
        public class Paths
        {
            internal const string AssetPath = "Ferustria/Assets/";
            internal const string TexturesPath = AssetPath + "Textures/";
            internal const string TexturesPathUIs = TexturesPath + "UIs/";
            internal const string TexturesPathNPCs = TexturesPath + "NPCs/";
            internal const string TexturesPathPrj = TexturesPath + "Projectiles/";
            internal const string TexturesPathBGs = TexturesPath + "BGs/";
            internal const string TexturesPathEnemies = TexturesPathNPCs + "Enemies/";

            internal static string GetChargeBarTexture(string item, bool bg = false) => TexturesPathUIs + item + "_ChargeBar" + (bg ? "_BG" : null);
            internal static string GetCahrgeBarElement(string item, string addition) => TexturesPathUIs + item + "_ChargeBar_" + addition;
        }

        public static class InnerDebug
        {
            static readonly bool UseDebug = false;

#nullable enable
            public static void Print(string? message = null)
            {
                //if (UseDebug) 
                   Console.WriteLine("[Ferustria] " + message);
            }
        }
    }
}