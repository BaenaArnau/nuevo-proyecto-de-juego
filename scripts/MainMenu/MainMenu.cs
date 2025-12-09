using Godot;
using System;

public partial class MainMenu : Node
{
	private void _on_launch_pressed()
	{
		GD.Print("Launch button pressed. Starting the game...");
		GetTree().ChangeSceneToFile("res://scenes/Niveles/node_2d.tscn");
	}

	private void _on_exit_pressed()
	{
		GD.Print("Exit button pressed. Quitting the game...");
		GetTree().Quit();
	}
}
