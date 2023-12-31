using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections;
using System.IO;

public partial class AchievementManager : Node
{
    //private Node2D sceneNode;
    // Achievement UI element
    private static PackedScene achievementUI = (PackedScene)ResourceLoader.Load("res://Assets/UI/achievement_panel.tscn");
    // Make a dictionary of all the achievements
    private static Dictionary<string, Dictionary<string, object>> achievements = new Dictionary<string, Dictionary<string, object>>();

    private static Control UI; 

    public static List<string> AchievementNameQueue;

    private static Panel activeAchievementPanel;

    private static string achievementFilePath = "achievements.json";

    public static void LoadAchievementManager(Node root)
    {
        UI = root.GetNode<Node2D>("Node2D").GetNode<CharacterBody2D>("Player").GetNode<Camera2D>("Camera2D").GetNode<CanvasLayer>("CanvasLayer").GetNode<Control>("UI");

        AchievementNameQueue = new List<string>();
    }

    // Make an achievement
    private static Dictionary<string, object> MakeAchievement(string name, string description, string icon, Int64 goal, Int64 progress)
    {
        Dictionary<string, object> achievement = new Dictionary<string, object>
        {
            { "name", name },
            { "description", description },
            { "icon", icon },
            { "goal", goal },
            { "progress", progress },
            { "completed", false }
        };
        return achievement;
    }

    // Add achievement to the dictionary
    private static void AddAchievement(string name, string description, string icon, Int64 goal, Int64 progress)
    {
        achievements.Add(name, MakeAchievement(name, description, icon, goal, progress));
    }

    // Constructor
    public AchievementManager()
    {
        achievements = LoadAchievementsFromFile(achievementFilePath);
        if (achievements == null)
        {
            achievements = new Dictionary<string, Dictionary<string, object>>();
            AddNewAchievements();
            achievements = LoadAchievementsFromFile(achievementFilePath);
        }
    }
    // Add progress to an achievement
    public static void AddProgress(string name, Int64 progress)
    {
        Dictionary<string, object> achievement = achievements[name];
        achievement["progress"] = (Int64)achievement["progress"] + progress;

        // Check if the achievement is completed
        if (!(bool)achievement["completed"])
        {
            if ((Int64)achievement["progress"] >= (Int64)achievement["goal"])
            {
                CompleteAchievement(name);
            }
        }
        SaveAchievementsToFile(achievements, achievementFilePath);
    }

    // Complete an achievement
    public static void CompleteAchievement(string name)
    {
        if ((bool)achievements[name]["completed"])
        {
            return;
        }
        Dictionary<string, object> achievement = achievements[name];
        achievement["completed"] = true;
        AchievementNameQueue.Add(name);
        if(AchievementNameQueue.Count == 1)
        {
            OutputAchievementCompletion(name);
        }
    }

    public static void SaveAchievementsToFile(Dictionary<string, Dictionary<string, object>> achievements, string filePath)
    {
        try
        {
            // Convert the achievements dictionary to a JSON string
            string json = JsonConvert.SerializeObject(achievements, Formatting.Indented);

            // Write the JSON string to the specified file
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            GD.Print("Error saving achievements to file: " + e.Message);
        }
    }

    public static Dictionary<string, Dictionary<string, object>> LoadAchievementsFromFile(string filePath)
    {
        try
        {
            // Read the JSON string from the specified file
            string json = File.ReadAllText(filePath);

            // Convert the JSON string to a dictionary
            Dictionary<string, Dictionary<string, object>> achievements = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);

            return achievements;
        }
        catch (Exception e)
        {
            GD.Print("Error loading achievements from file: " + e.Message);
            return null;
        }
    }

    // Reset all achievements
    public static void ResetAchievements()
    {
        // Reset all achievements
        foreach (KeyValuePair<string, Dictionary<string, object>> achievement in achievements)
        {
            achievement.Value["progress"] = 0L;
            achievement.Value["completed"] = false;
        }
        SaveAchievementsToFile(achievements, achievementFilePath);
    }

    // Hard reset all achievements
    public static void HardResetAchievements()
    {
        AddAchievement("Test achievement", "Start the test scene", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);


        // Reset all achievements
        foreach (KeyValuePair<string, Dictionary<string, object>> achievement in achievements)
        {
            achievement.Value["progress"] = 0L;
            achievement.Value["completed"] = false;
        }
        SaveAchievementsToFile(achievements, achievementFilePath);
    }

    // Add new achievements
    public static void AddNewAchievements()
    {
        // Add new achievements
        // DONE
        AddAchievement("Lights Out!", "Black out from alcohol overconsumption for the first time", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);
        // DONE
        AddAchievement("The Chemist", "Consume all psychoactive substances at the same time", "res://Assets/Sprites/Achievements/the_chemist.png", 1L, 0L);
        // DONE
        AddAchievement("The Speedrunner", "That's not the aim of the game", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);
        // DONE
        AddAchievement("The Good Citizen", "Finish a quest for the first time", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);
        // DONE
        AddAchievement("The Minimalist", "Don't consume anything", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);
        // DONE
        AddAchievement("Close Call", "Consume a healing item a second before dying", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);
        // DONE
        AddAchievement("Dressed to Impress", "Wear all clothing items at the same time", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);
        // DONE
        AddAchievement("The Troublemaker", "Give a psychoactive substance to a child", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);
        // DONE
        AddAchievement("The New Generation", "Give an explosive device to a child", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);
        // DONE
        AddAchievement("Go out in style", "Eat a pipe bomb [INSERT EXPLOSION EFFECT]", "res://Assets/Sprites/Achievements/test_achievement.png", 1L, 0L);
        SaveAchievementsToFile(achievements, achievementFilePath);
    }

    // Output achievement completion as UI element
    public static void OutputAchievementCompletion(string name)
    {
        // Get the achievement
        Dictionary<string, object> achievement = achievements[name];

        // Check if the achievement is completed
        if ((bool)achievement["completed"])
        {
            activeAchievementPanel = (Panel)achievementUI.Instantiate();
            activeAchievementPanel.GetNode<Label>("Title").Text = achievement["name"].ToString();
            activeAchievementPanel.GetNode<Label>("Description").Text = achievement["description"].ToString();
            activeAchievementPanel.GetNode<TextureRect>("Icon").Texture = (Texture2D)ResourceLoader.Load(achievement["icon"].ToString());

            UI.AddChild(activeAchievementPanel);

            Timer timer = activeAchievementPanel.GetNode<Timer>("Timer");
            timer.Timeout += OnTimerTimeout;
        }
        else
        {
            AchievementNameQueue.RemoveAt(0);
            if(AchievementNameQueue.Count > 0)
            {
                OutputAchievementCompletion(AchievementNameQueue[0]);
            }
        }
    }
    private static void OnTimerTimeout()
    {
        activeAchievementPanel.QueueFree();
        if(AchievementNameQueue.Count > 1)
        {
            AchievementNameQueue.RemoveAt(0);
            OutputAchievementCompletion(AchievementNameQueue[0]);
        }
        else if (AchievementNameQueue.Count == 1)
        {
            AchievementNameQueue.RemoveAt(0);
        }
    }
}
