using Godot;
using System;

namespace Lastdew
{
	public partial class ExitButton : SfxButton
	{
		public event Action OnToStartMenu;

		public bool InStartMenu { get; set; } = true;
		
		private CanvasLayer ExitPopup { get; set; }
		private SfxButton ToStartMenu { get; set; }
		private SfxButton ToDesktop { get; set; }
		private SfxButton Cancel { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
			ExitPopup = GetNode<CanvasLayer>("%ExitPopup");
			ToStartMenu = GetNode<SfxButton>("%ToStartMenu");
			ToDesktop = GetNode<SfxButton>("%ToDesktop");
			Cancel = GetNode<SfxButton>("%Cancel");

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
