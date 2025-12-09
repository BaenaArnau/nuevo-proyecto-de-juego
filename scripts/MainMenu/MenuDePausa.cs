using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.MainMenu
{
	/// <summary>
	/// Clase que gestiona el menú de pausa.
	/// Sigue el estilo y paradigmas usados en el resto del proyecto (namespaces, XML docs, comprobaciones nulas).
	/// </summary>
	public partial class MenuDePausa : Control
	{
	[Export] private Button _continuar, _opciones, _salir, _exit;
	[Export] private CanvasLayer _video, _sonido;

		/// <summary>
		/// Inicializa el estado del menú.
		/// </summary>
		public override void _Ready()
		{
			// Estado por defecto: menú oculto y juego sin pausar
			Visible = false;
			GetTree().Paused = false;
		}

		/// <summary>
		/// Gestiona la tecla de pausa. Usa IsActionJustPressed para evitar múltiples toggles por frame.
		/// </summary>
		/// <param name="delta">Tiempo en segundos desde el último frame (no usado).</param>
		public override void _Process(double delta)
		{
			if (Input.IsActionJustPressed("pause"))
			{
				if (!Visible)
				{
					Show();
					GetTree().Paused = true;
					Video_settings.SetPantalla(true);
				}
				else
				{
					Hide();
					GetTree().Paused = false;
					Video_settings.SetPantalla(false);
				}
			}
		}

		/// <summary>
		/// Continuar la partida desde el menú de pausa.
		/// </summary>
		public void _on_continuar_pressed()
		{
			GetTree().Paused = false;
			Hide();
		}

		/// <summary>
		/// Mostrar opciones dentro del menú de pausa.
		/// </summary>
		public void _on_opciones_pressed()
		{
			_continuar?.Hide();
			_opciones?.Hide();
			_salir?.Hide();
			if (_video != null) _video.Visible = true;
			if (_sonido != null) _sonido.Visible = true;
			_exit?.Show();
			Video_settings.SetPantalla(true);
		}

		/// <summary>
		/// Sale al menú principal (usa CallDeferred para evitar cambios de escena en medio de callbacks Godot).
		/// </summary>
		public void _on_salir_al_menu_principal_pressed()
		{
			GetTree().Paused = false;
			CallDeferred(nameof(ChangeSceneToMainMenu));
		}

		private void ChangeSceneToMainMenu()
		{
			// Ruta corregida a carpeta 'scenes' del proyecto
			GetTree().ChangeSceneToFile("scenes/Menus/main_menu.tscn");
		}

		/// <summary>
		/// Cierra la pestaña de ajustes y vuelve al menú de pausa.
		/// </summary>
		public void _on_exit_pressed()
		{
			_continuar?.Show();
			_opciones?.Show();
			_salir?.Show();
			_exit?.Hide();
			if (_video != null) _video.Hide();
			if (_sonido != null) _sonido.Hide();
			Video_settings.SetPantalla(false);
		}
	}
}
