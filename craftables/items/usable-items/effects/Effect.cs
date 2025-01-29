using Godot;

namespace Lastdew
{
	[GlobalClass, Tool]
	public abstract partial class Effect : Resource
	{
		public abstract void ApplyEffect(PlayerCharacter pc);
	}
}
