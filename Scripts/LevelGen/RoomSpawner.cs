using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class RoomSpawner : Node2D
{

	[Export] public int Direction { get; set; }

    List<PackedScene> bottomScenes = new List<PackedScene>();
    List<PackedScene> topScenes = new List<PackedScene>();
    List<PackedScene> leftScenes = new List<PackedScene>();
    List<PackedScene> rightScenes = new List<PackedScene>();

    private float Time = 0f;
    private Random random = new Random();
    private int rand = 0;
    private bool spawned = false;
    private float X, Y;
    private static int zIndex = 0;

    public override void _Ready()
    {
        RoomLists();


        //GD.Print(bottomScenes, topScenes, leftScenes, rightScenes);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!spawned && Time > 0.1f)
        {
            Spawn();
            spawned = true;
        }
        Time += (float)delta;
    }

    public void Spawn()
	{
        Vector2 spawnpoint = GlobalPosition;
        if (spawned == false)
        {
            zIndex--;

            if (Direction == 1)
            {
                //Down
                rand = random.Next(0, bottomScenes.Count);
                Node2D DownScene = bottomScenes[rand].Instantiate() as Node2D;
                DownScene.Position = spawnpoint;
                DownScene.ZIndex = zIndex;
                //GetTree().Root.AddChild(DownScene);
                GetTree().Root.GetNode<Node2D>("Node2D").AddChild(DownScene);
                EnableYSort(DownScene);


            }
            else if (Direction == 2)
            {
                //Top
                rand = random.Next(0, topScenes.Count);
                Node2D TopScene = topScenes[rand].Instantiate() as Node2D;
                TopScene.Position = spawnpoint;
                TopScene.ZIndex = zIndex+2;
                //GetTree().Root.AddChild(TopScene);
                GetTree().Root.GetNode<Node2D>("Node2D").AddChild(TopScene);
                EnableYSort(TopScene);
            }
            else if (Direction == 3)
            {
                //Left
                rand = random.Next(0, leftScenes.Count);
                Node2D LeftScene = leftScenes[rand].Instantiate() as Node2D;
                LeftScene.Position = new Vector2 (spawnpoint.X, spawnpoint.Y);
                LeftScene.ZIndex = zIndex;
                //GetTree().Root.AddChild(LeftScene);
                GetTree().Root.GetNode<Node2D>("Node2D").AddChild(LeftScene);
                EnableYSort(LeftScene);
            }

            else if (Direction == 4)
            {
                //Right
                rand = random.Next(0, rightScenes.Count);
                Node2D RightScene = rightScenes[rand].Instantiate() as Node2D;
                RightScene.Position = new Vector2(spawnpoint.X, spawnpoint.Y);
                RightScene.ZIndex = zIndex;
                //GetTree().Root.AddChild(RightScene);
                GetTree().Root.GetNode<Node2D>("Node2D").AddChild(RightScene);
                EnableYSort(RightScene);
            }
            spawned = true;
        }
    }

    private void EnableYSort(Node2D node)
    {
        node.YSortEnabled = true;
        node.ZIndex = 0;
        node.GetNode<TileMap>("TileMap").YSortEnabled = true;
        node.GetNode<TileMap>("TileMap").ZIndex = 0;
    }

    public void RoomLists() 
    {
        // SLIMAA METODE
        //string path = "Scenes/Allyways/";
        //var allScenes = Directory.GetFiles(path, "*.tscn*");

        // res://Scenes/Allyways/B.tscn


        List<string> allScenes = new List<string>();
        List<string> fileNames = new List<string>() {"B","BL","BR","BT","L","LR","R","T","TL","TR"};

        foreach (var fileName in fileNames)
        {
            allScenes.Add("res://Scenes/Allyways/" + fileName + ".tscn");
        }

        foreach (var scene in allScenes)
        {
            if (scene.Contains("B"))
            {
                PackedScene sceneBot = (PackedScene)ResourceLoader.Load(scene);
                bottomScenes.Add(sceneBot);
            }
            if (scene.Contains("T"))
            {
                PackedScene sceneTop = (PackedScene)ResourceLoader.Load(scene);
                topScenes.Add(sceneTop);
            }
            if (scene.Contains("L"))
            {
                PackedScene sceneLeft = (PackedScene)ResourceLoader.Load(scene);
                leftScenes.Add(sceneLeft);
            }
            if (scene.Contains("R"))
            {
                PackedScene sceneRight = (PackedScene)ResourceLoader.Load(scene);
                rightScenes.Add(sceneRight);
            }

        }
    }
}
