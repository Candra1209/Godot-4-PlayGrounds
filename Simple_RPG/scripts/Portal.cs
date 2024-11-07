using Godot;
using System;
using Playground_C.Simple_RPG.scripts;

public partial class Portal : StaticBody2D, IIntractbleObject
{
	[Export] bool _isActive = false;
	
	private AnimationPlayer _animationPlayer;
	private Sprite2D _effect;
	private Sprite2D _highlight;
	private Indicator _indicator;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// mendapatkan node dari tree
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_effect = GetNode<Sprite2D>("Effect");
		_highlight = GetNode<Sprite2D>("Highlight");
		_indicator = GetNode<Indicator>("indicator");
		
		//mematikan haighlight
		_highlight.Visible = false;
		_indicator.Hide();
		hide_Highlight();

		if (_isActive)
		{
			play_effect();
		}
		else
		{
			stop_effect();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Interact()
	{
		throw new NotImplementedException();
	}

	public void Interact(bool newState)
	{
		throw new NotImplementedException();
	}

	public void show_Highlight()
	{
		_highlight.Visible = true;
	}

	public void hide_Highlight()
	{
		_highlight.Visible = false;
	}

	public void play_effect()
	{
		_effect.Visible = true;
		_animationPlayer.Play("active_effect");
	}

	public void stop_effect()
	{
		_effect.Visible = false;
		_animationPlayer.Stop();
	}

	public void IsEnterDetectArea()
	{
		show_Highlight();
	}

	public void IsExitDetectArea()
	{
		hide_Highlight();
	}
	
	public void ShowIndicator()
	{
		_indicator.Activate();
	}

	public void HideIndicator()
	{
		_indicator.Deactivate();
	}
	
}
