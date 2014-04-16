using CharacterStats;

namespace Tester
{
    public class Modifiers
    {
        public class TurtleDowns : ModifierBase
        {
            private int _frameCount;

            public TurtleDowns()
                : base("Turtle Downs")
            {
            }

            public override object Clone()
            {
                return new TurtleDowns();
            }

            protected override bool AppliesTo(string statName)
            {
                return statName == "Strength" || statName == "Intelligence";
            }

            public override void OnUpdate(double delta, int frames)
            {
                _frameCount++;
            }

            public override bool StillValid()
            {
                return _frameCount < 10;
            }

            public override decimal Calculate(string statName, decimal indecimal)
            {
                return indecimal * 0.10m;
            }
        }
    }
}