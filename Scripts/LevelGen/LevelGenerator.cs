using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class LevelGenerator : Node
{
	private PackedScene[] roomTemplates;
    private Node2D Start = null;
    private Random random = new Random();
    private int RoomCount = 5;
	private float distance = 600;
	private float Xalign = 0;
    private float Yalign = 0;
	private List<Vector2> PreviousPositions;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		PreviousPositions = new List<Vector2>();

        roomTemplates = new PackedScene[15];

		for (int i = 0; i < roomTemplates.Length; i++)
		{
			roomTemplates[i] = (PackedScene)ResourceLoader.Load("res://Scenes/Room" + i + ".tscn");
		}

		Start = roomTemplates[1].Instantiate() as Node2D;
        PreviousPositions.Add(Start.Position);
        AddChild(Start);
        
        GenerateLevelGrid(Start);
	}

	private void GenerateLevelGrid(Node2D Start)
	{
		Node2D beforeRoom = Start;

		while (PreviousPositions.Count != RoomCount)
		{

			Node2D roomInstance = GetRoomWithExitPoints(beforeRoom);

			if (roomInstance != null)
			{
				if (beforeRoom != null)
				{
                    // GD.Print(Xalign, Yalign, beforeRoom.Position);
					roomInstance.Position = new Vector2(beforeRoom.Position.X + distance * Xalign, beforeRoom.Position.Y + distance * Yalign);
                }

				AddChild(roomInstance);

                PreviousPositions.Add(roomInstance.Position);
                beforeRoom = roomInstance;


			}
			else
			{
				GD.Print("Notworking");
			}
		}

	}

	private Node2D GetRoomWithExitPoints(Node2D beforeRoom)
	{

        int RandomRoom = random.Next(0, roomTemplates.Length);
		Node2D roomInstance = roomTemplates[RandomRoom].Instantiate() as Node2D;

        Node2D beforeExitPoints = beforeRoom.GetNodeOrNull("ExitPoints") as Node2D;

	
		if (beforeExitPoints != null && beforeExitPoints.GetChildCount() > 1)
		{
			Node2D beforeExitPoint = beforeExitPoints.GetChild(random.Next(0, beforeExitPoints.GetChildCount())) as Node2D;
			Node2D BeforeExitCol = beforeExitPoint.GetChild(0) as Node2D;
			int direction = 0;
			if (BeforeExitCol.Position.X < 0)
			{
				direction = 0;
			}
			if (BeforeExitCol.Position.X > 0)
			{
				direction = 1;
			}
			if (BeforeExitCol.Position.Y < 0)
			{
				direction = 2;
			}
			if (BeforeExitCol.Position.Y > 0)
			{
				direction = 3;
			}

			switch (direction)
			{
				case 0:
					//left
					Xalign = -1;
					Yalign = 0;
					break;
				case 1:
					//right
					Xalign = 1;
					Yalign = 0;
					break;
				case 2:
					//up
					Xalign = 0;
					Yalign = 1;
					break;
				case 3:
					//down
					Xalign = 0;
					Yalign = -1;
					break;
			}
            float BeforeX = beforeRoom.Position.X + distance * Xalign;
            float BeforeY = beforeRoom.Position.Y + distance * Yalign;
            foreach (Vector2 PreviousPosition in PreviousPositions) 
			{
                GD.Print(BeforeX, " ", BeforeY, " ", PreviousPosition.X, " ", PreviousPosition.Y);
                if (BeforeX == PreviousPosition.X && BeforeY == PreviousPosition.Y)
                {
                    GD.Print("Failed");
					return GetRoomWithExitPoints(beforeRoom);
					
                }
            }
            return roomInstance;
        }
		else
		{
			GD.Print("Nomore");
			return null;
		}
	}

	private void AlignExitPoints(Node2D previousRoom, Node2D currentRoom) 
	{
		Node2D previousExitPoints = previousRoom.GetNodeOrNull("ExitPoints") as Node2D;
		Node2D currentExitPoints = currentRoom.GetNodeOrNull("ExitPoints") as Node2D;

		if (previousExitPoints != null && currentExitPoints != null && previousExitPoints.GetChildCount() > 0 && currentExitPoints.GetChildCount() > 0) 
		{
			Node2D previousExitPoint = previousExitPoints.GetChild(0) as Node2D;
			Node2D currentExitPoint = currentExitPoints.GetChild(0) as Node2D;

			Node2D previousExitCol = previousExitPoint.GetChild(0) as Node2D;
            Node2D currentExitCol = currentExitPoint.GetChild(0) as Node2D;


            Vector2 offset = previousExitCol.Position - currentExitCol.Position;
			currentRoom.Position += offset;
		}
    } 

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
