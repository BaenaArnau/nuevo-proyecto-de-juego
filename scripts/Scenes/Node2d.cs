using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Scenes
{
	public partial class Node2d : Node
	{
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

