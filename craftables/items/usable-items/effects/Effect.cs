using System.Reflection.Metadata.Ecma335;
using Godot;

namespace Lastdew
{	
	public abstract partial class Effect : Resource
	{
		public abstract void ApplyEffect(PlayerCharacter pc);
	}
}
