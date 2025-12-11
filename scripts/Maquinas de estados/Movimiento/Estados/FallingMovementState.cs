using Godot;
using System;
using PlayerType = NuevoProyectodeJuego.scripts.Player.Player;

namespace NuevoProyectodeJuego.scripts.Maquinas_de_estados.Movimiento.Estados
{
	/// <summary>
	/// Estado de movimiento para la caída del jugador.
	/// </summary>
	public partial class FallingMovementState : State
	{
		/// <summary>
		/// Referencia al jugador.
		/// </summary>
		private PlayerType _player;

		/// <summary>
		/// Método llamado al iniciar el nodo.
		/// </summary>
		public override async System.Threading.Tasks.Task Ready()
		{
			_player = (PlayerType)GetTree().GetFirstNodeInGroup("NinjaFrogGroup");
			if (!_player.IsNodeReady())
				await ToSignal(_player, "ready");
		}
		
		/// <summary>Al entrar en Falling: reproducir animación de caída.</summary>
		public override void Enter()
		{
			_player.SetAnimation("fall");
		}

		/// <summary>Actualización por frame en Falling: transiciones al aterrizar o doble salto.</summary>
		/// <param name="delta">Delta en segundos.</param>
		public override void Update(double delta)
		{
			if (_player.IsOnFloor() && _player.Velocity.X == 0)
				stateMachine.TransitionTo("IdleMovementState");
		}

		/// <summary>Update de física en Falling: aplica gravedad y control horizontal inmediato en aire.</summary>
		/// <param name="delta">Delta en segundos.</param>
		public override void UpdatePhysics(double delta)
		{
			if (!_player.IsOnFloor())
			{
				// Si el jugador está en una pared mientras cae, reproducir la animación de wall (antes de saltar)
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
				float move = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
				if (Mathf.Abs(move) > 0f)
					velocity.X = move * PlayerType.Speed;
				else
					velocity.X = 0f;
				_player.Velocity = velocity;
				_player.MoveAndSlide();
			}

			if (_player.IsOnFloor())
			{
				if (Mathf.Abs(_player.Velocity.X) > 0.1f)
					stateMachine.TransitionTo("RunningMovementState");
				else
					stateMachine.TransitionTo("IdleMovementState");
			}
		}

		/// <summary>Procesa eventos de entrada mientras se está en Falling.</summary>
		/// <param name="ev">Evento de entrada recibido.</param>
		public override void HandleInput(InputEvent ev)
		{
			if (ev.IsActionPressed("jump"))
			{
				// Priorizar wall-jump si hay pared
				if (_player.IsOnWall())
				{
					stateMachine.TransitionTo("WallJumpMovementState");
					return;
				}
				if (_player.CoyoteTimeCounter > 0f)
				{
					stateMachine.TransitionTo("JumpingMovementState");
					return;
				}
				if (_player.DoubleJumpAvailable)
					stateMachine.TransitionTo("DoubleJumpMovementState");
			}
		}
	}
}
