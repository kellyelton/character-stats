using System;
using System.Linq;
using CharacterStats;

namespace Tester
{
    public class StatTypes
    {
        public class Stat : StatBase
        {
            private decimal _value;

            public Stat(string name, decimal value)
                : base(name)
            {
                Value = value;
            }

            protected override decimal GetValue()
            {
                return _value;
            }

            protected override void SetValue(decimal val)
            {
                _value = val;
            }

            protected override void OnCharacterUpdate(CharacterBase character)
            {
                throw new NotImplementedException();
            }
        }

        public class Vitality : Stat
        {
            public Vitality(decimal value)
                : base("Vitality", value)
            {
            }
        }
        public class Strength : Stat
        {
            public Strength(decimal value)
                : base("Strength", value)
            {
            }
        }
        public class Endurance : Stat
        {
            public Endurance(decimal value)
                : base("Endurance", value)
            {
            }
        }
        public class Intelligence : Stat
        {
            public Intelligence(decimal value)
                : base("Intelligence", value)
            {
            }
        }

        public class MaxHealth : CalculatedStat
        {
            private StatBase _vitality;

            public MaxHealth(CharacterBase character)
                : base("MaxHealth", getValue)
            {
                _vitality = character.GetStat("Vitality");
                this.LinkStat(_vitality);
            }

            private static decimal getValue(CalculatedStat stat)
            {
                var s = stat.LinkedStats.FirstOrDefault(x => x.Name == "Vitality");
                return s.Value / 10;
            }
        }

        public class MaxStamina : CalculatedStat
        {
            private StatBase _endurance;

            public MaxStamina(CharacterBase character)
                : base("MaxStamina", getValue)
            {
                _endurance = character.GetStat("Endurance");
                this.LinkStat(_endurance);
            }

            private static decimal getValue(CalculatedStat stat)
            {
                var s = stat.LinkedStats.FirstOrDefault(x => x.Name == "Endurance");
                return s.Value / 13;
            }
        }
    }
}