#if TOOLS
using Godot;
using System.Collections.Generic;

namespace Lastdew
{
    [Tool]
    public partial class EffectsDisplay : Label
    {
        public void Setup(Effect[] effects)
        {
            List<string> effectStrings = new();
            foreach (Effect effect in effects)
            {
                if (effect is RelievePainEffect relievePain)
                {
                    effectStrings.Add($"Relieve pain {relievePain.ReliefAmount} for {relievePain.Duration} s");
                }
            }
            string labelText = string.Join(", ", effectStrings);
            Text = labelText;
        }
    }
}
#endif
