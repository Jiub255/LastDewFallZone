using Godot;
using System;

namespace Lastdew
{
	public partial class ExitButton : Button
	{
		public event Action OnToStartMenu;
		public event Action OnToDesktop;

		public bool InStartMenu { get; set; } = true;
		
		private CanvasLayer ExitPopup { get; set; }
		private Button ToStartMenu { get; set; }
		private Button ToDesktop { get; set; }
		private Button Cancel { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
			ExitPopup = GetNode<CanvasLayer>("%ExitPopup");
			ToStartMenu = GetNode<Button>("%ToStartMenu");
			ToDesktop = GetNode<Button>("%ToDesktop");
			Cancel = GetNode<Button>("%Cancel");

			ExitPopup.Hide();

			Pressed += HandlePress;
			ToStartMenu.Pressed += ExitToStartMenu;
			ToDesktop.Pressed += ExitToDesktop;
			Cancel.Pressed += ExitPopup.Hide;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Pressed -= HandlePress;
            ToStartMenu.Pressed -= ExitToStartMenu;
            ToDesktop.Pressed -= ExitToDesktop;
            Cancel.Pressed -= ExitPopup.Hide;
        }

        private void HandlePress()
        {
            if (InStartMenu)
            {
				ExitToDesktop();
            }
            else
            {
				ExitPopup.Show();
            }
        }
        
        private void ExitToStartMenu()
        {
            OnToStartMenu?.Invoke();
            ExitPopup.Hide();
        }
        
        private void ExitToDesktop()
        {
            SceneTree tree = GetTree();
			tree.Root.PropagateNotification((int)NotificationWMCloseRequest);
			tree.Quit();
        }
    }
}
