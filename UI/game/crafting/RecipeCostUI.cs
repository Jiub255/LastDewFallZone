using Godot;
using Lastdew;

public partial class RecipeCostUi : HBoxContainer
{
	private TextureRect Icon { get; set; }
	private Label ItemName { get; set; }
	private Label Amount { get; set; }
	
	public void Initialize(CraftingMaterial material, int owned, int required)
	{
		Icon = GetNode<TextureRect>("%Icon");
		ItemName = GetNode<Label>("%Name");
		Amount = GetNode<Label>("%Amount");
		
		Icon.Texture = material.Icon;
		ItemName.Text = material.Name;
		Amount.Text = $"{owned} / {required}";
	}
}
