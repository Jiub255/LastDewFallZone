using System;
using Godot;

namespace Lastdew
{
	public partial class PcButton : Button
	{
		public event Action<PlayerCharacter> OnSelectPc;
	
		private TextureRect PcIcon { get; set; }
		private RichTextLabel PcName { get; set; }
		private ProgressBar PainBar { get; set; }
		private ProgressBar InjuryBar { get; set; }
		private PlayerCharacter Pc { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			PcIcon = GetNode<TextureRect>("%Icon");
			PcName = GetNode<RichTextLabel>("%Name");
			PainBar = GetNode<ProgressBar>("%PainBar");
			InjuryBar = GetNode<ProgressBar>("%InjuryBar");
			
			Pressed += SelectPc;
		}
		
		public override void _ExitTree()
		{
			base._ExitTree();
			
			Pressed -= SelectPc;
			Pc.Health.OnHealthChanged -= SetHealthBars;
		}
		
		public void Setup(PlayerCharacter pc)
		{
			PcIcon.Texture = pc.Icon;
			PcName.Text = pc.Name;
			Pc = pc;
			
			SetHealthBars();
			
			Pc.Health.OnHealthChanged += SetHealthBars;
		}
		
		private void SetHealthBars(int _)
		{
			SetHealthBars();
		}
		
		public void SetHealthBars()
		{
			PainBar.Value = Pc.Health.Pain;
			InjuryBar.Value = Pc.Health.Injury;
		}
		
		private void SelectPc()
		{
			GetViewport().SetInputAsHandled();
			OnSelectPc?.Invoke(Pc);
		}
	}
}
