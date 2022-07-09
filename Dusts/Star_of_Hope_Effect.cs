using Terraria;
using Terraria.ModLoader;

namespace Ferustria.Dusts
{
	public class Star_of_Hope_Effect : ModDust
	{
        public override string Texture => "Ferustria/Dusts/Angelic_Particles";

        public override void OnSpawn(Dust dust)
		{
			//dust.velocity *= 0.35f;
			dust.noGravity = true;
			dust.noLight = true;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity * .5f;
			//dust.velocity *= .98f;
			//dust.rotation += dust.velocity.X * .18f;
			dust.scale *= .96f;
			float light = 0.83f * dust.scale;
			Lighting.AddLight(dust.position, light * 0.85f, light * 0.8f, 0);
			if (dust.scale < .3f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}