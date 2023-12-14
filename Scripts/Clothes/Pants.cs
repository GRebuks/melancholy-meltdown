using Godot;
using System;

public partial class Pants : Clothes
{
    public Pants()
    {
        Type = "Pants";
        Title = "Pants";
        Description = "Gives Dzhupel pants with a belt";
        PermanentHealthEffect = 10f;
        PermanentSpeedEffect = 50f;
        TextureName = "pants";

        Effects.Add("PermanentHealth", PermanentHealthEffect);
        Effects.Add("PermanentSpeed", PermanentSpeedEffect);
    }
}
