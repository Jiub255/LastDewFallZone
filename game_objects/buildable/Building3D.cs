using Godot;

namespace Lastdew
{
	public partial class Building3D : Node3D
	{
		private const float RADIUS_INCREASE = 1f;
		
		private bool _done;
		private bool _overlapping;
		
		private MeshInstance3D MeshInstance { get; set; }
		private Color OkColor { get; set; } = Colors.Green;
		private Color NoColor { get; set; } = Colors.Red;
		private BoxShape3D BoxShape { get; set; }
		
		public bool Overlapping
		{
			get => _overlapping;
			private set
			{
				if (_overlapping == value)
				{
					return;
				}

				SetColor(!value);
				_overlapping = value;
			}
		}

		public override void _Ready()
		{
			if (!HasNode("%MeshInstance3D"))
			{
				_done = true;
				return;
			}
			
			ProcessMode = ProcessModeEnum.Always;
			MeshInstance = GetNode<MeshInstance3D>("%MeshInstance3D");
			
			Setup();
		}

		public override void _PhysicsProcess(double delta)
		{
			if (_done) return;
			
			base._Process(delta);
			Overlapping = IsOverlapping();
		}

		public void SetBuilding()
		{
			MeshInstance.QueueFree();
			ProcessMode = ProcessModeEnum.Inherit;
			_done = true;
		}

		private void Setup()
		{
			SetupCastShape();
			SetupIndicatorMesh();
			SetColor(!IsOverlapping());
		}

		private bool IsOverlapping()
		{
			return this.ShapeCast3D(BoxShape, 0b1).Count > 1;
		}

		private void SetupCastShape()
		{
			CollisionShape3D collisionShape = GetNode<CollisionShape3D>("%CollisionShape3D");
			// TODO: Just assuming box collision shapes for buildings for now.
			BoxShape3D shape = (BoxShape3D)collisionShape.Shape;
			BoxShape3D biggerShape = (BoxShape3D)shape.Duplicate();
			biggerShape.Size = new Vector3(
				shape.Size.X + RADIUS_INCREASE * 2,
				3,
				shape.Size.Z + RADIUS_INCREASE * 2);
			BoxShape = biggerShape;
		}

		private void SetupIndicatorMesh()
		{
			BoxMesh boxMesh = (BoxMesh)MeshInstance.Mesh;
			boxMesh.Size = new Vector3(
				BoxShape.Size.X,
				0.01f,
				BoxShape.Size.Z);
		}

		private void SetColor(bool ok)
		{
			StandardMaterial3D material = (StandardMaterial3D)MeshInstance.Mesh.SurfaceGetMaterial(0);
			material.AlbedoColor = ok ? OkColor : NoColor;
		}
	}
}
