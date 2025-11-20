using System.Collections.Generic;
using Godot;

public partial class MovementStateMachine : Node
{
	[Export] public NodePath initialState;

    private Dictionary<string, State> _states;
    private State _current_state;

    public override void _Ready()
    {
        _states = new Dictionary<string, State>();
        foreach (Node node in GetChildren())
        {
            if (node is State s)
            {
                _states[node.Name] = s;
                s.stateMachine = this;
                s.Ready();
                s.Exit();
            }
        }

        _current_state = GetNode<State>(initialState);
        _current_state.Enter();
    }

    public override void _Process(double delta)
    {
        _current_state.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _current_state.UpdatePhysics(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        _current_state.HandleInput(@event);
    }

    public void TransitionTo(string key)
    {
        if (!_states.ContainsKey(key) || _current_state == _states[key])
        {
            return;
        }

        _current_state.Exit();
        _current_state = _states[key];
        _current_state.Enter();
    }   
}
