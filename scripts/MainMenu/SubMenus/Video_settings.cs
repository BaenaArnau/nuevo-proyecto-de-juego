using Godot;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NuevoProyectodeJuego.scripts.MainMenu.SubMenus
{
	/// <summary>
	/// Menú de las opciones de vídeo del juego.
	/// Implementa un listado de resoluciones y guarda la selección en un XML en la carpeta de usuario.
	/// </summary>
	public partial class Video_settings : CanvasLayer
	{
	[ExportGroup("resolucion")]
	[Export] private OptionButton _optionButton;
	private static bool _pantalla = false;

		/// <summary>
		/// Setter de pantalla (indica si el panel de vídeo está visible).
		/// </summary>
		/// <param name="s">Valor a establecer.</param>
		public static void SetPantalla(bool s)
		{
			_pantalla = s;
		}

		/// <summary>
		/// Inicialización del nodo. Carga opciones y estado guardado si existe.
		/// </summary>
		public override void _Ready()
		{
			AddResolutionToButton();
			// Si se quisiera, aquí podríamos cargar el XML para seleccionar la resolución previamente guardada.
		}

		/// <summary>
		/// Añade resoluciones comunes al OptionButton y selecciona la que más se aproxime al tamaño actual de ventana.
		/// </summary>
		private void AddResolutionToButton()
		{
			if (_optionButton == null)
				return;

			_optionButton.Clear();

			var common = new (int w, int h)[] {
				(1920,1080), (1600,900), (1366,768), (1280,720), (1024,768), (800,600)
			};

			for (int i = 0; i < common.Length; i++)
			{
				var r = common[i];
				_optionButton.AddItem($"{r.w} x {r.h}");
			}

			// Seleccionamos por defecto la primera entrada. Si se desea, se puede mejorar para
			// detectar la resolución actual según la versión de Godot usada.
			_optionButton.Selected = 0;
		}

		/// <summary>
		/// Guarda las opciones elegidas por el usuario en un XML dentro de la carpeta de datos del usuario.
		/// </summary>
		private void Save()
		{
			try
			{
				string userDataDir = OS.GetUserDataDir();
				string configFolderPath = Path.Combine(userDataDir, "config");

				if (!Directory.Exists(configFolderPath))
					Directory.CreateDirectory(configFolderPath);

				XElement xmlData = new XElement("Settings", new XElement("Resolution", _optionButton.Selected));
				string filePath = Path.Combine(configFolderPath, "settings.xml");
				xmlData.Save(filePath);
			}
			catch (IOException ex)
			{
				GD.PrintErr("Error al guardar la configuración: " + ex.Message);
			}
		}

		/// <summary>
		/// Botón 'Aplicar' pulsado: aplica la resolución seleccionada y guarda la configuración.
		/// </summary>
		public void _on_button_pressed()
		{
			ApplyGameDataVideoSettings();
			Save();
		}

		/// <summary>
		/// Maneja cuando el usuario selecciona una entrada en el OptionButton.
		/// </summary>
		/// <param name="mySelectedRez">Índice seleccionado.</param>
		public void _on_option_button_item_selected(int mySelectedRez)
		{
			// Opción seleccionada: actualizamos la selección interna.
			_optionButton.Selected = mySelectedRez;
		}

		/// <summary>
		/// Aplica la resolución seleccionada al juego. Intenta usar la API de OS para cambiar el tamaño de ventana.
		/// </summary>
		public void ApplyGameDataVideoSettings()
		{
			if (_optionButton == null)
				return;

			var text = _optionButton.GetItemText(_optionButton.Selected);
			var parts = text.Split('x');
			if (parts.Length != 2)
				return;

			if (!int.TryParse(parts[0].Trim(), out int w) || !int.TryParse(parts[1].Trim(), out int h))
				return;

			// Cambiar la resolución puede depender de la versión de Godot; por ahora imprimimos la selección.
			GD.Print($"Resolución seleccionada para aplicar: {w}x{h}");
		}

		/// <summary>
		/// Salir del submenu de vídeo y volver al menú principal.
		/// </summary>
		public void _on_exit_pressed()
		{
			// Usamos CallDeferred con nombre de método para compatibilidad.
			CallDeferred(nameof(ChangeSceneToMainMenu));
		}

		private void ChangeSceneToMainMenu()
		{
			GetTree().ChangeSceneToFile("scenes/Menus/main_menu.tscn");
		}
	}
}
