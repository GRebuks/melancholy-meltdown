using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    public float BaseSpeed = 300.0f;
    public float Speed = 300.0f;

    public float timePassed = 0f;
    Vector2 targetPosition = new Vector2();

    public Consumable consumable = null;

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
    private float _health = 60f;

    public float MaxBAC { get; private set; } = 6f;
    public float MaxTHC { get; private set; } = 3f;
    public float MaxHealth { get; private set; } = 60f;

    public float _healthDecreaseMultiplier = 1.0f;
    private float _healthDecreaseRate = 1.0f;
    private float _speedDecreaseRate = 5.0f;
    private float _minSpeedDecreaseRate = 5.0f;

    private float _alcoholDecreaseRate = 5.0f;
    private float _minAlcoholDecreaseRate = 0.001f;

    private float _thcDecreaseRate = 5.0f;
    private float _minTHCDecreaseRate = 0.001f;
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
        camera = (Camera2D)GetNode("Camera2D");
        _healthBar = camera.GetNode<ProgressBar>("UI/HealthBar");
        _healthBarLabel = _healthBar.GetNode<Label>("EmotionalDamage");

        _bacBar = camera.GetNode<ProgressBar>("UI/BACBar");
        _bacBarLabel = _bacBar.GetNode<Label>("BAC");

        _thcBar = camera.GetNode<ProgressBar>("UI/THCBar");
        _thcBarLabel = _thcBar.GetNode<Label>("THC");

        Blackout = camera.GetNode<ColorRect>("UI/Blackout");
        Baked = camera.GetNode<ColorRect>("UI/Baked");

        QuestCard = camera.GetNode<Panel>("UI/QuestCard");
        ClearQuestCard();

        ConsumableCard = camera.GetNode<Panel>("UI/ConsumableCard");
        ClearConsumableCard();

        EffectLabel = (PackedScene)ResourceLoader.Load(_effectLabelPath);

        BloodAlcoholContent = 0f;
        BloodTHCContent = 0f;
        animation = GetNode<AnimationPlayer>("AnimationPlayer");
        sprite = GetNode<Sprite2D>("Sprite");
    }


    public override void _PhysicsProcess(double delta)
    {
        if(Health <= 0)
        {
            ZoomCamera();
        }
        Vector2 velocity = Velocity;
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        _speedDecreaseRate = 0.0005f * MathF.Pow(Speed - BaseSpeed, 2) + _minSpeedDecreaseRate;
        _alcoholDecreaseRate = 0.000015f * MathF.Pow(BloodAlcoholContent, 2) + _minAlcoholDecreaseRate;
        _thcDecreaseRate = 0.000015f * MathF.Pow(BloodTHCContent, 2) + _minTHCDecreaseRate;

        if (direction != Vector2.Zero)
        {
            if (direction.Y < 0)
            {
                sprite.Scale = new Vector2(-1, sprite.Scale.Y);
                animation.Play("RunUp");
            } 
            else if (direction.Y > 0) 
            {
                animation.Play("RunDown");
                sprite.Scale = new Vector2(1, sprite.Scale.Y);
            } 
            else if (direction.X < 0)
            {
                animation.Play("Run");
                sprite.Scale = new Vector2(-1, sprite.Scale.Y);
            }
            else if (direction.X > 0)
            {
                animation.Play("Run");
                sprite.Scale = new Vector2(1, sprite.Scale.Y);
            }
            velocity.X = direction.X * Speed;
            velocity.Y = direction.Y * Speed;
        }
        else
        {
            animation.Play("Idle");
            velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(velocity.Y, 0, Speed);
        }

        if (consumable != null)
        {
            consumable.Position = new Vector2(this.Position.X, this.Position.Y - 30);
            // Consume the consumable on key press E
            if (Input.IsActionJustPressed("ui_accept"))
            {
                ConsumeConsumable();
            }
            // Drop consumable on key press Q
            else if (Input.IsActionJustPressed("ui_cancel"))
            {
                // Drop the consumable
                consumable.Position = new Vector2(this.Position.X, this.Position.Y - 30);
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
                    velocity.X = Mathf.MoveToward(velocity.X, targetPosition.X, BloodAlcoholContent * 20 / timePassed);
                    velocity.Y = Mathf.MoveToward(velocity.Y, targetPosition.Y, BloodAlcoholContent * 20 / timePassed);
                }
                else if (timePassed > 2f)
                {
                    targetPosition.X = (float)GD.RandRange(-BloodAlcoholContent * 100, BloodAlcoholContent * 100);
                    targetPosition.Y = (float)GD.RandRange(-BloodAlcoholContent * 100, BloodAlcoholContent * 100);
                    timePassed = 0f;
                }

                timePassed += (float)delta;
            }

            if (BloodAlcoholContent > 5f)
            {
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
        if (Speed > 301f)
        {
            Speed -= _speedDecreaseRate * (float)delta;
        } 
        else if (Speed < 300f)
        {
            Speed += _speedDecreaseRate * (float)delta;
        }

        // Moved this to _PhysicsProcess, sorry not sorry
        Health -= _healthDecreaseRate * _healthDecreaseMultiplier * (float)delta;
    }

    // on area entered
    private void _on_area_entered(Area2D area)
    {
        // If the area is in group consumable
        if (area.IsInGroup("Consumable"))
        {
            // Get information about the consumable
            Consumable consumable = (Consumable)area;
            if (this.consumable == null)
            {
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
        // Apply effects
        ApplyConsumableEffects(consumable);
        GD.Print("");
        GD.Print("You consumed a " + consumable.Title);

        // Destroy the consumable
        consumable.QueueFree();
        consumable = null;
        ClearConsumableCard();
    }

    private void InstantiateRewardConsumable(Quest quest)
    {
        GetTree().Root.GetNode("TestScene").AddChild(quest.RewardConsumableNode);
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
            else if (effect.Key == "Alcohol" || effect.Key == "THC")
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
            else if (effect.Key == "Alcohol" || effect.Key == "THC")
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
}
