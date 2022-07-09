using Terraria;
using Terraria.ModLoader;

namespace Ferustria.Dusts
{
	public class Angelic_Particles : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.35f;
			dust.noGravity = true;
			dust.noLight = true;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity * .6f;
			dust.rotation += dust.velocity.X * .18f;
			dust.scale *= .987f;
			float light = 0.83f * dust.scale;
			Lighting.AddLight(dust.position, light * 0.85f, light * 0.8f, 0);
			if (dust.scale < .23f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}