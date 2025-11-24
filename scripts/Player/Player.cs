using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Player
{
	public partial class Player : CharacterBody2D
	{
		/// <summary>Velocidad horizontal configurada del jugador (px/s).</summary>
		public const float Speed = 300.0f;

		/// <summary>Velocidad vertical aplicada al iniciar un salto (negativa = hacia arriba).</summary>
		public const float JumpVelocity = -500.0f;

		/// <summary>
		/// Bandera serializable que indica si el jugador aún puede realizar el segundo salto.
		/// Se expone al editor para facilitar pruebas y depuración desde Godot.
		/// </summary>
		[Export]
		public bool IsMySecondJumpAvailable = true;

		/// <summary>Actualización de física del jugador. Mantener ligera para lógica central.</summary>
		/// <param name="delta">Tiempo en segundos desde el último paso de física.</param>
		public override void _PhysicsProcess(double delta) { }

		/// <summary>
		/// Reproduce una animación en el <see cref="AnimatedSprite2D"/> hijo.
		/// Método auxiliar usado por los estados para cambiar la animación.
		/// </summary>
		/// <param name="animationName">Nombre de la animación a reproducir.</param>
		public void SetAnimation(string animationName)
		{
			AnimatedSprite2D animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			if (animatedSprite != null)
			{
				animatedSprite.Play(animationName);
				GD.Print($"Setting animation to: {animationName}");
			}
		}
	}
}
