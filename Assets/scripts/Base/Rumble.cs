
namespace GameExtensions
{
    public static class Rumble
    {
        private static readonly Rumbler Rumbler = Rumbler.Instance;

        private static readonly float[] Strengths = 
        {
            0.1f,
            0.25f,
            0.5f,
            0.75f,
            1f
        };

        public static void RumbleFor(float small, float large, float timeSeconds = 1)
        {
            Rumbler.Rumble(small,large);
            Rumbler.Invoke(nameof(Rumbler.StopRumbling),timeSeconds);
        }

        public static void RumbleFor(RumbleStrength small, RumbleStrength large, float timeSeconds = 1)
        {
            RumbleFor(Strengths[(int)small],Strengths[(int)large],timeSeconds);
        }

        public enum RumbleStrength
        {
            Light,
            Moderate,
            Medium,
            Strong,
            Maximum
        }
    }
}