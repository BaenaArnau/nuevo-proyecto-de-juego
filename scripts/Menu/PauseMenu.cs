namespace NuevoProyectodeJuego.scripts.Menu;
using Godot;

/// <summary>
/// Clase que controla el menú de pausa y el estado pausado del juego.
/// </summary>
public partial class PauseMenu : CanvasLayer
{
    private Control _settingsMenu;
    [Export] private Button _resumeButton;
    [Export] private Button _settingsButton;
    [Export] private Button _quitButton;

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

    public void _on_resume_button_pressed() => TogglePause(false);

    public void _on_quit_button_pressed()
    {
        GetTree().Paused = false;
        GetTree().Quit();
    }

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
