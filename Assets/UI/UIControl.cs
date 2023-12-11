using Godot;
using System;

public partial class UIControl : Control
{
    private ProgressBar progressBar;
    private Label label;

    public override void _Ready()
    {
        // Assuming you have a ProgressBar and a Label as children of this node
        progressBar = GetNode<ProgressBar>("HealthBar");
        label = GetNode<Label>("EmotionalDamage");

        // Set the maximum value of the ProgressBar
        progressBar.MaxValue = 60;

        UpdateLabel();
    }

    //public override void _Process(float delta)
    //{
    //    // Update the label text
    //    UpdateLabel();
    //}

    private void UpdateLabel()
    {
        // Update the label text with the ProgressBar value
        label.Text = $"Emotional Damage: {(int)progressBar.Value}%";
    }
}