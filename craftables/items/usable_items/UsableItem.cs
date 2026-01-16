using Godot;
using Godot.Collections;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public partial class UsableItem : Item
	{
		[Export]
		public bool Reusable { get; private set; }

		[Export]
		public Array<Effect> Effects { get; private set; }

		public override void OnClickItem(TeamData teamData)
		{
			PlayerCharacter pc = teamData.Pcs[teamData.MenuSelectedIndex];
			pc.UseItem(this, teamData);
		}
	}
}
