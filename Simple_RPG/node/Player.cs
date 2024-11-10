using System;
using System.Collections.Generic;
using Godot;
using Playground_C.Simple_RPG.scripts;

public partial class Player : CharacterBody2D
{
	[Export]
	private float BaseSpeed = 300f;
	
	

	//STATE
	private int _currentObjectIndex = -1;
	private int _currAttackSequence = 1;
	private IPlayerState _currentState;
	private bool _isAttack = false;
	private readonly WalkState walkState = new WalkState();
	private readonly RunState runState = new RunState();
	Vector2 _lastMove = new Vector2(0, 1);

	
	private List<IIntractbleObject> _objectsInArea = new List<IIntractbleObject>();
	private List<String> _attackAnimaionlist = new List<String>()
	{
		"attack_belakang_1","attack_belakang_2",
		"attack_depan_1", "attack_depan_2",
		"attack_samping_1", "attack_samping_2"
	};
	
	//GET NODE
	private Timer _attackIntervalTimer = new Timer();
	AnimatedSprite2D animationPlayer = new AnimatedSprite2D();
	private CollisionShape2D _currAttackCollision = new CollisionShape2D();
	
	
	private void changeState(IPlayerState state)
	{
		_currentState = state;
		_currentState.EnterState(this);
	}
	public override void _Ready()
	{
		//get reff
		_attackIntervalTimer = GetNode<Timer>("AttackInterval");
		animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		changeState(walkState);
		PlayAnimationIdle(_lastMove);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_isAttack != true)
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
	}

	public override void _Process(double delta)
	{
		if (!_isAttack)
		{
			if (Input.IsActionJustPressed("attack"))
			{
				_isAttack = true;
				Attack(_lastMove);
				_attackIntervalTimer.Start();
			
			}
		}
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
			GD.Print(_objectsInArea.ToArray());
		}

		if (body is IEnemy enemy)
		{
			GD.Print("musuh memasuki area interaksi");
		}
		
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
			GD.Print(_objectsInArea.ToArray());
		}

		if (body is IEnemy enemy)
		{
			GD.Print("musuh keluar area interaksi");
		}
		
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

	private void InAttackIntervalTimeOut()
	{
		// kode ini akan dipanggil oleh sinyal dari Timer,
		// saat waktu habis, sequence attack akan direset
		GD.Print("time out, reset the attack sequence");
		_currAttackSequence = 1;
	}

	
	private void AttackEnd()
	{
		//fungsi ini akan dipanggil saat sinyal dari AnimationPlayer,
		//saat animasi selesai dimain kan :
		// ubah state _isAttack, dan hilangkan collison berkaitan dan matikan collison
		
		
		//cek apakah animasi yang dimainkan merupakan animasi dari daftar animasi attack
		if (_attackAnimaionlist.Contains(animationPlayer.GetAnimation()))
		{
			_isAttack = false;
			_currAttackCollision.Visible = false;
			_currAttackCollision.Disabled = true;
		}
	}

	private void Attack(Vector2 direction)
	{
		//get node animasi
		AnimatedSprite2D animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		//var dinamis, karena animasi akan berubah-ubah sesuai arah menghadap
		string animation;
		
		// cek apakah x dan y ditekan bersamaan, untuk mengatur gerakan menyamping
		if (direction.X != 0 && direction.Y != 0)
		{
			//hanya akan memainkan animasi attack keatas dan kebawah
			if (direction.Y > 0)
			{
				//ambil node collisin attack yang sesuai, munculkan dan aktivkan collision
				_currAttackCollision = GetNode<CollisionShape2D>("AttackArea/AttackCollisionDepan");
				_currAttackCollision.Visible = true;
				_currAttackCollision.Disabled = false;
				
				//Mainkan animasi
				animation = "attack_depan_" + _currAttackSequence;
				animationPlayer.Play(animation);
			}
			else
			{
				//ambil node collisin attack yang sesuai, munculkan dan aktivkan collision
				_currAttackCollision = GetNode<CollisionShape2D>("AttackArea/AttackCollisionBelakang");
				_currAttackCollision.Visible = true;
				_currAttackCollision.Disabled = false;
				
				//mainkan animasi
				animation = "attack_belakang_" + _currAttackSequence;
				animationPlayer.Play(animation);
			}
			
		}else if (direction.Y == 0) // jika hanya vector x yang memiliki nilai (hanya bergerak kanan, kiri)
		{
			// string untuk membuat var dinamis animasi yang sesuai
			animation = "attack_samping_" + _currAttackSequence;
			
			if (direction.X > 0)
			{	//ambil node collisin attack yang sesuai, munculkan dan aktivkan collision
				_currAttackCollision = GetNode<CollisionShape2D>("AttackArea/AttackCollisionKanan");
				_currAttackCollision.Visible = true;
				_currAttackCollision.Disabled = false;
				
				//balik sprite, dan mainkan animasi
				animationPlayer.FlipH = false;
				animationPlayer.Play(animation);
			}
			else
			{
				//ambil node collisin attack yang sesuai, munculkan dan aktivkan collision
				_currAttackCollision = GetNode<CollisionShape2D>("AttackArea/AttackCollisionKiri");
				_currAttackCollision.Visible = true;
				_currAttackCollision.Disabled = false;
				
				//balik sprite, dan mainkan animasi
				animationPlayer.FlipH = true;
				animationPlayer.Play(animation);
			}
		}
		else if (direction.X == 0)// jika hanya vector y yang memiliki nilai (bergerak atas, bawah)
		{
			if (direction.Y > 0)
			{
				//ambil node collisin attack yang sesuai, munculkan dan aktivkan collision
				_currAttackCollision = GetNode<CollisionShape2D>("AttackArea/AttackCollisionDepan");
				_currAttackCollision.Visible = true;
				_currAttackCollision.Disabled = false;
				
				//mainkan animasi yang sesuai
				animation = "attack_depan_" + _currAttackSequence;
				animationPlayer.Play(animation);
			}
			else
			{
				//ambil node collisin attack yang sesuai, munculkan dan aktivkan collision
				_currAttackCollision = GetNode<CollisionShape2D>("AttackArea/AttackCollisionBelakang");
				_currAttackCollision.Visible = true;
				_currAttackCollision.Disabled = false;
				
				//mainkan animasi yang sesuai
				animation = "attack_belakang_" + _currAttackSequence;
				animationPlayer.Play(animation);
			}
		}

		//kode dibawah untuk menjaga rantai serangan
		//cek apakah sequence serangan saat ini tidak sama dengan 2 (kita memiliki 2 sequence attack)
		if (_currAttackSequence != 2)
		{
			// jika benar, maka sequence beriutnya
			_currAttackSequence += 1;
		}
		else
		{
			// jika salah, ulang dari 1
			_currAttackSequence = 1;
		}
	}

	public void EnemyEnterAttackArea(Node2D body)
	{
		//jika body yang masuk area ini adalah child class dari IEnemy
		if (body is IEnemy enemy)
		{
			//panggil fungsi hit dari class nya
			enemy.Hit();
		}
	}
}
