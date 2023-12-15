using Godot;
using System;

public partial class ChineseFood : Consumable
{
    public ChineseFood()
    {
        Type = "Food";
        Title = "Chinese takeaway box";
        Description = "Packed with warmth and nostalgia, it's the perfect remedy for a cozy, soothing moment";
        HealthEffect = 8f;
        SpeedEffect = -30f;

        Effects.Add("Health", HealthEffect);
        Effects.Add("Speed", SpeedEffect);
    }
}
