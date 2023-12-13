using Godot;
using System;

public partial class TestQuest : Quest
{
    public TestQuest()
    {
        Title = "This is a test quest";
        Description = "Bringing me some hashish for some boze";
        HealthEffect = 100f;
        SpeedEffect = 100f;

        // REQUIRED CONSUMABLE
        // Add the required consumable file name to make this consumable required
        RequiredConsumableFile = "marijuana.tscn";
        RequiredConsumable = (PackedScene)ResourceLoader.Load(ConsumablePath + RequiredConsumableFile);
        RequiredConsumableNode = RequiredConsumable.Instantiate() as Consumable;


        // REWARD CONSUMABLE
        // Add the reward consumable file name to make this consumable as a reward
        RewardConsumableFile = "booze.tscn";
        RewardConsumable = (PackedScene)ResourceLoader.Load(ConsumablePath + RewardConsumableFile);
        RewardConsumableNode = RewardConsumable.Instantiate() as Consumable;
        rewardClothes = new Pants();

        Effects.Add("Health", HealthEffect);
        Effects.Add("Speed", SpeedEffect);
    }
}
