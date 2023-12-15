using Godot;
using System;

public partial class Chicken : Consumable
{
    public Chicken()
    {
        Type = "Food";
        Title = "Rotisserie chicken";
        Description = "A mighty meal for a mighty man";
        HealthEffect = 8f;
        SpeedEffect = -50f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Speed", SpeedEffect);
    }
}
