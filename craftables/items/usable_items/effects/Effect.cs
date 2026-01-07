using Godot;

namespace Lastdew
{
	[GlobalClass, Tool]
	public abstract partial class Effect : Resource
	{
		public abstract string Description { get; }
		public abstract string Abbreviation { get; }
		public abstract void ApplyEffect(TeamData teamData);
	}
}
