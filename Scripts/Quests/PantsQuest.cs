using Godot;
using System;

public partial class PantsQuest : Quest
{
    public PantsQuest()
    {
        Title = "Impoverished Hobo";
        Description = "Bring me some booze, I'll reward you with these pants. Just don't mind the stain.";

        // REQUIRED CONSUMABLE
        // Add the required consumable file name to make this consumable required
        RequiredConsumableFile = "booze.tscn";
        RequiredConsumable = (PackedScene)ResourceLoader.Load(ConsumablePath + RequiredConsumableFile);
        RequiredConsumableNode = RequiredConsumable.Instantiate() as Consumable;


        // REWARD CONSUMABLE
        // Add the reward consumable file name to make this consumable as a reward
        RewardConsumableFile = "pants.tscn";
        RewardConsumable = (PackedScene)ResourceLoader.Load(ConsumablePath + RewardConsumableFile);
        RewardConsumableNode = RewardConsumable.Instantiate() as Consumable;
    }
}
