using Godot;
using System;
using Playground_C.Simple_RPG.scripts;

public partial class DummyEnemy : StaticBody2D, IEnemy
{
	[Export]public int Health { get; set; }
	[Export]public int Defender { get; set; }
	[Export]public int Power { get; set; }
	
	private MarkerDialog _markerDialog;
	private AnimationPlayer _animation;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_markerDialog = GetNode<MarkerDialog>("MarkerDialog");
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void Hit()
	{
		_markerDialog.HitDialogActive();
		_animation.Play("getHit");
	}
}
