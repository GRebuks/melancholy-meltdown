using Godot;
using System;

public partial class SweaterConsumable : Consumable
{
    public SweaterConsumable()
    {
        Type = "Clothes";
        Title = "Sweaty Sweater";
        Description = "This sweater comes with sweat.";
        PermanentHealthEffect = 10f;
        PermanentSpeedEffect = 50f;
        TextureName = "sweater";
        DisplayColor = new Color(120, 120, 0, 1);

        Effects.Add("Max Health", PermanentHealthEffect);
        Effects.Add("Base Speed", PermanentSpeedEffect);
    }
}
