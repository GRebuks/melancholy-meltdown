using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	public float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public float Health = 100f;
	public float Addiction = 0f;

	public Consumable consumable = null;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
            velocity.Y = direction.Y * Speed;
        }
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(velocity.Y, 0, Speed);
        }

		if (consumable != null)
		{
            consumable.Position = new Vector2(this.Position.X, this.Position.Y - 30);
            // Consume the consumable on key press E
            if (Input.IsActionJustPressed("ui_accept"))
            {
                consumeConsumable();
            }
            // Drop consumable on key press Q
            else if (Input.IsActionJustPressed("ui_cancel"))
            {
                // Drop the consumable
                consumable.Position = new Vector2(this.Position.X, this.Position.Y - 30);
                consumable = null;
            }
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	// on area entered
	private void _on_area_entered(Area2D area)
	{
		// If the area is in group consumable
		if (area.IsInGroup("Consumable"))
		{
			// Get information about the consumable
			Consumable consumable = (Consumable)area;
			if (this.consumable == null)
			{
				this.consumable = consumable;
			}
            GD.Print("");
            GD.Print("You collected a " + consumable.Title);

            //// Destroy the consumable
            //area.QueueFree();

        }
    }

	private void applyConsumableEffects(Consumable consumable)
	{
		// Apply the health effect
		Health += consumable.HealthEffect;

		// Apply the addiction effect
		Addiction += consumable.AddictionEffect;

        // Apply the speed effect
        Speed += consumable.SpeedEffect;
    }

	private void displayPlayerStats()
	{
        GD.Print("Health: " + Health);
        GD.Print("Addiction: " + Addiction);
        GD.Print("Speed: " + Speed);
    }

	private void consumeConsumable()
	{
		// Apply effects
		applyConsumableEffects(consumable);
        GD.Print("");
        GD.Print("You consumed a " + consumable.Title);
		displayPlayerStats();

        // Destroy the consumable
        consumable.QueueFree();

		consumable = null;
    }
}
