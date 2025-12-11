using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Player
{
	/// <summary>
	/// Clase que representa al jugador.
	/// </summary>
	public partial class Player : CharacterBody2D
	{
		/// <summary>Velocidad horizontal configurada del jugador (px/s).</summary>
		public const float Speed = 300.0f;

		/// <summary>Velocidad vertical aplicada al iniciar un salto (negativa = hacia arriba).</summary>
		public const float JumpVelocity = -500.0f;

		/// <summary>Velocidad horizontal aplicada al realizar un wall-jump (valor absoluto, se invertirá según lado).</summary>
		public const float WallJumpHorizontal = 400.0f;

		/// <summary>Velocidad vertical aplicada al realizar un wall-jump (negativa = hacia arriba).</summary>
		public const float WallJumpVertical = -450.0f;

		/// <summary>Velocidad vertical aplicada al rebotar sobre un enemigo (negativa = hacia arriba).</summary>
		public const float BounceVelocity = -300.0f;

		/// <summary>Indica si el doble salto está disponible.</summary>
		public bool DoubleJumpAvailable { get; internal set; } = true;

		/// <summary>Duración máxima del salto coyote en segundos.</summary>
		public const float CoyoteTimeMax = 0.15f;
		/// <summary>Tiempo restante para salto coyote (lectura pública, modificación privada).</summary>
		private float _coyoteTimeCounter;
		public float CoyoteTimeRemaining => _coyoteTimeCounter;

		[Signal] public delegate void InJumpingEventHandler();

		/// <summary>
		/// Compatibilidad: propiedad con el nombre antiguo usada por estados externos.
		/// Permite lectura pública y asignación interna.
		/// </summary>
		public float CoyoteTimeCounter { get => _coyoteTimeCounter; internal set => _coyoteTimeCounter = value; }

		/// <summary>
		/// Compatibilidad: exponer AnimatedSprite2D como propiedad pública para estados que lo usan directamente.
		/// </summary>
		public AnimatedSprite2D animatedSprite
		{
			get
			{
				if (_animatedSpriteNode is AnimatedSprite2D aspr)
					return aspr;
				return GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			}
		}

		/// <summary>Bool para indicar que el jugador está en proceso de muerte (solo lectura externa).</summary>
		public bool IsDying { get; private set; }

		/// <summary> Sprite animado del jugador para controlar las animaciones (privado).</summary>
		private Node _animatedSpriteNode;

		// RayCast2D opcionales colocados como hijos del jugador para detectar paredes de forma fiable
		private RayCast2D _wallCheckLeftNode;
		private RayCast2D _wallCheckRightNode;

		/// <summary>
		/// Distancia en píxeles para comprobar colisión con pared a izquierda/derecha.
		/// </summary>
		public const float WallCheckDistance = 12.0f;

		/// <summary>
		/// Método llamado al iniciar el nodo.
		/// </summary>
		public override void _Ready()
		{
			_animatedSpriteNode = GetNodeOrNull("AnimatedSprite2D");
			_wallCheckLeftNode = GetNodeOrNull("WallCheckLeft") as RayCast2D;
			_wallCheckRightNode = GetNodeOrNull("WallCheckRight") as RayCast2D;
		}

		/// <summary>Actualización de física del jugador. Mantener ligera para lógica central.</summary>
		/// <param name="delta">Tiempo en segundos desde el último paso de física.</param>
		public override void _PhysicsProcess(double delta)
		{
			// Control de flip del sprite:
			// - Si estamos pegados a una pared en el aire (IsOnWall && !IsOnFloor && !IsOnCeiling) usamos IsWallOnLeft/Right
			//   para orientar al jugador AWAY de la pared (mirando hacia fuera).
			// - En otro caso, conservar comportamiento por velocidad.
			if (IsOnWall() && !IsOnFloor() && !IsOnCeiling())
			{
				if (_animatedSpriteNode is AnimatedSprite2D aspr)
				{
					// Si la pared está a la izquierda, queremos que el jugador mire a la derecha (FlipH = false).
					if (IsWallOnLeft())
						aspr.FlipH = false;
					else if (IsWallOnRight())
						aspr.FlipH = true;
				}
			}
			else
			{
				if (Velocity.X < 0f)
				{
					if (_animatedSpriteNode is AnimatedSprite2D aspr)
						aspr.FlipH = true;
				}
				else if (Velocity.X > 0f)
				{
					if (_animatedSpriteNode is AnimatedSprite2D aspr2)
						aspr2.FlipH = false;
				}
			}

			if (IsOnFloor())
				_coyoteTimeCounter = CoyoteTimeMax;
			else
			{
				_coyoteTimeCounter -= (float)delta;
				if (_coyoteTimeCounter < 0f)
					_coyoteTimeCounter = 0f;
			}
		}

		/// <summary>
		/// Reproduce una animación en el <see cref="AnimatedSprite2D"/> hijo.
		/// Método auxiliar usado por los estados para cambiar la animación.
		/// </summary>
		/// <param name="animationName">Nombre de la animación a reproducir.</param>
		public void SetAnimation(string animationName)
		{
			if (IsDying)
				return;

			if (_animatedSpriteNode is AnimatedSprite2D aspr)
				aspr.Play(animationName);

		}

		/// <summary>
		/// Método llamado cuando el jugador recibe daño.
		/// </summary>
		public async System.Threading.Tasks.Task HitAsync()
		{
			try
			{
				IsDying = true;
				if (_animatedSpriteNode is AnimatedSprite2D aspr)
				{
					aspr.Play("hit");
					await ToSignal(aspr, "animation_finished");
				}
				GetTree().CallDeferred("reload_current_scene");
			}
			catch (Exception)
			{
				GetTree().CallDeferred("reload_current_scene");
			}
		}

		/// <summary>
		/// Comprueba si hay una pared a la izquierda del jugador en un pequeño rango.
		/// </summary>
		public bool IsWallOnLeft()
		{
			// Si existe un RayCast2D hijo, usarlo (más fiable y estable con el orden de física)
			if (_wallCheckLeftNode != null)
				return _wallCheckLeftNode.IsColliding();

			var space = GetWorld2D().DirectSpaceState;
			Vector2 from = GlobalPosition;
			Vector2 to = from + new Vector2(-WallCheckDistance, 0);
			var query = new PhysicsRayQueryParameters2D();
			query.From = from;
			query.To = to;
			var result = space.IntersectRay(query);
			return result != null && result.Count > 0;
		}

		/// <summary>
		/// Comprueba si hay una pared a la derecha del jugador en un pequeño rango.
		/// </summary>
		public bool IsWallOnRight()
		{
			if (_wallCheckRightNode != null)
				return _wallCheckRightNode.IsColliding();

			var space = GetWorld2D().DirectSpaceState;
			Vector2 from = GlobalPosition;
			Vector2 to = from + new Vector2(WallCheckDistance, 0);
			var query = new PhysicsRayQueryParameters2D();
			query.From = from;
			query.To = to;
			var result = space.IntersectRay(query);
			return result != null && result.Count > 0;
		}
	}
}
