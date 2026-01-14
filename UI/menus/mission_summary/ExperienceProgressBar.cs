using Godot;
using System;

namespace Lastdew
{
	public partial class ExperienceProgressBar : VBoxContainer
	{
		public event Action<int> OnLevelUp;

		[Export]
		private float FillAnimationDuration { get; set; } = 2f;
	    private ProgressBar ProgressBar { get; set; }
	    private Label AmountLabel { get; set; }
	    private ExperienceFormula Formula { get; set; }
	    private int ExpGained { get; set; }
	    private double TimeBetweenExpGains { get; set; }
	    private double Timer { get; set; }

	    public override void _Ready()
	    {
		    base._Ready();
		    
		    ProgressBar = GetNode<ProgressBar>("%ExpBar");
		    AmountLabel = GetNode<Label>("%ExpLabel");
	    }

	    public void Initialize(ExperienceFormula formula, int beginningExp, int finalExp)
	    {
		    Formula = formula;
		    
		    int level = formula.LevelFromExperience(beginningExp);
		    (long startLvlExp, long nextLvlExp) endpoints = formula.ExperienceRangeFromLevel(level);
		    ProgressBar.MinValue = endpoints.startLvlExp;
		    ProgressBar.MaxValue = endpoints.nextLvlExp;
		    ProgressBar.Value = beginningExp;

		    AmountLabel.Text = $"{beginningExp}/{endpoints.nextLvlExp} XP";
		    
		    ExpGained = finalExp - beginningExp;
		    TimeBetweenExpGains = FillAnimationDuration / ExpGained;
	    }

	    public override void _Process(double delta)
	    {
		    base._Process(delta);

		    if (ExpGained <= 0)
		    {
			    return;
		    }
		    
		    Timer += delta;
		    
		    if (Timer >= TimeBetweenExpGains)
		    {
			    int expGains = Mathf.FloorToInt(Timer / TimeBetweenExpGains);
			    Timer -= expGains * TimeBetweenExpGains;
			    expGains = Mathf.Min(ExpGained, expGains);
			    AddExp(expGains);
			    ExpGained -= expGains;
		    }
	    }

	    private void AddExp(int exp)
	    {
		    ProgressBar.Value += exp;
		    int level = Formula.LevelFromExperience((long)ProgressBar.Value);
		    GD.Print($"Level: {level}");
		    (long startLvlExp, long nextLvlExp) endpoints = Formula.ExperienceRangeFromLevel(level);
		    GD.Print($"Value: {ProgressBar.Value}, Max: {ProgressBar.MaxValue}" +
		             $", Level? {ProgressBar.Value >= ProgressBar.MaxValue}");
		    if (ProgressBar.Value >= ProgressBar.MaxValue)
		    {
			    ProgressBar.MinValue = endpoints.startLvlExp;
			    ProgressBar.MaxValue = endpoints.nextLvlExp;
			    
			    OnLevelUp?.Invoke(level);
		    }
		    AmountLabel.Text = $"{(int)ProgressBar.Value}/{endpoints.nextLvlExp} XP";
	    }
	}
}
