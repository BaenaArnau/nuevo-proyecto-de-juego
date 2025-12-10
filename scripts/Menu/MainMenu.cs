using Godot;
using System;
using NuevoProyectodeJuego.scripts.Menu.SubMenus;

namespace NuevoProyectodeJuego.scripts.Menu
{
	/// <summary>
	/// Controla el menú principal del juego (botones de iniciar, opciones y salir).
	/// Se mantienen los nombres de campos exportados para conservar las vinculaciones del inspector.
	/// </summary>
	public partial class MainMenu : Node
	{
		/// <summary>
		/// Menú de configuración.
		/// </summary>
		[Export] private Settings settingsMenu;
    
		/// <summary>
		/// Botón para iniciar el juego.
		/// </summary>
		[Export] private Button launchButton;
    
		/// <summary>
		/// Botón para salir del juego.
		/// </summary>
		[Export] private Button exitButton;
    
		/// <summary>
		/// Botón para abrir el menú de configuración.
		/// </summary>
		[Export] private Button settingsButton;

		/// <summary>
		/// Inicializa el estado del menú al entrar en la escena.
		/// </summary>
		public override void _Ready()
		{
			settingsMenu.Visible = false;
			launchButton.Visible = true;
			exitButton.Visible = true;
			settingsButton.Visible = true;
		}

		/// <summary>
		/// Procesamiento por frame del menú (mantiene visibilidad por simplicidad).
		/// </summary>
		public override void _Process(double delta)
		{
			if (!settingsMenu.Visible)
			{
				launchButton.Visible = true;
				exitButton.Visible = true;
				settingsButton.Visible = true;
			}
		}

		/// <summary>
		/// Maneja la pulsación del botón "Iniciar" y cambia a la escena del nivel.
		/// </summary>
		private void _on_launch_pressed()
		{
			GD.Print("Launch button pressed. Starting the game...");
			GetTree().ChangeSceneToFile("res://scenes/Niveles/node_2d.tscn");
		}

		/// <summary>
		/// Sale del juego.
		/// </summary>
		private void _on_exit_pressed()
		{
			GD.Print("Exit button pressed. Quitting the game...");
			GetTree().Quit();
		}

		/// <summary>
		/// Abre el panel de opciones ocultando los botones principales.
		/// </summary>
		private void _on_settings_pressed()
		{
			launchButton.Visible = false;
			exitButton.Visible = false;
			settingsButton.Visible = false;
			settingsMenu.Visible = true;
		}
	}
}
