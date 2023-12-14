using Godot;
using System;

public partial class JunkieQuest : Quest
{
    public JunkieQuest()
    {
        Title = "Desperate Junkie";
        string[] descriptions = new string[]
        {
            "Young man, help me out, will ya? I need my daily dose of Basil to keep me going.",
            "Hey, you! I need some Basil, you got some? I'll reward you if you do.",
            "Yo man! Ya got some of the green stuff? I'mma reward ya good, man!",
            "Hey there! Seeking a Basil boost. Your help would be greatly appreciated!",
            "Yo, young blood! I'm runnin' low on that Basil vibe, you got my back?",
            "Hey you! I'm hustlin' for some Basil, you hook me up, and I'll hook you up, ya feel?",
            "Hey, player! I'm fiendin' for that Basil boost. You share, I share. It's a street deal, my friend.",
            "Wassup, G? I'm runnin' low on that Basil love. You got the hookup? I'll bless you back real good.",
        };

        Description = descriptions[new Random().Next(0, descriptions.Length)];
        HealthEffect = 15f;

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

        Effects.Add("Health", HealthEffect);
    }
}
