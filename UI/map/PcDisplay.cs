using Godot;

namespace Lastdew
{
	public partial class PcDisplay : HBoxContainer
	{
		private TextureRect Icon { get; set; }
		private Label NameLabel { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			Icon = GetNode<TextureRect>("%Icon");
			NameLabel = GetNode<Label>("%Name");
		}
		
		public void Initialize(Texture2D icon, string name)
		{
			Icon.Texture = icon;
			NameLabel.Text = name;
		}
	}
}
