using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ferustria.Content.Projectiles.Friendly
{
	public class Testing_Projectile_FromGround : ModProjectile
	{
		public override string Texture => Ferustria.Paths.TexturesPathPrj + "A_poop_QM";


		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 80;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.alpha = 0;
			Projectile.scale = 1.2f;
		}


		public override void OnKill(int timeLeft)
		{
            if (Projectile.ai[0] != 1)
            {
                Point globalPosition = new((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16);

                for (int y = globalPosition.Y - 15; y < globalPosition.Y + 16; y++)
                {
                    for (int x = globalPosition.X - 15; x < globalPosition.X + 16; x++)
                    {
                        Tile checkingTile = Main.tile[x, y];
                        Tile tileAbove = Main.tile[x, y - 1];
                        Tile leftTile = Main.tile[x - 1, y];
                        Tile rightTile = Main.tile[x + 1, y];
                        if (checkingTile.HasTile)
                            if (Main.tileSolid[checkingTile.TileType] && checkingTile.TileType != TileID.Platforms)
                                if (!tileAbove.HasTile)
                                {
                                    if (++Projectile.localAI[0] % 5 == 0)
                                    {
                                        Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), (x * 16, y * 16), (0f, -4.5f), Projectile.type, Projectile.damage, 0.5f,
                                        Projectile.owner, 1f);
                                        proj.tileCollide = false;
                                        //Mod.Logger.Debug($"Checked Tile: {checkingTile};\n Above Tile: {tileAbove};\n X and Y: {x} : {y};\n" +
                                        //    $"New Projectile pos: {proj.position / 16};\nNew Projectile vel: {proj.velocity};" +
                                        //    $"Projectile pos divided: {Projectile.position / 16} ({globalPosition});\nPlayers pos divided: {Main.player[Projectile.owner].position / 16};\n");
                                    }
                                }
                    }
                }
            }
		}

		public override void AI()
        {
            if (Projectile.ai[0] != 1f) Projectile.tileCollide = Projectile.timeLeft < 73; 
            Projectile.netUpdate = true;
            Projectile.SetStraightRotation();
            Projectile.velocity *= 0.985f;
        }
	}

}