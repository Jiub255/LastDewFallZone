using Godot;

namespace Lastdew
{
	public partial class PcDisplay : HBoxContainer
	{
		private TextureRect Icon { get; set; }
		private Label NameLabel { get; set; }
		
		public void Initialize(Texture2D icon, string name)
		{
			Icon = GetNode<TextureRect>("%Icon");
			NameLabel = GetNode<Label>("%Name");
			
			Icon.Texture = icon;
			NameLabel.Text = name;
		}
	}
}
