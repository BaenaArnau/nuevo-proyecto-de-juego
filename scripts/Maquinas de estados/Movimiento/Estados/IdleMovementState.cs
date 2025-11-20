using Godot;
using System;

public partial class IdleMovementState : State
{
	private Player _player;

    public override async void Ready()
    {
        _player = (Player)GetTree().GetFirstNodeInGroup("NinjaFrogGroup");
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
    }

    public override void HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("move_left") || @event.IsActionPressed("move_right"))
            stateMachine.TransitionTo("RunningMovementState");
        if (@event.IsActionPressed("jump") && _player.IsOnFloor())
            stateMachine.TransitionTo("JumpingMovementState");
    }
}
