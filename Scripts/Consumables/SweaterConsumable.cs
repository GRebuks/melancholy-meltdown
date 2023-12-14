using Godot;
using System;

public partial class SweaterConsumable : Consumable
{
    public SweaterConsumable()
    {
        Type = "Clothes";
        Title = "Sweater";
        Description = "This sweater comes with sweat.";
        PermanentHealthEffect = 10f;
        PermanentSpeedEffect = 50f;
        TextureName = "sweater";

        Effects.Add("PermanentHealth", PermanentHealthEffect);
        Effects.Add("PermanentSpeed", PermanentSpeedEffect);
    }
}
