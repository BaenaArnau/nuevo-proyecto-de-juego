using Godot;
using System;
using PlayerType = NuevoProyectodeJuego.scripts.Player.Player;

namespace NuevoProyectodeJuego.scripts.Maquinas_de_estados.Movimiento.Estados
{
	public partial class JumpingMovementState : State
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
			_player.SetAnimation("jump");

			_player.Velocity = new Vector2(_player.Velocity.X, PlayerType.JumpVelocity);
			_player.MoveAndSlide();
		}

		public override void Update(double delta)
		{
			if (_player.Velocity.Y >= 0)
			{
				GD.Print("Transitioning to falling state from jumping.");
				stateMachine.TransitionTo("FallingMovementState");
				return;
			}

			if (_player.IsOnFloor())
			{
				if (Mathf.Abs(_player.Velocity.X) > 0.1f)
				{
					GD.Print("Transitioning to running state from jumping (landed).");
					stateMachine.TransitionTo("RunningMovementState");
				}
				else
				{
					GD.Print("Transitioning to idle state from jumping (landed).");
					stateMachine.TransitionTo("IdleMovementState");
				}
			}
		}
		
		public override void UpdatePhysics(double delta)
		{
			if (!_player.IsOnFloor())
			{
				Vector2 velocity = _player.Velocity;
				velocity += _player.GetGravity() * (float)delta;
				_player.Velocity = velocity;

				Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
				if (direction != Vector2.Zero)
					_player.Velocity = new Vector2(direction.X * PlayerType.Speed, _player.Velocity.Y);
				_player.MoveAndSlide();
			}
		}
	}
}
