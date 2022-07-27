using Terraria;
using Terraria.ModLoader;

namespace Ferustria.Content.Dusts
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
			if (light > 1f) light = 1f;
			Lighting.AddLight(dust.position, 0, light * 0.8f, light * 0.85f);
			if (dust.scale < .3f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}