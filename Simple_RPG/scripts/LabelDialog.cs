using Godot;
using System;

public partial class LabelDialog : Label
{
	private AnimationPlayer _animationPlayer;
	private Random _random = new Random();

	public override void _Ready()
	{
		
		_animationPlayer =GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("dialog_show");
	}

	public void AnimationFinished()
	{
		this.QueueFree();
		GD.Print("menghapus + ", this.Name);
	}

	public void TimeOut()
	{
		this.QueueFree();
	}
}
