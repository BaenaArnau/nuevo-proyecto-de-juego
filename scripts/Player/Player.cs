using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Player
{
	public partial class Player : CharacterBody2D
	{
		public const float Speed = 300.0f;
		public const float JumpVelocity = -500.0f;

		public override void _PhysicsProcess(double delta) {}

		public void SetAnimation(string animationName)
		{
			AnimatedSprite2D animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			animatedSprite.Play(animationName);
			GD.Print($"Setting animation to: {animationName}");
		}
	}
}
