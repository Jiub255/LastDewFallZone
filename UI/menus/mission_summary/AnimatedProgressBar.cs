using Godot;
using System;

namespace Lastdew
{
	public partial class AnimatedProgressBar : VBoxContainer
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
		    (int startLvlExp, int nextLvlExp) endpoints = formula.ExperienceRangeFromLevel(level);
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
		    // TODO: Check in case Timer goes multiple TimeBetweenExpGains's between ticks.
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
		    int level = Formula.LevelFromExperience((int)ProgressBar.Value);
		    (int startLvlExp, int nextLvlExp) endpoints = Formula.ExperienceRangeFromLevel(level);
		    if (ProgressBar.Value >= ProgressBar.MaxValue)
		    {
			    ProgressBar.MinValue = endpoints.startLvlExp;
			    ProgressBar.MaxValue = endpoints.nextLvlExp;
			    
			    // TODO: Connect to PcInfoDisplay to change level label
			    OnLevelUp?.Invoke(level);
		    }
		    AmountLabel.Text = $"{(int)ProgressBar.Value}/{endpoints.nextLvlExp} XP";
	    }
	}
}
