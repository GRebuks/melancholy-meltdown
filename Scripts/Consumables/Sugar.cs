using Godot;
using System;

public partial class Sugar : Consumable
{
    public Sugar()
    {
        Type = "Drug";
        Title = "\"Sugar\"";
        Description = "Adds some emotional stability and SPEEED";
        HealthEffect = 10f;
        SpeedEffect = 500f;
        SugarEffect = 10f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Speed", SpeedEffect);
        Effects.Add("Sugar", SugarEffect);
    }
}
