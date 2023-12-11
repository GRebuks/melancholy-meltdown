using Godot;
using System;

public partial class HealthBar : ProgressBar
{
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        Value = currentHealth / maxHealth;
    }
}
