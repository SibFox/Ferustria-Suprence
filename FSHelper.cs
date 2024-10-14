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
using Mono.Cecil;

namespace Ferustria
{
    internal static class FSHelper
    {
        internal static GameCulture RuTrans = GameCulture.FromCultureName(GameCulture.CultureName.Russian);

        /// <summary>
        /// Высчитывает нужный скейл для сложностей.
        /// </summary>
        /// <param name="n">Нормальный</param>
        /// <param name="e">Эксперт</param>
        /// <param name="m">Мастер</param>
        internal static int Scale(int n, int e, int m)
        {
            if (!Main.expertMode && !Main.masterMode) return n;
            else if (Main.expertMode && !Main.masterMode) return e / 2;
            else if (Main.masterMode) return m / 3;
            else return 1;
        }

        internal static int WOScale(int n, int e, int m)
        {
            if (!Main.expertMode && !Main.masterMode) return n;
            else if (Main.expertMode && !Main.masterMode) return e;
            else if (Main.masterMode) return m;
            else return 1;
        }        

        internal static Item GetItem<T>() where T : ModItem => ModContent.GetModItem(ModContent.ItemType<T>()).Item;

        //// Баллистический рассчёт
        // Solve firing angles for a ballistic projectile with speed and gravity to hit a fixed position.
        //
        // projPos (Vector2): point projectile will fire from
        // projSpeed (float): scalar speed of projectile
        // target (Vector2): point projectile is trying to hit
        // gravity (float): force of gravity, positive down
        //
        // s0 (out Vector2): firing solution (low angle) 
        // s1 (out Vector2): firing solution (high angle)
        //
        // return (int): number of unique solutions found: 0, 1, or 2.
        public static int SolveBalisticArc(Vector2 projPos, float projSpeed, Vector2 target, float gravity, out Vector2 s0, out Vector2 s1)
        {

            // C# requires out variables be set
            s0 = Vector2.Zero;
            s1 = Vector2.Zero;

            // Derivation
            //   (1) x = v*t*cos O
            //   (2) y = v*t*sin O - .5*g*t^2
            // 
            //   (3) t = x/(cos O*v)                                        [solve t from (1)]
            //   (4) y = v*x*sin O/(cos O * v) - .5*g*x^2/(cos^2 O*v^2)     [plug t into y=...]
            //   (5) y = x*tan O - g*x^2/(2*v^2*cos^2 O)                    [reduce; cos/sin = tan]
            //   (6) y = x*tan O - (g*x^2/(2*v^2))*(1+tan^2 O)              [reduce; 1+tan O = 1/cos^2 O]
            //   (7) 0 = ((-g*x^2)/(2*v^2))*tan^2 O + x*tan O - (g*x^2)/(2*v^2) - y    [re-arrange]
            //   Quadratic! a*p^2 + b*p + c where p = tan O
            //
            //   (8) let gxv = -g*x*x/(2*v*v)
            //   (9) p = (-x +- sqrt(x*x - 4gxv*(gxv - y)))/2*gxv           [quadratic formula]
            //   (10) p = (v^2 +- sqrt(v^4 - g(g*x^2 + 2*y*v^2)))/gx        [multiply top/bottom by -2*v*v/x; move 4*v^4/x^2 into root]
            //   (11) O = atan(p)

            Vector2 diff = target - projPos;
            float groundDist = diff.Length();
            float rotation = diff.SafeNormalize(Vector2.Zero).ToRotation();

            float speed2 = projSpeed * projSpeed;
            float speed4 = projSpeed * projSpeed * projSpeed * projSpeed;
            float y = diff.Y;
            float x = groundDist;
            float gx = gravity * x;

            float root = speed4 - gravity * (gravity * x * x + 2 * y * speed2);

            // No solution
            if (root < 0)
                return 0;

            root = MathF.Sqrt(root);

            float lowAng = MathF.Atan2(speed2 - root, gx);
            float highAng = MathF.Atan2(speed2 + root, gx);
            int numSolutions = lowAng != highAng ? 2 : 1;

            Vector2 groundDir = diff.SafeNormalize(Vector2.Zero);
            s0 = groundDir * MathF.Cos(lowAng) * projSpeed + new Vector2(0, -1f) * MathF.Sin(lowAng) * projSpeed;
            if (numSolutions > 1)
                s1 = groundDir * MathF.Cos(highAng) * projSpeed + new Vector2(0, -1f) * MathF.Sin(highAng) * projSpeed;

            return numSolutions;
        }

    }

    internal static class Extensions
    {
        // Vector2
        internal static Vector2 GetVectorWithAngle(float angle) => new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        internal static Vector2 GetVectorToAngle(this Vector2 vector, float angle) => vector * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)).SafeNormalize(default);
        internal static Vector2 GetVectorToAngleWithMult(this Vector2 vector, float angle, float speed) =>
            vector * (new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)).SafeNormalize(default) * speed);
        internal static Vector2 GetVectorToAngleWithMult(this Vector2 vector, double angle, float speed) =>
            vector * (new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)).SafeNormalize(default) * speed);
        public static void ApplyMuzzleOffset(this Vector2 position, Vector2 velocity, float offset = 25f)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }


        // Tile
        internal static bool IsTileSolid(this Tile tile) => Main.tileSolid[tile.TileType] && tile.HasTile && tile.TileType != TileID.Platforms;
        internal static bool IsTileEmpty(this Tile tile) => !Main.tileSolid[tile.TileType] && !tile.HasTile && tile.TileType != TileID.Platforms;


        // NPC
        internal static int TargetClosestNPCid(this NPC npc, bool setToLocalAI3 = false)
        {
            float minDistance = 999999;
            int closeNpcID = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC closeNpc = Main.npc[i];
                if (closeNpc.active)
                {
                    float distance = npc.DistanceSQ(closeNpc.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closeNpcID = i;
                        if (setToLocalAI3)
                            npc.localAI[3] = i;
                    }
                }
            }
            return closeNpcID;
        }

        internal static NPC TargetClosestNPC(this NPC npc)
        {
            float minDistance = 999999;
            NPC closeNpcReturn = new NPC();
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC closeNpc = Main.npc[i];
                if (closeNpc.active)
                {
                    float distance = npc.DistanceSQ(closeNpc.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closeNpcReturn = closeNpc;
                    }
                }
            }
            return closeNpcReturn;
        }


        // Projectile
        internal static float GetStraightRotation(this Projectile projectile) => projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        internal static void SetStraightRotation(this Projectile projectile) => projectile.rotation = projectile.GetStraightRotation();

        public static void FindClosestNPC(this Projectile projectile, float maxDetectDistance, out NPC closestNPC, int idIgnore = -1) =>
            projectile.FindClosestNPC(maxDetectDistance, out closestNPC, new int[1] { idIgnore });

        public static void FindClosestNPC(this Projectile projectile, float maxDetectDistance, out NPC closestNPC, int[] idIgnore = null)
        {
            closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target.CanBeChasedBy() &&
                    target.active &&
                    target.lifeMax > 5 &&
                    !target.CountsAsACritter &&
                    !target.immortal &&
                    !target.friendly)
                {
                    if (idIgnore != null && idIgnore.Length > 0 && !idIgnore.Contains(k))
                    {
                        // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                        float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, projectile.Center);

                        // Check if it is within the radius
                        if (sqrDistanceToTarget < sqrMaxDetectDistance)
                        {
                            sqrMaxDetectDistance = sqrDistanceToTarget;
                            closestNPC = target;
                        }
                    }
                }
            }
        }

    }

    internal struct CraftMaterial
    {
        internal int itemID;
        internal int count;

        internal CraftMaterial(int id, int c = 1)
        {
            itemID = id;
            count = c;
        }
    }

    internal static class RegisterRecipe
    {

        internal static void Reg(CraftMaterial[] items, int result, int resultCount = 1, int tile = -1)
        {
            Recipe recipe = Recipe.Create(result, resultCount);
            foreach (var item in items) recipe.AddIngredient(item.itemID, item.count);
            if (tile > -1) recipe.AddTile(tile);
            recipe.Register();
        }

        internal static void Reg(CraftMaterial item, int result, int resultCount = 1, int tile = -1)
        {
            Recipe recipe = Recipe.Create(result, resultCount);
            recipe.AddIngredient(item.itemID, item.count);
            if (tile > -1) recipe.AddTile(tile);
            recipe.Register();
        }

        internal static void Reg(CraftMaterial[] items, int result, int[] tiles, int resultCount = 1)
        {
            Recipe recipe = Recipe.Create(result, resultCount);
            foreach (var item in items) recipe.AddIngredient(item.itemID, item.count);
            foreach (var tile in tiles) recipe.AddTile(tile);
            recipe.Register();
        }

        internal static void Reg(CraftMaterial item, int result, int[] tiles, int resultCount = 1)
        {
            Recipe recipe = Recipe.Create(result, resultCount);
            recipe.AddIngredient(item.itemID, item.count);
            foreach (var tile in tiles) recipe.AddTile(tile);
            recipe.Register();
        }


        //internal RegisterRecipe(CraftMaterial[] items, int result, int resultCount = 1, int tile = -1)
        //{
        //    Recipe recipe = Recipe.Create(result, resultCount);
        //    foreach (var item in items) recipe.AddIngredient(item.itemID, item.count);
        //    if (tile > -1) recipe.AddTile(tile);
        //    recipe.Register();
            
        //}
        //internal RegisterRecipe(CraftMaterial item, int result, int resultCount = 1, int tile = -1)
        //{
        //    Recipe recipe = Recipe.Create(result, resultCount);
        //    recipe.AddIngredient(item.itemID, item.count);
        //    if (tile > -1) recipe.AddTile(tile);
        //    recipe.Register();
        //}

        //internal RegisterRecipe(CraftMaterial[] items, int result, int[] tiles, int resultCount = 1)
        //{
        //    Recipe recipe = Recipe.Create(result, resultCount);
        //    foreach (var item in items) recipe.AddIngredient(item.itemID, item.count);
        //    foreach (var tile in tiles) recipe.AddTile(tile);
        //    recipe.Register();
        //}

        //internal RegisterRecipe(CraftMaterial item, int result, int[] tiles, int resultCount = 1)
        //{
        //    Recipe recipe = Recipe.Create(result, resultCount);
        //    recipe.AddIngredient(item.itemID, item.count);
        //    foreach (var tile in tiles) recipe.AddTile(tile);
        //    recipe.Register();
        //}

        
    }
}
