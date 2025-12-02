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
		/// Sprite de la trampa.
		/// </summary>
		public Sprite2D sprite;

		/// <summary>
		/// Método llamado al iniciar el nodo.
		/// </summary>
		public override void _Ready()
		{
			sprite = GetNode<Sprite2D>("Sprite2D");
		}

		/// <summary>
		/// Método de procesamiento por frame.
		/// </summary>
		/// <param name="delta">Delta en segundos desde el último frame.</param>		
		public override void _Process(double delta)
		{
			sprite.Rotation += (float)(delta * 10.0);
		}

		/// <summary>
		/// Método llamado al detectar una colisión con otro cuerpo.
		/// </summary>
		/// <param name="body">El cuerpo que ha entrado en colisión.</param>
		private void _on_body_entered(Node2D body)
		{
			if (body.IsInGroup("NinjaFrogGroup"))
			{
				GD.Print("Player hit by saw trap, dying.");
				body.Call("Hit");
			}
		}
	}
}
