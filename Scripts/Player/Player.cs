using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public float BaseSpeed = 300.0f;
    public float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;
    public float Addiction = 0f;

    [Export] public float BloodAlcoholContent = 0f;
    public float timePassed = 0f;
    Vector2 targetPosition = new Vector2();

    public Consumable consumable = null;

    // Health bar
    private ProgressBar _healthBar;
    private Label label;

    // Camera
    public Camera2D camera;

    private float _health = 60f;
    public float MaxHealth { get; private set; } = 60f;

    public float _healthDecreaseMultiplier = 1.0f;
    private float _healthDecreaseRate = 1.0f;
    private float _speedDecreaseRate = 5.0f;
    private float _minSpeedDecreaseRate = 5.0f;
    public float Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0f, MaxHealth);
            _healthBar.Value = _health;
            label.Text = $"Emotional Damage: {(int)(_health * 100 / MaxHealth)}% | {(int)_health}/{(int)MaxHealth} | {MathF.Round(_health, 2)}s";
        }
    }

    public override void _Ready()
    {
        camera = (Camera2D)GetNode("Camera2D");
        _healthBar = camera.GetNode<ProgressBar>("UI/HealthBar");
        label = camera.GetNode<Label>("UI/EmotionalDamage");
        Health = 60f;
    }


    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        _speedDecreaseRate = 0.0005f * MathF.Pow(Speed - BaseSpeed, 2) + _minSpeedDecreaseRate;

        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Y = direction.Y * Speed;
        }
        else
        {
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
            }
        }

        if (BloodAlcoholContent !> 0f)
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

            // Add camera rotation
            // camera.RotationDegrees += (float)GD.RandRange(-BloodAlcoholContent, BloodAlcoholContent);
            // Add mathf.clamp to keep the camera within the screen
            // camera.RotationDegrees = Mathf.Clamp(camera.RotationDegrees, -BloodAlcoholContent * 10, BloodAlcoholContent * 10);

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

            BloodAlcoholContent -= 0.001f;
            GD.Print("Blood Alcohol Content: " + BloodAlcoholContent);
        }
        Velocity = velocity;
        MoveAndSlide();

        // Increases or decreases player speed, depending on current value
        if (Speed > 301f)
        {
            Speed += _speedDecreaseRate * (float)delta;
            GD.Print(Speed);
            GD.Print(_speedDecreaseRate);
        } 
        else if (Speed < 300f)
        {
            Speed -= -_speedDecreaseRate * (float)delta;
            GD.Print("");
            GD.Print(Speed);
            GD.Print(_speedDecreaseRate);
            GD.Print(_speedDecreaseRate * (float)delta);
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
                GD.Print("");
                GD.Print("You collected a " + consumable.Title);
                GD.Print("Consumable type: " + consumable.Type);
            }
        }

        // If the area is in group consumable
        if (area.IsInGroup("Quest"))
        {
            // Get information about the consumable
            Quest quest = (Quest)area;
            if (this.consumable != null)
            {
                if (quest.RequiredConsumableType == this.consumable.Type)
                {
                    // Apply effects
                    ApplyQuestEffects(quest);
                    GD.Print("Quest completed!");
                    DisplayPlayerStats();

                    CallDeferred("InstantiateRewardConsumable", quest);

                    consumable.QueueFree();
                    consumable = null;

                    quest.QueueFree();
                }
            }
            else
            {
                GD.Print("");
                GD.Print("Quest: " + quest.Title);
                GD.Print("Required consumable: " + quest.RequiredConsumableType);
            }
        }
    }

    private void ApplyConsumableEffects(Consumable consumable)
    {
        // Apply the health effect
        Health += consumable.HealthEffect;

        // Apply the addiction effect
        Addiction += consumable.AddictionEffect;

        // Apply the speed effect
        Speed += consumable.SpeedEffect;

        // Apply the alcohol effect
        BloodAlcoholContent += consumable.AlcoholEffect;
    }
    private void ApplyQuestEffects(Quest quest)
    {
        // Apply the health effect
        Health += quest.HealthEffect;

        // Apply the addiction effect
        Addiction += quest.AddictionEffect;

        // Apply the speed effect
        Speed += quest.SpeedEffect;

        // Apply the alcohol effect
        BloodAlcoholContent += quest.AlcoholEffect;
    }

    private void DisplayPlayerStats()
    {
        GD.Print("Health: " + Health);
        GD.Print("Addiction: " + Addiction);
        GD.Print("Speed: " + Speed);
    }

    private void ConsumeConsumable()
    {
        // Apply effects
        ApplyConsumableEffects(consumable);
        GD.Print("");
        GD.Print("You consumed a " + consumable.Title);
        DisplayPlayerStats();

        // Destroy the consumable
        consumable.QueueFree();

        consumable = null;
    }

    private void InstantiateRewardConsumable(Quest quest)
    {
        Node2D rewardNode = quest.RewardConsumable.Instantiate() as Node2D;
        GetTree().Root.GetNode("TestScene").AddChild(rewardNode);
        rewardNode.Position = new Vector2(quest.Position.X, quest.Position.Y);

    }
}
