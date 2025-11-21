using Godot;
using System;
using PlayerType = NuevoProyectodeJuego.scripts.Player.Player;

namespace NuevoProyectodeJuego.scripts.Maquinas_de_estados.Movimiento.Estados
{
	public partial class RunningMovementState : State
	{
		private PlayerType _player;

		public override async void Ready()
		{
			_player = (PlayerType)GetTree().GetFirstNodeInGroup("NinjaFrogGroup");
			if (!_player.IsNodeReady())
				await ToSignal(_player, "ready");
		}

		public override void Enter()
		{
			_player.SetAnimation("run");
		}
		
		public override void Update(double delta)
		{
			if (!_player.IsOnFloor())
			{
				GD.Print("Transitioning to falling or jumping state from running.");
				if (_player.Velocity.Y < 0)
					stateMachine.TransitionTo("JumpingMovementState");
				else 
					stateMachine.TransitionTo("FallingMovementState");

				return;
			}
			if (Mathf.Abs((float)_player.Velocity.X) < 0.1f)
			{
				GD.Print("Transitioning to idle state from running.");
				stateMachine.TransitionTo("IdleMovementState");
			}
		}

		public override void UpdatePhysics(double delta)
		{
			Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
			if (direction != Vector2.Zero)
				_player.Velocity = new Vector2(direction.X * PlayerType.Speed, _player.Velocity.Y);
			else
				_player.Velocity = new Vector2(0, _player.Velocity.Y);
			_player.MoveAndSlide();
		}

		public override void HandleInput(InputEvent @event)
		{
			if (@event.IsActionPressed("jump") && _player.IsOnFloor())
				stateMachine.TransitionTo("JumpingMovementState");
		}
	}
}
