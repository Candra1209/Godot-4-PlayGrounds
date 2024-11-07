using Godot;
using System;

public partial class Indicator : Node2D
{
	[Export] private bool _isActive = false;
	
	private AnimationPlayer _animPlayer;
	
	public override void _Ready()
	{
		_animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		
		if (_isActive)
		{
			Activate();
		}
		else
		{
			Deactivate();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Activate()
	{
		this.Visible = true;
		_animPlayer.Play("idle");
	}

	public void Deactivate()
	{
		this.Visible = false;
		_animPlayer.Stop();
	}
}
