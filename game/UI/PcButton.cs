using Godot;

namespace Lastdew
{
	/// <summary>
	/// TODO: Add Pain/Injury bars to HUD.
	/// </summary>
	public partial class PcButton : Button
	{
		private TextureRect PcIcon { get; set; }
		private RichTextLabel PcName { get; set; }
		private ProgressBar PainBar { get; set; }
		private ProgressBar InjuryBar { get; set; }
		private PlayerCharacter PC { get; set; }
		private TeamData TeamData { get; set; }

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
			PC.Health.OnHealthChanged -= SetHealthBars;
		}
		
		public void Setup(PlayerCharacter pc, TeamData teamData)
		{
			PcIcon.Texture = pc.Icon;
			PcName.Text = pc.Name;
			PC = pc;
			TeamData = teamData;
			
			SetHealthBars();
			
			PC.Health.OnHealthChanged += SetHealthBars;
		}
		
		public void SetHealthBars()
		{
			PainBar.Value = PC.Health.Pain;
			InjuryBar.Value = PC.Health.Injury;
		}
		
		private void SelectPc()
		{
			TeamData.SelectPc(PC);
		}
	}
}
