#if TOOLS
using Godot;
using System.Collections.Generic;

namespace Lastdew
{
    [Tool]
    public partial class StatsDisplay : Label
    {
        private Dictionary<StatType, string> StatAbbreviationsByName = new Dictionary<StatType, string>()
        {
            { StatType.ATTACK, "Atk" },
            { StatType.DEFENSE, "Def" },
            { StatType.ENGINEERING, "Eng" },
            { StatType.FARMING, "Farm" },
            { StatType.MEDICAL, "Med" },
            { StatType.SCAVENGING, "Scav" },
        };
    
        public void Setup(Godot.Collections.Dictionary<StatType, int> statDict)
        {
            if (statDict == null)
            {
                Text = "";
                return;
            }
            List<string> stats = new();
            foreach (KeyValuePair<StatType, int> kvp in statDict)
            {
                stats.Add($"{StatAbbreviationsByName[kvp.Key]} {kvp.Value}");
            }
            string labelText = string.Join(", ", stats);
            Text = labelText;
        }
    }
}
#endif
