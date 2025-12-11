using Godot;
using System;
using PlayerType = NuevoProyectodeJuego.scripts.Player.Player;

namespace NuevoProyectodeJuego.scripts.Maquinas_de_estados.Movimiento.Estados
{
    /// <summary>
    /// Estado de movimiento para el wall-jump del jugador.
    /// </summary>
    public partial class WallJumpMovementState : State
    {
        private PlayerType _player;

        public override async System.Threading.Tasks.Task Ready()
        {
            _player = (PlayerType)GetTree().GetFirstNodeInGroup("NinjaFrogGroup");
            if (!_player.IsNodeReady())
                await ToSignal(_player, "ready");
        }

        public override void Enter()
        {
            // Determinar lado de la pared para calcular impulso horizontal
            bool wallLeft = _player.IsWallOnLeft();

            float horizontal = wallLeft ? PlayerType.WallJumpHorizontal : -PlayerType.WallJumpHorizontal;

            // Reproducir animación específica de wall-jump; el flip del sprite se gestiona en Player._PhysicsProcess
            _player.SetAnimation("wall_jump");
            _player.Velocity = new Vector2(horizontal, PlayerType.WallJumpVertical);
            _player.MoveAndSlide();

            _player.EmitSignal(nameof(PlayerType.InJumping));
        }

        public override void Exit()
        {
        }

        public override void Update(double delta)
        {
            // Cuando empieza a descender, pasar a falling
            if (_player.Velocity.Y >= 0)
                stateMachine.TransitionTo("FallingMovementState");
        }

        public override void UpdatePhysics(double delta)
        {
            if (!_player.IsOnFloor())
            {
                Vector2 velocity = _player.Velocity;
                velocity += _player.GetGravity() * (float)delta;

                // Control horizontal en aire estilo detenido inmediato si no hay input
                float move = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
                if (Mathf.Abs(move) > 0f)
                    velocity.X = move * PlayerType.Speed;
                else
                    velocity.X = 0f;

                _player.Velocity = velocity;
                _player.MoveAndSlide();
            }
        }

        public override void HandleInput(InputEvent ev)
        {
            // No manejamos saltos adicionales aquí; el jugador puede transicionar a double jump desde Falling
        }
    }
}
