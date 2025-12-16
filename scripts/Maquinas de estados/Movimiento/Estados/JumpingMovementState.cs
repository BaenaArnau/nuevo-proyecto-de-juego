using Godot;
using System;
using PlayerType = NuevoProyectodeJuego.scripts.Player.Player;

namespace NuevoProyectodeJuego.scripts.Maquinas_de_estados.Movimiento.Estados
{
	/// <summary>
	/// Estado de movimiento para el salto del jugador.
	/// </summary>
	public partial class JumpingMovementState : State
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

		/// <summary>Acciones al inicio del salto: aplicar la velocidad vertical de salto.</summary>
		public override void Enter()
		{
			_player.SetAnimation("jump");

            _player.CoyoteTimeCounter = 0f;
            _player.Velocity = new Vector2(_player.Velocity.X, PlayerType.JumpVelocity);
            _player.MoveAndSlide();

			_isEmittingJumpSignal = true;
			_player.EmitSignal(nameof(PlayerType.InJumping));
		}

		public override void Exit()
		{
			_isEmittingJumpSignal = false;
		}

		/// <summary>Actualización por frame durante el salto: gestiona transición a caída o al suelo.</summary>
		/// <param name="delta">Delta en segundos.</param>
		public override void Update(double delta)
		{
			if (_player.Velocity.Y >= 0)
				stateMachine.TransitionTo("FallingMovementState");

			if (_player.IsOnFloor())
			{
				if (Mathf.Abs(_player.Velocity.X) > 0.1f)
					stateMachine.TransitionTo("RunningMovementState");
				else
					stateMachine.TransitionTo("IdleMovementState");
			}
		}
		
		/// <summary>Update de física durante el salto. Implementa control horizontal inmediato (stop si no hay input).</summary>
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

        /// <summary>Gestión de input durante el salto: permite iniciar un doble salto si está disponible.</summary>
		/// <param name="ev">Evento de input.</param>
        public override void HandleInput(InputEvent ev)
		{
			if (ev.IsActionPressed("jump") && _player.DoubleJumpAvailable)
				stateMachine.TransitionTo("DoubleJumpMovementState");
		}
	}
}
