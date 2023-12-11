using Godot;
using System;

public partial class Booze : Consumable
{
    public Booze()
    {
        Type = "Booze";
        Title = "Bottle of Jack Daniels";
        HealthEffect = 0f;
        AddictionEffect = 0f;
        SpeedEffect = 0f;
    }
}
