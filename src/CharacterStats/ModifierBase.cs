using System;

namespace CharacterStats
{
    public abstract class ModifierBase : ICloneable
    {
        public string Name { get; set; }

        protected ModifierBase(string name)
        {
            Name = name;
        }

        public void ApplyTo(StatBase stat)
        {
            if (AppliesTo(stat.Name))
            {
                stat.AddModifier(this.Clone() as ModifierBase);
            }
        }

        public abstract decimal Calculate(string statName, decimal indecimal);
        public abstract void OnUpdate(double delta, int frames);
        public abstract bool StillValid();
        public abstract object Clone();

        protected abstract bool AppliesTo(string statName);
    }
}