using Godot;
using System;

public partial class Cactus : Consumable
{
    public Cactus()
    {
        Type = "Food";
        Title = "Cactus";
        Description = "A member of the infamous Cactaceae family. Wait, do you consume this?";
        HealthEffect = -10f;
        SugarEffect = 2f;
        SpeedEffect = 200f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Sugar", SugarEffect);
        Effects.Add("Speed", SpeedEffect);
    }
}
