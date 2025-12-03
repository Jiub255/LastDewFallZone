using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class CameraSettings : Resource
	{
		[Export(PropertyHint.Range, "5, 50, 1")]
		public float MovementSpeed { get; private set; } = 50f;
		
		/// <summary>
		/// The movement speed of the camera's target position
		/// </summary>
		[ExportGroup("Keyboard Movement")]
		[Export(PropertyHint.Range, "5, 50, 1")]
		public float MovementSensitivity { get; private set; } = 15f;

		[ExportGroup("Edge Scrolling")]
		[Export]
		public bool EdgeScrollingEnabled { get; private set; }
		[Export(PropertyHint.Range, "5, 50, 1")]
		public float ScrollSpeed { get; private set; } = 15f;
		[Export(PropertyHint.Range, "0, 50, 0.1")]
		public float ScrollZoneThicknessPct { get; private set; } = 10f;
		
		// [ExportGroup("Mouse Dragging")]
		// [Export(PropertyHint.Range, "0.05, 5, 0.05")]
		// public float DragSpeed { get; private set; } = 0.35f;
		
		[ExportGroup("Rotation")]
		[Export(PropertyHint.Range, "0, 30, 1")]
		public float RotationMinX { get; private set; } = 10f;
		[Export(PropertyHint.Range, "60, 90, 1")]
		public float RotationMaxX { get; private set; } = 80f;
		// TODO: Convert these rotation speeds from radians per pixel or whatever to something that makes more sense.
		[Export(PropertyHint.Range, "0.0001, 0.1, 0.0001")]
		public float RotationSpeedX { get; private set; } = 0.005f;
		[Export(PropertyHint.Range, "0.0001, 0.1, 0.0001")]
		public float RotationSpeedY { get; private set; } = 0.005f;
		
		[ExportGroup("Zoom")]
		[Export(PropertyHint.Range, "0, 100, 0.001")]
		public float ZoomSensitivity { get; private set; } = 1f;
		[Export(PropertyHint.Range, "0, 100, 0.001")]
		public float ZoomSpeed { get; private set; } = 20f;
		[Export(PropertyHint.Range, "0, 15, 0.1")]
		public float ZoomMinDistance { get; private set; } = 3f;
		[Export(PropertyHint.Range, "16, 255, 0.1")]
		public float ZoomMaxDistance { get; private set; } = 50f;
		[Export(PropertyHint.Range, "3, 35, 0.1")]
		public float ZoomDefaultDistance { get; private set; } = 7f;
	}
}
