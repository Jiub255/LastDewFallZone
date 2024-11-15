using Godot;

public partial class GameMenu : CanvasLayer
{
	private InventoryManager InventoryManager { get; set; }
	
	public void Initialize(InventoryManager inventoryManager)
	{
		InventoryManager = inventoryManager;
	}
}
