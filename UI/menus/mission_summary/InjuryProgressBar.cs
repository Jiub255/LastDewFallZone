using Godot;
using System;

namespace Lastdew
{
	public partial class InjuryProgressBar : VBoxContainer
	{
		[Export]
		private float FillAnimationDuration { get; set; } = 2f;
	    private ProgressBar ProgressBar { get; set; }
	    private Label AmountLabel { get; set; }
	    private int InjuryReceived { get; set; }
	    private double TimeBetweenIncreases { get; set; }
	    private double Timer { get; set; }

	    public override void _Ready()
	    {
		    base._Ready();
		    
		    ProgressBar = GetNode<ProgressBar>("%InjuryBar");
		    AmountLabel = GetNode<Label>("%InjuryLabel");
	    }

	    public void Initialize(int beginningInjury, int finalInjury)
	    {
		    ProgressBar.Value = beginningInjury;

		    AmountLabel.Text = $"{beginningInjury}/100 Injury";
		    
		    InjuryReceived = finalInjury - beginningInjury;
		    TimeBetweenIncreases = FillAnimationDuration / InjuryReceived;
	    }

	    public override void _Process(double delta)
	    {
		    base._Process(delta);

		    if (InjuryReceived <= 0)
		    {
			    return;
		    }
		    
		    Timer += delta;
		    
		    if (Timer >= TimeBetweenIncreases)
		    {
			    int injuryPoints = Mathf.FloorToInt(Timer / TimeBetweenIncreases);
			    Timer -= injuryPoints * TimeBetweenIncreases;
			    injuryPoints = Mathf.Min(InjuryReceived, injuryPoints);
			    AddInjury(injuryPoints);
			    InjuryReceived -= injuryPoints;
		    }
	    }

	    private void AddInjury(int injury)
	    {
		    ProgressBar.Value += injury;
		    AmountLabel.Text = $"{(int)ProgressBar.Value}/100 Injury";
	    }
	}
}
