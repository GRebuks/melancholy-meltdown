using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    private Node2D sceneNode;

    [Export] public float BaseSpeed = 300.0f;
    [Export] public float Speed = 300.0f;
    Texture2D texture;
    public Dictionary<string, Texture2D> skins;
    public string currentSkin = "naked";
    public Vector2 direction;
    public Vector2 velocity;

    public float timePassed = 0f;
    public float deathTimePassed = 0f;
    Vector2 targetPosition = new Vector2();
    public bool blockMovement = false;

    public Consumable consumable = null;

    // Audio
    public AudioStreamPlayer2D useConsumableSFX;
    public AudioStreamPlayer2D putDownConsumableSFX;
    public AudioStreamPlayer2D pickUpConsumableSFX;

    // Animation
    public Sprite2D sprite;
    private AnimationPlayer animation;

    // Health bar
    private ProgressBar _healthBar;
    private Label _healthBarLabel;

    // Blood alcohol content bar
    private ProgressBar _bacBar;
    private Label _bacBarLabel;
    private ColorRect Blackout;

    // THC bar
    private ProgressBar _thcBar;
    private Label _thcBarLabel;
    private ColorRect Baked;

    // Camera
    public Camera2D camera;

    // Info box
    private Panel QuestCard;
    private Panel ConsumableCard;

    private List<Label> QuestEffectLabels = new List<Label>();
    private List<Label> ConsumableEffectLabels = new List<Label>();

    private PackedScene EffectLabel;
    private string _effectLabelPath = "res://Assets/UI/InfoCard/effect_label.tscn";

    private float _bloodAlcoholContent = 0f;
    private float _bloodTHCContent = 0f;
    private float _health = 10f;

    // Achievement progress
    private int consumedItems = 0;
    private float totalPassedTime = 0f;

    private bool theGoodCitizen = false;
    private bool theMinimalist = false;
    private bool theSpeedrunner = false;
    private bool theChemist = false;
    private bool closeCall = false;
    private bool lightsOut = false;

    public float MaxBAC { get; private set; } = 6f;
    public float MaxTHC { get; private set; } = 3f;
    public float MaxHealth { get; private set; } = 60f;

    public float _healthDecreaseMultiplier = 1.0f;
    private float _healthDecreaseRate = 1.0f;
    private float _speedDecreaseRate = 5.0f;
    private float _minSpeedDecreaseRate = 5.0f;

    private float BloodSugarContent = 0.0f;
    private float _bloodSugarDecreaseRate = 1.0f;

    private float _alcoholDecreaseRate = 5.0f;
    private float _minAlcoholDecreaseRate = 0.001f;

    private float _thcDecreaseRate = 5.0f;
    private float _minTHCDecreaseRate = 0.001f;

    private const Int64 progress = 1L;
    public float Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0f, MaxHealth);
            _healthBar.Value = _health;
            _healthBarLabel.Text = $"Emotional Damage: {(int)(_health * 100 / MaxHealth)}% | {(int)_health}/{(int)MaxHealth} | {MathF.Round(_health, 2)}s";
        }
    }

    public float BloodAlcoholContent
    {
        get { return _bloodAlcoholContent; }
        set
        {
            _bloodAlcoholContent = Mathf.Clamp(value, 0f, MaxBAC);
            _bacBar.Value = _bloodAlcoholContent;
            float promiles = Mathf.Lerp(0f, 2, _bloodAlcoholContent / MaxBAC);
            _bacBarLabel.Text = $"Blood Alcohol Content: {MathF.Round(promiles, 2)} promiles";
        }
    }

    public float BloodTHCContent
    {
        get { return _bloodTHCContent; }
        set
        {
            _bloodTHCContent = Mathf.Clamp(value, 0f, MaxTHC);
            _thcBar.Value = _bloodTHCContent;
            float nanograms = Mathf.Lerp(0f, 300f, _bloodTHCContent / MaxTHC);
            _thcBarLabel.Text = $"Tetrahydrocannabinol: {MathF.Round(nanograms, 2)} ng/ml";
        }
    }

    public override void _Ready()
    {
        //sceneNode = GetTree().Root.GetChild<Node2D>(0);
        camera = (Camera2D)GetNode("Camera2D");
        _healthBar = camera.GetNode<CanvasLayer>("CanvasLayer").GetNode<ProgressBar>("UI/HealthBar");
        _healthBarLabel = _healthBar.GetNode<Label>("EmotionalDamage");

        _bacBar = camera.GetNode<CanvasLayer>("CanvasLayer").GetNode<ProgressBar>("UI/BACBar");
        _bacBarLabel = _bacBar.GetNode<Label>("BAC");

        _thcBar = camera.GetNode<CanvasLayer>("CanvasLayer").GetNode<ProgressBar>("UI/THCBar");
        _thcBarLabel = _thcBar.GetNode<Label>("THC");

        Blackout = camera.GetNode<CanvasLayer>("CanvasLayer").GetNode<ColorRect>("UI/Blackout");
        Baked = camera.GetNode<CanvasLayer>("CanvasLayer").GetNode<ColorRect>("UI/Baked");

        QuestCard = camera.GetNode<CanvasLayer>("CanvasLayer").GetNode<Panel>("UI/QuestCard");
        ClearQuestCard();

        ConsumableCard = camera.GetNode<CanvasLayer>("CanvasLayer").GetNode<Panel>("UI/ConsumableCard");
        ClearConsumableCard();

        EffectLabel = (PackedScene)ResourceLoader.Load(_effectLabelPath);

        BloodAlcoholContent = 0f;
        BloodTHCContent = 0f;
        BloodSugarContent = 0f;
        animation = GetNode<AnimationPlayer>("AnimationPlayer");
        sprite = GetNode<Sprite2D>("Sprite");
        skins = new Dictionary<string, Texture2D>();
        Resource textureResource = ResourceLoader.Load("res://Assets/Sprites/Dzhupels.png");
        skins.Add("naked", (Texture2D)textureResource);
        textureResource = ResourceLoader.Load("res://Assets/Sprites/1DzhupelsSarkanasBikses.png");
        skins.Add("pants", (Texture2D)textureResource);
        textureResource = ResourceLoader.Load("res://Assets/Sprites/2DzhupelsSarkansDzempers.png");
        skins.Add("sweater", (Texture2D)textureResource);
        textureResource = ResourceLoader.Load("res://Assets/Sprites/3DzhupelsSarkansKostims.png");
        skins.Add("costume", (Texture2D)textureResource);

        useConsumableSFX = GetNode<AudioStreamPlayer2D>("useConsumable");
        putDownConsumableSFX = GetNode<AudioStreamPlayer2D>("putDownConsumable");
        pickUpConsumableSFX = GetNode<AudioStreamPlayer2D>("pickUpConsumable");

        AchievementManager.ResetAchievements();
        AchievementManager.AddProgress("The Tester", progress);
    }


    public override void _PhysicsProcess(double delta)
    {
        totalPassedTime += (float)delta;
        if(Health <= 0)
        {
            if(consumedItems == 0 && !theMinimalist)
            {
                AchievementManager.AddProgress("The Minimalist", progress);
                theMinimalist = true;
            }

            if(totalPassedTime < 60 && !theSpeedrunner)
            {
                AchievementManager.AddProgress("The Speedrunner", progress);
                theSpeedrunner = true;
            }
            // Destroy the consumable
            if (consumable != null)
            {
                // Destroy the consumable
                consumable.QueueFree();
                consumable = null;
                ClearConsumableCard();
            }
            // Block user movement
            blockMovement = true;
            camera.Zoom = new Vector2(5, 5);
            animation.Play("DzhupelsCrying");
            deathTimePassed += (float)delta;
            if (deathTimePassed > 3)
            {
                Blackout.Visible = true;
                if (Blackout.Color.A < 1)
                {
                    Blackout.Color = IncreaseAlpha(Blackout.Color);
                }
                else
                {
                    // Put route to main menu here
                }
            }
        }

        if(BloodAlcoholContent > 0 && BloodTHCContent > 0 && BloodSugarContent > 0 && !theChemist)
        {
            AchievementManager.AddProgress("The Chemist", progress);
            theChemist = true;
        }
        if (!blockMovement)
        {
            velocity = Velocity;
            direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        }

        _speedDecreaseRate = 0.0005f * MathF.Pow(Speed - BaseSpeed, 2) + _minSpeedDecreaseRate;
        _alcoholDecreaseRate = 0.000015f * MathF.Pow(BloodAlcoholContent, 2) + _minAlcoholDecreaseRate;
        _thcDecreaseRate = 0.000015f * MathF.Pow(BloodTHCContent, 2) + _minTHCDecreaseRate;

        if (BloodSugarContent > 0f)
        {
            direction = direction * -1f;
            BloodSugarContent -= _bloodSugarDecreaseRate * (float)delta;
        }

        if (direction != Vector2.Zero)
        {
            if (direction.Y < 0)
            {
                sprite.Scale = new Vector2(-Math.Abs(sprite.Scale.X), sprite.Scale.Y);
                animation.Play("Dzhupels4");
            }
            else if (direction.Y > 0)
            {
                animation.Play("Dzhupels2");
                sprite.Scale = new Vector2(Math.Abs(sprite.Scale.X), sprite.Scale.Y);
            }
            else if (direction.X < 0)
            {
                animation.Play("Dzhupels3");
                sprite.Scale = new Vector2(-Math.Abs(sprite.Scale.X), sprite.Scale.Y);
            }
            else if (direction.X > 0)
            {
                animation.Play("Dzhupels3");
                sprite.Scale = new Vector2(Math.Abs(sprite.Scale.X), sprite.Scale.Y);
            }

            velocity.X = direction.X * Speed;
            velocity.Y = direction.Y * Speed;
        }
        else if (!blockMovement)
        {
            animation.Play("Dzhupels1");
            velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(velocity.Y, 0, Speed);
        }

        if (consumable != null)
        {
            consumable.GetParent().RemoveChild(consumable);
            AddChild(consumable);
            // Set consumable position to the player's position
            consumable.Position = new Vector2(0, 0);

            consumable.ZIndex = 1;
            // Consume the consumable on key press E
            if (Input.IsActionJustPressed("use"))
            {
                useConsumableSFX.Play();
                ConsumeConsumable();
            }
            // Drop consumable on key press Q
            else if (Input.IsActionJustPressed("drop"))
            {
                // Drop the consumable
                RemoveChild(consumable);
                GetParent().AddChild(consumable);
                consumable.ZIndex = 1;
                putDownConsumableSFX.Play();
                consumable.Position = new Vector2(GlobalPosition.X, GlobalPosition.Y + 30f);
                consumable = null;
                ClearConsumableCard();
            }
        }

        if (BloodAlcoholContent > 0f)
        {
            // Camera shaka
            Vector2 cameraPosition = camera.Position;

            // Add camera shake
            cameraPosition.X += (float)GD.RandRange(-BloodAlcoholContent, BloodAlcoholContent);
            cameraPosition.Y += (float)GD.RandRange(-BloodAlcoholContent, BloodAlcoholContent);

            // Add mathf.clamp to keep the camera within the screen
            cameraPosition.X = Mathf.Clamp(cameraPosition.X, -BloodAlcoholContent * 100, BloodAlcoholContent * 100);
            cameraPosition.Y = Mathf.Clamp(cameraPosition.Y, -BloodAlcoholContent * 100, BloodAlcoholContent * 100);

            // Set the camera's position
            camera.Position = cameraPosition;

            if (BloodAlcoholContent > 0f)
            {
                _bacBar.Visible = true;
            }

            if (BloodAlcoholContent > 2f)
            {
                if (timePassed > 1f && timePassed < 2f)
                {
                    //Set velocity to move towards target position
                    velocity.X = Mathf.MoveToward(velocity.X, targetPosition.X, BloodAlcoholContent * 50 / timePassed);
                    velocity.Y = Mathf.MoveToward(velocity.Y, targetPosition.Y, BloodAlcoholContent * 50 / timePassed);
                }
                else if (timePassed > 2f)
                {
                    targetPosition.X = (float)GD.RandRange(-BloodAlcoholContent * 300, BloodAlcoholContent * 300);
                    targetPosition.Y = (float)GD.RandRange(-BloodAlcoholContent * 300, BloodAlcoholContent * 300);
                    timePassed = 0f;
                }

                timePassed += (float)delta;
            }

            if (BloodAlcoholContent > 5f)
            {
                if(!lightsOut)
                {
                    AchievementManager.AddProgress("Lights Out!", progress);
                    lightsOut = true;
                }
                Blackout.Visible = true;
                if (Blackout.Color.A < 1)
                {
                    Blackout.Color = IncreaseAlpha(Blackout.Color);
                }
            }
            else
            {
                if (Blackout.Color.A > 0)
                {
                    Blackout.Color = DecreaseAlpha(Blackout.Color);
                }
                else
                {
                    Blackout.Visible = false;
                }
            }

            BloodAlcoholContent -= _alcoholDecreaseRate * (float)delta * 100;
        }
        else
        {
            _bacBar.Visible = false;
        }

        if (BloodTHCContent > 0f)
        {
            _thcBar.Visible = true;
            Baked.Color = new Color(Baked.Color.R, Baked.Color.G, Baked.Color.B, Mathf.Lerp(0, 0.7f, BloodTHCContent / MaxTHC));
            BloodTHCContent -= _thcDecreaseRate * (float)delta * 100;
        }
        else
        {
            _thcBar.Visible = false;
            Baked.Color = new Color(Baked.Color.R, Baked.Color.G, Baked.Color.B, 0);
        }

        Velocity = velocity;
        MoveAndSlide();

        // Increases or decreases player speed, depending on current value
        if (Speed > BaseSpeed + 1f)
        {
            Speed -= _speedDecreaseRate * (float)delta;
        } 
        else if (Speed < BaseSpeed)
        {
            Speed += _speedDecreaseRate * (float)delta;
        }

        // Moved this to _PhysicsProcess, sorry not sorry
        Health -= _healthDecreaseRate * _healthDecreaseMultiplier * (float)delta;
    }

    // on area entered
    private void _on_area_entered(Area2D area)
    {
        GD.Print("Area entered: ", area.Name);
        // If the area is in group consumable
        if (area.IsInGroup("Consumable"))
        {
            // Get information about the consumable
            Consumable consumable = (Consumable)area;
            if (this.consumable == null)
            {
                pickUpConsumableSFX.Play();
                this.consumable = consumable;
                UpdateConsumableCard(consumable);
            }

        }

        // If the area is in group consumable
        if (area.IsInGroup("Quest"))
        {
            // Get information about the consumable
            Quest quest = (Quest)area;
            UpdateQuestCard(quest);
            if (this.consumable != null)
            {
                if (quest.RequiredConsumableNode.Type == this.consumable.Type)
                {
                    ClearConsumableCard();
                    // Apply effects
                    ApplyQuestEffects(quest);
                    GD.Print("Quest completed!");
                    AchievementManager.AddProgress("The Good Citizen", progress);
                    theGoodCitizen = true;

                    CallDeferred("InstantiateRewardConsumable", quest);

                    consumable.QueueFree();
                    consumable = null;

                    quest.QueueFree();
                }
            }
        }
    }

    private void _on_area_exited(Area2D area)
    {
        if (area.IsInGroup("Quest"))
        {
            ClearQuestCard();
        }
    }

    private void ApplyConsumableEffects(Consumable consumable)
    {
        foreach (KeyValuePair<string, float> effect in consumable.Effects)
        {
            // Apply the effect
            switch (effect.Key)
            {
                case "Health":
                    Health += effect.Value;
                    break;
                case "Speed":
                    Speed += effect.Value;
                    break;
                case "Alcohol":
                    BloodAlcoholContent += effect.Value;
                    break;
                case "THC":
                    BloodTHCContent += effect.Value;
                    break;
                case "Sugar":
                    BloodSugarContent += effect.Value;
                    break;
            }
        }
    }
    private void ApplyQuestEffects(Quest quest)
    {
        // Iterate through all elements in the effects dictionary
        foreach (KeyValuePair<string, float> effect in quest.Effects)
        {
            // Apply the effect
            switch (effect.Key)
            {
                case "Health":
                    Health += effect.Value;
                    break;
                case "Speed":
                    Speed += effect.Value;
                    break;
                case "Alcohol":
                    BloodAlcoholContent += effect.Value;
                    break;
                case "THC":
                    BloodTHCContent += effect.Value;
                    break;
            }
        }
    }

    private void ConsumeConsumable()
    {
        if (consumable.Type == "Clothes")
        {
            ApplyClothing(consumable);
        } else
        {
            // If health is less than 1f
            if (Health <= 1f && !closeCall)
            {
                AchievementManager.AddProgress("Close Call", progress);
                closeCall = true;
            }
            consumedItems++;
            // Apply effects
            ApplyConsumableEffects(consumable);
            GD.Print("");
            GD.Print("You consumed a " + consumable.Title);
        }
        // Destroy the consumable
        consumable.QueueFree();
        consumable = null;
        ClearConsumableCard();
    }

    private void InstantiateRewardConsumable(Quest quest)
    {
        // SCENE CHANGE
        //GetTree().Root.GetNode("TestScene").AddChild(quest.RewardConsumableNode);
        GetTree().Root.GetNode<Node2D>("Node2D").AddChild(quest.RewardConsumableNode);
        //sceneNode.AddChild(quest.RewardConsumableNode);
        quest.RewardConsumableNode.Position = new Vector2(quest.Position.X, quest.Position.Y);
    }

    private Color IncreaseAlpha(Color prevColor)
    {
        return new Color(prevColor.R, prevColor.G, prevColor.B, prevColor.A += 0.015f);
    }
    private Color DecreaseAlpha(Color prevColor)
    {
        return new Color(prevColor.R, prevColor.G, prevColor.B, prevColor.A -= 0.015f);
    }

    private void ZoomCamera()
    {
        camera.Zoom = new Vector2(camera.Zoom.X + 0.01f, camera.Zoom.Y + 0.01f);
    }

    private Label CreateAppropriateLabel(KeyValuePair<string, float> effect, Panel card)
    {
        if (effect.Value > 0)
        {
            // If the effect is health or speed
            if (effect.Key == "Health" || effect.Key == "Speed")
            {
                Label EffectLabelNode = CreateEffectLabel(effect, card);
                EffectLabelNode.AddThemeColorOverride("font_color", new Color(0, 1, 0));
                return EffectLabelNode;
            }
            // If the effect is alcohol or THC
            else if (effect.Key == "Alcohol" || effect.Key == "THC" || effect.Key == "Sugar")
            {
                Label EffectLabelNode = CreateEffectLabel(effect, card);
                EffectLabelNode.AddThemeColorOverride("font_color", new Color(1, 0, 0));
                return EffectLabelNode;
            }
        }

        // If the effect is negative
        else if (effect.Value < 0)
        {
            // If the effect is health or speed
            if (effect.Key == "Health" || effect.Key == "Speed")
            {
                Label EffectLabelNode = CreateEffectLabel(effect, card);
                EffectLabelNode.AddThemeColorOverride("font_color", new Color(1, 0, 0));
                return EffectLabelNode;
            }
            // If the effect is alcohol or THC
            else if (effect.Key == "Alcohol" || effect.Key == "THC" || effect.Key == "Sugar")
            {
                Label EffectLabelNode = CreateEffectLabel(effect, card);
                EffectLabelNode.AddThemeColorOverride("font_color", new Color(0, 1, 0));
                return EffectLabelNode;
            }
        }
        return null;
    }

    private Label CreateEffectLabel(KeyValuePair<string, float> effect, Panel card)
    {
        Label EffectLabelNode = EffectLabel.Instantiate() as Label;
        card.GetNode<VBoxContainer>("VBoxContainer").AddChild(EffectLabelNode);
        EffectLabelNode.Text = effect.Key + ": " + effect.Value;

        return EffectLabelNode;
    }

    private void UpdateConsumableCard(Consumable consumable)
    {
        ConsumableCard.GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("Title").Text = consumable.Title;
        ConsumableCard.GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("Title").AddThemeColorOverride("font_color", consumable.DisplayColor);
        ConsumableCard.GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("Description").Text = consumable.Description;

        // Foreach effect in the quest
        foreach (KeyValuePair<string, float> effect in consumable.Effects)
        {
            ConsumableEffectLabels.Add(CreateAppropriateLabel(effect, ConsumableCard));
        }

        ConsumableCard.Visible = true;
    }

    private void UpdateQuestCard(Quest quest)
    {
        QuestCard.GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("Title").Text = quest.Title;
        QuestCard.GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("Description").Text = quest.Description;

        QuestCard.GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("RequiredConsumable").Text = quest.RequiredConsumableNode.Title;
        QuestCard.GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("RequiredConsumable").AddThemeColorOverride("font_color", quest.RequiredConsumableNode.DisplayColor);

        QuestCard.GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("RewardConsumable").Text = quest.RewardConsumableNode.Title;
        QuestCard.GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("RewardConsumable").AddThemeColorOverride("font_color", quest.RewardConsumableNode.DisplayColor);

        // Foreach effect in the quest
        foreach (KeyValuePair<string, float> effect in quest.Effects)
        {
            // If the effect is positive
            QuestEffectLabels.Add(CreateAppropriateLabel(effect, QuestCard));
        }

        QuestCard.Visible = true;
    }

    private void ClearQuestCard()
    {
        // Iterate through quest effect labels
        foreach(Label label in QuestEffectLabels)
        {
            label.QueueFree();
        }
        QuestEffectLabels.Clear();
        QuestCard.Visible = false;
    }

    private void ClearConsumableCard()
    {
        // Iterate through quest effect labels
        foreach (Label label in ConsumableEffectLabels)
        {
            label.QueueFree();
        }

        ConsumableEffectLabels.Clear();
        ConsumableCard.Visible = false;
    }

    private void SkinChoice(string skinName)
    {
        GD.Print("Skin choice: " + currentSkin);
        if (currentSkin != skinName)
        {
            if (currentSkin != "naked")
            {
                currentSkin = "costume";
                sprite.Texture = skins[currentSkin];
                AchievementManager.AddProgress("Dressed to Impress", progress);
                return;
            }
        }

        if(currentSkin == "naked")
        {
            currentSkin = skinName;
            sprite.Texture = skins[skinName];
        }
        GD.Print("Skin changed to " + skinName);
    }

    private void ApplyClothing(Consumable clothes)
    {
        SkinChoice(clothes.TextureName);
        ApplyClothingEffects(clothes);
    }

    private void ApplyClothingEffects(Consumable clothes)
    {
        foreach (KeyValuePair<string, float> effect in clothes.Effects)
        {
            if (effect.Key == "PermanentHealth")
            {
                MaxHealth += effect.Value;
                Health += effect.Value;
            }
            if (effect.Key == "PermanentSpeed")
            {
                BaseSpeed += effect.Value;
                Speed += effect.Value;
            }
        }
    }
}
