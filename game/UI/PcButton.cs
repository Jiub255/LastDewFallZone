using Godot;
using System;

namespace Lastdew
{	
	public partial class PcButton : Button
	{
		public event Action OnClickPc;
		
		private new TextureRect Icon { get; set; }
		private new RichTextLabel Name { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			Icon = GetNode<TextureRect>("%Icon");
			Name = GetNode<RichTextLabel>("%Name");
			
			Pressed += SelectPc;
		}
	
		public override void _ExitTree()
		{
			base._ExitTree();
			
			Pressed -= SelectPc;
		}
		
		public void Setup(Texture2D icon, string name)
		{
			Icon.Texture = icon;
			Name.Text = name;
		}
		
		private void SelectPc()
		{
			OnClickPc?.Invoke();
		}
	}
}
