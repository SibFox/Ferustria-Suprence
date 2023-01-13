using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Ferustria.Content.Dusts
{
	public class Angelic_Particles_Gold : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.35f;
			dust.noGravity = true;
			dust.noLight = true;
            dust.velocity = Vector2.Clamp(dust.velocity, new Vector2(-2f, -2f), new Vector2(2f, 2f));
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity * .6f;
			dust.rotation += dust.velocity.X * .18f;
			dust.scale *= .987f;
			float light = 0.83f * dust.scale;
			if (light > 1f) light = 1f;
			Lighting.AddLight(dust.position, light * 0.85f, light * 0.8f, 0);
			if (dust.scale < .35f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}