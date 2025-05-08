#if TOOLS
using System;
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class TagButton : PanelContainer
	{
        public event Action<TagButton> OnDelete;
	
        private ItemTags _tag = ItemTags.COMBAT;
        public ItemTags Tag
        {
            get => _tag; 
            set
            {
                _tag = value;
                if (MenuButton != null)
                {                    
                    MenuButton.Text = value.ToString().ToUpper()[..3];
                }
            }
        }
        
        private Button DeleteButton { get; set; }
        private MenuButton MenuButton { get; set; }
        private PopupMenu PopupMenu { get; set; }

        public override void _Ready()
        {
            base._Ready();

            DeleteButton = GetNode<Button>("%Delete");
            MenuButton = GetNode<MenuButton>("%MenuButton");
            
            PopupMenu = MenuButton.GetPopup();
            PopupMenu.Clear();
            foreach (ItemTags tag in Enum.GetValues<ItemTags>())
            {
                PopupMenu.AddItem(tag.ToString().Capitalize());
            }

            PopupMenu.IndexPressed += ChangeTag;
            DeleteButton.Pressed += Delete;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            PopupMenu.IndexPressed -= ChangeTag;
            DeleteButton.Pressed -= Delete;
        }
        
        private void ChangeTag(long index)
        {
            Tag = (ItemTags)index;
        }
        
        private void Delete()
        {
            OnDelete?.Invoke(this);
            QueueFree();
        }
	}
}
#endif
