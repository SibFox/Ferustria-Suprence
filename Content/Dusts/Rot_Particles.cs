using Terraria;
using Terraria.ModLoader;

namespace Ferustria.Content.Dusts
{
	public class Rot_Particles : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.35f;
			dust.noGravity = false;
			dust.noLight = true;
		}

		public override bool Update(Dust dust)
		{
			dust.velocity.X *= .8f;
			dust.rotation += dust.velocity.X * .2f;
			dust.scale *= .967f;
			if (dust.scale < .25f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}