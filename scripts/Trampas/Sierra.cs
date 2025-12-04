using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Trampas
{
	/// <summary>
	/// Clase que representa una trampa de sierra giratoria.
	/// </summary>
	public partial class Sierra : Area2D
	{
		/// <summary>
		/// Nodo Sprite de la trampa (privado, casteado al usar).
		/// </summary>
		private Node _spriteNode;

		/// <summary>
		/// Método llamado al iniciar el nodo.
		/// </summary>
		public override void _Ready()
		{
			_spriteNode = GetNodeOrNull("Sprite2D");
			if (_spriteNode == null)
				GD.PrintErr("Sprite2D no encontrado en Sierra. Comprueba la escena.");
		}

		/// <summary>
		/// Método de procesamiento por frame.
		/// </summary>
		/// <param name="delta">Delta en segundos desde el último frame.</param>		
		public override void _Process(double delta)
		{
			if (_spriteNode is Sprite2D spr)
				spr.Rotation += (float)(delta * 10.0);
		}

		/// <summary>
		/// Método llamado al detectar una colisión con otro cuerpo.
		/// </summary>
		/// <param name="body">El cuerpo que ha entrado en colisión.</param>
		private void _on_body_entered(Node2D body)
		{
			if (body is NuevoProyectodeJuego.scripts.Player.Player p)
			{
				GD.Print("Player hit by saw trap, dying.");
				_ = p.HitAsync();
			}
		}
	}
}
