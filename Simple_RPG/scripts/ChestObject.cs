using Godot;
using System;
using Godot.Collections;
using Playground_C.Simple_RPG.scripts;

public partial class ChestObject : StaticBody2D,IIntractbleObject
{

	private Dictionary<int,string[]> _chestSkin = new Dictionary<int,string[]>()
	{
		{0 , new string[] {"Chest1", "Highlight_Chest1"}},
		{1 , new string[] {"Chest2", "Highlight_Chest2"}},
		{2 , new string[] {"Chest3", "Highlight_Chest3"}},
		{3 , new string[] {"Chest4", "Highlight_Chest4"}}
	};
	
	private enum ChestSkin
	{	
		Chest1,Chest2,Chest3,Chest4
	}
	
	
	AnimatedSprite2D _chestSprite = new AnimatedSprite2D();
	AnimatedSprite2D _chestHighlight = new AnimatedSprite2D();
	
	[Export] private ChestSkin _skin = ChestSkin.Chest1;

	//private string _chestSkin;
	private int _indexSkin;

	private int _firstIndexSprite;
	private int _lastIndexSprite;

	private Indicator _indicator;
	
	[Export]
	private bool _isOpen = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//mendapatkan node AnimatedSprite2D
		_chestSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_chestHighlight = GetNode<AnimatedSprite2D>("Highlight");
		
		_chestHighlight.Visible = false;
		
		//mendapatkan node indicator
		_indicator = GetNode<Indicator>("Indicator");
		
		//mengubah enum ke dalam string agar dapat digunakan
		// _chestSkin = _skin.GetHashCode();
		_indexSkin = (int)_skin;
		
		//menampilkan skin chest yang sesuai dengan pilihan
		_chestSprite.Animation = _chestSkin[_indexSkin][1];
		_chestHighlight.Animation = _chestSkin[_indexSkin][1];
		
		//mendapatkan sprite awal dan akhir dari animation saat ini
		_firstIndexSprite = 0;
		_lastIndexSprite = _chestSprite.SpriteFrames.GetFrameCount(_chestSprite.Animation) - 1;
		
		// menampilkan sprite terkahir jika isOpen true || sprite terakhir jika false
		if (_isOpen)
		{
			_chestSprite.Frame = _lastIndexSprite;
		}
		else
		{
			_chestSprite.Frame = _firstIndexSprite;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OpenChest()
	{
		_chestSprite.Play(_chestSkin[_indexSkin][1]);
		_chestHighlight.Play(_chestSkin[_indexSkin][1]);
		_isOpen = true;
	}

	private void CloseChest()
	{
		_chestSprite.Play(_chestSkin[_indexSkin][1],-1f,true);
		_chestHighlight.Play(_chestSkin[_indexSkin][1],-1,true);
		_isOpen = false;
	}

	public void Interact()
	{
		if (_isOpen)
		{
			CloseChest();
		}
		else
		{
			OpenChest();
		}
	}

	public void Interact(bool newState)
	{
		if (_isOpen)
		{
			CloseChest();
		}
		else
		{
			OpenChest();
		}
	}

	public void play_animation()
	{
		throw new NotImplementedException();
	}

	public void show_Highlight()
	{
		_chestHighlight.Visible = true;
		if (_isOpen)
		{
			_chestHighlight.Frame = _lastIndexSprite;
		}
		else
		{
			_chestHighlight.Frame = _firstIndexSprite;
		}
	}

	public void ShowIndicator()
	{
		_indicator.Activate();
	}

	public void HideIndicator()
	{ 
		_indicator.Deactivate();
	}

	public void hide_Highlight()
	{
		_chestHighlight.Visible = false;
	}

	public void _on_button_pressed()
	{
		Interact(!_isOpen);
	}

	public void _on_button_open_pressed()
	{
		OpenChest();
	}

	public void _on_button_close_pressed()
	{
		CloseChest();
	}

	public void IsEnterDetectArea()
	{
		show_Highlight();
	}

	public void IsExitDetectArea()
	{
		hide_Highlight();
	}
}
