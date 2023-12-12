using Godot;
using System;

public partial class Consumable : Area2D
{
    // Declare member variables here. Examples:
    public string Type = "Consumable Type";
    public string Title = "Consumable Title";
    public float HealthEffect = 0f;
    public float AddictionEffect = 0f;
    public float SpeedEffect = 0;
    public float Weight = 0f;
    public float AlcoholEffect = 0f;

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
