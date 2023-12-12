using Godot;
using System;

public partial class TestQuest : Quest
{
    public TestQuest()
    {
        Title = "This is a test quest";
        HealthEffect = 100f;
        AddictionEffect = 0f;
        SpeedEffect = 100f;
        AlcoholEffect = 0f;

        RewardConsumableName = "booze.tscn";

        RewardConsumablePath = ConsumablePath + RewardConsumableName;
        RewardConsumable = (PackedScene)ResourceLoader.Load(RewardConsumablePath);
    }
}
