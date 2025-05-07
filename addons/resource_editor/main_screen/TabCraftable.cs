#if TOOLS
using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{
	[Tool]
	public abstract partial class TabCraftable : MarginContainer
	{
        public event Action<Craftable> OnOpenPopupPressed;

        protected VBoxContainer Parent { get; set; }
        protected Dictionary<long, CraftableDisplay> DisplaysByUid { get; private set; }
        protected ICollection<Craftable> Craftables { get; set; }
        protected PackedScene CraftableDisplayScene { get; set; }

        private Button NewButton { get; set; }

        public override void _Ready()
        {
            base._Ready();

            Parent = GetNode<VBoxContainer>("%Parent");
            NewButton = GetNode<Button>("%New");

            DisplaysByUid = [];
            Craftables = [];

            NewButton.Pressed += CreateNewCraftable;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            NewButton.Pressed -= CreateNewCraftable;
            foreach (CraftableDisplay display in DisplaysByUid.Values)
            {
                display.OnOpenPopupPressed -= OpenPopup;
            }
        }

        public void SetupResourceDisplays()
        {
            foreach (Craftable craftable in Craftables)
            {
                SetupNewDisplay(craftable);
            }
        }

        private void SetupNewDisplay(Craftable craftable)
        {
            CraftableDisplay display = (CraftableDisplay)CraftableDisplayScene.Instantiate();
            Parent.AddChild(display);
            this.PrintDebug($"Display type: {display.GetType()}, Craftable type: {craftable.GetType()}");
            display.Setup(craftable);
            // TODO: Unsubscribe on delete resource.
            display.OnOpenPopupPressed += OpenPopup;
            DisplaysByUid[craftable.GetUid()] = display;
        }

        public void UpdateDisplay(long uid)
        {
            Craftable craftable = Databases.CRAFTABLES[uid];
            if (DisplaysByUid.TryGetValue(uid, out CraftableDisplay craftableDisplay))
            {
                craftableDisplay.Setup(craftable);
            }
            else
            {
                SetupNewDisplay(craftable);
            }
        }

        protected void OpenPopup(Craftable craftable)
        {
            OnOpenPopupPressed?.Invoke(craftable);
        }

        protected abstract void CreateNewCraftable();
	}
}
#endif
