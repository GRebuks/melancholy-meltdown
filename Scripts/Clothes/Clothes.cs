using Godot;
using System;
using System.Collections.Generic;

public partial class Clothes : Node
{
    // Declare member variables here. Examples:
    public string Type = "Clothes Type";
    public string Title = "Clothes Title";
    public string Description = "Clothes Description";
    public string TextureName = "";

    protected float HealthEffect = 0f;
    protected float SpeedEffect = 0;
    protected float Weight = 0f;
    protected float PermanentHealthEffect = 0f;
    protected float PermanentSpeedEffect = 0f;


    public Dictionary<string, float> Effects = new Dictionary<string, float>();
}
