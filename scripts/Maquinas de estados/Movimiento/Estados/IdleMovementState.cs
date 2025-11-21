using Godot;
using System;
using PlayerType = NuevoProyectodeJuego.scripts.Player.Player;
using NuevoProyectodeJuego.scripts.Maquinas_de_estados;

namespace NuevoProyectodeJuego.scripts.Maquinas_de_estados.Movimiento.Estados
{
    public partial class IdleMovementState : State
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
            _player.SetAnimation("idle");
            _player.Velocity = new Vector2(0, _player.Velocity.Y);
            _player.MoveAndSlide();
        }

        public override void Update(double delta)
        {
            if (!_player.IsOnFloor())
            {
                GD.Print("Transitioning to falling or jumping state from idle.");
                if (_player.Velocity.Y < 0)
                    stateMachine.TransitionTo("JumpingMovementState");
                else
                    stateMachine.TransitionTo("FallingMovementState");
            }
            
            if (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right"))
            {
                GD.Print("Transitioning to running state from idle (input polling).");
                stateMachine.TransitionTo("RunningMovementState");
                return;
            }

            if (Mathf.Abs(_player.Velocity.X) > 0)
            {
                GD.Print("Transitioning to running state from idle (velocity). ");
                stateMachine.TransitionTo("RunningMovementState");
            }
        }

        public override void HandleInput(InputEvent @event)
        {
            if (@event.IsActionPressed("move_left") || @event.IsActionPressed("move_right"))
                stateMachine.TransitionTo("RunningMovementState");
            if (@event.IsActionPressed("jump") && _player.IsOnFloor())
                stateMachine.TransitionTo("JumpingMovementState");
        }
    }
}
