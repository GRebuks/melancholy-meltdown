using Godot;
using System;
using System.Collections.Generic;

public partial class Quest : Area2D
{
    public string Type = "Health quest";
    public string Title = "Consumable quest";
    public string Description = "Bring a consumable";

    protected float HealthEffect = 0f;
    protected float SpeedEffect = 0f;
    protected float Weight = 0f;
    protected float AlcoholEffect = 0f;

    public Dictionary<string, float> Effects = new Dictionary<string, float>();

    protected string ConsumablePath = "res://Assets/Objects/Consumables/";
    protected string RewardConsumableFile;
    public string RequiredConsumableFile;

    protected PackedScene RewardConsumable;
    public Consumable RewardConsumableNode;

    protected PackedScene RequiredConsumable;
    public Consumable RequiredConsumableNode;
}
