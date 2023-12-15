using Godot;
using System;

public partial class IceCream : Consumable
{
    public IceCream()
    {
        Type = "Food";
        Title = "Ice Cream Cone";
        Description = "Each scoop is a momentary escape into a world of creamy bliss, offering a comforting hug for your taste buds.";
        HealthEffect = 5f;
        SpeedEffect = 50f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Speed", SpeedEffect);
    }
}
