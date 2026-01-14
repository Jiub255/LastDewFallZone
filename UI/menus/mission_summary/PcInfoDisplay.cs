using Godot;

namespace Lastdew
{
	public partial class PcInfoDisplay : PanelContainer
	{
		private TextureRect Icon { get; set; }
		private Label NameLabel { get; set; }
		private Label LevelLabel { get; set; }
		private ExperienceProgressBar ExperienceBar { get; set; }
		private InjuryProgressBar InjuryBar { get; set; }

		public void Setup(
			PlayerCharacter pc,
			ExperienceFormula formula,
			int beginningExp,
			int beginningInjury)
		{
			Icon = GetNode<TextureRect>("%Icon");
			NameLabel = GetNode<Label>("%Name");
			LevelLabel = GetNode<Label>("%Level");
			ExperienceBar = GetNode<ExperienceProgressBar>("%ExpGained");
			InjuryBar = GetNode<InjuryProgressBar>("%Injury");
			
			Icon.Texture = pc.Data.Icon;
			NameLabel.Text = pc.Data.Name;
			LevelLabel.Text = $"Lvl. {pc.StatManager.Experience.Level}";
			ExperienceBar.CallDeferred(
				ExperienceProgressBar.MethodName.Initialize,
				formula,
				beginningExp,
				pc.StatManager.Experience.Experience);
			InjuryBar.CallDeferred(
				InjuryProgressBar.MethodName.Initialize,
				beginningInjury,
				pc.StatManager.Health.Injury);

			ExperienceBar.OnLevelUp += SetLevelText;
		}

		public override void _ExitTree()
		{
			base._ExitTree();

			if (ExperienceBar != null)
			{
				ExperienceBar.OnLevelUp -= SetLevelText;
			}
		}

		private void SetLevelText(int level)
		{
			LevelLabel.Text = $"Lvl. {level}";
		}
	}
}
