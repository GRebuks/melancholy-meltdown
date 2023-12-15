using Godot;
using System;
using System.IO;

public partial class MainMenu : Control
{
    private static string achievementFilePath = "achievements.json";
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        //string achievementFilePath = "Scripts/Achievements/achievements.json";
        if (!File.Exists(achievementFilePath))
        {
            File.Create(achievementFilePath);
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_begin_pressed() 
	{
		GetTree().ChangeSceneToFile("res://Scenes/Start.tscn");
	}

	public void _on_quit_pressed() 
	{
		GetTree().Quit();
	}
}
