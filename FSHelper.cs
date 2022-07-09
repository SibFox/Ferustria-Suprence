using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace Ferustria
{
    internal static class FSHelper
    {
        public static GameCulture RuTrans() => GameCulture.FromCultureName(GameCulture.CultureName.Russian);

        /// <summary>
        /// Высчитывает нужный скейл для сложностей. n - Нормальная; e - Эксперт; m - Мастер
        /// </summary>
        /// <param name="n">Нормальная</param>
        /// <param name="e">Эксперт</param>
        /// <param name="m">Мастер</param>
        public static int Scale(int n, int e, int m)
        {
            if (!Main.expertMode && !Main.masterMode) return n;
            else if (Main.expertMode && !Main.masterMode) return e / 2;
            else if (Main.masterMode) return m / 3;
            else return 1;
        }

        public static int WOScale(int n, int e, int m)
        {
            if (!Main.expertMode && !Main.masterMode) return n;
            else if (Main.expertMode && !Main.masterMode) return e;
            else if (Main.masterMode) return m;
            else return 1;
        }
    }
}
