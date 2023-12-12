using Godot;
using System;

public partial class Booze : Consumable
{
    public Booze()
    {
        Type = "Booze";
        Title = "a Bottle of Jack Daniels";
        HealthEffect = 10f;
        AddictionEffect = 0f;
        SpeedEffect = 0f;
        AlcoholEffect = 2f;
    }
}
