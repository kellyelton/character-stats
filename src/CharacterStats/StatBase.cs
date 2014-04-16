using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace CharacterStats
{
    public abstract class StatBase : IUpdatable
    {
        protected ObservableCollection<ModifierBase> Modifiers { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Value
        {
            get { return GetValue(); }
            set
            {
                if (value == GetValue())
                    return;
                SetValue(value);
                OnStatChange();
            }
        }

        protected StatBase(string name)
        {
            Name = name;
            Modifiers = new ObservableCollection<ModifierBase>();
        }

        public void AddModifier(ModifierBase mod)
        {
            Modifiers.Add(mod);
        }

        protected abstract decimal GetValue();
        protected abstract decimal GetBaseValue();
        protected abstract void SetValue(decimal val);
        protected abstract void OnCharacterUpdate(CharacterBase character);

        public event PropertyChangedEventHandler StatChanged;

        protected virtual void OnStatChange()
        {
            PropertyChangedEventHandler handler = StatChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(Name));
        }

        public void OnUpdate(double delta, int frames)
        {
            var startval = GetValue();
            var val = startval;
            foreach (var m in Modifiers.ToArray())
            {
                if (m.StillValid() == false)
                {
                    Modifiers.Remove(m);
                    continue;
                }
                val = m.Calculate(Name,val);
                m.OnUpdate(delta, frames);
            }
            if (val != startval)
            {
                
            }
        }
    }

    public class CalculatedStat : StatBase
    {
        public List<StatBase> LinkedStats { get; set; }
        private readonly Func<CalculatedStat, decimal> _getValue;
        private bool _recalculate;
        private decimal _value;

        public CalculatedStat(string name, Func<CalculatedStat, decimal> getValue)
            : base(name)
        {
            _recalculate = true;
            _getValue = getValue;
            LinkedStats = new List<StatBase>();
        }

        public void LinkStat<T>(T character, Expression<Func<T, StatBase>> link)
        {
            var stat = link.Compile().Invoke(character);
            LinkStat(stat);
        }

        public void LinkStat(StatBase stat)
        {
            if (stat == null)
                throw new ArgumentException("Invalid link", "stat");
            LinkedStats.Add(stat);
            stat.StatChanged += StatOnStatChanged;
        }

        private void StatOnStatChanged(object sender, PropertyChangedEventArgs stat)
        {
            _recalculate = true;
        }

        protected override void OnCharacterUpdate(CharacterBase character)
        {
            _recalculate = true;
        }

        protected override decimal GetValue()
        {
            if (_recalculate)
            {
                _value = _getValue(this);
                _recalculate = false;
            }
            return _value;
        }

        protected override void SetValue(decimal val)
        {
            throw new NotImplementedException("You cannot set the value of a CalculatedStat");
        }
    }
}