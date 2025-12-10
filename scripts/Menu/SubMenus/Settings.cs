using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Menu.SubMenus
{
	/// <summary>
	/// Panel de configuración (resolución, volumen y otras opciones).
	/// Añadido namespace y comentarios para Doxygen.
	/// </summary>
	public partial class Settings : Control
	{
		/// <summary>
		/// Inicialización del panel de ajustes.
		/// </summary>
		public override void _Ready()
		{
			GetViewport().SizeChanged += OnResized;
		}
        
		/// <summary>
		/// Ajusta el volumen maestro (en dB).
		/// </summary>
		private void _on_volume_value_changed(float value)
		{
			AudioServer.SetBusVolumeDb(0, value);
		}

		/// <summary>
		/// Activa o desactiva el mute del bus maestro.
		/// </summary>
		private void _on_check_box_toggled(bool pressed)
		{
			AudioServer.SetBusMute(0, pressed);
		}

		/// <summary>
		/// Aplica una resolución seleccionada por índice desde la UI.
		/// </summary>
		private void _on_resolutions_item_selected(int index)
		{
			Vector2I size;
			switch (index)
			{
				case 0: size = new Vector2I(1920, 1080); break;
				case 1: size = new Vector2I(1600, 900); break;
				case 2: size = new Vector2I(1366, 768); break;
				case 3: size = new Vector2I(1280, 720); break;
				case 4: size = new Vector2I(1024, 768); break;
				case 5: size = new Vector2I(800, 600); break;
				default: size = DisplayServer.WindowGetSize(); break;
			}

			// Aplica el nuevo tamaño
			DisplayServer.WindowSetSize(size);

			// Centrar sólo si no estamos ejecutando embebido en el editor (el editor gestiona sus propios paneles)
			if (!Engine.IsEditorHint())
			{
				var screenSize = DisplayServer.ScreenGetSize(0);
				DisplayServer.WindowSetPosition((screenSize - size) / 2);
			}
		}


		/// <summary>
		/// Cambia entre modo ventana y pantalla completa.
		/// </summary>
		private void _on_full_screen_toggled(bool pressed)
		{
			if (pressed)
				GetWindow().SetMode(Window.ModeEnum.Fullscreen);
			else
				GetWindow().SetMode(Window.ModeEnum.Windowed);
		}

		/// <summary>
		/// Cierra el panel de ajustes volviendo invisble el control.
		/// </summary>
		private void _on_return_pressed()
		{
			Visible = false;
		}

		/// <summary>
		/// Handler llamado cuando la ventana/interna cambia de tamaño.
		/// </summary>
		private void OnResized()
		{
			GD.Print("Resolución cambiada a: " + GetViewport().GetVisibleRect().Size);
		}
	}
}
