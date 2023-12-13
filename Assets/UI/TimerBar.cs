using Godot;
using System;

public partial class TimerBar : ProgressBar
{
	private Timer timer;

	public override void _Ready()
	{
		timer = GetParent().GetNode<Timer>("Timer");
        MaxValue = timer.WaitTime;
        GD.Print(timer.WaitTime);
    }

	public override void _Process(double delta)
	{
        Value = timer.TimeLeft;
    }
}
