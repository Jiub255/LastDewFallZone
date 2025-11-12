using Godot;

namespace Lastdew
{
	public partial class LocationInfoUi : VBoxContainer
	{
		private Label PlaceName { get; set; }
		private TextureRect Image { get; set; }
		private Label Description { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			PlaceName = GetNode<Label>("%PlaceName");
			Image = GetNode<TextureRect>("%Image");
			Description = GetNode<Label>("%Description");
		}
		
		public void Setup(string name, Texture2D image, string description)
		{
			PlaceName.Text = name;
			Image.Texture = image;
			Description.Text = description;
		}
	}
}
