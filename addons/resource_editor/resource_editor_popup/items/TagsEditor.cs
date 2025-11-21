#if TOOLS
using Godot;
using Godot.Collections;

namespace Lastdew
{
	[Tool]
	public partial class TagsEditor : HBoxContainer, IPropertyEditor
	{
        private HBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene TagButtonScene { get; } = GD.Load<PackedScene>(Uids.TAG_BUTTON);
        private Array<ItemTags> Tags
        {
            get
            {
                return GatherTags();
            }
        }

        public override void _Ready()
        {
            base._Ready();

            Parent = GetNode<HBoxContainer>("%Parent");
            Add = GetNode<Button>("%Add");

            Add.Pressed += NewTagButton;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Add.Pressed -= NewTagButton;
        }
        
        public void Setup(Array<ItemTags> itemTags)
        {
            ClearTags();
            foreach (ItemTags tag in itemTags)
            {
                NewTagButton(tag);
            }
        }

        private void ClearTags()
        {
            foreach (Node node in Parent.GetChildren())
            {
                if (node is TagButton tagButton)
                {
                    tagButton.QueueFree();
                }
            }
            Add.Show();
        }

        private Array<ItemTags> GatherTags()
        {
            Array<ItemTags> tags;
            tags = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is TagButton tagButton)
                {
                    tags.Add(tagButton.Tag);
                }
            }
            return tags;
        }
        
        private void NewTagButton()
        {
            NewTagButton(ItemTags.COMBAT);
        }
        
        private void NewTagButton(ItemTags tag)
        {
            int children = Parent.GetChildren().Count;
            if (children <= 3)
            {
                TagButton tagButton = (TagButton)TagButtonScene.Instantiate();
                Parent.AddChild(tagButton);
                tagButton.Tag = tag;
                tagButton.OnDelete += OnDeleteTag;
            }
            if (children == 3)
            {
                Add.Hide();
            }
        }
        
        private void OnDeleteTag(TagButton tagButton)
        {
            tagButton.OnDelete -= OnDeleteTag;
            Add.Show();
        }

        public void SetProperty(Craftable craftable)
        {
            if (craftable is Item item)
            {
                item.Set(Item.PropertyName.Tags, Tags);
            }
        }
    }
}
#endif
