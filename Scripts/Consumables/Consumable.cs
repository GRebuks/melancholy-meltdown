using Godot;
using System;

public partial class Consumable : Area2D
{
    // Declare member variables here. Examples:
    public string Type = "Health";
    public string Title = "Pack of Marlboro reds";
    public float HealthEffect = 10f;
    public float AddictionEffect = 5f;
    public float SpeedEffect = 100f;
    public float Weight = 0f;

    // Class constructor for default values
    public Consumable()
    {
        Type = "Health";
        Title = "Pack of Marlboro reds";
        HealthEffect = 10f;
        AddictionEffect = 5f;
        SpeedEffect = 100f;
    }

    // Class constructor for custom values
    public Consumable(string Type = "Health", string Title = "Consumable", float HealthEffect = 0f, float AddictionEffect = 0f, float SpeedEffect = 0f)
    {
        this.Type = Type;
        this.Title = Title;
        this.HealthEffect = HealthEffect;
        this.AddictionEffect = AddictionEffect;
        this.SpeedEffect = SpeedEffect;
    }
}
