using Godot;
using System;

public partial class FallingMovementState : State
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
        _player.SetAnimation("fall");
    }

	public override void Update(double delta)
    {
		if (_player.IsOnFloor() && _player.Velocity.X != 0)
		{
			GD.Print("Transitioning to running state from falling.");
			stateMachine.TransitionTo("RunningMovementState");	
		}
		if (_player.IsOnFloor() && _player.Velocity.X == 0)
		{
			GD.Print("Transitioning to idle state from falling.");
			stateMachine.TransitionTo("IdleMovementState");
		}
	}

	public override void HandleInput(InputEvent @event)
    {
		if (!@event.IsActionPressed("move_left") || !@event.IsActionPressed("move_right") && _player.IsOnFloor())
			stateMachine.TransitionTo("IdleMovementState");
		if (@event.IsActionReleased("move_left") && @event.IsActionReleased("move_right") && _player.IsOnFloor())
			stateMachine.TransitionTo("RunningMovementState");
    }

	public override void UpdatePhysics(double delta)
	{
	    if (!_player.IsOnFloor())
	    {
			Vector2 velocity = _player.Velocity;
	        velocity += _player.GetGravity() * (float)delta;
			_player.Velocity = velocity;
			_player.MoveAndSlide();
	    }
	}
}
