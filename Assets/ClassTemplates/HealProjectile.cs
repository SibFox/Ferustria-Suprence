using Microsoft.Xna.Framework;
using Ferustria.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Ferustria.Assets.ClassTemplates
{
    public abstract class HealProjectile : ModProjectile
    {
        string _texture = "Ferustria/emptyPixel";
        public string SetTexture { get => _texture; set => _texture = Mod.Name + "/" + value; }

        public override string Texture => SetTexture;

        public (int min, int max) HealAmout
        {
            private get;
            set;
        }

        public int timeLeft = 260;
        public float speed = 20f;

        public (float r, float g, float b) Light
        {
            private get;
            set;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
        }

        public override void SetDefaults()
        {
            SetValues();
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.timeLeft = timeLeft;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.knockBack = 0f;
            Projectile.alpha = 255;
        }

        public abstract void SetValues();

        public override void AI()
        {
            Player player = null;
            if (Projectile.owner != -1)
            {
                player = Main.player[Projectile.owner];
            }
            else if (Projectile.owner == 255)
            {
                player = Main.LocalPlayer;
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            Lighting.AddLight(Projectile.position, Light.r, Light.g, Light.b);
            CreateTrail();
            Vector2 center = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
            Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(default) * speed;
            if ((player.Center - center).Length() < 50f && Projectile.position.X < player.position.X + player.width && Projectile.position.X + Projectile.width > player.position.X && Projectile.position.Y < player.position.Y + player.height && Projectile.position.Y + Projectile.height > player.position.Y)
            {
                if (Projectile.owner == Main.myPlayer && !player.moonLeech)
                {
                    int heal = Main.rand.Next(HealAmout.min - 1, HealAmout.max) + 1;
                    player.statLife += heal;
                    if (player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
                    player.HealEffect(heal);
                    NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, Projectile.owner, heal, 0.0f, 0.0f, 0, 0, 0);
                }
                Projectile.Kill();
            }
        }
        public virtual void CreateTrail()
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, new(0, 0), 60, default, 1f);
        }
    }
}