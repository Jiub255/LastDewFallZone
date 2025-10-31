#if TOOLS
using Godot;
using Godot.Collections;

namespace Lastdew
{
	[Tool]
	public partial class EffectsEditor : HBoxContainer, IPropertyEditor
	{
        private HBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene EffectButtonScene { get; } = GD.Load<PackedScene>(UiDs.EFFECT_BUTTON);
        private Array<Effect> Effects 
        {
            get
            {
                return GatherEffects();
            }
        }

        public override void _Ready()
        {
            base._Ready();

            Parent = GetNode<HBoxContainer>("%Parent");
            Add = GetNode<Button>("%Add");

            Add.Pressed += NewEffect;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Add.Pressed -= NewEffect;
        }
        
        public void Setup(Array<Effect> effects)
        {
            ClearEffectButtons();
            foreach (Effect effect in effects)
            {
                NewEffect(effect);
            }
        }

        public void SetProperty(Craftable craftable)
        {
            if (craftable is UsableItem usableItem)
            {
                usableItem.Set(UsableItem.PropertyName.Effects, Effects);
            }
        }

        private Array<Effect> GatherEffects()
        {
            Array<Effect> effects;
            effects = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is EffectButton effectButton && effectButton.Effect != null)
                {
                    effects.Add(effectButton.Effect);
                }
            }
            return effects;
        }

        private void NewEffect(Effect effect)
        {
            int children = Parent.GetChildren().Count;
            if (children <= 3)
            {
                EffectButton effectButton = (EffectButton)EffectButtonScene.Instantiate();
                Parent.AddChild(effectButton);
                effectButton.Setup(effect);
                effectButton.OnDelete += OnRemoveEffect;
            }
            if (children == 3)
            {
                Add.Hide();
            }
        }

        private void NewEffect()
        {
            NewEffect(null);
        }
        
        private void OnRemoveEffect(EffectButton effectButton)
        {
            effectButton.OnDelete -= OnRemoveEffect;
        }
        
        private void ClearEffectButtons()
        {
            foreach (Node node in Parent.GetChildren())
            {
                if (node is EffectButton effectButton)
                {
                    effectButton.QueueFree();
                }
            }
            Add.Show();
        }
    }
}
#endif
