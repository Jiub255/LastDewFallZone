using Godot;
using System;

namespace Lastdew
{
	public partial class PcInfoDisplay : PanelContainer
	{
		private TextureRect Icon { get; set; }
		private Label NameLabel { get; set; }
		private Label LevelLabel { get; set; }
		private AnimatedProgressBar ExpGained { get; set; }

		public void Setup(
			PlayerCharacter pc,
			ExperienceFormula formula,
			int beginningExp,
			int beginningInjury)
		{
			Icon = GetNode<TextureRect>("%Icon");
			NameLabel = GetNode<Label>("%Name");
			LevelLabel = GetNode<Label>("%Level");
			ExpGained = GetNode<AnimatedProgressBar>("%ExpGained");
			
			Icon.Texture = pc.Data.Icon;
			NameLabel.Text = pc.Data.Name;
			LevelLabel.Text = $"Lvl. {pc.StatManager.Experience.Level}";
			ExpGained.CallDeferred(
				AnimatedProgressBar.MethodName.Initialize,
				formula,
				beginningExp,
				pc.StatManager.Experience.Experience);
		}
	}
}
