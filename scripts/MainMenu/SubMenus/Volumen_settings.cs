using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.MainMenu.SubMenus
{
	/// <summary>
	/// Clase que gestiona las opciones de sonido (volúmenes).
	/// Ajusta los buses de audio usando AudioServer y mantiene las etiquetas de porcentaje.
	/// </summary>
	public partial class Volumen_settings : CanvasLayer
	{
		private int _masterBus, _vfxBus, _musicBus;

		[Export] private HSlider _master_slider, _vfx_slider, _music_slider;

		[Export] private Label _master_porcentual, _vfx_porcentual, _music_porcentual;

		/// <summary>
		/// Inicializa índices de buses y sincroniza sliders con el estado actual del AudioServer.
		/// </summary>
		public override void _Ready()
		{
			// Intentamos obtener los índices por nombre, con fallback a índices 0..2 si no existen.
			try
			{
				_masterBus = AudioServer.GetBusIndex("Master");
				if (_masterBus < 0) _masterBus = 0;
			}
			catch { _masterBus = 0; }

			try
			{
				_vfxBus = AudioServer.GetBusIndex("VFX");
				if (_vfxBus < 0) _vfxBus = 1;
			}
			catch { _vfxBus = 1; }

			try
			{
				_musicBus = AudioServer.GetBusIndex("Music");
				if (_musicBus < 0) _musicBus = 2;
			}
			catch { _musicBus = 2; }

			// Sincronizamos sliders con los valores actuales (si están asignados).
			try
			{
				if (_master_slider != null)
				{
					float db = AudioServer.GetBusVolumeDb(_masterBus);
					float linear = Mathf.DbToLinear(db);
					_master_slider.Value = linear * 100f;
					_master_porcentual.Text = ((int)_master_slider.Value) + "%";
				}

				if (_vfx_slider != null)
				{
					float db = AudioServer.GetBusVolumeDb(_vfxBus);
					float linear = Mathf.DbToLinear(db);
					_vfx_slider.Value = linear * 100f;
					_vfx_porcentual.Text = ((int)_vfx_slider.Value) + "%";
				}

				if (_music_slider != null)
				{
					float db = AudioServer.GetBusVolumeDb(_musicBus);
					float linear = Mathf.DbToLinear(db);
					_music_slider.Value = linear * 100f;
					_music_porcentual.Text = ((int)_music_slider.Value) + "%";
				}
			}
			catch (Exception ex)
			{
				GD.PrintErr("Error al inicializar Volumen_settings: " + ex.Message);
			}
		}

		/// <summary>
		/// Método conectado al cambio del slider Master.
		/// </summary>
		/// <param name="volumen">Valor del slider en porcentaje (0-100).</param>
		public void _on_volumen_master_value_changed(float volumen)
		{
			if (_master_porcentual != null)
				_master_porcentual.Text = (int)volumen + "%";

			// Convertimos porcentaje a valor lineal 0..1 antes de convertir a dB
			float linear = volumen / 100f;
			AudioServer.SetBusVolumeDb(_masterBus, Mathf.LinearToDb(linear));
		}

		/// <summary>
		/// Método conectado al cambio del slider VFX.
		/// </summary>
		/// <param name="volumen">Valor del slider en porcentaje (0-100).</param>
		public void _on_volumen_vfx_value_changed(float volumen)
		{
			if (_vfx_porcentual != null)
				_vfx_porcentual.Text = (int)volumen + "%";

			float linear = volumen / 100f;
			AudioServer.SetBusVolumeDb(_vfxBus, Mathf.LinearToDb(linear));
		}

		/// <summary>
		/// Método conectado al cambio del slider Música.
		/// </summary>
		/// <param name="volumen">Valor del slider en porcentaje (0-100).</param>
		public void _on_volumen_musica_value_changed(float volumen)
		{
			if (_music_porcentual != null)
				_music_porcentual.Text = (int)volumen + "%";

			float linear = volumen / 100f;
			AudioServer.SetBusVolumeDb(_musicBus, Mathf.LinearToDb(linear));
		}
	}
}
