using Godot;
using System;

public partial class TileCheck : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_area_entered(Node2D area) 
	{

		if (area.Name != "TileCheck" && area.Name != "Area2D")
		{
            string spawn = area.Name;
			if (spawn.Contains("SpawnPoint"))
			{
				GD.Print(area.Name);
				area.QueueFree();
			}
		}
	}
}
