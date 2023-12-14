using Godot;
using System;

public partial class PantsConsumable : Consumable
{
    public PantsConsumable()
    {
        Type = "Clothes";
        Title = "Stained Pants";
        Description = "Old pair of pants. Comes with a belt and a conspicuous brown stain.";
        PermanentHealthEffect = 10f;
        PermanentSpeedEffect = 50f;
        TextureName = "pants";
        DisplayColor = new Color(120, 120, 0, 1);

        Effects.Add("Max Health", PermanentHealthEffect);
        Effects.Add("Base Speed", PermanentSpeedEffect);
    }
}
