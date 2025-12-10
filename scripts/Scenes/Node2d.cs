using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Scenes
{
	/// <summary>
	/// Nodo2D que maneja la transición a la siguiente escena al entrar en un área específica.
	/// </summary>
	public partial class Node2d : Node
	{
		/// <summary>
		/// Maneja la entrada del jugador en el área designada para cambiar de escena.
		/// </summary>
		private void _on_next_area_2d_body_entered (Node2D body)
		{
			if (body is NuevoProyectodeJuego.scripts.Player.Player)
			{
				GD.Print("Player entered next area, changing to next scene...");
				GetTree().CallDeferred("change_scene_to_file", "res://scenes/Niveles/level_2.tscn");
			}
		}
	}
}

