using Godot;
using System;
using System.Collections.Generic;

public partial class ConsumableSpawn : Node2D
{
	private List<string> consumables = new List<string>()
	{
		"booze",
        "booze",
        "booze",
        "marijuana",
        "marijuana",
        "marijuana",
        "sugar",
        "sugar",
        "pipe_bomb",
		"cigarettes",
		"chicken",
		"chinese_food",
		"money",
		"weird_liquid",
		"coke",
		"ice_cream",
		"cactus",
	};

	private Node2D scene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Random random = new Random();
		int randomChance = random.Next(100);

		if (randomChance < 70)
		{
            SpawnConsumable();
        }
	}

	public void AddConsumable(Consumable consumable)
	{
		GetParent().AddChild(consumable);
    }

	public void SpawnConsumable()
	{
        scene = GetTree().Root.GetNode<Node2D>("Node2D");

        // Get the consumable scene

        // Get a random consumable
        Random random = new Random();
        int randomIndex = random.Next(consumables.Count);
        string randomConsumable = consumables[randomIndex];

        var consumableScene = (PackedScene)ResourceLoader.Load($"res://Assets/Objects/Consumables/{randomConsumable}.tscn");

        // Instantiate the consumable scene
        Consumable consumable = consumableScene.Instantiate() as Consumable;

        // Add the consumable to the scene
        CallDeferred("AddConsumable", consumable);
		// Set consumable position to ConsumableSpawn position
		consumable.Position = Position;
    }
}
