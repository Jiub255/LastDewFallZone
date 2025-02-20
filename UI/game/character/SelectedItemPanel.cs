using System;
using Godot;

namespace Lastdew
{
	public partial class SelectedItemPanel : PanelContainer
	{
		public ItemButton ItemButton { get; private set; }
		public TextureRect ItemDisplay { get; private set; }
		public RichTextLabel Description { get; private set; }
		public Button UseEquip { get; private set; }
		public Button Drop { get; private set; }
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
		public void SetItem(ItemButton itemButton)
		{
			ItemButton = itemButton;
			ItemDisplay.Texture = itemButton.Item.Icon;
			Description.Text = itemButton.Item.Description;
			UseEquip.Visible = true;
			Drop.Visible = true;
			if (itemButton.Item is UsableItem)
			{
				UseEquip.Text = "Use";
			}
			else if (itemButton.Item is Equipment)
			{
				UseEquip.Text = "Equip";
			}
		}
		
		public void ClearItem()
		{
			ItemButton = null;
			ItemDisplay.Texture = null;
			Description.Text = "";
			UseEquip.Visible = false;
			Drop.Visible = false;
		}
	
		public override void _ExitTree()
		{
			base._ExitTree();
			
			UseEquip.Pressed -= UseOrEquipItem;
			Drop.Pressed -= DropItem;
		}
	
		private void UseOrEquipItem()
		{
			ItemButton.Item.OnClickItem(TeamData.Pcs[TeamData.MenuSelectedIndex]);
			if (ItemButton != null)
			{
				ItemButton.Amount--;
				if (ItemButton.Amount == 0)
				{
					ClearItem();
				}
			}
		}
	
		private void DropItem()
		{
			throw new NotImplementedException();
		}
	}
}
