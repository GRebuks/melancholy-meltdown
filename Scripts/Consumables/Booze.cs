using Godot;
using System;

public partial class Booze : Consumable
{
    public Booze()
    {
        Type = "Drug";
        Title = "Bottle of Booze";
        Description = "Might as well be water for Dzhupels";
        HealthEffect = 8f;
        AlcoholEffect = 2f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Alcohol", AlcoholEffect);
    }
}
