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
        public static GameCulture RuTrans = GameCulture.FromCultureName(GameCulture.CultureName.Russian);

        /// <summary>
        /// Высчитывает нужный скейл для сложностей.
        /// </summary>
        /// <param name="n">Нормальный</param>
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

        public static float GetStraightRotation(this Projectile projectile) => projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        public static void SetStraightRotation(this Projectile projectile) => projectile.rotation = projectile.GetStraightRotation();

        public static Item GetItem<T>() where T : ModItem => ModContent.GetModItem(ModContent.ItemType<T>()).Item;

        public static void CreateRecipe(CraftMaterial[] items, int result, int resultCount = 1, int tile = -1)
        {
            Recipe recipe = Recipe.Create(result, resultCount);
            foreach (var item in items) recipe.AddIngredient(item.itemID, item.count);
            if (tile > -1) recipe.AddTile(tile);
            recipe.Register();
        }
    }

    internal struct CraftMaterial
    {
        public int itemID;
        public int count;

        public CraftMaterial(int id, int c = 1)
        {
            itemID = id;
            count = c;
        }
    }

    internal class RegisterRecipe
    {
        public RegisterRecipe(CraftMaterial[] items, int result, int resultCount = 1, int tile = -1)
        {
            Recipe recipe = Recipe.Create(result, resultCount);
            foreach (var item in items) recipe.AddIngredient(item.itemID, item.count);
            if (tile > -1) recipe.AddTile(tile);
            recipe.Register();
        }

        public RegisterRecipe(CraftMaterial item, int result, int resultCount = 1, int tile = -1)
        {
            Recipe recipe = Recipe.Create(result, resultCount);
            recipe.AddIngredient(item.itemID, item.count);
            if (tile > -1) recipe.AddTile(tile);
            recipe.Register();
        }
    }
}
