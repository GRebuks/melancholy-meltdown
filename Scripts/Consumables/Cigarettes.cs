using Godot;
using System;

public partial class Cigarettes : Consumable
{
    public Cigarettes()
    {
        Type = "Drug";
        Title = "Pack of cigarettes";
        Description = "A momentary escape from the harsh realities of the dystopian cityscape";
        HealthEffect = 15f;
        SpeedEffect = -50f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Speed", SpeedEffect);
    }
}
