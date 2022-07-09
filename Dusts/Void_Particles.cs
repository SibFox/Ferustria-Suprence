using Terraria;
using Terraria.ModLoader;

namespace Ferustria.Dusts
{
	public class Void_Particles : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.25f;
			dust.noGravity = true;
			dust.noLight = true;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity * .6f;
			dust.rotation += dust.velocity.X * .1f;
			dust.scale *= .98f;
			float light = 0.72f * dust.scale;
			Lighting.AddLight(dust.position, light * 0.8f, 0, light);
			if (dust.scale < .2f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}