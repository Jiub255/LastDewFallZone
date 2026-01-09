using Godot;

namespace Lastdew
{
	public partial class LootDisplay : PanelContainer
	{
		private TextureRect Icon { get; set; }
		private Label NameLabel { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			Icon = GetNode<TextureRect>("%Icon");
			NameLabel = GetNode<Label>("%Name");
		}

		public void SetupDisplay(Texture2D icon, string amountAndName)
		{
			Icon.Texture = icon;
			NameLabel.Text = amountAndName;
		}
	}
}
