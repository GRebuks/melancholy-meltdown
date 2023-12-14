using Godot;
using System;

public partial class PantsConsumable : Consumable
{
    public PantsConsumable()
    {
        Type = "Clothes";
        Title = "Pants";
        Description = "Gives Dzhupel pants with a belt";
        PermanentHealthEffect = 10f;
        PermanentSpeedEffect = 50f;
        TextureName = "pants";

        Effects.Add("PermanentHealth", PermanentHealthEffect);
        Effects.Add("PermanentSpeed", PermanentSpeedEffect);
    }
}
