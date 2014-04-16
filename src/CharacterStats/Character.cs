using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharacterStats
{
    public abstract class CharacterBase : IGameObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<string, StatBase> Stats { get; private set; }

        public StatBase GetStat(string name)
        {
            return Stats[name];
        }

        protected CharacterBase(string name, string type, IEnumerable<string> tags)
        {
            Name = name;
            Type = type;
            Tags = new List<string>(tags);
            Stats = this.GetType()
                .GetProperties()
                .Where(x => x.PropertyType.IsSubclassOf(typeof(StatBase)))
                .Select(x => x.GetValue(this, null) as StatBase)
                .ToDictionary(x => x.Name, x => x);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("========= {0}({1}) =========\n", Name, Type);
            sb.AppendFormat("Tags		{0}\n", String.Join(",", Tags));
            foreach (var s in Stats)
            {
                if (s.Value is CalculatedStat)
                    sb.AppendFormat("# {0}		{1}\n", s.Key, s.Value.Value);
                else
                    sb.AppendFormat("{0}		{1}\n", s.Key, s.Value.Value);
            }
            return sb.ToString();
        }

        public void AddModifier(ModifierBase mod)
        {
            foreach (var s in Stats)
            {
                mod.ApplyTo(s.Value);
            }
        }

        public void OnUpdate(double delta, int frames)
        {
            foreach (var s in Stats)
            {
                s.Value.OnUpdate(delta,frames);
            }
        }
    }
}