using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class CraftingMaterialAmount : Resource
	{
		[Export]
		public CraftingMaterial Material { get; set; }
		[Export]
		public int Amount { get; set;}
		
		public CraftingMaterialAmount(){}
		
		public CraftingMaterialAmount(CraftingMaterial material, int amount)
		{
			Material = material;
			Amount = amount;
		}
	}
}
