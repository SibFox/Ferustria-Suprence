using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using Ferustria.Assets.ClassTemplates;

namespace Ferustria.Content.Projectiles.Friendly.HealProj
{
    public class Crimson_Heal : HealProjectile
    {
        public override void SetValues()
        {
            HealAmout = (1, 4);
            timeLeft = 120;
            speed = 17f;
            Light = (0.4f, 0.08f, 0.08f);
            SetTexture = "Assets/Textures/Projectiles/Crimson_Heal";
        }

        public override void CreateTrail()
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Blood, new(0, 0), 60, default, 1.2f);
        }
    }

}