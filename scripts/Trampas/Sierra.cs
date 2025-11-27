using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Trampas
{
	public partial class Sierra : Area2D
	{
		public Sprite2D sprite;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			sprite = GetNode<Sprite2D>("Sprite2D");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			sprite.Rotation += (float)(delta * 10.0);
		}

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
