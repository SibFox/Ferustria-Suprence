using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Ferustria.Content.Dusts
{
	public class Angelic_Particles_Stable : ModDust
	{
        public override string Texture => "Ferustria/Content/Dusts/Angelic_Particles";
        public override void OnSpawn(Dust dust)
		{
			dust.velocity = Vector2.Zero;
			dust.noGravity = true;
			dust.noLight = true;
            dust.velocity = Vector2.Clamp(dust.velocity, new Vector2(-2f, -2f), new Vector2(2f, 2f));
        }

		public override bool Update(Dust dust)
		{
			dust.scale *= .987f;
			float light = 0.83f * dust.scale;
			if (light > 1f) light = 1f;
			Lighting.AddLight(dust.position, 0, light * 0.8f, light * 0.85f);
			if (dust.scale < .35f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}