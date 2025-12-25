using Godot;

namespace Lastdew
{
	[GlobalClass, Tool]
	public abstract partial class Effect : Resource
	{
		public abstract string Description { get; }
		public abstract string Abbreviation { get; }
		// TODO: Pass TeamData here instead. To get maximum stat data, etc. Can still get 
		// menu selected pc from TeamData.
		public abstract void ApplyEffect(PlayerCharacter pc);
	}
}
