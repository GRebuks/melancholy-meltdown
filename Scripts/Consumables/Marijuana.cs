using Godot;
using System;

public partial class Marijuana : Consumable
{
    public Marijuana()
    {
        Type = "Drug";
        Title = "\"Basil\"";
        Description = "It's just a street name. Something to take the edge off with.";
        DisplayColor = new Color(0, 255, 0, 1);
        HealthEffect = 15f;
        SpeedEffect = -50f;
        THCEffect = 1f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Speed", SpeedEffect);
        Effects.Add("THC", THCEffect);
    }
}
