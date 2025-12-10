namespace NuevoProyectodeJuego.scripts.Menu;
using Godot;

/// <summary>
/// Clase que controla el menú de pausa y el estado pausado del juego.
/// </summary>
public partial class PauseMenu : CanvasLayer
{
    /// <summary>
    /// Menú de configuración.
    /// </summary>
    private Control _settingsMenu;

    /// <summary>
    /// Botón para reanudar el juego.
    /// </summary>
    [Export] private Button _resumeButton;

    /// <summary>
    /// Botón para abrir el menú de configuración.
    /// </summary>
    [Export] private Button _settingsButton;

    /// <summary>
    /// Botón para salir del juego.
    /// </summary>
    [Export] private Button _quitButton;

    /// <summary>
    /// Inicializa el menú de pausa.
    /// </summary>
    public override void _Ready()
    {
        Visible = false;

        _settingsMenu = GetNodeOrNull<Control>("Settings2");
        if (_settingsMenu != null)
            _settingsMenu.Visible = false;

        if (_resumeButton != null)
            _resumeButton.Visible = true;
        if (_settingsButton != null)
            _settingsButton.Visible = true;
        if (_quitButton != null)
            _quitButton.Visible = true;
    }

    /// <summary>
    /// Maneja la entrada no procesada (como la pulsación de la tecla de pausa).
    /// </summary>
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause") && !@event.IsEcho())
            TogglePause();
    }

    /// <summary>
    /// Alterna el estado de pausa (opcionalmente lo fija).
    /// </summary>
    public void TogglePause(bool? pause = null)
    {
        bool newState = pause ?? !GetTree().Paused;
        GetTree().Paused = newState;
        Visible = newState;
    }

    /// <summary>
    /// Maneja la pulsación del botón "Reanudar".
    /// </summary>
    public void _on_resume_button_pressed() => TogglePause(false);

    /// <summary>
    /// Maneja la pulsación del botón "Salir".
    /// </summary>
    public void _on_quit_button_pressed()
    {
        GetTree().Paused = false;
        GetTree().Quit();
    }

    /// <summary>
    /// Maneja la pulsación del botón "Configuración".
    /// </summary>
    public void _on_settings_button_pressed()
    {
        if (_resumeButton != null)
            _resumeButton.Visible = false;
        if (_settingsButton != null)
            _settingsButton.Visible = false;
        if (_quitButton != null)
            _quitButton.Visible = false;
        if (_settingsMenu != null)
            _settingsMenu.Visible = true;
    }

    /// <summary>
    /// Procesa la lógica del menú de pausa.
    /// </summary>
    /// <param name="delta">Delta en segundos.</param>
    public override void _Process(double delta)
    {
        if (_settingsMenu != null && !_settingsMenu.Visible)
        {
            if (_resumeButton != null)
                _resumeButton.Visible = true;
            if (_settingsButton != null)
                _settingsButton.Visible = true;
            if (_quitButton != null)
                _quitButton.Visible = true;
        }
    }
}
