using Godot;
using System;
using PlayerType = NuevoProyectodeJuego.scripts.Player.Player;

namespace NuevoProyectodeJuego.scripts.Maquinas_de_estados.Movimiento.Estados
{	
	/// <summary>
	/// Estado de movimiento para el doble salto del jugador.
	/// </summary>
	public partial class DoubleJumpMovementState : State
	{
		/// <summary>
		/// Referencia al jugador.
		/// </summary>
		private PlayerType _player;

		// Indica si en este estado se debe emitir la señal de salto cada frame
		private bool _isEmittingJumpSignal = false;

		/// <summary>
		/// Método llamado al iniciar el nodo.
		/// </summary>
		public override async System.Threading.Tasks.Task Ready()
		{
			_player = (PlayerType)GetTree().GetFirstNodeInGroup("NinjaFrogGroup");
			if (!_player.IsNodeReady())
				await ToSignal(_player, "ready");
		}
		
		/// <summary>Al entrar en el estado de doble salto aplica la velocidad vertical de doble salto.</summary>
		public override void Enter()
		{
			_player.DoubleJumpAvailable = false;
			_player.SetAnimation("double_jump");

			_player.Velocity = new Vector2(_player.Velocity.X, PlayerType.JumpVelocity);
			_player.MoveAndSlide();

			_isEmittingJumpSignal = true;
			_player.EmitSignal(nameof(PlayerType.InJumping));
		}

		public override void Exit()
		{
			_isEmittingJumpSignal = false;
		}

		public override void HandleInput(InputEvent ev)
		{
			if (ev.IsActionPressed("jump") && _player.IsOnWall())
			{
				stateMachine.TransitionTo("WallJumpMovementState");
				return;
			}
		}

		/// <summary>Update por frame en doble salto: transiciones a caída al empezar a descender.</summary>
		/// <param name="delta">Delta en segundos.</param>
		public override void Update(double delta)
		{
			if (_player.Velocity.Y >= 0)
				stateMachine.TransitionTo("FallingMovementState");

			if (_player.animatedSprite.Frame == 5)
				_player.SetAnimation("jump");
		}
		
		/// <summary>Update de física durante el salto. Implementa control horizontal inmediato (stop si no hay input).</summary>
		/// <param name="delta">Delta en segundos.</param>
		public override void UpdatePhysics(double delta)
		{
			if (!_player.IsOnFloor())
			{
				// Si tocamos una pared en el doble salto, reproducir la animación de wall para indicar el contacto
				if (_player.IsOnWall())
				{
					_player.SetAnimation("wall_jump");
					if (_player.animatedSprite is AnimatedSprite2D aspr)
					{
						if (_player.IsWallOnLeft())
							aspr.FlipH = false;
						else if (_player.IsWallOnRight())
							aspr.FlipH = true;
					}
				}

				Vector2 velocity = _player.Velocity;
				velocity += _player.GetGravity() * (float)delta;
				// Horizontal air control tipo Hollow Knight: detenerse inmediatamente si no hay input
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
