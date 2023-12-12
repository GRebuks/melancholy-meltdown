using Godot;
using System;

public partial class Quest : Area2D
{
    public string Type = "Health quest";
    public string Title = "Bring a consumable";

    public float HealthEffect = 0f;
    public float AddictionEffect = 0f;
    public float SpeedEffect = 0f;
    public float Weight = 0f;
    public float AlcoholEffect = 0f;

    public string RequiredConsumableType = "Booze";

    protected string ConsumablePath = "res://Scripts/Consumables/";
    protected string RewardConsumablePath;
    public string RewardConsumableName;
    public PackedScene RewardConsumable;
}
