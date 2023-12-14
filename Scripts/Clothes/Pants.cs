using Godot;
using System;

public partial class Pants : Clothes
{
    public Pants()
    {
        Type = "Pants";
        Title = "Stained Pants";
        Description = "Old pair of pants. Comes with a belt and a conspicuous brown stain.";
        PermanentHealthEffect = 10f;
        PermanentSpeedEffect = 50f;
        TextureName = "pants";

        Effects.Add("PermanentHealth", PermanentHealthEffect);
        Effects.Add("PermanentSpeed", PermanentSpeedEffect);
    }
}
