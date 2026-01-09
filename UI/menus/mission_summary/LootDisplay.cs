using Godot;

namespace Lastdew
{
	public partial class LootDisplay : PanelContainer
	{
		private TextureRect Icon { get; set; }
		private Label NameLabel { get; set; }


		public void SetupDisplay(Texture2D icon, string amountAndName)
		{
			Icon = GetNode<TextureRect>("%Icon");
			NameLabel = GetNode<Label>("%Name");
			
			Icon.Texture = icon;
			NameLabel.Text = amountAndName;
		}
	}
}
