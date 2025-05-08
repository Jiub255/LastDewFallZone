#if TOOLS
using Godot;
using System;

namespace Lastdew
{
	[Tool]
	public partial class EffectButton : Button
	{
        public event Action<EffectButton> OnDelete;

        private Effect _effect;
        
        public Effect Effect
        {
            get => _effect;
            set
            {
                _effect = value;
                Text = value?.Abbreviation;
                TooltipText = value?.Description;
            }
        }
	
        private Button DeleteButton { get; set; }
        private EditorInterface EditorInterface { get; } = EditorInterface.Singleton;
        private Callable SetEffectCallable { get; set; }

        public override void _Ready()
        {
            base._Ready();

            DeleteButton = GetNode<Button>("%Delete");

            SetEffectCallable = new Callable(this, MethodName.SetEffect);
            
            DeleteButton.Pressed += Delete;
            Pressed += ChooseEffect;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            DeleteButton.Pressed -= Delete;
            Pressed -= ChooseEffect;
        }
        
        public void Setup(Effect effect)
        {
            Effect = effect;
        }
        
        private void ChooseEffect()
        {
            EditorInterface.PopupQuickOpen(SetEffectCallable, ["Effect"]);
        }
        
        private void SetEffect(string path)
        {
            Effect = ResourceLoader.Load<Effect>(path);
        }
        
        private void Delete()
        {
            OnDelete?.Invoke(this);
            QueueFree();
        }
	}
}
#endif
