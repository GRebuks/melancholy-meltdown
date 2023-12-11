using Godot;
using System;
using System.Reflection.Emit;

public partial class PlayerHealth : CharacterBody2D
{

    private ProgressBar _healthBar;
    private Godot.Label label;
    // Health property
    private float _health = 60f; // Set an initial value
    public float MaxHealth { get; private set; } = 60f; // Set the maximum health value

    private float _healthDecreaseRate = 1.0f; // Decrease health by 1 per second
    public float Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0f, MaxHealth);

            // Update the health bar when the health changes
            _healthBar.Value = _health;
        }
    }

    public override void _Ready()
    {
        _healthBar = GetNode<ProgressBar>("UI/HealthBar");
        label = GetNode<Godot.Label>("UI/Label");
        Health = 60f;
    }

    public override void _Process(double delta) 
    {
        // Decrease health based on delta time
        Health -= _healthDecreaseRate * (float)delta;
    }

}
