using Godot;
using System;

public partial class DebugKamera : Camera2D
{
	public override void _Process(double delta)
	{
        Vector2 motion = Vector2.Zero;

        if (Input.IsActionPressed("ui_right"))
        {
            motion.X += 10;
        }
        if (Input.IsActionPressed("ui_left"))
        {
            motion.X -= 10;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            motion.Y += 10;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            motion.Y -= 10;
        }

        Offset += motion;
    }
}
