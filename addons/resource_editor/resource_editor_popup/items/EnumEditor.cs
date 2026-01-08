#if TOOLS
using System;
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class EnumEditor<T> : HBoxContainer where T : Enum
	{
        private T _enum;
    
        public T Enum
        {
            get => _enum;
            private set
            {
                _enum = value;
                if (MenuButton != null)
                {
                    MenuButton.Text = value.ToString().Capitalize();
                }
            }
        }
        
        private MenuButton MenuButton { get; set; }
        private Godot.PopupMenu PopupMenu { get; set; }
        
        public override void _Ready()
        {
            base._Ready();

            MenuButton = GetNode<MenuButton>("%MenuButton");
            
            PopupMenu = MenuButton.GetPopup();
            PopupMenu.Clear();
            foreach (T @enum in System.Enum.GetValues(typeof(T)))
            {
                PopupMenu.AddItem(@enum.ToString().Capitalize());
            }

            PopupMenu.IndexPressed += ChangeType;
        }

        public override void _ExitTree()
        {
            base._ExitTree();

            PopupMenu.IndexPressed -= ChangeType;
        }
        
        public void Setup(T @enum)
        {
            Enum = @enum;
        }

        private void ChangeType(long index)
        {
            Array values = System.Enum.GetValues(typeof(T));
            int value = 0;
            if (values.Length > index)
            {
                value = (int)values.GetValue(index)!;
            }
            else
            {
                GD.PushError($"Could not set enum, no value at index {index}");
            }

            Enum = (T)System.Enum.ToObject(typeof(T), value);
        }
    }
}
#endif
