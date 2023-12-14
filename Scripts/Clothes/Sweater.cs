using Godot;
using System;

public partial class Sweater : Clothes
{
    public Sweater()
    {
        Type = "Sweater";
        Title = "Sweaty Sweater";
        Description = "This sweater comes with sweat.";
        PermanentHealthEffect = 10f;
        PermanentSpeedEffect = 50f;
        TextureName = "sweater";

        Effects.Add("PermanentHealth", PermanentHealthEffect);
        Effects.Add("PermanentSpeed", PermanentSpeedEffect);
    }
}
