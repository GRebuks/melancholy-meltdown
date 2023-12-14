using Godot;
using System;

public partial class PlaytimeQuest : Quest
{
    public PlaytimeQuest()
    {
        Title = "Bored Child";
        string[] descriptions = new string[]
        {
            "Hi, mister! I'm looking for something awesome to play with. You got any toys or games that could make my day?",
            "Hey, do you have any cool toys or games? I'm kinda bored and looking for something fun to play with.",
            "Hello, mister.. I was wondering if you have any games or toys lying around..? If you have some, of course.. It's okay if you don't..",
            "Hey, do you know where I can find something fun to play with? Maybe some toys or games? Do you have anything?",
            "Hi! I'm on the lookout for something to play with. Got any cool toys or games you're not using?"
        };

        Description = descriptions[new Random().Next(0, descriptions.Length)];
        HealthEffect = 15f;

        // REQUIRED CONSUMABLE
        // Add the required consumable file name to make this consumable required
        RequiredConsumableFile = "any";

        Effects.Add("Health", HealthEffect);
    }
}
