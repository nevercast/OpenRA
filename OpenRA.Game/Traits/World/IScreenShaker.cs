namespace OpenRA.Traits
{
	public interface IScreenShaker
	{
		void AddEffect(int time, WPos position, int intensity);

		void AddEffect(int time, WPos position, int intensity, float2 multiplier);
	}
}
