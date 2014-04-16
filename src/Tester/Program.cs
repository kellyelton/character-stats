using System;
using System.Collections.Generic;
using System.Threading;
using CharacterStats;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var c1 = new JimCharacter();

            var startVitality = c1.Vitality.Value;
            var startEndurance = c1.Endurance.Value;
            for (var i = 0; i < 100; i++)
            {
                if (i == 50)
                {
                    c1.AddModifier(new Modifiers.TurtleDowns());
                }

                c1.Vitality.Value = i + startVitality;
                c1.Endurance.Value = i + startEndurance;

                c1.OnUpdate(0f, i);

                Console.Clear();
                Console.WriteLine(c1);
                Thread.Sleep(100);
            }
            Console.ReadKey();
        }
    }

    public class NormalCharacter : CharacterBase
    {
        public StatTypes.Vitality Vitality { get; set; }
        public StatTypes.Strength Strength { get; set; }
        public StatTypes.Endurance Endurance { get; set; }
        public StatTypes.Intelligence Intelligence { get; set; }

        public StatTypes.MaxHealth MaxHealth { get; set; }
        public CalculatedStat MaxStamina { get; set; }

        public NormalCharacter(string name, string type, IEnumerable<string> tags)
            : base(name, type, tags)
        {
            Vitality = new StatTypes.Vitality(100);
            Strength = new StatTypes.Strength(30);
            Endurance = new StatTypes.Endurance(40);
            Intelligence = new StatTypes.Intelligence(50);

            MaxHealth = new StatTypes.MaxHealth(this);

            MaxStamina = new StatTypes.MaxStamina(this);
        }
    }

    public class JimCharacter : NormalCharacter
    {
        public JimCharacter()
            : base("Jim", "Warlock", new[] { "player", "magic" })
        {
        }
    }
}
