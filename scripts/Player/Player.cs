using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Player
{
	public partial class Player : CharacterBody2D
	{
		public const float Speed = 300.0f;
		public const float JumpVelocity = -400.0f;

		public override void _PhysicsProcess(double delta)
		{
			/*Vector2 velocity = Velocity;
			// Add the gravity.
			if (!IsOnFloor())
				velocity += GetGravity() * (float)delta;

			Velocity = velocity;
			MoveAndSlide();*/
		}

		public void SetAnimation(string animationName)
		{
			AnimatedSprite2D animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			animatedSprite.Play(animationName);
			GD.Print($"Setting animation to: {animationName}");
		}
	}
}
