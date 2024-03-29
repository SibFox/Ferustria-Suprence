﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Ferustria
{
    public static class FerustriaFunctions
    {
        public static Vector2 VelocityToPoint(Vector2 A, Vector2 B, float Speed)
        {
            var Move = (B - A);
            return Move * (Speed / (float)Math.Sqrt(Move.X * Move.X + Move.Y * Move.Y));
        }

        //Какая-то точка в радиусе
        public static Vector2 RandomPointInArea(Vector2 v1, Vector2 v2)
        {
            return new Vector2(Main.rand.Next((int)v1.X, (int)v2.X) + 1, Main.rand.Next((int)v1.Y, (int)v2.Y) + 1);
        }

        public static Vector2 RandomPointInArea(Rectangle Area)
        {
            return new Vector2(Main.rand.Next(Area.X, Area.X + Area.Width), Main.rand.Next(Area.Y, Area.Y + Area.Height));
        }

        public static float RotateBetween2Points(Vector2 A, Vector2 B)
        {
            return (float)Math.Atan2(A.Y - B.Y, A.X - B.X);
        }

        //Центральная точка между двумя заданными
        public static Vector2 CenterPointBetween(Vector2 A, Vector2 B)
        {
            return new Vector2((A.X + B.X) / 2.0f, (A.Y + B.Y) / 2.0f);
        }

        //Движение к цели
        public static void MoveTowards(this NPC npc, Vector2 playerTarget, float speed, float turnResistance)
        {
            Vector2 Move = playerTarget - npc.Center;
            float Length = Move.Length();
            if (Length > speed)
            {
                Move *= speed / Length;
            }
            Move = (npc.velocity * (turnResistance - 1f) + Move) / (turnResistance + 1f);
            Length = Move.Length();
            if (Length > speed)
            {
                Move *= speed / Length;
            }
            npc.velocity = Move;
        }

        public static void HomingTowards(this Projectile proj, Vector2 target, float speed, float turnResistance)
        {
            Vector2 Move = target - proj.Center;
            float Length = Move.Length();
            if (Length > speed)
            {
                Move *= speed / Length;
            }
            Move = (proj.velocity * (turnResistance - 1f) + Move) / (turnResistance + 1f);
            Length = Move.Length();
            if (Length > speed)
            {
                Move *= speed / Length;
            }
            proj.velocity = Move;
        }

    }
}
