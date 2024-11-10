using Godot;
using System;

public partial class MarkerDialog : Marker2D
{
	private Random rand = new Random();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HitDialogActive()
	{
		Label _hitIndicator = new Label();
		_hitIndicator = ResourceLoader.Load<PackedScene>("res://Simple_RPG/node/LabelDialog.tscn").Instantiate<Label>();
		
		_hitIndicator.Position = _hitIndicator.Position + new Vector2(rand.Next(-10, 10), rand.Next(-10, 10));
		
		this.AddChild(_hitIndicator);
	}
	
	public void HitButtonPressed()
	{
		Label _hitIndicator = new Label();
		_hitIndicator = ResourceLoader.Load<PackedScene>("res://Simple_RPG/node/LabelDialog.tscn").Instantiate<Label>();
		
		_hitIndicator.Position = _hitIndicator.Position + new Vector2(rand.Next(-10, 10), rand.Next(-10, 10));
		
		this.AddChild(_hitIndicator);
		
	}
}
