using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using Ferustria.Assets.ClassTemplates;

namespace Ferustria.Content.Projectiles.Friendly.HealProj
{
    public class Neon_Heal : HealProjectile
    {
        public override void SetValues()
        {
            HealAmout = (4, 8);
            timeLeft = 240;
            speed = 22f;
            Light = (0, 0.4f, 0.4f);
            SetTexture = "Assets/Textures/Neon_Heal";
        }

        public override void CreateTrail()
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, new(0, 0), 60, new(0, Main.rand.Next(180, 256), 255), 1f);
        }
    }
}


