using Godot;
using System;

namespace Lastdew
{	
	public partial class PcButton : Button
	{
		private new TextureRect Icon { get; set; }
		private new RichTextLabel Name { get; set; }
		private PlayerCharacter PC { get; set; }
		private TeamData TeamData { get; set; }
	
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
		
		public void Setup(PlayerCharacter pc, TeamData teamData)
		{
			Icon.Texture = pc.Icon;
			Name.Text = pc.Name;
			PC = pc;
			TeamData = teamData;
		}
		
		private void SelectPc()
		{
			TeamData.SelectPc(PC);
		}
	}
}
