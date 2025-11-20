using Godot;
using System;

public partial class JumpingMovementState : State
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
        _player.SetAnimation("jump");

		_player.Velocity = new Vector2(_player.Velocity.X, Player.JumpVelocity);
		/*Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
	
		if (direction != Vector2.Zero)
			_player.Velocity = new Vector2(direction.X * Player.Speed, _player.Velocity.Y);
		else
			_player.Velocity = new Vector2(Mathf.MoveToward(_player.Velocity.X, 0, Player.Speed), _player.Velocity.Y);*/
		_player.MoveAndSlide();
    }

	public override void Update(double delta)
	{
		if (_player.Velocity.Y >= 0)
		{
			GD.Print("Transitioning to falling state from jumping.");
			stateMachine.TransitionTo("FallingMovementState");
		}
		if (_player.IsOnFloor() && _player.Velocity.X != 0)
		{
			GD.Print("Transitioning to running state from jumping.");
			stateMachine.TransitionTo("RunningMovementState");
		}
		if (_player.IsOnFloor() && _player.Velocity.X == 0)
		{
			GD.Print("Transitioning to idle state from jumping.");
			stateMachine.TransitionTo("IdleMovementState");
		}
	}

	public override void HandleInput(InputEvent @event)
    {
		if (!@event.IsActionPressed("move_left") || !@event.IsActionPressed("move_right"))
        {
            Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");

			if (direction != Vector2.Zero)
				_player.Velocity = new Vector2(direction.X * Player.Speed, _player.Velocity.Y);
			else
				_player.Velocity = new Vector2(Mathf.MoveToward(_player.Velocity.X, 0, Player.Speed), _player.Velocity.Y);

			_player.MoveAndSlide();
        }
		if (@event.IsActionReleased("move_left") && @event.IsActionReleased("move_right"))
		{
			Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
			if (direction != Vector2.Zero)
				_player.Velocity = new Vector2(direction.X * Player.Speed, _player.Velocity.Y);
			else
				_player.Velocity = new Vector2(Mathf.MoveToward(_player.Velocity.X, 0, Player.Speed), _player.Velocity.Y);
			_player.MoveAndSlide();
		}
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
