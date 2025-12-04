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
		private bool _isInside;

		/// <summary>
		/// Velocidad de movimiento del pato (editable en el inspector).
		/// </summary>
		[Export] public float Speed = 5.0f;

		// Nota: ya no usamos el muestreo de la velocidad del jugador aquí.
    
		/// <summary>
		/// Indica si el pato puede saltar (evita saltos múltiples en el aire).
		/// </summary>
		private bool _canJump = true;
		
		/// <summary>
		/// Referencia al jugador cuando está en el área de detección (Node guardado y casteado cuando hace falta).
		/// </summary>
		private Node _playerNode;

		/// <summary>
		/// Node del sprite animado del pato (se castea a AnimatedSprite2D al usar).
		/// </summary>
		private Node _animatedSpriteNode;

		/// <summary>Bool para indicar que el pato está en proceso de muerte (solo escritura interna).</summary>
		public bool IsDying { get; private set; }

		/// <summary>
		/// Inicializa el pato obteniendo la referencia al AnimatedSprite2D.
		/// </summary>
		public override void _Ready()
		{
			_animatedSpriteNode = GetNodeOrNull("AnimatedSprite2D");
			if (_animatedSpriteNode == null)
				GD.PrintErr("AnimatedSprite2D no encontrado en Pato. Comprueba la escena.");
		}

		/// <summary>
		/// Procesa la lógica del pato en cada frame.
		/// Detecta cuando el jugador salta (cambio en velocidad Y) y hace que el pato salte también.
		/// Controla las animaciones del pato según su estado.
		/// </summary>
		/// <param name="delta">Tiempo transcurrido desde el último frame</param>
		public override void _Process(double delta)
		{
			if (LinearVelocity.Y == 0 && !IsDying)
			{
				SetAnimation("idle");
				_canJump = true;
			}
		}

		/// <summary>
		/// Se ejecuta cuando un cuerpo entra en el área de colisión del pato.
		/// Si el jugador toca al pato, este recibe daño.
		/// </summary>
		/// <param name="body">El cuerpo que entró en el área</param>
		private void _on_area_2d_body_entered (Node2D body)
		{
			if (body is NuevoProyectodeJuego.scripts.Player.Player p && !IsDying)
			{ 
				GD.Print("Player hit by duck, dying.");
				_ = p.HitAsync();
			}
		}

		/// <summary>
		/// Se ejecuta cuando un cuerpo sale del área de trigger del pato.
		/// Desactiva la detección del jugador.
		/// </summary>
		/// <param name="body">El cuerpo que salió del área</param>
		private void _on_trigger_area_2d_body_exited (Node2D body)
		{
			if (body is NuevoProyectodeJuego.scripts.Player.Player p)
			{
				// Desconectamos la señal de salto para no recibir más eventos
				var callable = new Callable(this, nameof(OnPlayerInJumping));
				if (p.IsConnected("InJumping", callable))
					p.Disconnect("InJumping", callable);

				_isInside = false; // El jugador ya no está cerca
				this._playerNode = null; // Limpia la referencia al jugador
			}
		}

		/// <summary>
		/// Se ejecuta cuando un cuerpo entra en el área de trigger del pato.
		/// Activa la detección del jugador para que el pato pueda reaccionar a sus saltos.
		/// </summary>
		/// <param name="body">El cuerpo que entró en el área</param>
		private void _on_trigger_area_2d_body_entered (Node2D body)
		{
			if (body is NuevoProyectodeJuego.scripts.Player.Player p)
			{
				_isInside = true;
				this._playerNode = p; 

				var callable = new Callable(this, nameof(OnPlayerInJumping));
				if (!p.IsConnected("InJumping", callable))
					p.Connect("InJumping", callable);
			}
		}

		/// <summary>
		/// Manejador llamado cuando el jugador emite la señal de salto.
		/// Hace que el pato salte si está dentro del área y puede hacerlo.
		/// </summary>
		private void OnPlayerInJumping()
		{
			if (_isInside && _canJump && !IsDying)
			{
				SetAnimation("jump");
				LinearVelocity = new Vector2(LinearVelocity.X, JumpVelocity);
				_canJump = false;
			}
		}
    
		/// <summary>
		/// Se ejecuta cuando un cuerpo entra en el área de daño del pato.
		/// Si el jugador toca esta área, el pato muere y el jugador rebota.
		/// </summary>
		/// <param name="body">El cuerpo que entró en el área</param>
		private void _on_damage_area_2d_body_entered (Node2D body)
		{
			if (body is NuevoProyectodeJuego.scripts.Player.Player p)
			{
				IsDying = true;
				GD.Print("Duck hit by player, dying.");

				_ = DieAsync();

				p.Velocity = new Vector2(p.Velocity.X, NuevoProyectodeJuego.scripts.Player.Player.BounceVelocity);
				p.MoveAndSlide();
			}
		}
    
		/// <summary>
		/// Establece la animación del pato.
		/// </summary>
		/// <param name="animationName">Nombre de la animación a reproducir</param>
		public void SetAnimation(string animationName)
		{
			if (_animatedSpriteNode is AnimatedSprite2D aspr)
				aspr.Play(animationName);
		}

		/// <summary>
		/// Ejecuta la secuencia de muerte del pato.
		/// Reproduce la animación "hit", espera a que termine y luego elimina el pato de la escena.
		/// </summary>
		public async System.Threading.Tasks.Task DieAsync()
		{
			try
			{
				if (_animatedSpriteNode is AnimatedSprite2D aspr)
				{
					aspr.Play("hit");
					GD.Print("Duck is dying.");
					await ToSignal(aspr, "animation_finished");
				}
				CallDeferred("queue_free");
			}
			catch (Exception ex)
			{
				GD.PrintErr("Error en DieAsync: ", ex);
				CallDeferred("queue_free");
			}
		}
	}
}