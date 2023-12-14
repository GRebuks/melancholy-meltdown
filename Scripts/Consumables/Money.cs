using Godot;
using System;

public partial class Money : Consumable
{
    public Money()
    {
        Type = "Food";
        Title = "A wad of cash";
        Description = "Just the smell of it fills you with determination. Or, in this case, with emotional stability.";
        HealthEffect = 15f;
        SugarEffect = 2f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Sugar", SugarEffect);
    }
}
