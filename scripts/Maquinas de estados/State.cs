using Godot;
using System;
using NuevoProyectodeJuego.scripts.Maquinas_de_estados.Movimiento;

namespace NuevoProyectodeJuego.scripts.Maquinas_de_estados
{
    public partial class State : Node
    {
        public MovementStateMachine stateMachine;

        public virtual void Enter() { }
        public virtual void Exit() { }

        public new virtual void Ready() { }
        public virtual void Update(double delta) { }
        public virtual void UpdatePhysics(double delta) { }
        public virtual void HandleInput(InputEvent @event) { }
    }
}
