using Godot;
using System;

public partial class Marijuana : Consumable
{
    public Marijuana()
    {
        Type = "Marijuana";
        Title = "some hashish";
        HealthEffect = 15f;
        AddictionEffect = 10f;
        SpeedEffect = -50f;
        AlcoholEffect = 0f;
    }
}
