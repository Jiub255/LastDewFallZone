using Godot;
using System;

namespace Lastdew
{
	public partial class PcDisplay : HBoxContainer
	{
		public event Action<PcDisplay> OnRemovePc;
		
		public PlayerCharacter Pc { get; private set; }
		private TextureRect Icon { get; set; }
		private Label NameLabel { get; set; }
		private Button RemoveButton { get; set; }

		public override void _ExitTree()
		{
			base._ExitTree();
			
			RemoveButton.Pressed -= RemovePc;
		}

		public void Initialize(PlayerCharacter pc)
		{
			Icon = GetNode<TextureRect>("%Icon");
			NameLabel = GetNode<Label>("%Name");
			RemoveButton = GetNode<Button>("%RemoveButton");
			RemoveButton.Pressed += RemovePc;
			
			if (pc == null)
			{
				GD.PushError("PlayerCharacter is null");
				return;
			}
			
			Pc = pc;
			Icon.Texture = pc.Data.Icon;
			NameLabel.Text = pc.Data.Name;
		}

		private void RemovePc()
		{
			OnRemovePc?.Invoke(this);
		}
	}
}
