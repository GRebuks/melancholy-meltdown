using Godot;
using System;

public partial class Coke : Consumable
{
    public Coke()
    {
        Type = "Food";
        Title = "Bottle of coke";
        Description = "A bottle of coke - the beverage kind.";
        HealthEffect = 5f;
        SpeedEffect = 50f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Speed", SpeedEffect);
    }
}
