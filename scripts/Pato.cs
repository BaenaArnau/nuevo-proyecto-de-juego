using Godot;
using System;


namespace NuevoProyectodeJuego.scripts.Enemigos
{
	public partial class Pato : RigidBody2D
	{
		
		public const float JumpVelocity = -500.0f;
		private bool isInside = false;

		// Valores exportados para configurar desde el editor (coinciden con pato.tscn)
		public float speed = 5.0f;

		// Última componente Y del jugador para detectar el inicio del salto (edge detect)
		private float _lastPlayerVelY = 0f;
    
		private bool canJump = true;
		private Player.Player player;
    	public AnimatedSprite2D animatedSprite;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (isInside && player != null)
			{
				if (player.Velocity.Y < -10f && _lastPlayerVelY >= -10f && canJump)
				{
					animatedSprite.SetAnimation("jump");
					LinearVelocity = new Vector2(LinearVelocity.X, JumpVelocity);
          			canJump = false;
				}

				_lastPlayerVelY = player.Velocity.Y;
			}
			else
			{
				_lastPlayerVelY = 0f;
			}

			if (LinearVelocity.Y == 0)
            {
				animatedSprite.SetAnimation("idle");
				canJump = true;
            }
		}

		private void _on_area_2d_body_entered (Node2D body)
		{
			if (body.IsInGroup("NinjaFrogGroup"))
			{
				GD.Print("Player hit by duck, dying.");
				body.Call("Hit");
			}
		}

		private void _on_trigger_area_2d_body_exited (Node2D body)
		{
			// Evitar shadowing: no declarar nueva variable local con el mismo nombre.
			if (body is Player.Player)
			{
				isInside = false;
				this.player = null;
			}
		}

		private void _on_trigger_area_2d_body_entered (Node2D body)
		{
			// Usar un nombre local distinto y asignar al campo 'this.player' para
			// evitar shadowing y asegurar que la referencia de campo se inicializa.
			if (body is Player.Player p)
			{
				isInside = true;
				this.player = p;
			}
		}
    
		public void SetAnimation(string animationName)
		{
			if (animatedSprite != null)
			{
				animatedSprite.Play(animationName);
				GD.Print($"Setting animation to: {animationName}");
			}
		}
	}
}