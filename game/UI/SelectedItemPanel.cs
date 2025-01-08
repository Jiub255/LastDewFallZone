using System;
using Godot;

namespace Lastdew
{	
	public partial class SelectedItemPanel : PanelContainer
	{
		public Item Item { get; private set; }
		public TextureRect ItemDisplay { get; private set; }
		public RichTextLabel Description { get; private set; }
		private Button UseEquip { get; set; }
		private Button Drop { get; set; }
		private TeamData TeamData { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
	
			ItemDisplay = GetNode<TextureRect>("%ItemIcon");
			Description = GetNode<RichTextLabel>("%DescriptionLabel");
			UseEquip = GetNode<Button>("%UseEquipButton");
			Drop = GetNode<Button>("%DropButton");
	
			UseEquip.Pressed += UseOrEquipItem;
			Drop.Pressed += DropItem;
		}
		
		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
		}
	
		// TODO: Add amount to display ui?
		public void SetItem(Item item, int amount)
		{
			Item = item;
			ItemDisplay.Texture = item.Icon;
			Description.Text = item.Description;
			if (item is UsableItem)
			{
				UseEquip.Text = "Use";
			}
			else
			{
				UseEquip.Text = "Equip";
			}
		}
	
		public override void _ExitTree()
		{
			base._ExitTree();
			
			UseEquip.Pressed -= UseOrEquipItem;
			Drop.Pressed -= DropItem;
		}
	
		private void UseOrEquipItem()
		{
			Item.OnClickItem(TeamData.Pcs[TeamData.MenuSelectedIndex]);
		}
	
		private void DropItem()
		{
			throw new NotImplementedException();
		}
	}
}
