using Godot;
using System;

public partial class WeirdLiquid : Consumable
{
    public WeirdLiquid()
    {
        Type = "Food";
        Title = "Weird liquid";
        Description = "Smells funny. Will you take the risk?";
        
        HasUnknownStats = true;

        Random random = new Random();
        int randomNumber = random.Next(1, 10);
        if (randomNumber == 1)
        {
            HealthEffect = -100f;
        }
        else
        {
            HealthEffect = random.Next(-10, 10);
        }

        AlcoholEffect = 1f;
        SugarEffect = 3f;
        SpeedEffect = random.Next(-20, 20);


        Effects.Add("Health", HealthEffect);
        Effects.Add("Alcohol", AlcoholEffect);
        Effects.Add("Sugar", SugarEffect);
        Effects.Add("Speed", SpeedEffect);
    }
}
