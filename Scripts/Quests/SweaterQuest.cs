using Godot;
using System;

public partial class SweaterQuest : Quest
{
    public SweaterQuest()
    {
        Title = "Unstable Runner";
        Description = "Hey.. I need something to get my blood going.. you get me? I need some speed, some.. \"sugar\".. You got some sugar? I'll reward you!!";

        // REQUIRED CONSUMABLE
        // Add the required consumable file name to make this consumable required
        RequiredConsumableFile = "sugar.tscn";
        RequiredConsumable = (PackedScene)ResourceLoader.Load(ConsumablePath + RequiredConsumableFile);
        RequiredConsumableNode = RequiredConsumable.Instantiate() as Consumable;


        // REWARD CONSUMABLE
        // Add the reward consumable file name to make this consumable as a reward
        RewardConsumableFile = "booze.tscn";
        RewardConsumable = (PackedScene)ResourceLoader.Load(ConsumablePath + RewardConsumableFile);
        RewardConsumableNode = RewardConsumable.Instantiate() as Consumable;

        rewardClothes = new Sweater();
    }
}
