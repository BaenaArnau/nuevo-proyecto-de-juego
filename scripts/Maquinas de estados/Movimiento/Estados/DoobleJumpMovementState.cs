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
		}
	}
}
