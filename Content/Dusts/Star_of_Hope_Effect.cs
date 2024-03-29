﻿using Terraria;
using Terraria.ModLoader;

namespace Ferustria.Content.Dusts
{
	public class Star_of_Hope_Effect : ModDust
	{
        public override string Texture => "Ferustria/Content/Dusts/Angelic_Particles";

        public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = true;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity * .5f;
			dust.velocity *= .98f;
			//dust.rotation += dust.velocity.X * .18f;
			dust.scale *= .975f;
			float light = 0.83f * dust.scale;
			Lighting.AddLight(dust.position, 0, light * 0.8f, light * 0.85f);
			if (dust.scale < .3f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}