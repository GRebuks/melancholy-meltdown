using Godot;
using System;
using static System.Formats.Asn1.AsnWriter;

public partial class TileCheck : Area2D
{

	private static bool spawningOnce = false;
	public void _on_area_entered(Node2D area) 
	{

		if (area.Name != "TileCheck")
		{
			GD.Print(area.Name);
			area.QueueFree();
		
		}

		CallDeferred("DoubleTileCheck", area);


	}
    private void DoubleTileCheck(Node2D area)
    {
        if (area.Name == "TileCheck")
        {
            GD.Print(area.Name);
            area.GetParent().QueueFree();
            if (!spawningOnce)
            {
                PackedScene scene = (PackedScene)ResourceLoader.Load("res://Scenes/Allyways/4Ways.tscn");
                Node2D paths = scene.Instantiate() as Node2D;
                paths.GlobalPosition = area.GlobalPosition;
                GetTree().Root.GetNode<Node2D>("Node2D").AddChild(paths);
                spawningOnce = true;
            }
        }
    }
}


