using Godot;
using System;

public partial class PipeBomb : Consumable
{
    public PipeBomb()
    {
        Type = "Bomb";
        Title = "Pipe Bomb";
        Description = "Don't do it";
        HealthEffect = -100f;

        Effects.Add("Health", HealthEffect);
    }
}
