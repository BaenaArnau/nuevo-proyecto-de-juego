using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Enemigos
{
	/// <summary>
	/// Clase que representa un enemigo tipo pato que salta cuando el jugador salta cerca de él.
	/// Hereda de RigidBody2D para tener física realista.
	/// </summary>
	public partial class Pato : RigidBody2D
	{
		/// <summary>
		/// Velocidad de salto del pato (valor negativo para ir hacia arriba).
		/// </summary>
		public const float JumpVelocity = -500.0f;
		
		/// <summary>
		/// Indica si el jugador está dentro del área de detección del pato.
		/// </summary>
		private bool isInside = false;

		/// <summary>
		/// Velocidad de movimiento del pato.
		/// </summary>
		public float speed = 5.0f;

		/// <summary>
		/// Última componente Y de la velocidad del jugador para detectar el inicio del salto.
		/// </summary>
		private float _lastPlayerVelY = 0f;
    
		/// <summary>
		/// Indica si el pato puede saltar (evita saltos múltiples en el aire).
		/// </summary>
		private bool canJump = true;
		
		/// <summary>
		/// Referencia al jugador cuando está en el área de detección.
		/// </summary>
		private Player.Player player;
		
		/// <summary>
		/// Sprite animado del pato para controlar las animaciones.
		/// </summary>
    	public AnimatedSprite2D animatedSprite;
      
		/// <summary>Bool para indicar que el jugador está en proceso de muerte.</summary>
		public bool IsDying = false;

		/// <summary>
		/// Inicializa el pato obteniendo la referencia al AnimatedSprite2D.
		/// </summary>
		public override void _Ready()
		{
			animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		}

		/// <summary>
		/// Procesa la lógica del pato en cada frame.
		/// Detecta cuando el jugador salta (cambio en velocidad Y) y hace que el pato salte también.
		/// Controla las animaciones del pato según su estado.
		/// </summary>
		/// <param name="delta">Tiempo transcurrido desde el último frame</param>
		public override void _Process(double delta)
		{
			if (isInside && player != null && !IsDying)
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
				_lastPlayerVelY = 0f;

			if (LinearVelocity.Y == 0)
            {
				animatedSprite.SetAnimation("idle");
				canJump = true; 
            }
		}

		/// <summary>
		/// Se ejecuta cuando un cuerpo entra en el área de colisión del pato.
		/// Si el jugador toca al pato, este recibe daño.
		/// </summary>
		/// <param name="body">El cuerpo que entró en el área</param>
		private void _on_area_2d_body_entered (Node2D body)
		{
			if (body.IsInGroup("NinjaFrogGroup") && !IsDying)
			{	
				GD.Print("Player hit by duck, dying.");
				body.Call("Hit");
			}
		}

		/// <summary>
		/// Se ejecuta cuando un cuerpo sale del área de trigger del pato.
		/// Desactiva la detección del jugador.
		/// </summary>
		/// <param name="body">El cuerpo que salió del área</param>
		private void _on_trigger_area_2d_body_exited (Node2D body)
		{
			if (body is Player.Player)
			{
				isInside = false; // El jugador ya no está cerca
				this.player = null; // Limpia la referencia al jugador
			}
		}

		/// <summary>
		/// Se ejecuta cuando un cuerpo entra en el área de trigger del pato.
		/// Activa la detección del jugador para que el pato pueda reaccionar a sus saltos.
		/// </summary>
		/// <param name="body">El cuerpo que entró en el área</param>
		private void _on_trigger_area_2d_body_entered (Node2D body)
		{
			if (body is Player.Player p)
			{
				isInside = true; // El jugador está dentro del área de detección
				this.player = p; // Guarda la referencia al jugador
			}
		}
    
		/// <summary>
		/// Se ejecuta cuando un cuerpo entra en el área de daño del pato.
		/// Si el jugador toca esta área, el pato muere y el jugador rebota.
		/// </summary>
		/// <param name="body">El cuerpo que entró en el área</param>
		private void _on_damage_area_2d_body_entered (Node2D body)
		{
			if (body.IsInGroup("NinjaFrogGroup"))
			{
				IsDying = true;
				GD.Print("Duck hit by player, dying.");

				Die();

				player.Velocity = new Vector2(player.Velocity.X, Player.Player.BounceVelocity);
				player.MoveAndSlide();
				return;
			}
		}
    
		/// <summary>
		/// Establece la animación del pato.
		/// </summary>
		/// <param name="animationName">Nombre de la animación a reproducir</param>
		public void SetAnimation(string animationName)
		{
			if (animatedSprite != null)
			{
				animatedSprite.Play(animationName);
				GD.Print($"Setting animation to: {animationName}");
			}
		}

		/// <summary>
		/// Ejecuta la secuencia de muerte del pato.
		/// Reproduce la animación "hit", espera a que termine y luego elimina el pato de la escena.
		/// </summary>
		public async void Die()
		{
			animatedSprite.Play("hit");
			GD.Print("Duck is dying.");
			await ToSignal(animatedSprite, "animation_finished");
			QueueFree();
		}
	}
}