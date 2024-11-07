using System.Collections.Generic;
using Godot;
using Playground_C.Simple_RPG.scripts;

public partial class Player : CharacterBody2D
{
	[Export]
	private float BaseSpeed = 300f;
	
	Vector2 _lastMove = new Vector2(0, 1);

	private IPlayerState _currentState;
	private readonly WalkState walkState = new WalkState();
	private readonly RunState runState = new RunState();

	private List<IIntractbleObject> _objectsInArea = new List<IIntractbleObject>();
	private int _currentObjectIndex = -1;
	
	private void changeState(IPlayerState state)
	{
		_currentState = state;
		_currentState.EnterState(this);
	}
	public override void _Ready()
	{
		changeState(walkState);
		PlayAnimationIdle(_lastMove);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 directionMove = Input.GetVector("walk_kiri", "walk_kanan", "walk_maju", "walk_mundur");

		if (Input.IsActionJustPressed("float"))
		{
			changeState(runState);
		}

		if (Input.IsActionJustReleased("float"))
		{
			changeState(walkState);
		}
		
		if (Input.IsActionPressed("walk_maju") || Input.IsActionPressed("walk_mundur") || Input.IsActionPressed("walk_kiri") || Input.IsActionPressed("walk_kanan") )
		{
			if (directionMove != Vector2.Zero) //if there no input, skip it
			{
				PlayAnimation(directionMove);
				_lastMove = directionMove;
			}
			
		}else
		{
			PlayAnimationIdle(_lastMove);
		}
		
		_currentState.MoveState(this,directionMove,(float)delta);
		
		MoveAndSlide();
	}

	public override void _Process(double delta)
	{

		if (Input.IsActionJustPressed("next_item"))
		{
			_objectsInArea[_currentObjectIndex].HideIndicator();

			_currentObjectIndex = (_currentObjectIndex + 1) % _objectsInArea.Count;
			_objectsInArea[_currentObjectIndex].ShowIndicator();
			
		}
		if (Input.IsActionJustPressed("interact"))
		{
			_objectsInArea[_currentObjectIndex].Interact();
		}

		if (Input.IsActionJustPressed("next_item"))
		{
			GD.Print("next_item");
		}
	}

	private void PlayAnimation(Vector2 direction)
	{
		AnimatedSprite2D animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		//cek apakah kedua arah vector memiliki nilai (bergerak diagonal)
		if (direction.X != 0 && direction.Y != 0)
		{
			//hanya akan memainkan animasi berjalan keatas dan kebawah
			if (direction.Y > 0)
			{
				
				animationPlayer.Play(_currentState.getAnimationDepan());
			}
			else
			{
				
				animationPlayer.Play(_currentState.getAnimationBelakang());
			}
		}else if (direction.Y == 0) // jika hanya vector x yang memiliki nilai (hanya bergerak kanan, kiri)
		{
			
			if (direction.X > 0)
			{
				animationPlayer.FlipH = false;
				animationPlayer.Play(_currentState.getAnimationSamping());
			}
			else
			{
				animationPlayer.FlipH = true;
				animationPlayer.Play(_currentState.getAnimationSamping());
			}
		}
		else if (direction.X == 0)// jika hanya vector y yang memiliki nilai (bergerak atas, bawah)
		{
			if (direction.Y > 0)
			{
				animationPlayer.Play(_currentState.getAnimationDepan());
			}
			else
			{
				animationPlayer.Play(_currentState.getAnimationBelakang());
			}
		}

		
	}

	private void PlayAnimationIdle(Vector2 direction)
	{
		AnimatedSprite2D animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		//cek apakah kedua arah vector memiliki nilai (bergerak diagonal)
		if (direction.X != 0 && direction.Y != 0)
		{
			//hanya akan memainkan animasi berjalan keatas dan kebawah
			if (direction.Y > 0)
			{
				
				animationPlayer.Play("idle_depan");
			}
			else
			{
				
				animationPlayer.Play("idle_belakang");
			}
			
		}else if (direction.Y == 0) // jika hanya vector x yang memiliki nilai (hanya bergerak kanan, kiri)
		{
			if (direction.X > 0)
			{
				animationPlayer.FlipH = false;
				animationPlayer.Play("idle_samping");
			}
			else
			{
				animationPlayer.FlipH = true;
				animationPlayer.Play("idle_samping");
			}
		}
		else if (direction.X == 0)// jika hanya vector y yang memiliki nilai (bergerak atas, bawah)
		{
			if (direction.Y > 0)
			{
				animationPlayer.Play("idle_depan");
			}
			else
			{
				animationPlayer.Play("idle_belakang");
			}
		}
	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is IIntractbleObject intractbleObject)
		{
			_objectsInArea.Add(intractbleObject);
			intractbleObject.show_Highlight();

			if (_objectsInArea.Count == 1 || intractbleObject == _objectsInArea[_objectsInArea.Count - 1])
			{
				UpdateIndicator();
			}
		}

		GD.Print(_objectsInArea.ToArray());
		
	}

	private void _on_area_2d_body_exited(Node2D body)
	{
		if (body is IIntractbleObject intractbleObject && _objectsInArea.Contains(intractbleObject))
		{
			// Hapus objek dari daftar
			_objectsInArea.Remove(intractbleObject);
			intractbleObject.hide_Highlight(); // Matikan highlight pada objek yang keluar
			intractbleObject.HideIndicator();

			// Periksa apakah objek yang keluar adalah objek yang memiliki indikator aktif
			if (_objectsInArea.Count > 0)
			{
				// Jika masih ada objek dalam area, perbarui indikator
				if (_currentObjectIndex >= _objectsInArea.Count)
				{
					// Sesuaikan indeks jika objek terakhir dalam daftar keluar
					_currentObjectIndex = _objectsInArea.Count - 1;
				}
				UpdateIndicator();
			}
			else
			{
				// Jika tidak ada objek tersisa dalam area, reset indikator dan indeks
				_currentObjectIndex = -1;
			}
		}
		
		GD.Print(_objectsInArea.ToArray());
		
	}

	private void UpdateIndicator()
	{
		// Matikan indikator pada objek yang saat ini memiliki indikator aktif
		if (_currentObjectIndex >= 0 && _currentObjectIndex < _objectsInArea.Count)
		{
			_objectsInArea[_currentObjectIndex].HideIndicator();
		}

		// Periksa jika ada objek dalam area
		if (_objectsInArea.Count > 0)
		{
			// Set objek terakhir dalam daftar sebagai objek dengan indikator aktif
			_currentObjectIndex = _objectsInArea.Count - 1;
			_objectsInArea[_currentObjectIndex].ShowIndicator();
		}
		else
		{
			_currentObjectIndex = -1; // Reset indeks jika tidak ada objek di area
		}
	}
	
}
