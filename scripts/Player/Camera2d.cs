using Godot;
using System;

namespace NuevoProyectodeJuego.scripts.Player
{
	public partial class Camera2d : Camera2D
	{
		[Export] private Player player;

		private float targetDragLeft = 0f;
		private float targetDragRight = 0f;
		private float smoothSpeedHorizontal = 5f;
		private float smoothSpeedVertical = 8f;

		public override void _Ready()
		{
			PositionSmoothingEnabled = false;
			PositionSmoothingSpeed = smoothSpeedHorizontal;
		}

		public override void _Process(double delta)
        {
            PositionSmoothingEnabled = true;
            controlDrag();
            smoothingSpeed(delta);
        }

        private void smoothingSpeed(double delta)
        {
            DragLeftMargin = Mathf.Lerp(DragLeftMargin, targetDragLeft, (float)delta * 5f);
            DragRightMargin = Mathf.Lerp(DragRightMargin, targetDragRight, (float)delta * 5f);

            float deltaY = player.GlobalPosition.Y - GlobalPosition.Y;

            if (Mathf.Abs(deltaY) > 0.1f)
                PositionSmoothingSpeed = smoothSpeedVertical;
            else
                PositionSmoothingSpeed = smoothSpeedHorizontal;
        }

        private void controlDrag()
        {
            if (player.Velocity.X > 10)
            {
                targetDragLeft = 0.6f;
                targetDragRight = -1.0f;
            }
            else if (player.Velocity.X < -10)
            {
                targetDragLeft = -1.0f;
                targetDragRight = 0.6f;
            }
            else
            {
                targetDragLeft = 0f;
                targetDragRight = 0f;
            }
        }
    }
}