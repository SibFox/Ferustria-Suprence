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
        }

		public override bool Update(Dust dust)
		{
			dust.scale *= .95f;
			if (dust.scale < .35f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}