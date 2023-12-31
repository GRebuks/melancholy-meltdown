using Godot;
using System;
using System.Collections.Generic;

public partial class Consumable : Area2D
{
    // Declare member variables here. Examples:
    public string Type = "Consumable Type";
    public string Title = "Consumable Title";
    public string Description = "Consumable Description";
    public Color DisplayColor = new Color(0, 153, 255, 1);

    protected float HealthEffect = 0f;
    protected float SpeedEffect = 0;
    protected float Weight = 0f;
    protected float AlcoholEffect = 0f;
    protected float THCEffect = 0f;
    protected float SugarEffect = 0f;
    protected float PermanentHealthEffect = 0f;
    protected float PermanentSpeedEffect = 0f;
    public string TextureName = "";
    public bool HasHiddenStats = false;
    public bool HasUnknownStats = false;

    public Dictionary<string, float> Effects = new Dictionary<string, float>();

    //deconstructor
    ~Consumable()
    {
        GD.Print("Consumable deconstructor called");
    }
}
