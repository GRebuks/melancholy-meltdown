using Godot;
using System;
using System.Collections.Generic;

public partial class ConsumableSpawn : Node2D
{
	private List<string> consumables = new List<string>()
	{
		"booze",
		"marijuana",
		"sugar",
	};

	private Node2D scene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		// 50 50 chance of spawning a consumable
		Random random = new Random();
		int randomChance = random.Next(100);

		if (randomChance < 50)
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
