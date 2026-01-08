using System;
using Godot;

namespace Lastdew
{
	public partial class SelectedItemPanel : PanelContainer
	{
		private const string USE_TEXT = "Use";
		private const string EQUIP_TEXT = "Equip";
		
		public ItemButton ItemButton { get; private set; }
		public TextureRect ItemDisplay { get; private set; }
		public RichTextLabel Description { get; private set; }
		public SfxButton UseEquip { get; private set; }
		public SfxButton Drop { get; private set; }
		private TeamData TeamData { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
	
			ItemDisplay = GetNode<TextureRect>("%ItemIcon");
			Description = GetNode<RichTextLabel>("%DescriptionLabel");
			UseEquip = GetNode<SfxButton>("%UseEquipButton");
			Drop = GetNode<SfxButton>("%DropButton");
	
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
			UseEquip.Text = itemButton.Item switch
			{
				UsableItem => USE_TEXT,
				Equipment => EQUIP_TEXT,
				_ => UseEquip.Text
			};
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
			if (ItemButton == null)
			{
				return;
			}
			
			ItemButton.Item?.OnClickItem(TeamData);
			ItemButton.Amount--;
			if (ItemButton.Amount <= 0)
			{
				ClearItem();
			}
			// TODO: Refresh Character tab, at least the item grid part. 
		}
	
		private void DropItem()
		{
			throw new NotImplementedException();
		}
	}
}
