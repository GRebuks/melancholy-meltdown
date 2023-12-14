using Godot;
using System;

public partial class AlcoholicQuest : Quest
{
    public AlcoholicQuest()
    {
        Title = "Struggling Alcoholic";
        string[] descriptions = new string[]
        {
            "Excuse me, friend. I'm battling some serious demons and could use a bit of solace. Any chance you've got a drink to spare?",
            "I hate to ask, but I'm struggling, and a little something to numb the pain would mean the world to me right now. Can you help a person out?",
            "Listen up! I'm in a tough spot, and I need a damn drink to take the edge off. Cut the sympathy crap, and hand me that bottle. I'm not here for pity; I'm here for a momentary escape. Got it?",
            "I've hit a low point, and the bottle is my misguided refuge. If you can share a drink, it's a fleeting comfort in a world that feels overwhelming right now.",
        };

        Description = descriptions[new Random().Next(0, descriptions.Length)];
        HealthEffect = 15f;

        // REQUIRED CONSUMABLE
        // Add the required consumable file name to make this consumable required
        RequiredConsumableFile = "booze.tscn";
        RequiredConsumable = (PackedScene)ResourceLoader.Load(ConsumablePath + RequiredConsumableFile);
        RequiredConsumableNode = RequiredConsumable.Instantiate() as Consumable;

        Effects.Add("Health", HealthEffect);
    }
}
