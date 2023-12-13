using Godot;
using System;

public partial class Booze : Consumable
{
    public Booze()
    {
        Type = "Booze";
        Title = "Bottle of Jack Daniels";
        Description = "Might as well be water for Dzhupels";
        HealthEffect = 10f;
        AlcoholEffect = 2f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Alcohol", AlcoholEffect);
    }
}
