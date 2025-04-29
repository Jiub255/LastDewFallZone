#if TOOLS
using Godot;
using System;

namespace Lastdew
{
	[Tool]
	public partial class TabCraftingMaterials : MarginContainer
	{
        // TODO: Send this from each tab, then have the editor main screen check the types in whatever method they all get subscribed to.
        public event Action<Type> OnCreateNew;
	
        private VBoxContainer Parent { get; set; }
        private Button NewButton { get; set; }

        public override void _Ready()
        {
            base._Ready();

            Parent = GetNode<VBoxContainer>("%Parent");
            NewButton = GetNode<Button>("%New");

            NewButton.Pressed += CreateNewCraftable;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            NewButton.Pressed -= CreateNewCraftable;
        }
        
        private void CreateNewCraftable()
        {
            OnCreateNew?.Invoke(typeof(CraftingMaterial));
        }
	}
}
#endif
