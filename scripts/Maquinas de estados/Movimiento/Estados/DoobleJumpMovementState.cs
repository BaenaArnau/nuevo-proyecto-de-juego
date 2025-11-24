using Godot;
using System;
using PlayerType = NuevoProyectodeJuego.scripts.Player.Player;

namespace NuevoProyectodeJuego.scripts.Maquinas_de_estados.Movimiento.Estados
{
	public partial class DoobleJumpMovementState : State
	{
		private PlayerType _player;

		public override async void Ready()
		{
			_player = (PlayerType)GetTree().GetFirstNodeInGroup("NinjaFrogGroup");
			if (!_player.IsNodeReady())
				await ToSignal(_player, "ready");
		}
		/// <summary>Al entrar en el estado de doble salto aplica la velocidad vertical de doble salto.</summary>
		public override void Enter()
		{
			_player.SetAnimation("jump");

			GD.Print("Entered DoobleJumpMovementState (double jump)");

			_player.Velocity = new Vector2(_player.Velocity.X, PlayerType.JumpVelocity);
			_player.MoveAndSlide();

			_player.IsMySecondJumpAvailable = false;
		}

		/// <summary>Actualización por frame en doble salto: transiciones a caída o al aterrizar.</summary>
		/// <param name="delta">Delta en segundos.</param>
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

		/// <summary>Update de física en doble salto: aplica gravedad y control horizontal inmediato.</summary>
		/// <param name="delta">Delta en segundos.</param>
		public override void UpdatePhysics(double delta)
		{
			if (!_player.IsOnFloor())
			{
				Vector2 velocity = _player.Velocity;
				velocity += _player.GetGravity() * (float)delta;
				float move = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
				if (Mathf.Abs(move) > 0f)
					velocity.X = move * PlayerType.Speed;
				else
					velocity.X = 0f;
				_player.Velocity = velocity;
				_player.MoveAndSlide();
			}
		}
	}
}
