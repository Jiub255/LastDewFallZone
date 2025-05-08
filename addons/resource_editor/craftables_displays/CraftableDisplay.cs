#if TOOLS
using System;
using Godot;

namespace Lastdew
{
    // TODO: Add delete craftable button. Pressing it shows a confirmation popup before deleting the resource.
    [Tool]
    public abstract partial class CraftableDisplay : PanelContainer
    {
        public event Action<Craftable> OnOpenPopupPressed;

        private Craftable Craftable;
        private TextureRect Icon { get; set; }
        protected Label NameLabel { get; set; }
        private Label Description { get; set; }
        private StatsDisplay StatsNeededToCraft { get; set; }
        private RecipeCostsDisplay RecipeCosts { get; set; }
        private RequiredBuildingsDisplay RequiredBuildings { get; set; }
        private ScrapResultsDisplay ScrapResults { get; set; }
        private Button DeleteButton { get; set; }
        private ConfirmationDialog Dialog { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
            Icon = GetNode<TextureRect>("%Icon");
            NameLabel = GetNode<Label>("%Name");
            Description = GetNode<Label>("%Description");
            StatsNeededToCraft = GetNode<StatsDisplay>("%StatsNeededToCraft");
            RecipeCosts = GetNode<RecipeCostsDisplay>("%RecipeCosts");
            RequiredBuildings = GetNode<RequiredBuildingsDisplay>("%RequiredBuildings");
            ScrapResults = GetNode<ScrapResultsDisplay>("%ScrapResults");
            DeleteButton = GetNode<Button>("%Delete");
            Dialog = GetNode<ConfirmationDialog>("%ConfirmationDialog");

            DeleteButton.Pressed += ConfirmDeleteCraftable;
            Dialog.Canceled += Dialog.Hide;
            Dialog.Confirmed += DeleteCraftable;
            
            // TODO:
            //MouseEntered += // Set to hover color. Use selfmodulate to not conflict with button modulate?
            //MouseExited += // Set back to normal
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            DeleteButton.Pressed -= ConfirmDeleteCraftable;
            Dialog.Canceled -= Dialog.Hide;
            Dialog.Confirmed -= DeleteCraftable;
        }

        public override void _GuiInput(InputEvent @event)
        {
            base._GuiInput(@event);
            
            if (@event is InputEventMouseButton button && button.ButtonIndex == MouseButton.Left)
            {
                if (button.Pressed)
                {
                    Modulate = new Color(0.65f, 0.65f, 0.65f, 1f);
                }
                else
                {
                    Modulate = new Color(1f, 1f, 1f, 1f);
                    OpenEditPopup();
                }
            }
        }

        public virtual void Setup(Craftable craftable)
        {
            Craftable = craftable;
            Icon.Texture = craftable.Icon;
            NameLabel.Text = craftable.Name;
            Description.Text = craftable.Description;
            Description.TooltipText = craftable.Description;
            StatsNeededToCraft.Setup(craftable.StatsNeededToCraft);
            RecipeCosts.Setup(craftable);
            RequiredBuildings.Setup(craftable);
            ScrapResults.Setup(craftable);
        }

        private void OpenEditPopup()
        {
            OnOpenPopupPressed?.Invoke(Craftable);
        }

        private void ConfirmDeleteCraftable()
        {
            Dialog.Show();
        }

        private void DeleteCraftable()
        {
            string path = Craftable.ResourcePath;
            string name = Craftable.Name;
            
            // Delete craftable from Craftables database.
            bool deletion_successful = Databases.CRAFTABLES.DeleteCraftable(Craftable);
            if (!deletion_successful)
            {
                GD.PushError($"Craftable {name} not deleted from Craftables Database.");
            }
            
            // Stop referencing craftable resource.
            Craftable = null;
            
            // Delete craftable display.
            QueueFree();
            
            // Delete craftable file.
            Error error = DirAccess.RemoveAbsolute(path);
            if (error != Error.Ok)
            {
                GD.PushError($"File not deleted successfully. {error}");
            }
            
            // Refresh FileSystem dock
            EditorInterface.Singleton.GetResourceFilesystem().Scan();
        }
    }
}
#endif
